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

    [SerializeField, Header("カード効果テキストPrefab")]
    private GameObject _cardEffectTextPrefab = default;

    [SerializeField, Header("カード背景Image")]
    private Image _cardBackImage = default;

    [SerializeField, Header("カード名Text")]
    private Text _cardNameText = default;

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
    private Sprite _cardBackSpritePlayer = default;
    [SerializeField, Header(" 敵側カード背景")]
    private Sprite _cardBackSpriteEnemy = default;
    [SerializeField,Header("ボーナスカード背景")] 
    private Sprite _cardBackSpriteBonus = null;

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
    /// <summary>
    /// キャラに合わせて背景を設定
    /// </summary>
    /// <param name="cardControllerChara">キャラのID</param>
    public void SetCardBackSprite(int cardControllerChara)
    {
        //プレイヤーの時
        if (cardControllerChara == CardScript.CharaID_Player)
        {
            _cardBackImage.sprite = _cardBackSpritePlayer;
        }
        //敵の時
        else if (cardControllerChara == CardScript.CharaID_Enemy)
        {
            _cardBackImage.sprite = _cardBackSpriteEnemy;
        }
        //ボーナスの時
        else if (cardControllerChara == CardScript.CharaID_Bonus)
        {
            _cardBackImage.sprite = _cardBackSpriteBonus;
        }
    }
    /// <summary>
    /// 名前を決定する
    /// </summary>
    /// <param name="name">カードネーム</param>
    public void SetCardNameText(string name)
    {
        _cardNameText.text = name;
    }
    /// <summary>
	/// カード効果Textを追加
	/// </summary>
	public void AddCardEffectText(CardEffectDefineScript effectData)
    {
        //オブジェクト作成
        GameObject obj = Instantiate(_cardEffectTextPrefab, _cardEffectTextParent);
        //TextUIとカード効果を紐づける
        _cardEffectTextDic.Add(effectData, obj.GetComponent<Text>());
        //Textの内容を更新
        ApplyCardEffectText(effectData);
    }
    /// <summary>
	/// カード効果Textの表示内容を更新
	/// </summary>
	public void ApplyCardEffectText(CardEffectDefineScript effectData)
    {
        //対象のTextUIを取得
        Text targetText = _cardEffectTextDic[effectData];
        //効果量を取得
        int effectValue = effectData.GetValue;
        string effectValueMes = "";

        //効果量を文字列化
        effectValueMes = effectValue.ToString();

        //UI表示
        targetText.text = string.Format(CardEffectDefineScript.Dic_EffectName[effectData.GetEffect], effectValueMes);
    }
   /// <summary>
   /// カード強度のText表示
   /// </summary>
   /// <param name="hardnessValue">カードの強度</param>
	public void SetForcePointText(int hardnessValue)
    {
        //0より大きいと表示
        if (hardnessValue > 0)
        {
            _cardForceText.text = hardnessValue.ToString();
            _cardForceBackImage.color = Color.white;
        }
        //非表示
        else
        {
            _cardForceText.text = "";
            _cardForceBackImage.color = Color.clear;
        }
    }
    /// <summary>
    /// カード枚数表示
    /// </summary>
    /// <param name="holdCard">所持枚数</param>
	public void SetAmountText(int holdCard)
    {
        //枚数を代入する
        _quantityText.text = "x" + holdCard;
        _quantityBackImage.color = Color.white;
    }
    /// <summary>
    /// カードの表示を決める
    /// </summary>
    /// <param name="mode">カードが壊れているか</param>
	public void SetHilightImage(bool mode)
    {
        //表示、非表示
        _hilightImageObject.SetActive(mode);
    }
    /// <summary>
	/// アイコンと効果の表示をリセットする
	/// </summary>
	public void ClearIconsAndEffects()
    {
        // アイコン初期化
        int length = _cardEffectTextParent.childCount;
        for (int i = 0; i < length; i++)
        {
            Destroy(_cardEffectTextParent.GetChild(i).gameObject);
        }
    }
}
