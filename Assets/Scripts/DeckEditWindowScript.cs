using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditWindowScript : MonoBehaviour
{
    [SerializeField, Header("�J�[�h�v���n�u")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("�f�b�L��,�E��f�b�L����������")]
    private Text _deckLogoText = default;
    [SerializeField, Header("�f�b�L����O��")]
    private Button _backToStorageButton = default;
    [SerializeField, Header("�f�b�L�ɓ����")]
    private Button _intoDeckButton = default;
    [SerializeField, Header("�ۊǒ��J�[�h�ꗗ�I�u�W�F�N�gTransform")]
    private Transform _storageCardsAreaTransform = default;
    //�����ςݕۊǒ��J�[�h���X�g
    private List<GameObject> _storageCardObjects = default;
    //�����J�[�h
    private Dictionary<GameObject, CardScript> _dicStorageCardObjectByCard = default;
    [SerializeField, Header("�f�b�L�I�u�W�F�N�gTransform")]
    private Transform _deckAreaTransform = default;
    //�����ς݃f�b�L���J�[�h���X�g
    private List<GameObject> _deckCardObjects = default;
    private Dictionary<GameObject, CardScript> _dic_DeckCardObjectByCard = default;
    ////�^�C�g���Ǘ��N���X
    //private TitleManagerScript _titleManager = default;
    //�I�𒆃J�[�h���
    private CardScript _selectCard = default;
    //�萔��`
    //�ŏ��f�b�L����
    public const int MinDeckNum = 1;
    //�ő�f�b�L����
    public const int MaxDeckNum = 60;
    // �������֐�(TitleManager.cs����ďo)
    public void Init(TitleManagerScript _titleManager)
    {
        //�ϐ�������
        _deckCardObjects = new List<GameObject>();
        _dic_DeckCardObjectByCard = new Dictionary<GameObject, CardScript>();
        _storageCardObjects = new List<GameObject>();
        _dicStorageCardObjectByCard = new Dictionary<GameObject, CardScript>();

        //UI��\��
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �E�B���h�E��\������
    /// </summary>
    public void OpenWindow()
    {
        //���ɐ�������Ă���J�[�h�I�u�W�F�N�g���폜
        if (_deckCardObjects != null)
        {
            foreach (GameObject cardObject in _deckCardObjects)
            {
                Destroy(cardObject.gameObject);
            }
            _deckCardObjects.Clear();
        }
        if (_storageCardObjects != null)
        {
            foreach (GameObject cardObject in _storageCardObjects)
            {
                Destroy(cardObject);
            }
            _storageCardObjects.Clear();
        }

        //UI������
        gameObject.SetActive(true);
        DeselectCard();
        RefreshDeckNumToUI();

        //�ۊǒ��J�[�h���f
        foreach (KeyValuePair<int, int> storageCard in PlayerDeckDataScript._storageCardList)
        {
            if (storageCard.Value > 0)
            {
                CreateStorageCardObject(PlayerDeckDataScript._cardDatasBySerialNum[storageCard.Key]);
            }
        }
        //�f�b�L���f
        foreach (int deckCard in PlayerDeckDataScript._deckCardList)
        {
            CreateDeckCardObject(PlayerDeckDataScript._cardDatasBySerialNum[deckCard]);
        }
        //�J�[�h���X�g����
        AlignDeckList();
        AlignStorageList();
    }
    /// <summary>
    /// �E�B���h�E���\���ɂ���
    /// </summary>
    public void CloseWindow()
    {
        //�I������
        DeselectCard();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// �ۊǒ��J�[�h�I�u�W�F�N�g���쐬����
    /// </summary>
    private void CreateStorageCardObject(CardDataSO cardData)
    {
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardPrefab, _storageCardsAreaTransform);
        //�J�[�h�����N���X���擾�E���X�g�Ɋi�[
        CardScript objCard = obj.GetComponent<CardScript>();
        _storageCardObjects.Add(obj);
        _dicStorageCardObjectByCard.Add(obj, objCard);

        //�J�[�h�����ݒ�
        objCard.InitDeck(this, false);
        objCard.SetInitialCardData(cardData, CardScript.CharaID_Player);
        objCard.ShowCardAmountInStorage();
    }
    /// <summary>
    /// �f�b�L���J�[�h�I�u�W�F�N�g���쐬����
    /// </summary>
    private void CreateDeckCardObject(CardDataSO cardData)
    {
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardPrefab, _deckAreaTransform);
        //�J�[�h�����N���X���擾�E���X�g�Ɋi�[
        CardScript objCard = obj.GetComponent<CardScript>();
        _deckCardObjects.Add(obj);
        _dic_DeckCardObjectByCard.Add(obj, objCard);

        // �J�[�h�����ݒ�
        objCard.InitDeck(this, true);
        objCard.SetInitialCardData(cardData, CardScript.CharaID_Player);

        //�f�b�L���J�[�h�I�u�W�F�N�g���񏈗�
        int alignToIndex = 99;
        int cardObjectListLength = _deckAreaTransform.childCount;
        for (int i = 0; i < cardObjectListLength; i++)
        {
            Transform cardObject = _deckAreaTransform.GetChild(i);
            if (objCard.GetCardData.GetSerialNum <= cardObject.GetComponent<CardScript>().GetCardData.GetSerialNum)
            {
                alignToIndex = i;
                break;
            }
        }
        obj.transform.SetSiblingIndex(alignToIndex);
    }

    /// <summary>
    /// �f�b�L�J�[�h�I�u�W�F�N�g�̕��т�ύX����
    /// </summary>
    public void AlignDeckList()
    {
        //�f�b�L�J�[�h�I�u�W�F�N�g�̃��X�g�𐮗񂳂���
        _deckCardObjects.Sort((a, b) => _dic_DeckCardObjectByCard[a].GetCardData.GetSerialNum - _dic_DeckCardObjectByCard[b].GetCardData.GetSerialNum);
        //���ёւ������X�g�����Transform�̌Z��֌W�����񂳂���
        int length = _deckCardObjects.Count;
        for (int i = 0; i < length; i++)
        {
            _deckCardObjects[i].transform.SetAsLastSibling();
        }
    }
    /// <summary>
    /// �ۊǒ��J�[�h�I�u�W�F�N�g�̕��т�ύX����
    /// </summary>
    public void AlignStorageList()
    {
        //�I�u�W�F�N�g�̃��X�g�𐮗񂳂���
        _storageCardObjects.Sort((a, b) => _dicStorageCardObjectByCard[a].GetCardData.GetSerialNum - _dicStorageCardObjectByCard[b].GetCardData.GetSerialNum);
        //���ёւ������X�g�����Transform�̌Z��֌W�����񂳂���
        int length = _storageCardObjects.Count;
        for (int i = 0; i < length; i++)
        {
            _storageCardObjects[i].transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// �^�b�v��̃J�[�h�I������
    /// </summary>
    public void SelectCard(CardScript targetCard)
    {
        //�O��I�������J�[�h�̋����\���I��
        if (_selectCard != null)
        {
            _selectCard.SetCardHilight(false);
        }

        //�I�����擾
        _selectCard = targetCard;
        //�I���J�[�h�����\��
        targetCard.SetCardHilight(true);

        //�e��{�^���̉����ۏ�Ԃ�ύX
        if (_selectCard.isInDeckCard)
        {
            //�ҏW���f�b�L���J�[�h�I����
            _backToStorageButton.interactable = true;
            _intoDeckButton.interactable = false;
        }
        else
        {
            //�ۊǒ����X�g���J�[�h�I����
            _backToStorageButton.interactable = false;
            _intoDeckButton.interactable = true;
        }
    }
    /// <summary>
    /// �J�[�h�̑I������������
    /// </summary>
    public void DeselectCard()
    {
        //�I���J�[�h�����\���I��
        if (_selectCard != null)
        {
            _selectCard.SetCardHilight(false);
        }

        //�I�����N���A
        _selectCard = null;

        //�e��{�^���̉����ۏ�Ԃ�ύX
        _backToStorageButton.interactable = false;
        _intoDeckButton.interactable = false;
    }

    /// <summary>
    /// �f�b�L�����̕\�����X�V����
    /// </summary>
    private void RefreshDeckNumToUI()
    {
        int nowDeckNum = PlayerDeckDataScript._deckCardList.Count;

        //�f�b�L�����̕\�����X�V����
        _deckLogoText.text = "�f�b�L";
        _deckLogoText.text += "<size=20>(" + nowDeckNum + ")</size>";

        //�f�b�L�������Œᖇ���ȉ��̎��͋����\��
        if (nowDeckNum < MinDeckNum)
        {
            _deckLogoText.text = _deckLogoText.text.Insert(0, "�f�b�L<color=red>");
            _deckLogoText.text += "</color>";
        }
    }

    #region �{�^������������
    /// <summary>
    /// �{�^������������
    /// </summary>
    public void Button_BackToStorage()
    {
        //�f�b�L�����������ɒB���Ă��鎞�͒ǉ������I��
        if (PlayerDeckDataScript._deckCardList.Count <= MinDeckNum)
        {
            return;
        }

        //�I���J�[�h�L���E�I������
        if (_selectCard == null)
        {
            return;
        }
        CardScript targetCard = _selectCard;
        DeselectCard();

        //�P�E���ɂ���J�[�h���L��
        CardScript nextSelectCard = null;
        int selectCardIndexInList = _deckCardObjects.IndexOf(targetCard.gameObject);
        if (selectCardIndexInList + 1 < _deckCardObjects.Count)
        {
            nextSelectCard = _dic_DeckCardObjectByCard[_deckCardObjects[selectCardIndexInList + 1]];
        }

        //�f�b�L����J�[�h���폜
        PlayerDeckDataScript.RemoveCardFromDeck(targetCard.GetCardData.GetSerialNum);
        //�ۊǒ����X�g�ɃJ�[�h��ǉ�
        PlayerDeckDataScript.ChangeStorageCardNum(targetCard.GetCardData.GetSerialNum, 1);

        //�ۊǒ��G���A�̃J�[�h�I�u�W�F�N�g�ǉ�
        CardScript cardInstanceInStorageArea = null;
        for (int i = 0; i < _storageCardObjects.Count; i++)
        {
            CardScript targetStorageCard = _dicStorageCardObjectByCard[_storageCardObjects[i]];
            if (targetCard.GetCardData.GetSerialNum == targetStorageCard.GetCardData.GetSerialNum)
            {
                cardInstanceInStorageArea = targetStorageCard;
                break;
            }
        }
        if (cardInstanceInStorageArea != null)
        {
            //�Y���̃J�[�h�I�u�W�F�N�g�����݂���
            //�J�[�h�̕\�����X�V
            cardInstanceInStorageArea.ShowCardAmountInStorage();
        }
        else
        {
            //�Y���̃J�[�h�I�u�W�F�N�g�����݂��Ȃ�
            //�J�[�h�I�u�W�F�N�g�쐬
            CreateStorageCardObject(targetCard.GetCardData);
            //�ۊǒ��J�[�h���X�g����
            AlignStorageList();
        }
        //�ҏW���f�b�L�G���A�̃I�u�W�F�N�g�폜
        _deckCardObjects.Remove(targetCard.gameObject);
        _dic_DeckCardObjectByCard.Remove(targetCard.gameObject);
        Destroy(targetCard.gameObject);

        //�f�b�L�����\���X�V
        RefreshDeckNumToUI();
        //�J�[�h�I������
        DeselectCard();
        //���̃J�[�h������Ȃ玩���I�ɑI��
        if (nextSelectCard != null)
        {
            SelectCard(nextSelectCard);
        }

    }

    /// <summary>
    /// �f�b�L�ɓ����{�^��
    /// </summary>
    public void Button_IntoDeck()
    {
        //�f�b�L����������ɒB���Ă��鎞�͒ǉ������I��
        if (PlayerDeckDataScript._deckCardList.Count >= MaxDeckNum)
        {
            return;
        }

        //�I���J�[�h�L���E�I������
        if (_selectCard == null)
        {
            return;
        }
        CardScript targetCard = _selectCard;

        //�f�b�L�ɃJ�[�h��ǉ�
        PlayerDeckDataScript.AddCardToDeck(targetCard.GetCardData.GetSerialNum);
        //�ۊǒ����X�g����J�[�h���폜
        PlayerDeckDataScript.ChangeStorageCardNum(targetCard.GetCardData.GetSerialNum, -1);

        //�ۊǒ��G���A�̃J�[�h�I�u�W�F�N�g�폜
        if (PlayerDeckDataScript._storageCardList[targetCard.GetCardData.GetSerialNum] > 0)
        {
            targetCard.ShowCardAmountInStorage();
        }
        else
        {
            //�J�[�h�I������
            DeselectCard();
            //�I�u�W�F�N�g�폜
            _storageCardObjects.Remove(targetCard.gameObject);
            _dicStorageCardObjectByCard.Remove(targetCard.gameObject);
            Destroy(targetCard.gameObject);
        }

        //�f�b�L�����\���X�V
        RefreshDeckNumToUI();
        //�ҏW���f�b�L�G���A�̃I�u�W�F�N�g�ǉ�
        CreateDeckCardObject(targetCard.GetCardData);
        //�f�b�L�J�[�h���X�g����
        AlignDeckList();
    }
    #endregion
}

