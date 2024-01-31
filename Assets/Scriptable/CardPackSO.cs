using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カードパックデータ定義クラス
/// </summary>
[CreateAssetMenu(fileName = "CardPack", menuName = " ScriptableObjects/CardPack")]
public class CardPackSO : ScriptableObject
{
    [SerializeField, Header("名前")]
    private string _name = default;
    [SerializeField, Header("説明")]
    private string _explain = default;
    [SerializeField, Header("パックアイコン画像")]
    private Sprite _packIcon = default;
    [SerializeField, Header("値段")]
    private int _price = default;
    [SerializeField, Header("カード入手枚数")]
    private int _cardNum = default;
    [SerializeField, Header("出現カードリスト")]
    private List<CardDataSO> _includedCards = default;

    public string GetName { get => _name; }
    public string GetExpLain { get => _explain; }
    public Sprite GetIcon { get => _packIcon; }
    public int GetPrice { get => _price; }
    public int GetCardNum { get => _cardNum; }
    public List<CardDataSO> GetCards { get => _includedCards; }
}