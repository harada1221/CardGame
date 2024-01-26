//-----------------------------------------------------------------------
/* �v���C���[�f�b�L�̏������s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerDeckDataScript : MonoBehaviour
{
    [SerializeField, Header("�v���C���[���J�[�h�S���̃��X�g")]
    private  List<CardDataSO> _allPlayerCardsList = default;
    [SerializeField, Header("�v���C���[���f�b�L�̃��X�g")]
    private List<CardDataSO> _playerInitialDeck = default;
    [SerializeField,Header("�v���C���[���S�J�[�h�f�[�^�ƒʂ��ԍ�")]
    public static Dictionary<int, CardDataSO> _cardDatasBySerialNum = default;
    //�v���C���[�f�b�L�J�[�h�̃V���A���l
    public static List<int> _deckCardList = default;
    //���݂̕ۊǒ��J�[�h�f�[�^(�ʂ��ԍ��ŊǗ�)
    public static Dictionary<int, int> _storageCardList = default;

    public List<int> GetDeckCardList { get => _deckCardList; }
    public Dictionary<int, CardDataSO> GetCardDatasBySerialNum { get => _cardDatasBySerialNum; }
    public Dictionary<int, int> GetStorageCardList { get => _storageCardList; }
    public List<CardDataSO> GetAllPlayerCardsList { get => _allPlayerCardsList; }

    // ����������
    public void Init()
    {
        //�v���C���[���S�J�[�h�f�[�^�ƒʂ��ԍ���R�Â���
        _cardDatasBySerialNum = new Dictionary<int, CardDataSO>();
        foreach (CardDataSO item in _allPlayerCardsList)
        {
            _cardDatasBySerialNum.Add(item.GetSerialNum, item);
        }
        //�ۊǒ��J�[�h���N���A
        _storageCardList = new Dictionary<int, int>();
        foreach (var playerCard in _allPlayerCardsList)
        {
            _storageCardList.Add(playerCard.GetSerialNum, 0);
        }

        //�S���̃J�[�h���P�����ۊǒ����X�g�ɉ�����
        //Dictionary��Key�̃��X�g���쐬
        List<int> keys = _storageCardList.Keys.ToList();
        for (int i = 0; i < _storageCardList.Count; i++)
        {
            //Dictionary�̑S�Ă�Key�ɑΉ�����Value�����ꂼ��1�����Z
            _storageCardList[keys[i]] += 1;
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
	public static void AddCardToDeck(int cardSerialNum)
    {
        _deckCardList.Add(cardSerialNum);
        _deckCardList.Sort();
    }
    /// <summary>
	/// �f�b�L����J�[�h���P���폜����
	/// </summary>
	/// <param name="cardSerialNum">�J�[�h�̌ŗL�ԍ�</param>
	public static void RemoveCardFromDeck(int cardSerialNum)
    {
        _deckCardList.Remove(cardSerialNum);
    }

    /// <summary>
    /// �ۊǒ��̃J�[�h���ʂ�ύX����
    /// </summary>
    /// <param name="cardSerialNum">�J�[�h�̒ʂ��ԍ�</param>
    /// <param name="amount">�ω���</param>
    public static void ChangeStorageCardNum(int cardSerialNum, int amount)
    {
        _storageCardList[cardSerialNum] += amount;
    }
}
