//-----------------------------------------------------------------------
/* �o�g���̏������Ǘ�����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
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
    [SerializeField, Header("��V��ʃN���X")]
    private RewardScript _rewardPanel = default;
    [SerializeField, Header("�J�[�h���ʔ����Ǘ��N���X")]
    private PlayBoardManagerScript _playBoardManager = default;
    [SerializeField, Header("�X�e�[�W��")]
    private Text _stageNameText = default;
    [SerializeField, Header("�X�e�[�W�A�C�R��")]
    private Image _stageIconImage = default;
    [SerializeField, Header("�X�e�[�W�w�i")]
    private Image _stageBackGroundImage = default;
    [SerializeField, Header("�X�e�[�W�i�s�x")]
    private Image _progressGageImage = default;
    [SerializeField, Header("�U�����X�e�[�W")]
    private StageSO _stageSO = default;
    [SerializeField, Header("�{�X�\��")]
    private BossIncomingScript _bossIncoming = default;
    [SerializeField, Header("�X�e�[�W�N���A�N���X")]
    private StageClearScript _stageClear = default;
    [SerializeField, Header("�Q�[���I�[�o�[�N���X")]
    private GameOverScript _gameOver = default;
    //�v���C���[�f�[�^UI
    [SerializeField, Header("�o���l��Text")]
    private Text _playerExpText = default;
    [SerializeField, Header("��������Text")]
    private Text _playerGoldText = default;
    //�o���l��Text�\���p�ϐ�
    private int _playerExpDisp = default;
    //��������Text�\���p�ϐ�
    private int _playerGoldDisp = default;
    //���݂̌o�߃^�[��
    private int _nowTurns = default;
    //���݂̃X�e�[�W�i�s�x
    private int _nowProgress = default;
    //�{�X�o���i�s�x
    private int _battleNum = default;
    //�萔��`
    //�Q�[�W�\�����o����
    private const float GageAnimationTime = 2.0f;
    private const float AnimationTime = 1.0f;
    //�{�[�i�X�ʌv�Z���̃X�e�[�W�i�s�x���Z��
    private const int BonusValueBase = 4;
    //�{�[�i�X�ʂ̃����_����:�ŏ�
    private const float BonusRandomMulti_Min = 0.8f;
    //�{�[�i�X�ʂ̃����_����:�ő�
    private const float BonusRandomMulti_Max = 1.4f;
    public FieldAreaManagerScript GetFieldManager { get => _fieldAreaScript; }
    public CharacterManagerScript GetCharacterManager { get => _characterManager; }
    public PlayBoardManagerScript GetPlayBoardManager { get => _playBoardManager; }
    public int GetNowTurns { get => _nowTurns; }
    /// <summary>
    /// ����������
    /// </summary>
    private void Start()
    {
        _stageSO = DataScript._date.GetStageSOs[DataScript._date._nowStageID];
        //�i�s�x������
        _nowProgress = -1;
        //�X�e�[�W�{�X���o������i�s�x���擾
        _battleNum = _stageSO.GetAppearEnemyTables.Count;
        //FieldAreaManagerScript���擾����
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //�e�R���|�[�l���g������
        _fieldAreaScript.Init(this);
        _characterManager.Init(this);
        _playBoardManager.Init(this);
        _bossIncoming.Init();
        _stageClear.Init();
        _gameOver.Init();
        _rewardPanel.Init(this);
        //�o���l�E����UI������
        ApplyEXPText();
        ApplyGoldText();
        //�X�e�[�W���\��
        ApplyStageUIs();
        //�G����ʂɏo��������
        DOVirtual.DelayedCall(
            1.0f, //1�b�x��
            () =>
            {
                ProgressingStage();
            }, false
        );
        //�X�e�[�WBGM�Đ�
        AudioSource audioSource = GetComponent<AudioSource>();
        //BGM�N���b�v�ݒ�
        audioSource.clip = _stageSO.GetBGM;
        //�Đ�
        audioSource.Play(); 
    }

    #region �X�e�[�W�i�s�֘A
    /// <summary>
	/// �X�e�[�W�̐i�s�x��i�߂Đ퓬���J�n���邩�X�e�[�W���I������
	/// </summary>
	public void ProgressingStage()
    {
        //�i�s�x��i�߂�
        _nowProgress++;

        //�i�s�x��0.0f~1.0f�Ŏ擾���ĕ\��
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
            //�X�e�[�W�N���A���o�J�n
            _stageClear.StartAnimation();
        }

        //�G�L�����N�^�[�o������
        //�o������G������
        List<EnemyStatusSO> appearEnemyTable = _stageSO.GetAppearEnemyTables[_nowProgress].GetAppearEnemys;
        int rand = Random.Range(0, appearEnemyTable.Count);
        _characterManager.SpawnEnemy(appearEnemyTable[rand]);
        //�o������G�����ނ������Ȃ��Ȃ�{�X�p���o
        if (appearEnemyTable.Count == 1)
        {
            _bossIncoming.StartAnimation();
        }

        //�퓬�J�n����
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
            _fieldAreaScript.DestroyAllCards();

            //�����L�����N�^�[�ʏ���(�x�����s)
            DOVirtual.DelayedCall(
                0.5f,
                () =>
                {
                    // �v���C���[�s�k��
                    if (isPlayerLose)
                    {
                        //�Q�[���I�[�o�[���o�J�n
                        _gameOver.StartAnimation();
                    }
                    // �v���C���[������
                    else if (isPlayerWin)
                    {
                        //������ʕ\��
                        _rewardPanel.OpenWindow(_characterManager.GetEnemyDate);
                    }
                }
            );
            return;
        }
        //���̃^�[���J�n
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
    /// <summary>
	/// �o���l��Text�̕\�����X�V����
	/// </summary>
	public void ApplyEXPText()
    {
        //�������������ω����鉉�o
        DOTween.To(() =>
            _playerExpDisp, (n) => _playerExpDisp = n, DataScript._date.GetPlayerExp, AnimationTime)
            .OnUpdate(() =>
            {
                _playerExpText.text = _playerExpDisp.ToString("#,0") + " EXP";
            });
    }
    /// <summary>
    /// ��������Text�̕\�����X�V����
    /// </summary>
    public void ApplyGoldText()
    {
        //�������������ω����鉉�o
        DOTween.To(() =>
            _playerGoldDisp, (n) => _playerGoldDisp = n, DataScript._date.GetPlayerGold, AnimationTime)
            .OnUpdate(() =>
            {
                _playerGoldText.text = _playerGoldDisp.ToString("#,0") + " G";
            });
    }
    #endregion

    #region �X�e�[�W�퓬��V
    /// <summary>
	/// �G���j�{�[�i�X�̓�����ݗʂ�Ԃ�
	/// </summary>
	public int GetBonusGoldValue()
    {
        //��{�l����
        int value = _stageSO.GetGold * (BonusValueBase + _nowProgress);
        //�����_�����K�p
        value = (int)(value * Random.Range(BonusRandomMulti_Min, BonusRandomMulti_Max));
        return value;
    }
    /// <summary>
	/// �G���j�{�[�i�X�̓���o���l�ʂ�Ԃ�
	/// </summary>
	public int GetBonusEXPValue()
    {
        //��{�l����
        int value = _stageSO.GetExp * (BonusValueBase + _nowProgress);
        //�����_�����K�p
        value = (int)(value * Random.Range(BonusRandomMulti_Min, BonusRandomMulti_Max));
        return value;
    }
    /// <summary>
    /// �G���j�{�[�i�X��HP�񕜗ʂ�Ԃ�
    /// </summary>
    public int GetBonusHealValue()
    {
        //��{�񕜗�
        int value = _stageSO.GetHeal;
        //�����_�����K�p
        value = (int)(value * Random.Range(BonusRandomMulti_Min, BonusRandomMulti_Max));
        return value;
    }
    #endregion
}
