//-----------------------------------------------------------------------
/* �X�e�[�W�i�s���Ǘ�����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = " ScriptableObjects/StageSO")]
public class StageSO : ScriptableObject
{
	[SerializeField,Header("�X�e�[�W��")]
	private string _stageName = default;

	[Space(10)]
	[Header("�X�e�[�W�A�C�R���摜")]
	public Sprite _stageIcon = default;
	[Header("�X�e�[�W�w�i�摜")]
	public Sprite _stageBackGround = default;

	[Space(10)]
	[SerializeField,Header("�e�i�s�x�ʂ̓G�̏o���e�[�u��")]
	private List<appearEnemyTable> _appearEnemyTables = default;

	public List<appearEnemyTable> GetAppearEnemyTables { get => _appearEnemyTables; }
	public string GetStageName { get => _stageName; }
	public Sprite GetStageIcon { get => _stageIcon; }
	public Sprite GetStageBackGround { get => _stageBackGround; }
}

/// <summary>
/// �e�i�s�x�ʂ̓G�̏o���e�[�u���N���X
/// </summary>
[System.Serializable]
public class appearEnemyTable
{
	//�G�o���e�[�u��(1�݂̂̂̎w��Ń{�X�G�����ɂ���)
	public List<EnemyStatusSO> _appearEnemys = default;
}