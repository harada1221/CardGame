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
		Debug.Log("�J�[�h���^�b�v����܂���");
	}
	/// <summary>
	/// �N���b�N���I��������Ɏ��s����
	/// </summary>
	/// <param name="eventData">�N���b�N���</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log("�J�[�h�ւ̃^�b�v���I�����܂���");
	}
}
