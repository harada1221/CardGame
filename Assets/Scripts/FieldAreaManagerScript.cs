//-----------------------------------------------------------------------
/* �t�B�[���h�̊Ǘ����s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldAreaManagerScript : MonoBehaviour
{
    #region �ϐ��錾
    [SerializeField, Header("Canvas��RectTransform")]
    private RectTransform _canvasRectTransform = default;
    [SerializeField, Header("���C���J����")]
    private Camera _mainCamera = default;
    [SerializeField, Header("�f�b�L�A�C�R���̃I�u�W�F�N�g")]
    private GameObject _deckIconObject = default;
    [SerializeField, Header("�f�b�L�̎c�薇���\���e�L�X�g")]
    private Text _deckRemainingNum = default;
    [SerializeField, Header("�v���C�{�^��")]
    private Button _cardPlayButton = default;

    //�J�[�h�֘A
    [SerializeField, Header("�J�[�h�̃v���n�u")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("��������J�[�h�I�u�W�F�N�g�̐eTransform")]
    private Transform _cardsParentTransForm = default;
    [SerializeField, Header("�f�b�L�I�u�W�F�N�gTransform")]
    private Transform _deckTransForm = default;

    //�퓬��ʃ}�l�[�W���[
    private BattleManagerScript _battleManager = default;
    //�_�~�[��D����N���X
    private DammyHandScript _dammyHand = default;
    //�v���C���[�̌��݂̃f�b�L
    private List<CardDataSO> _playerDeckData = default;
    //�v���C���[�f�b�L�̃o�b�N�A�b�v
    private List<CardDataSO> _playerDeckDataBackUp = default;
    //�h���b�O���쒆�J�[�h
    private CardScript _draggingCard = default;
    //���������v���C���[����J�[�h���X�g
    private List<CardScript> _cardInstances = default;
    //��D����t���O
    private bool ishandSort = false;
    //��D��[���t���O
    private bool isDraw = false;
    //�J�[�h���ʎ��s���t���O
    private bool isCardPlaying = false;

    //ray�̒���
    private const float _rayDistance = 10.0f;
    // �h���[�Ԃ̎��ԊԊu
    private const float _drawIntervalTime = 0.1f;
    //�F��ς���f�b�L����
    private const int _sufficientLine = 10;
    #endregion

    #region ����������
    /// <summary>
    /// �������������Ԃ��֌W�Ȃ�����
    /// </summary>
    private void Start()
    {
        _dammyHand = GameObject.FindObjectOfType<DammyHandScript>();
          //�f�b�L�f�[�^����
        _playerDeckData = new List<CardDataSO>();
        _playerDeckDataBackUp = new List<CardDataSO>();
    }
    /// <summary>
    /// �����������ς����́A���Ԃ����������Ȃ�Ȃ��悤�ɂ���
    /// </summary>
    /// <param name="battleManager">�g��BattleManagerScript</param>
    public void Init(BattleManagerScript battleManager)
    {
        //�����Ă���BattleManagerScript���i�[����
        _battleManager = battleManager;
        //�ϐ�������
        _cardInstances = new List<CardScript>();
        // UI������
        _deckRemainingNum.color = Color.clear;
    }
    #endregion

    #region �X�V����
    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        //�h���b�O���쒆�̏���
        if (_draggingCard != null)
        {
            //�X�V����
            UpdateDragging();
        }
    }
    private void OnGUI()
    {
        // ��D����t���O�������Ă���Ȃ琮��
        if (ishandSort == true)
        {
            //���񂳂���
            AlignHandCards();
            //���񊮗�
            ishandSort = false;
        }
    }
    #endregion

    #region �i�s�Ǘ�
    /// <summary>
    /// �o�g���J�n����
    /// </summary>
    public void OnBattleStarting()
    {
        //�f�b�L�f�[�^����
        _playerDeckData = new List<CardDataSO>();
        _playerDeckDataBackUp = new List<CardDataSO>();
        //�f�b�L�f�[�^�擾
        foreach (int cardData in PlayerDeckDataScript._deckCardList)
        {
            _playerDeckData.Add(PlayerDeckDataScript._cardDatasBySerialNum[cardData]);
            _playerDeckDataBackUp.Add(PlayerDeckDataScript._cardDatasBySerialNum[cardData]);
        }
        //�f�b�L�c�薇���\��
        PrintPlayerDeckNum();
    }
    /// <summary>
	/// �^�[���J�n������
	/// </summary>
	public void OnTurnStarting()
    {
        //�J�[�h�̃h���[��������
        int nextHandCardsNum = 5;// ��D����
        //�h���[����
        DrawCardsUntilNum(nextHandCardsNum);
        ishandSort = true;
        //�J�[�h���s�{�^����L����
        _cardPlayButton.interactable = true;
        //�G�̃J�[�h�ݒu
        PlacingEnemyCards();
    }
    #endregion
    /// <summary>
	/// (�^�[���J�n��)�G���̃J�[�h���v���C�{�[�h�ɑS�Đݒu����
	/// </summary>
	private void PlacingEnemyCards()
    {
        //�G�f�[�^�擾
        EnemyStatusSO enemyData = _battleManager.GetCharacterManager.GetEnemyDate;

        //���̃^�[���Ɏg�p����G�J�[�h�Z�b�g�̔ԍ����擾
        int enemyAttackOrderID = _battleManager.GetNowTurns % enemyData.GetUseCardDatas.Count;

        //�G�J�[�h�ݒu����
        EnemyStatusSO.EnemyUseCardData useCardDatasInThisTurn = enemyData.GetUseCardDatas[enemyAttackOrderID]; // ���̃^�[���̎g�p�J�[�h���X�g
        for (int i = 0; i < PlayBoardManagerScript.PlayBoardCardNum; i++)
        {
            //�e�]�[���ɑ΂���ݒu�J�[�h���擾
            CardDataSO cardData = null;
            switch (i)
            {
                case 0:
                    cardData = useCardDatasInThisTurn.placeCardData0; break;
                case 1:
                    cardData = useCardDatasInThisTurn.placeCardData1; break;
                case 2:
                    cardData = useCardDatasInThisTurn.placeCardData2; break;
                case 3:
                    cardData = useCardDatasInThisTurn.placeCardData3; break;
                case 4:
                    cardData = useCardDatasInThisTurn.placeCardData4; break;
            }
            //�ݒu�J�[�h�������Ȃ玟��
            if (cardData == null)
            {
                continue;
            }


            //�ݒu��]�[��ID���擾
            CardZoneScript.ZoneType areaType = CardZoneScript.ZoneType.PlayBoard0 + i;
            // �ݒu��Vector2���W���擾
            Vector2 targetPosition = _battleManager.GetPlayBoardManager.GetPlayZonePos(i);

            //�I�u�W�F�N�g�쐬
            GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
            //�J�[�h�����N���X���擾�E���X�g�Ɋi�[
            CardScript objCard = obj.GetComponent<CardScript>();
            _cardInstances.Add(objCard);

            //�J�[�h�����ݒ�
            objCard.Init(this, _battleManager.GetCharacterManager.GetEnemyPosition());
            objCard.PutToZone(areaType, targetPosition);
            //�GID���w��
            objCard.SetInitialCardData(cardData, CardScript.CharaID_Enemy);
        }
    }

    /// <summary>
	/// �J�[�h���ʔ����{�^������������
	/// </summary>
	public void CardPlayButton()
    {
        //�J�[�h�h���b�O���Ȃ珈�����Ȃ�
        if (_draggingCard != null)
        {
            return;
        }


        //���s�{�^�����ꎞ�I�ɖ�����
        _cardPlayButton.interactable = false;
        //���ʎ��s��
        isCardPlaying = true;

        //�v���C�{�[�h��J�[�h�̔z����쐬
        CardScript[] boardCards = new CardScript[PlayBoardManagerScript.PlayBoardCardNum];
        //�v���C�{�[�h��̃J�[�h���擾���Ĕz��Ɋi�[
        foreach (CardScript card in _cardInstances)
        {
            // �z����̎w��̈ʒu�ɊY���J�[�h���i�[����
            if (card.GetNowZone >= CardZoneScript.ZoneType.PlayBoard0 &&
                card.GetNowZone <= CardZoneScript.ZoneType.PlayBoard4)
            {
                int arrayID = (int)card.GetNowZone - (int)CardZoneScript.ZoneType.PlayBoard0;
                boardCards[arrayID] = card;
            }
        }

        //�e�J�[�h�̌��ʂ����s
        _battleManager.GetPlayBoardManager.BoardCardsPlay(boardCards);
    }
    /// <summary>
	/// �^�[���I�����Ɏ��s����鏈��
	/// </summary>
	public void OnTurnEnd()
    {
        //���ʎ��s�������t���O
        isCardPlaying = false;
    }
    #region �v���C���[����D�̏���
    /// <summary>
    /// �f�b�L����J�[�h���P������
    /// </summary>
    /// <param name="handID">�Ώێ�D�ԍ�</param>
    private void DrawCard(int handID)
    {
        //�f�b�L�c�薇��
        int deckCardNum = _playerDeckData.Count;
        // �f�b�L�̎c�肪�����ꍇ�̓h���[�����I��
        if (deckCardNum <= 0)
        {
            return;
        }

        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
        //�J�[�h�����N���X���擾�E���X�g�Ɋi�[
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        //�f�b�L������������J�[�h�������_���Ɍ���
        CardDataSO targetCard = _playerDeckData[Random.Range(0, deckCardNum)];
        //�f�b�L���X�g����Y���J�[�h���폜
        _playerDeckData.Remove(targetCard);

        //�J�[�h�����ݒ�
        objCard.Init(this, _deckTransForm.position);
        objCard.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(handID));
        objCard.SetInitialCardData(targetCard, CardScript.CharaID_Player);

        //�f�b�L�c�薇���\��
        PrintPlayerDeckNum();
    }

    /// <summary>
	/// ��D���w�薇���ɂȂ�܂ŃJ�[�h������
	/// </summary>
	/// <param name="num">�w�薇��</param>
	private void DrawCardsUntilNum(int num)
    {
        //���݂̎�D�������擾
        int nowHandNum = 0;
        foreach (CardScript card in _cardInstances)
        {
            //��D�ɂ���Ή��Z
            if (card.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                nowHandNum++;
            }
        }
        //�V���Ɉ����ׂ��������擾
        int drawNum = num - nowHandNum;
        //0�ȉ����ƏI���
        if (drawNum <= 0)
        {
            return;
        }
        //�f�b�L�������h���[������菭�Ȃ����f�b�L�����ɍ��킹��
        if (_playerDeckData.Count < drawNum)
        {
            drawNum = _playerDeckData.Count;
        }

        //��DUI�ɖ������w��
        _dammyHand.SetHandNum(nowHandNum + drawNum);

        //�A���ŃJ�[�h������
        Sequence drawSequence = DOTween.Sequence();
        //��D��[��
        isDraw = true;
        for (int i = 0; i < drawNum; i++)
        {
            //�P����������(Sequence�ɒǉ�)
            drawSequence.AppendCallback(() =>
            {
                DrawCard(nowHandNum);
                nowHandNum++;
            });
            // ���ԊԊu��Sequence�ɒǉ�
            drawSequence.AppendInterval(_drawIntervalTime);
        }
        drawSequence.OnComplete(() => isDraw = false);
    }
    /// <summary>
	/// ��D�̃J�[�h�𐮗񂳂���
	/// </summary>
	private void AlignHandCards()
    {
        //��D���ԍ�
        int index = 0;
        // �_�~�[��D�𐮗�
        _dammyHand.ApplyLayout();
        //�e�J�[�h���_�~�[��D�ɍ��킹�Ĉړ�
        foreach (CardScript card in _cardInstances)
        {
            //��D�ɂ���΍��W�ύX����index����
            if (card.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                card.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(index));
                index++;
            }
        }
    }
    /// <summary>
	/// ���݂̎�D�̖�������DUI�����N���X�ɔ��f�����Đ��񂷂�
	/// </summary>
	private void CheckHandCardsNum()
    {
        //���݂̎�D�������擾
        int nowHandNum = 0;
        foreach (CardScript item in _cardInstances)
        {
            //�J�[�h����D�ɂ��邩
            if (item.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                nowHandNum++;
            }
        }
        //�_�~�[��D�ɖ������w��
        _dammyHand.SetHandNum(nowHandNum);
        //��D�����ɍ��킹�Ď�D�𐮗�
        ishandSort = true;
    }
    /// <summary>
	/// �v���C���[���f�b�L�c�薇���̕\�����X�V����
	/// </summary>
	private void PrintPlayerDeckNum()
    {
        //�f�b�L�c�薇��
        int deckCardNum = _playerDeckData.Count;
        //������Text�\��
        _deckRemainingNum.text = _playerDeckData.Count.ToString();

        //�c�薇���ɉ����ĕ\���F��ύX
        if (deckCardNum >= _sufficientLine)
        {
            _deckRemainingNum.color = Color.white;
        }
        else if (deckCardNum > 0)
        {
            _deckRemainingNum.color = Color.yellow;
        }
        else
        {
            _deckRemainingNum.color = Color.clear;
        }
        //�c�薇�����P���ł�����΃f�b�L�̃J�[�h�A�C�R����\������
        if (deckCardNum > 0)
        {
            _deckIconObject.SetActive(true);
        }
        else
        {
            _deckIconObject.SetActive(false);
        }
    }
    #endregion

    #region �J�[�h�h���b�O����
    /// <summary>
	/// �J�[�h�̃h���b�O������J�n����
	/// </summary>
	/// <param name="dragCard">����ΏۃJ�[�h</param>
	public void StartDragging(CardScript dragCard)
    {
        //��D��[���o���Ȃ�I��
        if (isDraw == true)
        {
            return;
        }
        // �J�[�h�̌��ʎ��s���Ȃ�I��
        if (isCardPlaying == true)
        {
            return;
        }
        //����ΏۃJ�[�h���L��
        _draggingCard = dragCard;
        //�őO�ʕ\���ɂ���
        _draggingCard.transform.SetAsLastSibling();
    }
    /// <summary>
	/// �h���b�O����X�V����
	/// </summary>
	private void UpdateDragging()
    {
        //�N���b�N�ʒu���擾
        Vector2 clickPos = Input.mousePosition;
        //�N���b�N���W���X�N���[�����W��Canvas�̃��[�J�����W�ɕϊ�
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRectTransform, //Canvas��RectTransform
            clickPos,             //�ϊ������W�f�[�^
            _mainCamera,          //���C���J����
            out clickPos);        //�ϊ�����W�f�[�^

        //���W��K�p
        _draggingCard._rectTransform.anchoredPosition = clickPos;
    }
    /// <summary>
	/// �J�[�h�̃h���b�O������I������
	/// </summary>
	public void EndDragging()
    {
        //�J�[�h�𑀍쒆�łȂ��Ȃ�I��
        if (_draggingCard == null)
        {
            return;
        }

        //�I�u�W�F�N�g�̃X�N���[�����W���擾����
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(_mainCamera, _draggingCard.transform.position);
        //���C���J���������L�Ŏ擾�������W�Ɍ�����Ray���΂�
        Ray ray = _mainCamera.ScreenPointToRay(pos);

        //�h���b�O��J�[�h�]�[��
        CardZoneScript targetZone = default;
        //�h���b�O��J�[�h
        CardScript targetCard = default;
        //Ray�����������S�I�u�W�F�N�g�ɑ΂��Ă̏���
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(ray.origin, ray.direction, _rayDistance))
        {
            //���������I�u�W�F�N�g�����݂��Ȃ��Ȃ�I��
            if (!hit.collider)
            {
                break;
            }
            //���������I�u�W�F�N�g���h���b�O���̃J�[�h�Ɠ���Ȃ玟��
            GameObject hitObj = hit.collider.gameObject;
            if (hitObj == _draggingCard.gameObject)
            {
                continue;
            }
            // �I�u�W�F�N�g���J�[�h�G���A�Ȃ�擾����
            CardZoneScript hitArea = hitObj.GetComponent<CardZoneScript>();
            if (hitArea != null)
            {
                targetZone = hitArea;
                continue;
            }
            // �I�u�W�F�N�g���J�[�h�Ȃ�擾���Ď���
            CardScript hitCard = hitObj.GetComponent<CardScript>();
            if (hitCard != null)
            {
                targetCard = hitCard;
                continue;
            }
        }

        //�v���C�{�[�h�ɂ���J�[�h�ǂ������d�Ȃ�����
        if (targetCard != null && (targetCard.GetNowZone >= CardZoneScript.ZoneType.PlayBoard0 && targetCard.GetNowZone <= CardZoneScript.ZoneType.PlayBoard4))
        {
            //��������
            CompositeCard(targetCard, _draggingCard);
            CheckHandCardsNum();
        }
        //�J�[�h�Əd�Ȃ炸�J�[�h�G���A�Əd�Ȃ����ꍇ
        else if (targetZone != null)
        {
            //�ݒu����
            _draggingCard.PutToZone(targetZone.GetZoneType, targetZone.GetComponent<RectTransform>().position);
            CheckHandCardsNum();
            //��D�ȊO�����D�ւ̈ړ��̏ꍇ�A�J�[�h�����X�g���ň�Ԍ��ɂ���
            if (_draggingCard.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                _cardInstances.Remove(_draggingCard);
                _cardInstances.Add(_draggingCard);
            }
        }
        //������Ƃ��d�Ȃ�Ȃ������ꍇ
        else
        {
            //���̈ʒu�ɖ߂�
            _draggingCard.BackToBasePos();
        }

        //����������
        _draggingCard = null;
    }
    /// <summary>
	/// 2���̃J�[�h����������1���̃J�[�h�ɂ���
	/// </summary>
	/// <param name="baseCard">���ƂȂ�J�[�h)</param>
	/// <param name="consumeCard">�����̑f�ނƂ���J�[�h</param>
	private void CompositeCard(CardScript baseCard, CardScript consumeCard)
    {
        //�J�[�h���Ȃ�������I���
        if (baseCard == null || consumeCard == null)
        {
            return;
        }
        //�����s��
        if (baseCard.CheckContainEffect(CardEffectDefineScript.CardEffect.PartsOnly) ||//�J�[�h�ɑf�ތ�����ʂ�����
            consumeCard.CheckContainEffect(CardEffectDefineScript.CardEffect.BaseOnly) ||//�J�[�h�ɖ{�̌�����ʂ�����
            baseCard.CheckContainEffect(CardEffectDefineScript.CardEffect.NoCompo) ||//�J�[�h�ɍ����������ʂ�����
            consumeCard.CheckContainEffect(CardEffectDefineScript.CardEffect.NoCompo))//�J�[�h�ɍ����������ʂ�����
        {
            //�h���b�O���J�[�h�����̈ʒu�ɖ߂��č��������I��
            _draggingCard.BackToBasePos();
            return;
        }
        //���x�̉��Z�z������J�[�h�폜
        if (baseCard.SetForcePoint(baseCard.GetForcePoint + consumeCard.GetForcePoint))
        {
            DestroyCardObject(baseCard);
            DestroyCardObject(consumeCard);
            return;
        }

        //���ʂ̍���
        foreach (CardEffectDefineScript effect in consumeCard.GetCardEffects)
        {
            baseCard.CompoCardEffect(effect);
        }
        //����J�[�h���폜
        DestroyCardObject(consumeCard);
    }
    #endregion

    #region �J�[�h�����n
    /// <summary>
    /// �w��̃J�[�h�I�u�W�F�N�g���t�B�[���h����폜����
    /// </summary>
    /// <param name="targetCard">�폜�ΏۃJ�[�h</param>
    public void DestroyCardObject(CardScript targetCard)
    {
        //���X�g����폜
        _cardInstances.Remove(targetCard);
        //�I�u�W�F�N�g�폜
        Destroy(targetCard.gameObject);
    }

    /// <summary>
    /// �S�ẴJ�[�h�I�u�W�F�N�g���t�B�[���h����폜����
    /// </summary>
    public void DestroyAllCards()
    {
        //�I�u�W�F�N�g�폜
        foreach (CardScript item in _cardInstances)
        {
            Destroy(item.gameObject);
        }
        //���X�g��������
        _cardInstances.Clear();
    }
    #endregion
    /// <summary>
	/// �퓬���Ƀf�b�L���̃J�[�h������ǉ�����
	/// </summary>
	/// <param name="amount">�ǉ���</param>
	public void AddDeckCardsNum(int amount)
    {
        //1�����ǉ�
        for (int i = 0; i < amount; i++)
        {
            //�ǉ�����J�[�h��퓬�J�n���̃f�b�L�f�[�^�����烉���_���Ɍ���
            CardDataSO targetCard = _playerDeckDataBackUp[Random.Range(0, _playerDeckDataBackUp.Count)];
            //���݂̃f�b�L�f�[�^�ɒǉ�
            _playerDeckData.Add(targetCard);
        }

        //�f�b�L�c�薇���\��
        PrintPlayerDeckNum();
    }
}
