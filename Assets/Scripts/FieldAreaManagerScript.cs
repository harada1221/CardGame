//-----------------------------------------------------------------------
/* フィールドの管理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldAreaManagerScript : MonoBehaviour
{
    #region 変数宣言
    [SerializeField, Header("CanvasのRectTransform")]
    private RectTransform _canvasRectTransform = default;
    [SerializeField, Header("メインカメラ")]
    private Camera _mainCamera = default;
    [SerializeField, Header("デッキアイコンのオブジェクト")]
    private GameObject _deckIconObject = default;
    [SerializeField, Header("デッキの残り枚数表示テキスト")]
    private Text _deckRemainingNum = default;
    [SerializeField, Header("プレイボタン")]
    private Button _cardPlayButton = default;
    [SerializeField, Header("デッキ補充ボタン")]
    private Image _replenishButtonImage = default;
    //カード関連
    [SerializeField, Header("カードのプレハブ")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("生成するカードオブジェクトの親Transform")]
    private Transform _cardsParentTransForm = default;
    [SerializeField, Header("デッキオブジェクトTransform")]
    private Transform _deckTransForm = default;

    //戦闘画面マネージャー
    private BattleManagerScript _battleManager = default;
    //プレイヤーデッキクラス
    private PlayerDeckDataScript _playerDeckDataScript = default;
    //ダミー手札制御クラス
    private DammyHandScript _dammyHand = default;
    //プレイヤーの現在のデッキ
    private List<CardDataSO> _playerDeckData = default;
    //プレイヤーデッキのバックアップ
    private List<CardDataSO> _playerDeckDataBackUp = default;
    //ドラッグ操作中カード
    private CardScript _draggingCard = default;
    //生成したプレイヤー操作カードリスト
    private List<CardScript> _cardInstances = default;
    //手札整列フラグ
    private bool ishandSort = false;
    //手札補充中フラグ
    private bool isDraw = false;
    //カード効果実行中フラグ
    private bool isCardPlaying = false;

    //rayの長さ
    private const float _rayDistance = 10.0f;
    //ドロー間の時間間隔
    private const float _drawIntervalTime = 0.1f;
    //色を変えるデッキ枚数
    private const int _sufficientLine = 10;
    #endregion

    #region 初期化処理
    /// <summary>
    /// 初期化処理順番が関係ないもの
    /// </summary>
    private void Start()
    {
        _dammyHand = GameObject.FindObjectOfType<DammyHandScript>();
        _playerDeckDataScript = GameObject.FindObjectOfType<PlayerDeckDataScript>();
        //デッキデータ生成
        _playerDeckData = new List<CardDataSO>();
        _playerDeckDataBackUp = new List<CardDataSO>();
    }
    /// <summary>
    /// 初期化処理変わるもの、順番がおかしくならないようにする
    /// </summary>
    /// <param name="battleManager">使うBattleManagerScript</param>
    public void Init(BattleManagerScript battleManager)
    {
        //送られてきたBattleManagerScriptを格納する
        _battleManager = battleManager;
        //変数初期化
        _cardInstances = new List<CardScript>();
        // UI初期化
        _deckRemainingNum.color = Color.clear;
        //デッキ補充ボタンの演出
        _replenishButtonImage.DOFade(1.0f, 0.8f).SetLoops(-1, LoopType.Yoyo);
        _replenishButtonImage.gameObject.SetActive(false);
    }
    #endregion

    #region 更新処理
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //ドラッグ操作中の処理
        if (_draggingCard != null)
        {
            //更新処理
            UpdateDragging();
        }
    }
    private void OnGUI()
    {
        //手札整列フラグが立っているなら整列
        if (ishandSort == true)
        {
            //整列させる
            AlignHandCards();
            //整列完了
            ishandSort = false;
        }
    }
    #endregion

    #region 進行管理
    /// <summary>
    /// バトル開始処理
    /// </summary>
    public void OnBattleStarting()
    {
        //デッキデータ生成
        _playerDeckData = new List<CardDataSO>();
        _playerDeckDataBackUp = new List<CardDataSO>();
        //デッキデータ取得
        foreach (int cardData in _playerDeckDataScript.GetDeckCardList)
        {
            _playerDeckData.Add(_playerDeckDataScript.GetCardDatasBySerialNum[cardData]);
            _playerDeckDataBackUp.Add(_playerDeckDataScript.GetCardDatasBySerialNum[cardData]);
        }
        //デッキ残り枚数表示
        PrintPlayerDeckNum();
    }
    /// <summary>
	/// ターン開始時処理
	/// </summary>
	public void OnTurnStarting()
    {
        //手札所持枚数
        int nextHandCardsNum = DataScript._date.GetPlayerHandNum;
        //ドロー処理
        DrawCardsUntilNum(nextHandCardsNum);
        ishandSort = true;
        //デッキ補充ボタン表示・非表示
        if (_playerDeckData.Count > 0)
        {
            _replenishButtonImage.gameObject.SetActive(false);
        }
        else
        {
            _replenishButtonImage.gameObject.SetActive(true);
        }
        //カード実行ボタンを有効化
        _cardPlayButton.interactable = true;
        //敵のカード設置
        PlacingEnemyCards();
    }
    #endregion
    /// <summary>
	/// 敵側のカードをプレイボードに全て設置する
	/// </summary>
	private void PlacingEnemyCards()
    {
        //敵データ取得
        EnemyStatusSO enemyData = _battleManager.GetCharacterManager.GetEnemyDate;

        //このターンに使用する敵カードセットの番号を取得
        int enemyAttackOrderID = _battleManager.GetNowTurns % enemyData.GetUseCardDatas.Count;

        //敵カード設置処理
        //このターンの使用カードリスト
        EnemyStatusSO.EnemyUseCardData useCardDatasInThisTurn = enemyData.GetUseCardDatas[enemyAttackOrderID];
        for (int i = 0; i < PlayBoardManagerScript.PlayBoardCardNum; i++)
        {
            //各ゾーンに対する設置カードを取得
            CardDataSO cardData = null;
            switch (i)
            {
                case 0:
                    cardData = useCardDatasInThisTurn.placeCardData0; break;
                case 1:
                    cardData = useCardDatasInThisTurn.placeCardData1; break;
                case 2:
                    cardData = useCardDatasInThisTurn.placeCardData2; break;
                case 3:
                    cardData = useCardDatasInThisTurn.placeCardData3; break;
                case 4:
                    cardData = useCardDatasInThisTurn.placeCardData4; break;
            }
            //設置カードが無いなら次へ
            if (cardData == null)
            {
                continue;
            }


            //設置先ゾーンIDを取得
            CardZoneScript.ZoneType areaType = CardZoneScript.ZoneType.PlayBoard0 + i;
            //設置先Vector2座標を取得
            Vector2 targetPosition = _battleManager.GetPlayBoardManager.GetPlayZonePos(i);

            //オブジェクト作成
            GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
            //カード処理クラスを取得・リストに格納
            CardScript objCard = obj.GetComponent<CardScript>();
            _cardInstances.Add(objCard);

            //カード初期設定
            objCard.InitField(this, _battleManager.GetCharacterManager.GetEnemyPosition());
            objCard.PutToZone(areaType, targetPosition);
            //敵IDを指定
            objCard.SetInitialCardData(cardData, CardScript.CharaID_Enemy);
        }
    }

    /// <summary>
	/// カード効果発動ボタン押下時処理
	/// </summary>
	public void CardPlayButton()
    {
        //カードドラッグ中なら処理しない
        if (_draggingCard != null)
        {
            return;
        }
        //実行ボタンを一時的に無効化
        _cardPlayButton.interactable = false;
        //効果実行中
        isCardPlaying = true;

        //プレイボード上カードの配列を作成
        CardScript[] boardCards = new CardScript[PlayBoardManagerScript.PlayBoardCardNum];
        //プレイボード上のカードを取得して配列に格納
        foreach (CardScript card in _cardInstances)
        {
            // 配列内の指定の位置に該当カードを格納する
            if (card.GetNowZone >= CardZoneScript.ZoneType.PlayBoard0 &&
                card.GetNowZone <= CardZoneScript.ZoneType.PlayBoard4)
            {
                int arrayID = (int)card.GetNowZone - (int)CardZoneScript.ZoneType.PlayBoard0;
                boardCards[arrayID] = card;
            }
        }

        //各カードの効果を実行
        _battleManager.GetPlayBoardManager.BoardCardsPlay(boardCards);
        //トラッシュゾーン内カードの消去処理
        DeleteCardsOnTrashArea();
    }
    /// <summary>
	/// ターン終了時に実行される処理
	/// </summary>
	public void OnTurnEnd()
    {
        //効果実行処理中フラグ
        isCardPlaying = false;
    }
    #region プレイヤー側手札の処理
    /// <summary>
    /// デッキからカードを１枚引く
    /// </summary>
    /// <param name="handID">対象手札番号</param>
    private void DrawCard(int handID)
    {
        //デッキ残り枚数
        int deckCardNum = _playerDeckData.Count;
        // デッキの残りが無い場合はドローせず終了
        if (deckCardNum <= 0)
        {
            return;
        }

        //オブジェクト作成
        GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
        //カード処理クラスを取得・リストに格納
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        //デッキ内から引かれるカードをランダムに決定
        CardDataSO targetCard = _playerDeckData[Random.Range(0, deckCardNum)];
        //デッキリストから該当カードを削除
        _playerDeckData.Remove(targetCard);

        //カード初期設定
        objCard.InitField(this, _deckTransForm.position);
        objCard.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(handID));
        objCard.SetInitialCardData(targetCard, CardScript.CharaID_Player);

        //デッキ残り枚数表示
        PrintPlayerDeckNum();
        //音を鳴らす
        SEManagerScript.instance.PlaySE(SEManagerScript.SEName.DrawCard);
    }

    /// <summary>
	/// 手札が指定枚数になるまでカードを引く
	/// </summary>
	/// <param name="num">指定枚数</param>
	private void DrawCardsUntilNum(int num)
    {
        //現在の手札枚数を取得
        int nowHandNum = 0;
        foreach (CardScript card in _cardInstances)
        {
            //手札にあれば加算
            if (card.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                nowHandNum++;
            }
        }
        //新たに引くべき枚数を取得
        int drawNum = num - nowHandNum;
        //0以下だと終わる
        if (drawNum <= 0)
        {
            return;
        }
        //デッキ枚数がドロー枚数より少ない時デッキ枚数に合わせる
        if (_playerDeckData.Count < drawNum)
        {
            drawNum = _playerDeckData.Count;
        }

        //手札UIに枚数を指定
        _dammyHand.SetHandNum(nowHandNum + drawNum);

        //連続でカードを引く
        Sequence drawSequence = DOTween.Sequence();
        //手札補充中
        isDraw = true;
        for (int i = 0; i < drawNum; i++)
        {
            //１枚引く処理(Sequenceに追加)
            drawSequence.AppendCallback(() =>
            {
                DrawCard(nowHandNum);
                nowHandNum++;
            });
            //時間間隔をSequenceに追加
            drawSequence.AppendInterval(_drawIntervalTime);
        }
        drawSequence.OnComplete(() => isDraw = false);
    }
    /// <summary>
	/// 手札のカードを整列させる
	/// </summary>
	private void AlignHandCards()
    {
        //手札内番号
        int index = 0;
        //ダミー手札を整列
        _dammyHand.ApplyLayout();
        //各カードをダミー手札に合わせて移動
        foreach (CardScript card in _cardInstances)
        {
            //手札にあれば座標変更してindex増加
            if (card.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                card.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(index));
                index++;
            }
        }
    }
    /// <summary>
	/// 現在の手札の枚数を手札UI処理クラスに反映させて整列する
	/// </summary>
	private void CheckHandCardsNum()
    {
        //現在の手札枚数を取得
        int nowHandNum = 0;
        foreach (CardScript item in _cardInstances)
        {
            //カードが手札にあるか
            if (item.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                nowHandNum++;
            }
        }
        //ダミー手札に枚数を指定
        _dammyHand.SetHandNum(nowHandNum);
        //手札枚数に合わせて手札を整列
        ishandSort = true;
    }
    /// <summary>
	/// プレイヤー側デッキ残り枚数の表示を更新する
	/// </summary>
	private void PrintPlayerDeckNum()
    {
        //デッキ残り枚数
        int deckCardNum = _playerDeckData.Count;
        //枚数をText表示
        _deckRemainingNum.text = _playerDeckData.Count.ToString();

        //残り枚数に応じて表示色を変更
        if (deckCardNum >= _sufficientLine)
        {
            _deckRemainingNum.color = Color.white;
        }
        else if (deckCardNum > 0)
        {
            _deckRemainingNum.color = Color.yellow;
        }
        else
        {
            _deckRemainingNum.color = Color.clear;
        }
        //残り枚数が１枚でもあればデッキのカードアイコンを表示する
        if (deckCardNum > 0)
        {
            _deckIconObject.SetActive(true);
        }
        else
        {
            _deckIconObject.SetActive(false);
        }
    }

    /// <summary>
	/// デッキ補充ボタン押下時処理
	/// </summary>
	public void ReplenishButton()
    {
        //カード効果実行中なら処理しない
        if (_battleManager.GetPlayBoardManager.IsPlayingCards())
        {
            return;
        }
        //カードドラッグ中なら処理しない
        if (_draggingCard != null)
        {
            return;
        }
        //デッキ補充
        int backUpDeckNum = _playerDeckDataBackUp.Count;
        for (int i = 0; i < backUpDeckNum; i++)
        {
            _playerDeckData.Add(_playerDeckDataBackUp[i]);
        }
        //各カードの効果を実行
        CardPlayButton();
        //デッキ残り枚数表示
        PrintPlayerDeckNum();
        //HP半減
        _battleManager.GetCharacterManager.ChangeStatus_NowHP(CardScript.CharaID_Player, -_battleManager.GetCharacterManager.GetNowHP[CardScript.CharaID_Player] / 2);
        //ボタンを非表示
        _replenishButtonImage.gameObject.SetActive(false);
    }
    #endregion

    #region カードドラッグ処理
    /// <summary>
	/// カードのドラッグ操作を開始する
	/// </summary>
	/// <param name="dragCard">操作対象カード</param>
	public void StartDragging(CardScript dragCard)
    {
        //手札補充演出中なら終了
        if (isDraw == true)
        {
            return;
        }
        //カードの効果実行中なら終了
        if (isCardPlaying == true)
        {
            return;
        }
        //敵のカードではない
        if (dragCard.GetControllerCharaID == CardScript.CharaID_Enemy)
        {
            return;
        }
        //操作対象カードを記憶
        _draggingCard = dragCard;
        //最前面表示にする
        _draggingCard.transform.SetAsLastSibling();
    }
    /// <summary>
	/// ドラッグ操作更新処理
	/// </summary>
	private void UpdateDragging()
    {
        //クリック位置を取得
        Vector2 clickPos = Input.mousePosition;
        //クリック座標をスクリーン座標をCanvasのローカル座標に変換
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, //CanvasのRectTransform
            clickPos,             //変換元座標データ
            _mainCamera,          //メインカメラ
            out clickPos);        //変換先座標データ

        //座標を適用
        _draggingCard._rectTransform.anchoredPosition = clickPos;
    }
    /// <summary>
	/// カードのドラッグ操作を終了する
	/// </summary>
	public void EndDragging()
    {
        //カードを操作中でないなら終了
        if (_draggingCard == null)
        {
            return;
        }

        //オブジェクトのスクリーン座標を取得する
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(_mainCamera, _draggingCard.transform.position);
        //メインカメラから上記で取得した座標に向けてRayを飛ばす
        Ray ray = _mainCamera.ScreenPointToRay(pos);

        //ドラッグ先カードゾーン
        CardZoneScript targetZone = default;
        //ドラッグ先カード
        CardScript targetCard = default;
        //Rayが当たった全オブジェクトに対しての処理
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, _rayDistance))
        {
            //当たったオブジェクトが存在しないなら終了
            if (!hit.collider)
            {
                break;
            }
            //当たったオブジェクトがドラッグ中のカードと同一なら次へ
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj == _draggingCard.gameObject)
            {
                continue;
            }
            // オブジェクトがカードエリアなら取得する
            CardZoneScript hitArea = hitObj.GetComponent<CardZoneScript>();
            if (hitArea != null)
            {
                targetZone = hitArea;
                continue;
            }
            // オブジェクトがカードなら取得して次へ
            CardScript hitCard = hitObj.GetComponent<CardScript>();
            if (hitCard != null)
            {
                targetCard = hitCard;
                continue;
            }
        }

        //プレイボードにあるカードどうしが重なったら
        if (targetCard != null && (targetCard.GetNowZone >= CardZoneScript.ZoneType.PlayBoard0 && targetCard.GetNowZone <= CardZoneScript.ZoneType.PlayBoard4))
        {
            //合成処理
            CompositeCard(targetCard, _draggingCard);
            CheckHandCardsNum();
        }
        //カードと重ならずカードエリアと重なった場合
        else if (targetZone != null)
        {
            //設置処理
            _draggingCard.PutToZone(targetZone.GetZoneType, targetZone.GetComponent<RectTransform>().position);
            CheckHandCardsNum();
            //手札以外から手札への移動の場合、カードをリスト内で一番後ろにする
            if (_draggingCard.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                _cardInstances.Remove(_draggingCard);
                _cardInstances.Add(_draggingCard);
            }
        }
        //いずれとも重ならなかった場合
        else
        {
            //元の位置に戻す
            _draggingCard.BackToBasePos();
        }

        //初期化処理
        _draggingCard = null;
    }
    /// <summary>
	/// 2枚のカードを合成して1枚のカードにする
	/// </summary>
	/// <param name="baseCard">元となるカード)</param>
	/// <param name="consumeCard">合成の素材とするカード</param>
	private void CompositeCard(CardScript baseCard, CardScript consumeCard)
    {
        //カードがなかったら終わる
        if (baseCard == null || consumeCard == null)
        {
            return;
        }
        //合成不可か
        if (baseCard.CheckContainEffect(CardEffectDefineScript.CardEffect.PartsOnly) ||//カードに素材限定効果がある
            consumeCard.CheckContainEffect(CardEffectDefineScript.CardEffect.BaseOnly) ||//カードに本体限定効果がある
            baseCard.CheckContainEffect(CardEffectDefineScript.CardEffect.NoCompo) ||//カードに合成無効効果がある
            consumeCard.CheckContainEffect(CardEffectDefineScript.CardEffect.NoCompo))//カードに合成無効効果がある
        {
            //ドラッグ中カードを元の位置に戻して合成せず終了
            _draggingCard.BackToBasePos();
            return;
        }
        //強度の加算越えたらカード削除
        if (baseCard.SetForcePoint(baseCard.GetForcePoint + consumeCard.GetForcePoint))
        {
            DestroyCardObject(baseCard);
            DestroyCardObject(consumeCard);
            return;
        }

        //効果の合成
        foreach (CardEffectDefineScript effect in consumeCard.GetCardEffects)
        {
            baseCard.CompoCardEffect(effect);
        }
        //消費カードを削除
        DestroyCardObject(consumeCard);
    }
    #endregion

    #region カード消去系
    /// <summary>
    /// 指定のカードオブジェクトをフィールドから削除する
    /// </summary>
    /// <param name="targetCard">削除対象カード</param>
    public void DestroyCardObject(CardScript targetCard)
    {
        //リストから削除
        _cardInstances.Remove(targetCard);
        //オブジェクト削除
        Destroy(targetCard.gameObject);
    }

    /// <summary>
    /// 全てのカードオブジェクトをフィールドから削除する
    /// </summary>
    public void DestroyAllCards()
    {
        //オブジェクト削除
        foreach (CardScript item in _cardInstances)
        {
            Destroy(item.gameObject);
        }
        //リストを初期化
        _cardInstances.Clear();
    }

    /// <summary>
    /// トラッシュゾーン内にある全カードを画面外に退避・消去する
    /// </summary>
    private void DeleteCardsOnTrashArea()
    {
        //消去対象カードリスト
        List<CardScript> trashCard = new List<CardScript>();

        //トラッシュゾーン内カードリストを作成
        foreach (CardScript instance in _cardInstances)
        {
            //トラッシュゾーンにあるカードをリストに追加
            if (instance.GetNowZone == CardZoneScript.ZoneType.Trash)
            {
                trashCard.Add(instance);
            }
        }
        //消去対象カードリストをシーン内での表示順で並び替えする
        trashCard.Sort((a, b) => b.transform.GetSiblingIndex() - a.transform.GetSiblingIndex());
        foreach (CardScript card in trashCard)
        {
            DestroyCardObject(card);
        }

    }
    #endregion
    /// <summary>
	/// 戦闘中にデッキ内のカード枚数を追加する
	/// </summary>
	/// <param name="amount">追加量</param>
	public void AddDeckCardsNum(int amount)
    {
        //1枚ずつ追加
        for (int i = 0; i < amount; i++)
        {
            //追加するカードを戦闘開始時のデッキデータ内からランダムに決定
            CardDataSO targetCard = _playerDeckDataBackUp[Random.Range(0, _playerDeckDataBackUp.Count)];
            //現在のデッキデータに追加
            _playerDeckData.Add(targetCard);
        }

        //デッキ残り枚数表示
        PrintPlayerDeckNum();
    }
}
