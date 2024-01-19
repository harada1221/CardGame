//-----------------------------------------------------------------------
/* カードの処理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
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
    [SerializeField,Header("オブジェクトのCanvasGroup")]
    private CanvasGroup _canvasGroup = default;

    //ドラッグ終了後に戻ってくる座標
    private Vector2 _basePos = default;
    //移動Tween
    private Tween _moveTween = default;
    //カードゾーンの種類
    private CardZoneScript.ZoneType _nowZone = default;

    //基になるカードデータ
    private CardDataSO _cardDate = default;
    //カード効果のリスト
    private List<CardEffectDefineScript> _cardEffects = default;
    //カードの強度
    private int _forcePoint = default;
    //カードを使用キャラクター
    private int _controllerCharaID = default;

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
    //カードアイコンの最大数
    private const int MaxIcons = 6;
    //カード効果の最大数
    private const int MaxEffects = 6;
    #endregion

    #region プロパティ
    public CardZoneScript.ZoneType GetNowZone { get => _nowZone; }
    public int GetControllerCharaID { get => _controllerCharaID; }
    public List<CardEffectDefineScript> GetCardEffects { get => _cardEffects; }
    public int GetForcePoint { get => _forcePoint; }
    #endregion

    #region 初期処理
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="_fieldManager">フィールドマネージャー</param>
    /// <param name="initPos">初期座標</param>
    public void Init(FieldAreaManagerScript _fieldManager, Vector2 initPos)
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
    /// カードのパラメータを取得する
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="ControllerCharaID"></param>
    public void SetInitialCardData(CardDataSO cardData, int ControllerCharaID)
    {
        //カードデータ取得
        _cardDate = cardData;
        // カード名
        _cardUI.SetCardNameText(cardData.GetCardName);

        //カード効果リスト
        foreach (CardEffectDefineScript item in cardData.GetEffectList)
        {
            AddCardEffect(item);
        }
        //強度を設定
        SetForcePoint(cardData.GetForce);
        // カード使用者データ
        _controllerCharaID = ControllerCharaID;
        _cardUI.SetCardBackSprite(ControllerCharaID); // カード背景UIに適用
    }
    #endregion

    #region 移動処理
    /// <summary>
    /// 基本座標までカードを移動させる
    /// </summary>
    public void BackToBasePos()
    {
        // 既に実行中の移動アニメーションがあれば停止
        if (_moveTween != null)
        {
            _moveTween.Kill();
        }
        // 指定地点まで移動するアニメーション(DOTween)
        _moveTween = _rectTransform
            .DOMove(_basePos, MoveTime) // 移動Tween
            .SetEase(Ease.OutQuart);    // 変化の仕方を指定
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
    #endregion

    #region クリック処理
    /// <summary>
    /// クリックしたときに実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //クリック開始処理
        _fieldAreaScript.StartDragging(this);
    }
    /// <summary>
    /// クリックが終わった時に実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //クリック終了処理
        _fieldAreaScript.EndDragging();
    }
    #endregion

    #region パラメータ変更
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
	/// カードの強度をセットする
	/// </summary>
	public void SetForcePoint(int forcevalue)
    {
        //パラメータをセット
        _forcePoint = forcevalue;
        //UI表示
        _cardUI.SetForcePointText(forcevalue);
    }
    #endregion
}
