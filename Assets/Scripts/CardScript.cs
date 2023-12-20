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

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform _rectTransform;
    // �t�B�[���h�Ǘ��N���X
    [SerializeField,Header("�t�B�[���h���Ǘ�����X�N���v�g")]
    private FieldAreaManagerScript _fieldAreaScript = default;

    // ��{���W(�h���b�O�I����ɖ߂��Ă�����W)
    private Vector2 _basePos = default;
    // �ړ��p��Tween
    private Tween _moveTween = default;

    // �J�[�h�ړ��A�j���[�V��������
    private const float MoveTime = 0.4f;
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="_fieldManager">�t�B�[���h�}�l�[�W���[</param>
    public void Init(FieldAreaManagerScript _fieldManager)
    {
        // �Q�Ǝ擾
        _fieldAreaScript = _fieldManager;
        // �ϐ�������
        _basePos = this.gameObject.transform.position;
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
        this.transform.DOMove(_basePos, MoveTime) // �ړ�Tween
            .SetEase(Ease.OutQuart);   // �ω��̎d�����w��
    }
    /// <summary>
    /// �N���b�N�����Ƃ��Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // �N���b�N���J�[�h��������
        _fieldAreaScript.StartDragging(this);
        Debug.Log("�N���b�N");
    }
    /// <summary>
    /// �N���b�N���I��������Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //�N���b�N�I������
        BackToBasePos();
        Debug.Log("�b����");
    }

    /// <summary>
    /// �h���b�N���̏���
    /// </summary>
    /// <param name="eventData">�h���b�N����</param>
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("�h���b�O");
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        TargetPos.z = 0;
        transform.position = TargetPos;
    }
}
