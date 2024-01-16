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
    //戦闘画面マネージャー
    private BattleManagerScript _battleManager = default;
    //ダミー手札制御クラス
    private DammyHandScript _dammyHand = default;

    //カード関連
    [SerializeField, Header("カードのプレハブ")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("生成するカードオブジェクトの親Transform")]
    private Transform _cardsParentTransForm = default;
    [SerializeField, Header("デッキオブジェクトTransform")]
    private Transform _deckTransForm = default;
    [SerializeField,Header("カードデータ")]
    private CardDataSO _testCardData = default;

    //ドラッグ操作中カード
    private CardScript _draggingCard = default;
    //生成したプレイヤー操作カードリスト
    private List<CardScript> _cardInstances = default;
    //手札整列フラグ
    private bool ishandSort = false;
    //手札補充中フラグ
    private bool isDraw = false;

    //rayの長さ
    const float _rayDistance = 10.0f;
    // ドロー間の時間間隔
    const float DrawIntervalTime = 0.1f;
    #endregion

    #region 初期化処理
    private void Start()
    {
        _dammyHand = GameObject.FindObjectOfType<DammyHandScript>();
    }
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="battleManager">使うBattleManagerScript</param>
    public void InBattleManager(BattleManagerScript battleManager)
    {
        //送られてきたBattleManagerScriptを格納する
        _battleManager = battleManager;
        //変数初期化
        _cardInstances = new List<CardScript>();

        // デバッグ用ドロー処理(遅延実行)
        DOVirtual.DelayedCall(
            1.0f, // 1.0秒遅延
            () =>
            {
                DrawCardsUntilNum(5);
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

    #region プレイヤー側手札の処理
    /// <summary>
    /// デッキからカードを１枚引き手札に加える
    /// </summary>
    /// <param name="handID">対象手札番号</param>
    private void DrawCard(int handID)
    {
        //オブジェクト作成
        GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
        //カード処理クラスを取得・リストに格納
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        //カード初期設定
        objCard.Init(this, _deckTransForm.position);
        objCard.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(handID));
        objCard.SetInitialCardData(_testCardData, CardScript.CharaID_Player);
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
            drawSequence.AppendInterval(DrawIntervalTime);
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
        // 現在の手札枚数を取得
        int nowHandNum = 0;
        foreach (CardScript item in _cardInstances)
        {
            //カードが手札にあるか
            if (item.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                nowHandNum++;
            }
        }
        // ダミー手札に枚数を指定
        _dammyHand.SetHandNum(nowHandNum);
        // 手札枚数に合わせて手札を整列
        ishandSort = true;
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
