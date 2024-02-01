//-----------------------------------------------------------------------
/* �f�[�^�̊Ǘ����s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    [SerializeField, Header("�V���O���g���ێ��p")]
    public static DataScript _date = default;
    [SerializeField, Header("�f�b�L�Ǘ��N���X")]
    private PlayerDeckDataScript _playerDeckData = default;
    [SerializeField, Header("�I���\�X�e�[�W")]
    private List<StageSO> _stageSOs = default;
    [SerializeField, Header("�v���C���[�̍ő�HP")]
    private int _playerMaxHP = 20;
    [SerializeField, Header("�v���C���[�̊e�^�[���̎�D����")]
    private int _playerHandNum = 5;
    //�i�s���X�e�[�WID
    public int _nowStageID = default;
    //�v���C���[�f�[�^
    //��������
    private int _playerGold = default;
    //�l���ς݌o���l
    private int _playerEXP = default;



    public int GetNowStageID { get => _nowStageID; }
    public int SetNowStageID { set { _nowStageID = value; } }
    public List<StageSO> GetStageSOs { get => _stageSOs; }
    public int GetPlayerGold { get => _playerGold; }
    public int GetPlayerExp { get => _playerEXP; }
    public int GetPlayerMaxHP { get => _playerMaxHP; }
    public int GetPlayerHandNum { get => _playerHandNum; }
    /// <summary>
    /// �����ݒ�
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
        //�����V�[�h�l������
        Random.InitState(System.DateTime.Now.Millisecond);
        //�v���C���[�f�b�L�f�[�^�̏�������
        _playerDeckData.Init();
        //�v���C���[�����J�[�h�f�[�^������
        _playerDeckData.DataInitialize();
    }
    #region �e��v���C���[�f�[�^�ύX����
    /// <summary>
    /// �v���C���[�̏������݂�ύX����
    /// </summary>
    /// <param name="value">�ω���</param>
    public void ChangePlayerGold(int value)
    {
        _playerGold += value;
    }
    /// <summary>
    /// �v���C���[�̌o���l�ʂ�ύX����
    /// </summary>
    /// <param name="value">�ω���</param>
    public void ChangePlayerEXP(int value)
    {
        _playerEXP += value;
    }
    /// <summary>
	/// �v���C���[�̍ő�HP��ύX����
	/// </summary>
	/// <param name="value">�ω���</param>
	public void ChangePlayerMaxHP(int value)
    {
        _playerMaxHP += value;
    }
    /// <summary>
    /// �v���C���[�̊e�^�[���̎�D������ύX����
    /// </summary>
    /// <param name="value">�ω���</param>
    public void ChangePlayerHandNum(int value)
    {
        _playerHandNum += value;
    }
    #endregion
}
