//-----------------------------------------------------------------------
/* プレイヤーデッキの処理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDeckDataScript : MonoBehaviour
{
    [SerializeField, Header("プレイヤー側カード全部のリスト")]
    private  List<CardDataSO> _allPlayerCardsList = default;
    [SerializeField, Header("プレイヤー側デッキのリスト")]
    private List<CardDataSO> _playerInitialDeck = default;
    [SerializeField,Header("プレイヤー側全カードデータと通し番号")]
    public static Dictionary<int, CardDataSO> _cardDatasBySerialNum = default;
    //プレイヤーデッキカードのシリアル値
    public static List<int> _deckCardList = default;
    //現在の保管中カードデータ(通し番号で管理)
    public static Dictionary<int, int> _storageCardList = default;

    public List<int> GetDeckCardList { get => _deckCardList; }
    public Dictionary<int, CardDataSO> GetCardDatasBySerialNum { get => _cardDatasBySerialNum; }
    public Dictionary<int, int> GetStorageCardList { get => _storageCardList; }
    public List<CardDataSO> GetAllPlayerCardsList { get => _allPlayerCardsList; }

    // 初期化処理
    public void Init()
    {
        //プレイヤー側全カードデータと通し番号を紐づける
        _cardDatasBySerialNum = new Dictionary<int, CardDataSO>();
        foreach (CardDataSO item in _allPlayerCardsList)
        {
            _cardDatasBySerialNum.Add(item.GetSerialNum, item);
        }
        //保管中カード情報クリア
        _storageCardList = new Dictionary<int, int>();
        foreach (var playerCard in _allPlayerCardsList)
        {
            _storageCardList.Add(playerCard.GetSerialNum, 0);
        }

        //全部のカードを１枚ずつ保管中リストに加える
        //DictionaryのKeyのリストを作成
        List<int> keys = _storageCardList.Keys.ToList();
        for (int i = 0; i < _storageCardList.Count; i++)
        {
            //Dictionaryの全てのKeyに対応するValueをそれぞれ1ずつ加算
            _storageCardList[keys[i]] += 1;
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
	public static void AddCardToDeck(int cardSerialNum)
    {
        _deckCardList.Add(cardSerialNum);
        _deckCardList.Sort();
    }
    /// <summary>
	/// デッキからカードを１枚削除する
	/// </summary>
	/// <param name="cardSerialNum">カードの固有番号</param>
	public static void RemoveCardFromDeck(int cardSerialNum)
    {
        _deckCardList.Remove(cardSerialNum);
    }

    /// <summary>
    /// 保管中のカード数量を変更する
    /// </summary>
    /// <param name="cardSerialNum">カードの通し番号</param>
    /// <param name="amount">変化量</param>
    public static void ChangeStorageCardNum(int cardSerialNum, int amount)
    {
        _storageCardList[cardSerialNum] += amount;
    }
}
