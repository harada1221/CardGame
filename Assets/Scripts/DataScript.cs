//-----------------------------------------------------------------------
/* �f�[�^�̊Ǘ����s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    [SerializeField ,Header("�V���O���g���ێ��p")]
    private static DataScript _date = default;
	/// <summary>
	/// 
	/// </summary>
	private void Awake()
	{
		//�V���O���g���p����
		if (_date != null)
		{
			Destroy(gameObject);
			return;
		}
		_date = this;
		//�����Ȃ��悤�ɂ���
		DontDestroyOnLoad(gameObject);
		//�Q�[���N��������
		InitialProcess();
	}
	/// <summary>
	/// �Q�[���J�n���ɗ�������
	/// </summary>
	private void InitialProcess()
    {
		// �����V�[�h�l������
		Random.InitState(System.DateTime.Now.Millisecond);
	}
}
