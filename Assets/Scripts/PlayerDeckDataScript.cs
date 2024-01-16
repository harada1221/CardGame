//-----------------------------------------------------------------------
/* �v���C���[�f�b�L�̏������s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckDataScript : MonoBehaviour
{
    [SerializeField, Header("�v���C���[���J�[�h�S���̃��X�g")]
    private List<CardDataSO> _allPlayerCardsList = default;
    [SerializeField, Header("�v���C���[���f�b�L�̃��X�g")]
    private List<CardDataSO> _playerInitialDeck = default;
    [SerializeField,Header("�v���C���[���S�J�[�h�f�[�^�ƒʂ��ԍ�")]
    private Dictionary<int, CardDataSO> _cardDatasBySerialNum;
    //�v���C���[�f�b�L
    private List<int> _deckCardList = default;
    // ����������
    public void Init()
    {
        // �v���C���[���S�J�[�h�f�[�^�ƒʂ��ԍ���R�Â���
        _cardDatasBySerialNum = new Dictionary<int, CardDataSO>();
        foreach (CardDataSO item in _allPlayerCardsList)
        {
            _cardDatasBySerialNum.Add(item.GetSerialNum, item);
        }
    }
    /// <summary>
	/// �����f�b�L��ݒ肷��
	/// </summary>
	public void DataInitialize()
    {
        //�����f�b�L�𐶐�
        _deckCardList = new List<int>();
        foreach (CardDataSO cardData in _playerInitialDeck)
        {
            AddCardToDeck(cardData.GetSerialNum);
        }
    }
    /// <summary>
	/// �f�b�L�ɃJ�[�h���P���ǉ�����
	/// </summary>
	/// <param name="cardSerialNum">�J�[�h�̌ŗL�ԍ�</param>
	public void AddCardToDeck(int cardSerialNum)
    {
        _deckCardList.Add(cardSerialNum);
        _deckCardList.Sort();
    }
}
