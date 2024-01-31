//-----------------------------------------------------------------------
/* カードの処理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region 変数宣言
    //フィールド管理クラス
    private FieldAreaManagerScript _fieldAreaScript = default;

    [SerializeField, Header("オブジェクトのRectTransform")]
    public RectTransform _rectTransform = default;
    [SerializeField, Header("カードUI表示")]
    private CardUIScript _cardUI = default;
    [SerializeField, Header("オブジェクトのCanvasGroup")]
    private CanvasGroup _canvasGroup = default;

    //デッキ編集画面用
    private DeckEditWindowScript _deckEditWindow = default;
    //ドラッグ終了後に戻ってくる座標
    private Vector2 _basePos = default;
    //移動Tween
    private Tween _moveTween = default;
    //カードゾーンの種類
    private CardZoneScript.ZoneType _nowZone = default;
    public bool isInDeckCard = false;

    //基になるカードデータ
    private CardDataSO _cardDate = default;
    //カード効果のリスト
    private List<CardEffectDefineScript> _cardEffects = default;
    //カードの強度
    private int _forcePoint = default;
    //カードを使用キャラクター
    private int _controllerCharaID = default;
    // 戦闘報酬画面用
    private RewardScript _rewardPanel = default;

    //カード移動アニメーション時間
    private const float MoveTime = 0.4f;
    //キャラクターID定数
    //戦闘内のキャラクター人数
    public const int CharaNum = 2;
    //キャラクターID:主人公(プレイヤーキャラ)
    public const int CharaID_Player = 0;
    //キャラクターID:敵
    public const int CharaID_Enemy = 1;
    //キャラクターID:(無し)
    public const int CharaID_None = -1;
    //キャラクターID:ボーナス用
    public const int CharaID_Bonus = 2;
    //カードアイコンの最大数
    private const int MaxIcons = 6;
    //カード効果の最大数
    private const int MaxEffects = 6;
    //カード強度の上限値(これを超えると破壊)
    private const int MaxForcePoint = 9;
    //CanvasGroupの変更先Alpha値
    private const float TargetAlpha = 0.5f;
    //演出時間
    private const float AnimTime = 0.3f;
    //移動先X相対座標
    private const float TargetPositionX_Relative = -300.0f;
    //演出時間
    private const float OutAnimTime = 1.0f;
    #endregion

    #region プロパティ
    public CardZoneScript.ZoneType GetNowZone { get => _nowZone; }
    public int GetControllerCharaID { get => _controllerCharaID; }
    public List<CardEffectDefineScript> GetCardEffects { get => _cardEffects; }
    public int GetForcePoint { get => _forcePoint; }
    public CardDataSO GetCardData { get => _cardDate; }
    #endregion

    #region 初期処理
    /// <summary>
    /// 初期化処理フィールドマネージャーから呼び出し
    /// </summary>
    /// <param name="_fieldManager">フィールドマネージャー</param>
    /// <param name="initPos">初期座標</param>
    public void InitField(FieldAreaManagerScript _fieldManager, Vector2 initPos)
    {
        //FieldAreaManagerScript参照取得
        _fieldAreaScript = _fieldManager;
        //UI初期設定
        _cardUI.Init(this);
        //変数初期化
        //初期位置に移動
        _rectTransform.position = initPos;
        //初期位置固定
        _basePos = initPos;
        //初期の場所
        _nowZone = CardZoneScript.ZoneType.Hand;
        //リスト生成
        _cardEffects = new List<CardEffectDefineScript>();
    }
    /// <summary>
	/// 初期化処理デッキ編集クラスから呼出用
	/// </summary>
	/// <param name="deckEditWindow">DeckEditWindow参照</param>
	/// <param name="_isInDeckCard">デッキ内カードフラグ</param>
	public void InitDeck(DeckEditWindowScript deckEditWindow, bool _isInDeckCard)
    {
        _deckEditWindow = deckEditWindow;
        isInDeckCard = _isInDeckCard;
        InitField(null, Vector2.zero);
    }
    /// <summary>
	/// 初期化処理RewardScript画面から呼出し
	/// </summary>
	public void InitReward(RewardScript rewardPanel)
    {
        _rewardPanel = rewardPanel;
        InitField(null, Vector2.zero);
    }

    /// <summary>
    /// カードのパラメータを取得する
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="ControllerCharaID"></param>
    public void SetInitialCardData(CardDataSO cardData, int ControllerCharaID)
    {
        //カードデータ取得
        _cardDate = cardData;
        //カード名
        _cardUI.SetCardNameText(cardData.GetCardName);

        //カード効果リスト
        foreach (CardEffectDefineScript item in cardData.GetEffectList)
        {
            AddCardEffect(item);
        }
        //強度を設定
        SetForcePoint(cardData.GetForce);
        //カード使用者データ
        _controllerCharaID = ControllerCharaID;
        //カード背景UIに適用
        _cardUI.SetCardBackSprite(ControllerCharaID);
    }
    #endregion

    #region 移動処理
    /// <summary>
    /// 基本座標までカードを移動させる
    /// </summary>
    public void BackToBasePos()
    {
        //既に実行中の移動アニメーションがあれば停止
        if (_moveTween != null)
        {
            _moveTween.Kill();
        }
        //指定地点まで移動するアニメーション(DOTween)
        _moveTween = _rectTransform
            .DOMove(_basePos, MoveTime) //移動Tween
            .SetEase(Ease.OutQuart);    //変化の仕方を指定
    }
    /// <summary>
	/// カードを指定のゾーンに設置する
	/// </summary>
	/// <param name="zoneType">対象カードゾーン</param>
	/// <param name="targetPos">対象座標</param>
	public void PutToZone(CardZoneScript.ZoneType zoneType, Vector2 targetPos)
    {
        //座標を取得
        _basePos = targetPos;
        //初期位置に戻す
        BackToBasePos();
        //カードゾーンの種類を保存
        _nowZone = zoneType;
    }

    /// <summary>
	/// カードを少しずつ薄くするTweenを実行する
	/// </summary>
	/// <returns>実行Tween</returns>
	public Tween HideFadeTween()
    {
        return _canvasGroup.DOFade(TargetAlpha, AnimTime);
    }
    /// <summary>
	/// カードを画面外に移動させるTweenを実行する
	/// </summary>
	/// <returns>実行Tween</returns>
	public void HideMoveTween()
    {
        _rectTransform.DOAnchorPosX(TargetPositionX_Relative, OutAnimTime)
            .SetRelative();
    }
    #endregion

    #region クリック処理
    /// <summary>
    /// クリックしたときに実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //ドラッグ開始処理
        if (_fieldAreaScript != null)
        {
            _fieldAreaScript.StartDragging(this);
        }
        else if (_deckEditWindow != null)
        {
            _deckEditWindow.SelectCard(this);
        }
        else if (_rewardPanel != null)
        {
            _rewardPanel.SelectCard(this);
        }
    }
    /// <summary>
    /// クリックが終わった時に実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //クリック終了処理
        if (_fieldAreaScript != null)
        {
            _fieldAreaScript.EndDragging();
        }

    }
    /// <summary>
	/// プレイヤーが保管中のカード数量を表示(デッキ編集時用)
	/// </summary>
	public void ShowCardAmountInStorage()
    {
        int amount = PlayerDeckDataScript._storageCardList[_cardDate.GetSerialNum];
        _cardUI.SetAmountText(amount);
    }
    /// <summary>
    /// カードの強調表示状態を変更する
    /// </summary>
    public void SetCardHilight(bool mode)
    {
        _cardUI.SetHilightImage(mode);
    }
    #endregion

    #region パラメータ変更
    /// <summary>
    /// カードの効果値を増減する
    /// </summary>
    /// <param name="effectType">対象効果データ</param>
    /// <param name="value">変化量</param>
    public void EnhanceCardEffect(CardEffectDefineScript.CardEffect effectType, int value)
    {
        for (int i = 0; i < _cardEffects.Count; i++)
        {
            //対象効果が存在するか
            if (_cardEffects[i].GetEffect == effectType)
            {
                //効果値を変更
                _cardEffects[i].GetValue += value;
                //UI表示
                _cardUI.ApplyCardEffectText(_cardEffects[i]);
                return;
            }
        }
    }
    /// <summary>
	/// 合成によってカード効果を追加する
	/// </summary>
	/// <param name="newEffect">効果の種類・数値データ</param>
	public void CompoCardEffect(CardEffectDefineScript newEffect)
    {
        //効果の合成可否を取得
        CardEffectDefineScript.EffectCompoMode compoMode = CardEffectDefineScript.Dic_EffectCompoMode[newEffect.GetEffect];
        //このカードが敵側のカードかを取得
        bool isEnemyCard = false;
        if (_controllerCharaID == CharaID_Enemy)
        {
            isEnemyCard = true;
        }

        //合成不可なら終了
        switch (compoMode)
        {
            case CardEffectDefineScript.EffectCompoMode.Impossible:
                //合成不可能
                return;

            case CardEffectDefineScript.EffectCompoMode.OnlyOwn:
                //自分とカードとのみ合成可能
                if (isEnemyCard)
                {
                    return;
                }
                else
                {
                    break;
                }
            case CardEffectDefineScript.EffectCompoMode.OnlyOwn_New:
                // 自分とカードとのみ合成可能(新規のみ)
                if (isEnemyCard)
                {
                    return;
                }
                else
                {
                    break;
                }

        }

        //追加される効果と同じ種類の効果が既に存在するかを調べる
        //現在の効果の種類数
        int length = _cardEffects.Count;
        //同じ効果データの配列内番号
        int index;
        for (index = 0; index < length; index++)
        {
            //存在した場合：番号を保存してループ終了
            if (_cardEffects[index].GetEffect == newEffect.GetEffect)
            {
                break;
            }

        }

        //効果の合成・追加処理
        if (index < length)
        {
            //同じ種類の効果がある：合成処理
            //新規追加限定の効果なら合成しない
            if (compoMode == CardEffectDefineScript.EffectCompoMode.OnlyNew ||
                compoMode == CardEffectDefineScript.EffectCompoMode.OnlyOwn_New)
            {
                return;
            }
            //効果値増減
            EnhanceCardEffect(index, newEffect.GetValue);
        }
        else
        {
            //同じ種類の効果がない
            AddCardEffect(newEffect);
        }
    }
    /// <summary>
    /// カード効果を新規追加する
    /// </summary>
    /// <param name="newEffect">効果の種類・数値データ</param>
    private void AddCardEffect(CardEffectDefineScript newEffect)
    {
        //カード効果数上限なら終了
        if (_cardEffects.Count >= MaxEffects)
        {
            return;
        }

        //効果データを新規作成する
        CardEffectDefineScript effectData = new CardEffectDefineScript();
        effectData.GetEffect = newEffect.GetEffect;
        effectData.GetValue = newEffect.GetValue;
        //効果リストに追加
        _cardEffects.Add(effectData);
        //UI表示
        _cardUI.AddCardEffectText(effectData);
    }
    /// <summary>
	/// カードの効果値を増減する
	/// </summary>
	/// <param name="index">対象効果データの配列内番号</param>
	/// <param name="value">変化量</param>
	public void EnhanceCardEffect(int index, int value)
    {
        //効果値を変更
        CardEffectDefineScript effectData = _cardEffects[index];
        effectData.GetValue += value;
        //UI表示
        _cardUI.ApplyCardEffectText(_cardEffects[index]);
    }
    /// <summary>
    /// カードの強度をセットする
    /// </summary>
    /// <param name="value">強度の変化量</param>
    /// <returns>破壊可能か</returns>
	public bool SetForcePoint(int value)
    {
        //パラメータをセット
        _forcePoint = value;
        //UI表示
        _cardUI.SetForcePointText(_forcePoint);

        //破壊可能か
        if (value > MaxForcePoint)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    #endregion

    #region その他Get
    /// <summary>
    /// 効果リストの中に該当する効果があるか
    /// </summary>
    /// <param name="effectType">検索する効果</param>
    /// <returns>存在するか</returns>
    public bool CheckContainEffect(CardEffectDefineScript.CardEffect effectType)
    {
        //該当する効果があればtrueを返す
        foreach (CardEffectDefineScript effect in _cardEffects)
        {
            if (effect.GetEffect == effectType)
            {
                return true;
            }
        }
        //ない場合はfalseを返す
        return false;
    }
    /// <summary>
	/// 効果リストの中に該当の効果種が存在する場合その効果量を返す
	/// </summary>
	public int GetEffectValue(CardEffectDefineScript.CardEffect effectType)
    {
        //該当する効果があれば効果量を返す
        foreach (CardEffectDefineScript effect in _cardEffects)
        {
            if (effect.GetEffect == effectType)
            {
                return effect.GetValue;
            }
        }
        //ない場合は-1を返す
        return -1;
    }
    #endregion
}
