using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingItemScript : MonoBehaviour
{
    // UIオブジェクト
    [SerializeField, Header("項目アイコン")]
    private Image iconImage = default;
    [SerializeField, Header("項目名Text")]
    private Text nameText = default;
    [SerializeField, Header("項目説明Text")]
    private Text explainText = default;
    [SerializeField, Header("取得ボタン")]
    private Button obtainButton = default;
    [SerializeField, Header("価格Text")]
    private Text priceText = default;
    //ショップウィンドウクラス
    private ShoppingWindowScript shoppingWindow = default;

    /// <summary>
    /// 購入ボタン押下時処理
    /// </summary>
    public void ObtainButton()
    {

    }
}
