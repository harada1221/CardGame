//-----------------------------------------------------------------------
/* �o�g���̏������Ǘ�����N���X
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

public class BattleManagerScript : MonoBehaviour
{
    //�t�B�[���h�̊Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;
    [SerializeField, Header("�L�����N�^�[�f�[�^�Ǘ��N���X")]
    private CharacterManagerScript _characterManager = default;
    //[SerializeField, Header("�o���G�f�[�^")]
    //private EnemyStatusSO _enemyStatusSO = default;
    [SerializeField, Header("�J�[�h���ʔ����Ǘ��N���X")]
    private PlayBoardManagerScript _playBoardManager = default;

    [SerializeField, Header("�X�e�[�W��")]
    private Text _stageNameText = default;
    [SerializeField, Header("�X�e�[�W�A�C�R��")]
    private Image _stageIconImage = default;
    [SerializeField, Header("�X�e�[�W�w�i")]
    private Image _stageBackGroundImage = default;
    [SerializeField, Header("")]
    private Image _progressGageImage = default;
    [SerializeField, Header("�U�����X�e�[�W")]
    private StageSO _stageSO = default;

    //���݂̌o�߃^�[��
    private int _nowTurns = default;
    //���݂̃X�e�[�W�i�s�x
    private int _nowProgress = default;
    //�{�X�o���i�s�x
    private int _battleNum = default;
    //�Q�[�W�\�����o����
    private const float GageAnimationTime = 2.0f;

    public FieldAreaManagerScript GetFieldManager { get => _fieldAreaScript; }
    public CharacterManagerScript GetCharacterManager { get => _characterManager; }
    public PlayBoardManagerScript GetPlayBoardManager { get => _playBoardManager; }
    public int GetNowTurns { get => _nowTurns; }
    /// <summary>
    /// ����������
    /// </summary>
    private void Start()
    {
        //�i�s�x������
        _nowProgress = -1;
        //�X�e�[�W�{�X���o������i�s�x���擾
        _battleNum = _stageSO.GetAppearEnemyTables.Count;
        //FieldAreaManagerScript���擾����
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //�R���|�[�l���g������
        _fieldAreaScript.InBattleManager(this);
        _characterManager.Init(this);
        _playBoardManager.Init(this);
        //�X�e�[�W���\��
        ApplyStageUIs();
        // (�f�o�b�O�p)�G����ʂɏo��������
        DOVirtual.DelayedCall(
            1.0f, // 1�b�x��
            () =>
            {
                ProgressingStage();
            }, false
        );
    }
    /// <summary>
    /// �X�V����
    /// </summary>
   
    #region �X�e�[�W�i�s�֘A
    /// <summary>
	/// �X�e�[�W�̐i�s�x��i�߂Đ퓬���J�n���邩�X�e�[�W���I������
	/// </summary>
	public void ProgressingStage()
    {
        //�i�s�x��i�߂�
        _nowProgress++;

        // �i�s�x��0.0f~1.0f�Ŏ擾���ĕ\��
        float progressRatio = (float)(_nowProgress % _battleNum + 1) / _battleNum;
        progressRatio = Mathf.Clamp(progressRatio, 0.0f, 1.0f);
        if (progressRatio < 0)
        {
            progressRatio = 0;
        }
        ShowProgressGage(progressRatio);
        //���̓G�o��
        DOVirtual.DelayedCall(
            0.5f,
            () =>
            {
                BattleStart();
            }
        );
    }
    /// <summary>
	/// �V�����G�Ƃ̐퓬���J�n����
	/// </summary>
	public void BattleStart()
    {
        //�^�[����������
        _nowTurns = 0;

        //�X�e�[�W�N���A�N���A�m�F
        if (_nowProgress >= _stageSO.GetAppearEnemyTables.Count)
        {
            //�S�Ă̓G�Ƃ̐퓬�ɏ�������
            Debug.Log("�X�e�[�W�N���A");
        }

        //�G�L�����N�^�[�o������
        //�o������G������
        List<EnemyStatusSO> appearEnemyTable = _stageSO.GetAppearEnemyTables[_nowProgress]._appearEnemys;
        int rand = Random.Range(0, appearEnemyTable.Count);
        _characterManager.SpawnEnemy(appearEnemyTable[rand]);

        //�퓬�J�n����(�x�����s)
        DOVirtual.DelayedCall(
            1.0f,
            () =>
            {
                //�t�B�[���h���퓬�J�n������
                _fieldAreaScript.OnBattleStarting();

                //�^�[���J�n������
                TurnStart();
            }, false
        );
    }
    /// <summary>
	/// �^�[�����J�n����
	/// </summary>
	public void TurnStart()
    {
        //FieldManager���^�[���J�n������
        _fieldAreaScript.OnTurnStarting();

        //�^�[�����J�E���g
        _nowTurns++;
    }
    /// <summary>
	/// �^�[�����I������
	/// </summary>
	public void TurnEnd()
    {
        //FieldManager���I���������ďo
        _fieldAreaScript.OnTurnEnd();
        //CharacterManager���I���������ďo
        _characterManager.OnTurnEnd();

        // �퓬�I�����m�F
        bool isPlayerWin = _characterManager.IsEnemyDefeated();
        bool isPlayerLose = _characterManager.IsPlayerDefeated();

        //�퓬�I������
        if (isPlayerWin || isPlayerLose)
        {
            //�t�B�[���h�̃J�[�h������
            //_fieldAreaScript.DestroyAllCards();

            //�����L�����N�^�[�ʏ���(�x�����s)
            DOVirtual.DelayedCall(
                0.5f,
                () =>
                {
                    // �v���C���[�s�k��
                    if (isPlayerLose)
                    {
                        Debug.Log("�Q�[���I�[�o�[");
                    }
                    // �v���C���[������
                    else if (isPlayerWin)
                    {
                        // �i�s�x����
                        ProgressingStage();
                    }
                }
            );

            return;
        }

        // ���̃^�[���J�n
        TurnStart();
    }
    #endregion

    #region �X�e�[�WUI
    /// <summary>
    /// �U�����X�e�[�W�f�[�^����Ɋe��X�e�[�W���UI�̕\�����X�V����
    /// </summary>
    private void ApplyStageUIs()
    {
        //�X�e�[�W��
        _stageNameText.text = _stageSO.GetStageName;

        //�X�e�[�W�A�C�R��
        _stageIconImage.sprite = _stageSO.GetStageIcon;

        //�X�e�[�W�w�i
        _stageBackGroundImage.sprite = _stageSO.GetStageBackGround;

        //�X�e�[�W�i�s�x�Q�[�W������
        _progressGageImage.fillAmount = 0.0f;
    }
    /// <summary>
	/// �X�e�[�W�i�s�x�Q�[�W��\������
	/// </summary>
	/// <param name="ratio">�i�s�x����(0.0f-1.0f)</param>
	public void ShowProgressGage(float ratio)
    {
        _progressGageImage.DOFillAmount(ratio, GageAnimationTime);
    }
    #endregion
}
