//-----------------------------------------------------------------------
/* フィールドの管理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
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
    [SerializeField, Header("プレイヤーデッキのスクリプト")]
    private PlayerDeckDataScript _playerDeckDataScript = default;
    [SerializeField, Header("デッキアイコンのオブジェクト")]
    private GameObject _deckIconObject = default;
    [SerializeField, Header("デッキの残り枚数表示テキスト")]
    private Text _deckRemainingNum = default;
    [SerializeField, Header("プレイボタン")]
    private Button _cardPlayButton = default;

    //カード関連
    [SerializeField, Header("カードのプレハブ")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("生成するカードオブジェクトの親Transform")]
    private Transform _cardsParentTransForm = default;
    [SerializeField, Header("デッキオブジェクトTransform")]
    private Transform _deckTransForm = default;

    //戦闘画面マネージャー
    private BattleManagerScript _battleManager = default;
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

    //rayの長さ
    private const float _rayDistance = 10.0f;
    // ドロー間の時間間隔
    private const float _drawIntervalTime = 0.1f;
    //色を変えるデッキ枚数
    private const int _sufficientLine = 10;
    #endregion

    #region 初期化処理
    /// <summary>
    /// 初期化処理変わらないもの
    /// </summary>
    private void Start()
    {
        _dammyHand = GameObject.FindObjectOfType<DammyHandScript>();
    }
    /// <summary>
    /// 初期化処理変わるもの、順番がおかしくならないようにする
    /// </summary>
    /// <param name="battleManager">使うBattleManagerScript</param>
    public void InBattleManager(BattleManagerScript battleManager)
    {
        //送られてきたBattleManagerScriptを格納する
        _battleManager = battleManager;
        //変数初期化
        _cardInstances = new List<CardScript>();
        // UI初期化
        _deckRemainingNum.color = Color.clear;

        // デバッグ用ドロー処理(遅延実行)
        DOVirtual.DelayedCall(
            1.0f, // 1.0秒遅延
            () =>
            {
                OnBattleStarting();
                OnTurnStarting();
            }
        );
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
        // 手札整列フラグが立っているなら整列
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
        //カードのドロー枚数決定
        int nextHandCardsNum = 5;// 手札枚数
        //ドロー処理
        DrawCardsUntilNum(nextHandCardsNum);
        ishandSort = true;
        //カード実行ボタンを有効化
        _cardPlayButton.interactable = true;
    }
    #endregion
    /// <summary>
	/// カード効果発動ボタン押下時処理
	/// </summary>
	public void CardPlayButton()
    {
        //カードドラッグ中なら処理しない
        if (_draggingCard != null)
        {
            Debug.Log("ないよ");
            return;
        }


        //実行ボタンを一時的に無効化
        _cardPlayButton.interactable = false;

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
        objCard.Init(this, _deckTransForm.position);
        objCard.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(handID));
        objCard.SetInitialCardData(targetCard, CardScript.CharaID_Player);

        //デッキ残り枚数表示
        PrintPlayerDeckNum();
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
            // 時間間隔をSequenceに追加
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
        // ダミー手札を整列
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

        ////プレイボードにあるカードどうしが重なったら
        //if (targetCard != null && (targetCard.GetNowZone >= CardZoneScript.ZoneType.PlayBoard0 && targetCard.GetNowZone <= CardZoneScript.ZoneType.PlayBoard4))
        //{
        //    //合成処理(未実装)
        //}
        //カードと重ならずカードエリアと重なった場合
        if (targetZone != null)
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
    #endregion
}
