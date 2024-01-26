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
    [SerializeField, Header("�X�e�[�W��")]
    private string _stageName = default;
    [Space(10)]
    [SerializeField, Header("��Փx�\��")]
    private string _difficulty = default;

    [Space(10)]
    [SerializeField, Header("�X�e�[�W�A�C�R���摜")]
    private Sprite _stageIcon = default;
    [SerializeField, Header("�X�e�[�W�w�i�摜")]
    private Sprite _stageBackGround = default;

    [Space(10)]
    [SerializeField, Header("�e�i�s�x�ʂ̓G�̏o���e�[�u��")]
    private List<appearEnemyTable> _appearEnemyTables = default;
    [Space(10)]
    [SerializeField, Header("�o���l�l���ʌW��")]
    private int _bonus_EXP = default;
    [SerializeField, Header("���݊l���ʌW��")]
    private int _bonus_Gold = default;
    [SerializeField, Header("�̗͉񕜗�(�Œ�)")]
    private int _bonus_Heal = default;

    public List<appearEnemyTable> GetAppearEnemyTables { get => _appearEnemyTables; }
    public string GetStageName { get => _stageName; }
    public Sprite GetStageIcon { get => _stageIcon; }
    public Sprite GetStageBackGround { get => _stageBackGround; }
    public string GetDifficulty { get => _difficulty; }
    public int GetExp { get => _bonus_EXP; }
    public int GetGold { get => _bonus_Gold; }
    public int GetHeal { get => _bonus_Heal; }
}

/// <summary>
/// �e�i�s�x�ʂ̓G�̏o���e�[�u���N���X
/// </summary>
[System.Serializable]
public class appearEnemyTable
{
    //�G�o���e�[�u��
    [SerializeField, Header("�G�̏o���e�[�u��")]
    private List<EnemyStatusSO> _appearEnemys = default;
    public List<EnemyStatusSO> GetAppearEnemys { get => _appearEnemys; }
}