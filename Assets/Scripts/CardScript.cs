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
    // �t�B�[���h�Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;

    [SerializeField, Header("�I�u�W�F�N�g��RectTransform")]
    public RectTransform _rectTransform = default;

    // �e��ϐ�
    // �h���b�O�I����ɖ߂��Ă�����W
    private Vector2 _basePos = default;
    // �ړ�Tween
    private Tween _moveTween = default;
    private CardZoneScript.ZoneType _nowZone = default;

    // �J�[�h�ړ��A�j���[�V��������
    const float MoveTime = 0.4f;

    public CardZoneScript.ZoneType GetNowZone { get => _nowZone; }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="_fieldManager">�t�B�[���h�}�l�[�W���[</param>
    /// <param name="initPos">�������W</param>
    public void Init(FieldAreaManagerScript _fieldManager, Vector2 initPos)
    {
        //�Q�Ǝ擾
        _fieldAreaScript = _fieldManager;
        //�ϐ�������
        //�����ʒu�Ɉړ�
        _rectTransform.position = initPos;
        //�����ʒu�Œ�
        _basePos = initPos;
        //�����̏ꏊ
        _nowZone = CardZoneScript.ZoneType.Hand;
    }
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
    /// <summary>
    /// �N���b�N�����Ƃ��Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // �N���b�N�J�n����
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
}
