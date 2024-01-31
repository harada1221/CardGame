using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�[�h�p�b�N�f�[�^��`�N���X
/// </summary>
[CreateAssetMenu(fileName = "CardPack", menuName = " ScriptableObjects/CardPack")]
public class CardPackSO : ScriptableObject
{
    [SerializeField, Header("���O")]
    private string _name = default;
    [SerializeField, Header("����")]
    private string _explain = default;
    [SerializeField, Header("�p�b�N�A�C�R���摜")]
    private Sprite _packIcon = default;
    [SerializeField, Header("�l�i")]
    private int _price = default;
    [SerializeField, Header("�J�[�h���薇��")]
    private int _cardNum = default;
    [SerializeField, Header("�o���J�[�h���X�g")]
    private List<CardDataSO> _includedCards = default;

    public string GetName { get => _name; }
    public string GetExpLain { get => _explain; }
    public Sprite GetIcon { get => _packIcon; }
    public int GetPrice { get => _price; }
    public int GetCardNum { get => _cardNum; }
    public List<CardDataSO> GetCards { get => _includedCards; }
}