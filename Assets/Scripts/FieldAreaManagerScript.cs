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
    //�퓬��ʃ}�l�[�W���[
    private BattleManagerScript _battleManager = default;
    //�_�~�[��D����N���X
    private DammyHandScript _dammyHand = default;

    //�J�[�h�֘A
    [SerializeField, Header("�J�[�h�̃v���n�u")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("��������J�[�h�I�u�W�F�N�g�̐eTransform")]
    private Transform _cardsParentTransForm = default;
    [SerializeField, Header("�f�b�L�I�u�W�F�N�gTransform")]
    private Transform _deckTransForm = default;
    [SerializeField,Header("�J�[�h�f�[�^")]
    private CardDataSO _testCardData = default;

    //�h���b�O���쒆�J�[�h
    private CardScript _draggingCard = default;
    //���������v���C���[����J�[�h���X�g
    private List<CardScript> _cardInstances = default;
    //��D����t���O
    private bool ishandSort = false;
    //��D��[���t���O
    private bool isDraw = false;

    //ray�̒���
    const float _rayDistance = 10.0f;
    // �h���[�Ԃ̎��ԊԊu
    const float DrawIntervalTime = 0.1f;
    #endregion

    #region ����������
    private void Start()
    {
        _dammyHand = GameObject.FindObjectOfType<DammyHandScript>();
    }
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="battleManager">�g��BattleManagerScript</param>
    public void InBattleManager(BattleManagerScript battleManager)
    {
        //�����Ă���BattleManagerScript���i�[����
        _battleManager = battleManager;
        //�ϐ�������
        _cardInstances = new List<CardScript>();

        // �f�o�b�O�p�h���[����(�x�����s)
        DOVirtual.DelayedCall(
            1.0f, // 1.0�b�x��
            () =>
            {
                DrawCardsUntilNum(5);
            }
        );
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

    #region �v���C���[����D�̏���
    /// <summary>
    /// �f�b�L����J�[�h���P��������D�ɉ�����
    /// </summary>
    /// <param name="handID">�Ώێ�D�ԍ�</param>
    private void DrawCard(int handID)
    {
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
        //�J�[�h�����N���X���擾�E���X�g�Ɋi�[
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        //�J�[�h�����ݒ�
        objCard.Init(this, _deckTransForm.position);
        objCard.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(handID));
        objCard.SetInitialCardData(_testCardData, CardScript.CharaID_Player);
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
            drawSequence.AppendInterval(DrawIntervalTime);
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
        // ���݂̎�D�������擾
        int nowHandNum = 0;
        foreach (CardScript item in _cardInstances)
        {
            //�J�[�h����D�ɂ��邩
            if (item.GetNowZone == CardZoneScript.ZoneType.Hand)
            {
                nowHandNum++;
            }
        }
        // �_�~�[��D�ɖ������w��
        _dammyHand.SetHandNum(nowHandNum);
        // ��D�����ɍ��킹�Ď�D�𐮗�
        ishandSort = true;
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

        ////�v���C�{�[�h�ɂ���J�[�h�ǂ������d�Ȃ�����
        //if (targetCard != null && (targetCard.GetNowZone >= CardZoneScript.ZoneType.PlayBoard0 && targetCard.GetNowZone <= CardZoneScript.ZoneType.PlayBoard4))
        //{
        //    //��������(������)
        //}
        //�J�[�h�Əd�Ȃ炸�J�[�h�G���A�Əd�Ȃ����ꍇ
        if (targetZone != null)
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
    #endregion
}
