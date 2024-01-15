//-----------------------------------------------------------------------
/* カードのUIを変更するクラス
 * 
 * 制作者　原田　智大
 * 制作日　1月１5日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIScript : MonoBehaviour
{
    // このカードの処理クラス
    private CardScript _card = default;

    [SerializeField, Header("カード効果テキスト")]
    private GameObject _cardEffectText = default;
    [SerializeField, Header("カード背景Image")]
    private Image _cardBackImage = default;
    [SerializeField, Header("カード名Text")]
    private Text _cardNameText = default;
    [SerializeField, Header("カードアイコンリストの親")]
    private Transform _cardIconParent = default;
    [SerializeField, Header("カード効果テキストリストの親")]
    private Transform _cardEffectTextParent = default;
    [SerializeField, Header("カード強度Text")]
    private Text _cardForceText = default;
    [SerializeField, Header("カード強度Textの背景Image")]
    private Image _cardForceBackImage = default;      
    [SerializeField, Header("カード数量Text")]
    private Text _quantityText = default;              
    [SerializeField, Header("カード数量Textの背景Image")]
    private Image _quantityBackImage = default;      
    [SerializeField, Header("選択時強調表示画像Object")]
    private GameObject _hilightImageObject = default;  
    [SerializeField, Header("プレイヤー側カード背景")]
    private Image _cardBackSprite_Player = default;  
    [SerializeField, Header(" 敵側カード背景")]
    private Image _cardBackSprite_Enemy = default; 

    //作成した効果Textリスト
    private Dictionary<CardEffectDefineScript, Text> _cardEffectTextDic;
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="_card">対応するカード</param>
    public void Init(CardScript card)
    {
        //使用するカード
        _card = card;
        //初期生成
        _cardEffectTextDic = new Dictionary<CardEffectDefineScript, Text>();
        // UI初期化
        _quantityText.text = "";
        _quantityBackImage.color = Color.clear;
    }
}
