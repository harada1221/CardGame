//-----------------------------------------------------------------------
/* ショップウィンドウ管理クラス
 * 
 * 制作者　原田　智大
 * 制作日　1月15日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShoppingWindowScript : MonoBehaviour
{
    [SerializeField, Header("ScrollView内のScrollbar")]
    private Scrollbar _scrollbar = default;
    [SerializeField, Header("所持Gold量Text")]
    private Text _goldText = default;
    [SerializeField, Header("商品のカードパックのリスト")]
    private List<CardPackSO> _cardPackSOs = default;
    //商品項目関連
    [SerializeField, Header("商品項目UIプレハブ")]
    private GameObject _shoppingItemPrefab = default;
    [SerializeField, Header("商品項目UIの親Transform")]
    private Transform _shoppingItemsParent = default;
    [SerializeField, Header("パック開封演出処理クラス")]
    private OpenPackEffecScript _openPackEffect = default;
    //タイトル管理クラス
    private TitleManagerScript _titleManager = default;
    //ウィンドウのRectTransform
    private RectTransform _windowRectTransform = default;
    //生成済み商品項目リスト
    private List<ShoppingItemScript> _shoppingItemInstances = default;
    //ウィンドウ表示・非表示Tween;
    private Tween _windowTween = default;
    //ウィンドウ表示アニメーション時間
    private const float _WindowAnimTime = 0.3f;
    //スクロールバー初期化フラグ
    private bool _reservResetScrollBar = default;
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="titleManager">TitleManagerScript</param>
    public void Init(TitleManagerScript titleManager)
    {
        //参照取得
        _titleManager = titleManager;
        _windowRectTransform = GetComponent<RectTransform>();

        //変数初期化
        _shoppingItemInstances = new List<ShoppingItemScript>();
        //商品項目作成
        for (int i = 0; i < _cardPackSOs.Count; i++)
        {
            //項目作成
            GameObject obj = Instantiate(_shoppingItemPrefab, _shoppingItemsParent);
            ShoppingItemScript shoppingItem = obj.GetComponent<ShoppingItemScript>();
            _shoppingItemInstances.Add(shoppingItem);
            //項目設定
            shoppingItem.Init(this, _cardPackSOs[i]);
        }

        //所持Gold量表示
        _goldText.text = DataScript._date.GetPlayerGold.ToString("#,0");

        //ウィンドウ非表示
        _windowRectTransform.transform.localScale = Vector2.zero;
        _windowRectTransform.gameObject.SetActive(true);
        _openPackEffect.Init(this);
    }

    //OnGUI (フレームごとに実行)
    private void OnGUI()
    {
        //画面スクロールを初期値に戻す処理
        if (_reservResetScrollBar)
        {
            _scrollbar.value = 1.0f;
            _reservResetScrollBar = false;
        }
    }

    /// <summary>
    /// ウィンドウを表示する
    /// </summary>
    public void OpenWindow()
    {
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }
        //ウィンドウ表示Tween
        _windowTween = _windowRectTransform.DOScale(1.0f, _WindowAnimTime).SetEase(Ease.OutBack);
        //ウィンドウ背景パネルを有効化
        _titleManager.SetWindowBackPanelActive(true);
        //SE再生
        SEManagerScript.instance.PlaySE(SEManagerScript.SEName.DecideA);
        //画面スクロールを初期値に戻す
        _reservResetScrollBar = true;
    }
    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }
        //SE再生
        SEManagerScript.instance.PlaySE(SEManagerScript.SEName.DecideA);
        //ウィンドウ非表示Tween
        _windowTween = _windowRectTransform.DOScale(0.0f, _WindowAnimTime).SetEase(Ease.InBack);
        //ウィンドウ背景パネルを無効化
        _titleManager.SetWindowBackPanelActive(false);
    }

    /// <summary>
    /// 該当の商品を購入する
    /// </summary>
    public void BuyItem(ShoppingItemScript targetItem)
    {
        //ランダムに指定枚数のカードを入手
        //パックデータ
        CardPackSO targetPack = targetItem.GetCardPack;
        //入手した全カードのリスト
        List<CardDataSO> obtainedCards = new List<CardDataSO>();
        for (int i = 0; i < targetPack.GetCardNum; i++)
        {
            //入手カードを決定
            CardDataSO cardData = targetPack.GetCards[Random.Range(0, targetPack.GetCards.Count)];
            obtainedCards.Add(cardData);
            //入手処理
            PlayerDeckDataScript.ChangeStorageCardNum(cardData.GetSerialNum, 1);
        }
        //カード入手演出再生
        _openPackEffect.StartEffect(obtainedCards);
        //Gold量減少
        DataScript._date.ChangePlayerGold(-targetItem.GetPrice);
        OnChangePlayerGold();
    }

    /// <summary>
    /// プレイヤーのGold量が変更された時に呼び出される
    /// </summary>
    public void OnChangePlayerGold()
    {
        //所持GOLD量表示
        _goldText.text = DataScript._date.GetPlayerGold.ToString("#,0");
        //各項目の取得ボタン押下可否設定
        foreach (ShoppingItemScript item in _shoppingItemInstances)
        {
            item.CheckPrice();
        }
    }
}
