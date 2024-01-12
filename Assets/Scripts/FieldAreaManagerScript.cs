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

    //�h���b�O���쒆�J�[�h
    private CardScript _draggingCard = default;
    //���������v���C���[����J�[�h���X�g
    private List<CardScript> _cardInstances = default;
    //��D����t���O
    private bool ishandSort = false;
    //��D��[���t���O
    private bool isDraw = false;

    // �h���[�Ԃ̎��ԊԊu
    const float DrawIntervalTime = 0.1f;

    private void Start()
    {
        _dammyHand = GameObject.FindObjectOfType<DammyHandScript>();
    }
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
    #region �v���C���[����D�̏���
    /// <summary>
	/// �f�b�L����J�[�h���P��������D�ɉ�����
	/// </summary>
	/// <param name="handID">�Ώێ�D�ԍ�</param>
	private void DrawCard(int handID)
    {
        // �I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardPrefab, _cardsParentTransForm);
        // �J�[�h�����N���X���擾�E���X�g�Ɋi�[
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        // �J�[�h�����ݒ�
        objCard.Init(this, _deckTransForm.position);
        objCard.PutToZone(CardZoneScript.ZoneType.Hand, _dammyHand.GetHandPos(handID));
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
        //��D���񏈗�
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
            if (item.GetNowZone == CardZoneScript.ZoneType.Hand)
                nowHandNum++;
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
        if (isDraw==true)
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
        //�^�b�v���W���X�N���[�����W��Canvas�̃��[�J�����W�ɕϊ�
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
        //�J�[�h�����̈ʒu�ɖ߂�
        _draggingCard.BackToBasePos();
        //����������
        _draggingCard = null;
    }
    #endregion
}
