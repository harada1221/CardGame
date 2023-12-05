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

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	//�J�[�\���̈ʒu
	private Vector3 _cursorPosition = default;


	/// <summary>
	/// �N���b�N�����Ƃ��Ɏ��s����
	/// </summary>
	/// <param name="eventData">�N���b�N���</param>
	public void OnPointerDown(PointerEventData eventData)
	{
		//Debug.Log("�J�[�h���^�b�v����܂���");
		//�N���b�N�������W��ۑ�
		_cursorPosition = gameObject.transform.position - GetMouseWorldPos();
	}
	/// <summary>
	/// �N���b�N���I��������Ɏ��s����
	/// </summary>
	/// <param name="eventData">�N���b�N���</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log("�J�[�h�ւ̃^�b�v���I�����܂���");
	}
	/// <summary>
	/// �h���b�O���Ă���ԌĂяo��
	/// </summary>
    public void OnMouseDrag()
    {
		Debug.Log("�h���b�O");
		//�I�u�W�F�N�g���ړ�������
		transform.position = GetMouseWorldPos() + _cursorPosition;
	}
	/// <summary>
	/// �X�N���[�����W���烏�[���h���W�ɕω�����
	/// </summary>
	/// <returns>���[���h���W</returns>
    private Vector3 GetMouseWorldPos()
    {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos);
	}
}
