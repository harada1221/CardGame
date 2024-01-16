//-----------------------------------------------------------------------
/* �g�p����J�[�h�̃f�[�^
 * 
 * ����ҁ@���c�@�q��
 * ������@1���P5��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CardData", menuName = " ScriptableObjects/CardData", order = 1)]
public class CardDataSO : ScriptableObject
{
    [SerializeField, Header("�J�[�h�ԍ�")]
    private int _serialNum = default;

    [SerializeField, Header("�J�[�h��")]
    private string _cardName = default;

    [SerializeField, Header("���ʃ��X�g")]
    private List<CardEffectDefineScript> _effectList;

    [SerializeField, Header("���x")]
    private int _force = default;

    public int GetSerialNum { get => _serialNum; }
    public string GetCardName { get => _cardName; }
    public List<CardEffectDefineScript> GetEffectList { get => _effectList; }
    public int GetForce { get => _force; }
}