//-----------------------------------------------------------------------
/* パック開封を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenPackEffecScript : MonoBehaviour
{
    [SerializeField, Header("UIオブジェクト")]
    private CanvasGroup _canvasGroup = default;
    // カード関連参照
    [SerializeField, Header(" カードプレハブ")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header(" カードオブジェクトの親Transform")]
    private Transform _cardsParent = default;

    //カード表示演出Sequence
    private Sequence _effectSequence = default;
    //ショップウィンドウクラス
    private ShoppingWindowScript _shoppingWindow = default;

    // 初期化関数(ShoppingWindow.csから呼出)
    public void Init(ShoppingWindowScript shoppingWindow)
    {
        //参照取得
        _shoppingWindow = shoppingWindow;
        //UI初期化
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// カード演出
    /// </summary>
    /// <param name="cardDatas">カードデータ</param>
    public void StartEffect(List<CardDataSO> cardDatas)
    {
        //TWEEN設定
        _effectSequence = DOTween.Sequence();

        //背景画像表示演出
        _effectSequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        //カードを順番に表示する演出
        foreach (CardDataSO cardData in cardDatas)
        {
            //オブジェクト作成
            GameObject obj = Instantiate(_cardPrefab, _cardsParent);
            CardScript objCard = obj.GetComponent<CardScript>();

            //カード初期設定
            objCard.InitField(null, Vector2.zero);
            objCard.SetInitialCardData(cardData, CardScript.CharaID_Player);
        }
    }

    /// <summary>
    /// 画面を非表示にする
    /// </summary>
    public void ClosePanel()
    {
        //UI非表示
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        //演出終了
        if (_effectSequence != null)
        {
            _effectSequence.Kill();
        }
        //生成されたカードを消去
        for (int i = 0; i < _cardsParent.childCount; i++)
        {
            Transform cardObject = _cardsParent.GetChild(i);
            Destroy(cardObject.gameObject);
        }
    }
}
