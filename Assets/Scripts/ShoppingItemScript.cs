//-----------------------------------------------------------------------
/* ショップ項目管理クラス
 * 
 * 制作者　原田　智大
 * 制作日　1月15日
 *--------------------------------------------------------------------------- 
*/
using UnityEngine;
using UnityEngine.UI;

public class ShoppingItemScript : MonoBehaviour
{
    // UIオブジェクト
    [SerializeField, Header("項目アイコン")]
    private Image _iconImage = default;
    [SerializeField, Header("項目名Text")]
    private Text _nameText = default;
    [SerializeField, Header("項目説明Text")]
    private Text _explainText = default;
    [SerializeField, Header("取得ボタン")]
    private Button _obtainButton = default;
    [SerializeField, Header("価格Text")]
    private Text _priceText = default;
    //ショップウィンドウクラス
    private ShoppingWindowScript _shoppingWindow = default;
    //この商品のパックデータ
    private CardPackSO _cardpackData = default;
    //項目の取得に必要なGOLD量
    private int _price = default;

    public CardPackSO GetCardPack { get => _cardpackData; }
    public int GetPrice { get => _price; }


    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="shoppingWindow">ShoppingWindowScriptクラス</param>
    /// <param name="cardPackData">CardPackSOデータ</param>
    public void Init(ShoppingWindowScript shoppingWindow, CardPackSO cardPackData)
    {
        //参照取得
        _shoppingWindow = shoppingWindow;
        _cardpackData = cardPackData;
        //UI初期化
        _nameText.text = _cardpackData.GetName + "(" + _cardpackData.GetCardNum + "枚)";
        _explainText.text = _cardpackData.GetExpLain;
        //パックアイコンImage
        _iconImage.sprite = _cardpackData.GetIcon;
        //必要Gold量
        _price = _cardpackData.GetPrice;
        _priceText.text = _price.ToString("#,0");
        CheckPrice();
    }

    /// <summary>
    /// 項目の取得に必要なEXPが足りているかを判定
    /// </summary>
    public void CheckPrice()
    {
        //ゴールドが足りているか
        if (DataScript._date.GetPlayerGold >= _price)
        {
            _obtainButton.interactable = true;
        }
        else
        {
            _obtainButton.interactable = false;
        }
    }
    /// <summary>
    /// 購入ボタン押下時処理
    /// </summary>
    public void ObtainButton()
    {
        _shoppingWindow.BuyItem(this);
    }
}
