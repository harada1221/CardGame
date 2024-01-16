//-----------------------------------------------------------------------
/* 使用するカードのデータ
 * 
 * 制作者　原田　智大
 * 制作日　1月１5日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CardData", menuName = " ScriptableObjects/CardData", order = 1)]
public class CardDataSO : ScriptableObject
{
    [SerializeField, Header("カード番号")]
    private int _serialNum = default;

    [SerializeField, Header("カード名")]
    private string _cardName = default;

    [SerializeField, Header("効果リスト")]
    private List<CardEffectDefineScript> _effectList;

    [SerializeField, Header("強度")]
    private int _force = default;

    public int GetSerialNum { get => _serialNum; }
    public string GetCardName { get => _cardName; }
    public List<CardEffectDefineScript> GetEffectList { get => _effectList; }
    public int GetForce { get => _force; }
}