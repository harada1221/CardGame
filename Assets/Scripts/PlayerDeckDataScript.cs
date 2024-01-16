//-----------------------------------------------------------------------
/* プレイヤーデッキの処理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckDataScript : MonoBehaviour
{
    [SerializeField, Header("プレイヤー側カード全部のリスト")]
    private List<CardDataSO> _allPlayerCardsList = default;
    [SerializeField, Header("プレイヤー側デッキのリスト")]
    private List<CardDataSO> _playerInitialDeck = default;
    [SerializeField,Header("プレイヤー側全カードデータと通し番号")]
    private Dictionary<int, CardDataSO> _cardDatasBySerialNum;
    //プレイヤーデッキ
    private List<int> _deckCardList = default;
    // 初期化処理
    public void Init()
    {
        // プレイヤー側全カードデータと通し番号を紐づける
        _cardDatasBySerialNum = new Dictionary<int, CardDataSO>();
        foreach (CardDataSO item in _allPlayerCardsList)
        {
            _cardDatasBySerialNum.Add(item.GetSerialNum, item);
        }
    }
    /// <summary>
	/// 初期デッキを設定する
	/// </summary>
	public void DataInitialize()
    {
        //初期デッキを生成
        _deckCardList = new List<int>();
        foreach (CardDataSO cardData in _playerInitialDeck)
        {
            AddCardToDeck(cardData.GetSerialNum);
        }
    }
    /// <summary>
	/// デッキにカードを１枚追加する
	/// </summary>
	/// <param name="cardSerialNum">カードの固有番号</param>
	public void AddCardToDeck(int cardSerialNum)
    {
        _deckCardList.Add(cardSerialNum);
        _deckCardList.Sort();
    }
}
