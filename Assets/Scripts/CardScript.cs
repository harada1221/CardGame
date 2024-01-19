//-----------------------------------------------------------------------
/* �J�[�h�̏������s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region �ϐ��錾
    //�t�B�[���h�Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;

    [SerializeField, Header("�I�u�W�F�N�g��RectTransform")]
    public RectTransform _rectTransform = default;
    [SerializeField, Header("�J�[�hUI�\��")] 
    private CardUIScript _cardUI = default;
    [SerializeField,Header("�I�u�W�F�N�g��CanvasGroup")]
    private CanvasGroup _canvasGroup = default;

    //�h���b�O�I����ɖ߂��Ă�����W
    private Vector2 _basePos = default;
    //�ړ�Tween
    private Tween _moveTween = default;
    //�J�[�h�]�[���̎��
    private CardZoneScript.ZoneType _nowZone = default;

    //��ɂȂ�J�[�h�f�[�^
    private CardDataSO _cardDate = default;
    //�J�[�h���ʂ̃��X�g
    private List<CardEffectDefineScript> _cardEffects = default;
    //�J�[�h�̋��x
    private int _forcePoint = default;
    //�J�[�h���g�p�L�����N�^�[
    private int _controllerCharaID = default;

    //�J�[�h�ړ��A�j���[�V��������
    private const float MoveTime = 0.4f;
    //�L�����N�^�[ID�萔
    //�퓬���̃L�����N�^�[�l��
    public const int CharaNum = 2;
    //�L�����N�^�[ID:��l��(�v���C���[�L����)
    public const int CharaID_Player = 0;
    //�L�����N�^�[ID:�G
    public const int CharaID_Enemy = 1;
    //�L�����N�^�[ID:(����)
    public const int CharaID_None = -1;
    //�J�[�h�A�C�R���̍ő吔
    private const int MaxIcons = 6;
    //�J�[�h���ʂ̍ő吔
    private const int MaxEffects = 6;
    #endregion

    #region �v���p�e�B
    public CardZoneScript.ZoneType GetNowZone { get => _nowZone; }
    public int GetControllerCharaID { get => _controllerCharaID; }
    public List<CardEffectDefineScript> GetCardEffects { get => _cardEffects; }
    public int GetForcePoint { get => _forcePoint; }
    #endregion

    #region ��������
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="_fieldManager">�t�B�[���h�}�l�[�W���[</param>
    /// <param name="initPos">�������W</param>
    public void Init(FieldAreaManagerScript _fieldManager, Vector2 initPos)
    {
        //FieldAreaManagerScript�Q�Ǝ擾
        _fieldAreaScript = _fieldManager;
        //UI�����ݒ�
        _cardUI.Init(this);
        //�ϐ�������
        //�����ʒu�Ɉړ�
        _rectTransform.position = initPos;
        //�����ʒu�Œ�
        _basePos = initPos;
        //�����̏ꏊ
        _nowZone = CardZoneScript.ZoneType.Hand;
        //���X�g����
        _cardEffects = new List<CardEffectDefineScript>();
    }
    /// <summary>
    /// �J�[�h�̃p�����[�^���擾����
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="ControllerCharaID"></param>
    public void SetInitialCardData(CardDataSO cardData, int ControllerCharaID)
    {
        //�J�[�h�f�[�^�擾
        _cardDate = cardData;
        // �J�[�h��
        _cardUI.SetCardNameText(cardData.GetCardName);

        //�J�[�h���ʃ��X�g
        foreach (CardEffectDefineScript item in cardData.GetEffectList)
        {
            AddCardEffect(item);
        }
        //���x��ݒ�
        SetForcePoint(cardData.GetForce);
        // �J�[�h�g�p�҃f�[�^
        _controllerCharaID = ControllerCharaID;
        _cardUI.SetCardBackSprite(ControllerCharaID); // �J�[�h�w�iUI�ɓK�p
    }
    #endregion

    #region �ړ�����
    /// <summary>
    /// ��{���W�܂ŃJ�[�h���ړ�������
    /// </summary>
    public void BackToBasePos()
    {
        // ���Ɏ��s���̈ړ��A�j���[�V����������Β�~
        if (_moveTween != null)
        {
            _moveTween.Kill();
        }
        // �w��n�_�܂ňړ�����A�j���[�V����(DOTween)
        _moveTween = _rectTransform
            .DOMove(_basePos, MoveTime) // �ړ�Tween
            .SetEase(Ease.OutQuart);    // �ω��̎d�����w��
    }
    /// <summary>
	/// �J�[�h���w��̃]�[���ɐݒu����
	/// </summary>
	/// <param name="zoneType">�ΏۃJ�[�h�]�[��</param>
	/// <param name="targetPos">�Ώۍ��W</param>
	public void PutToZone(CardZoneScript.ZoneType zoneType, Vector2 targetPos)
    {
        //���W���擾
        _basePos = targetPos;
        //�����ʒu�ɖ߂�
        BackToBasePos();
        //�J�[�h�]�[���̎�ނ�ۑ�
        _nowZone = zoneType;
    }
    #endregion

    #region �N���b�N����
    /// <summary>
    /// �N���b�N�����Ƃ��Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //�N���b�N�J�n����
        _fieldAreaScript.StartDragging(this);
    }
    /// <summary>
    /// �N���b�N���I��������Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //�N���b�N�I������
        _fieldAreaScript.EndDragging();
    }
    #endregion

    #region �p�����[�^�ύX
/// <summary>
/// �J�[�h���ʂ�V�K�ǉ�����
/// </summary>
/// <param name="newEffect">���ʂ̎�ށE���l�f�[�^</param>
private void AddCardEffect(CardEffectDefineScript newEffect)
    {
        //�J�[�h���ʐ�����Ȃ�I��
        if (_cardEffects.Count >= MaxEffects)
        {
            return;
        }

        //���ʃf�[�^��V�K�쐬����
        CardEffectDefineScript effectData = new CardEffectDefineScript();
        effectData.GetEffect = newEffect.GetEffect;
        effectData.GetValue = newEffect.GetValue;
        //���ʃ��X�g�ɒǉ�
        _cardEffects.Add(effectData);
        //UI�\��
        _cardUI.AddCardEffectText(effectData);
    }
    /// <summary>
	/// �J�[�h�̋��x���Z�b�g����
	/// </summary>
	public void SetForcePoint(int forcevalue)
    {
        //�p�����[�^���Z�b�g
        _forcePoint = forcevalue;
        //UI�\��
        _cardUI.SetForcePointText(forcevalue);
    }
    #endregion
}
