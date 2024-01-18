using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CharacterManagerScript : MonoBehaviour
{
    [SerializeField, Header("�v���C���[�X�e�[�^�X")]
    private StatusUIScript _playerStatusUI = default;
    [SerializeField, Header("�G�X�e�[�^�X")]
    private StatusUIScript _enemyStatusUI = default;
    [SerializeField, Header("����HP")]
    private int _fastHP = 30;
    //�퓬�̃}�l�[�W���[
    private BattleManagerScript _battleManager = default;
    //�G�̒�`�f�[�^
    private EnemyStatusSO _enemyDate = default;
    //���݂�HP�f�[�^
    private int[] _nowHP = default;
    //�ő�HP�f�[�^
    private int[] _maxHP = default;

    //�U�����o���x
    public const float _shakeAnimPower = 18.0f;
    //�U�����o����
    public const float _shakeAnimTime = 0.4f;
    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="battleManeger">�o�g���}�l�[�W���[</param>
    public void Init(BattleManagerScript battleManeger)
    {
        //BattleManagerScript�Q�Ǝ擾
        _battleManager = battleManeger;

        // �ϐ�������
        _nowHP = new int[CardScript.CharaNum];
        _maxHP = new int[CardScript.CharaNum];
        ResetHP_Player();

        //UI������
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
        // �G�X�e�[�^�X���\��
        _enemyStatusUI.HideCanvasGroup(false);
    }
    /// <summary>
	/// �v���C���[��HP������������
	/// </summary>
	public void ResetHP_Player()
    {
        //HP������
        _maxHP[CardScript.CharaID_Player] = _fastHP;
        _nowHP[CardScript.CharaID_Player] = _maxHP[CardScript.CharaID_Player];
        //HP�\��
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
    }
    /// <summary>
    /// ����HP��ύX����
    /// </summary>
    /// <param name="charaID">�L����ID</param>
    /// <param name="value">�ω���</param>
    public void ChangeStatus_NowHP(int charaID, int value)
    {
        //���Ɍ���HP0�ȉ��Ȃ珈�����Ȃ�
        if (_nowHP[charaID] <= 0)
        {
            return;
        }
        //����HP�ύX
        _nowHP[charaID] += value;
        //�ő�HP���z���Ȃ��悤�ɂ���
        if (_nowHP[charaID] > _maxHP[charaID])
        {
            _nowHP[charaID] = _maxHP[charaID];
        }
        //UI���f
        if (charaID == CardScript.CharaID_Player)
        {
            //�v���C���[��HP
            _playerStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }
        else
        {
            //�G��HP
            _enemyStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }

    }/// <summary>
     /// �ő�HP��ύX����
     /// </summary>
     /// <param name="charaID">�L����ID</param>
     /// <param name="value">�ω���</param>
    public void ChangeStatus_MaxHP(int charaID, int value)
    {
        //���ɍő�HP0�Ȃ珈�����Ȃ�
        if (_maxHP[charaID] <= 0)
        {
            return;
        }

        //�ő�HP�ύX
        _maxHP[charaID] += value;
        //����HP�̏���E�����𔽉f
        _nowHP[charaID] = Mathf.Clamp(_nowHP[charaID], 0, _maxHP[charaID]);

        // UI���f
        if (charaID == CardScript.CharaID_Player)
        {
            //�v���C���[��HP
            _playerStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }
        else
        {
            //�G��HP
            _enemyStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }
    }
    #region �G�ւ̏���
    /// <summary>
    /// �G�̏o�����s��
    /// </summary>
    /// <param name="spawnEnemyData">�o��������G�̃f�[�^</param>
    public void SpawnEnemy(EnemyStatusSO spawnEnemyData)
    {
        //�G�f�[�^�擾
        _enemyDate = spawnEnemyData;

        //�G�X�e�[�^�X������
        _nowHP[CardScript.CharaID_Enemy] = _enemyDate.GetHP;
        _maxHP[CardScript.CharaID_Enemy] = _enemyDate.GetHP;

        //�G�X�e�[�^�XUI�\��
        _enemyStatusUI.ShowCanvasGroup();
        _enemyStatusUI.SetCharacterName(_enemyDate.GetEnemyName);
        _enemyStatusUI.SetHPView(_nowHP[CardScript.CharaID_Enemy], _maxHP[CardScript.CharaID_Enemy]);
    }
    /// <summary>
	/// �G����������
	/// </summary>
	public void DeleteEnemy()
    {

    }
    #endregion

    #region �e�픻��
    /// <summary>
    /// �v���C���[��HP��0�ȉ���
    /// </summary>
    /// <returns>�v���C���[�s�k�t���O</returns>
    public bool IsPlayerDefeated()
    {
        if (_nowHP[CardScript.CharaID_Player] <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// �G��HP��0�ȉ���
    /// </summary>
    /// <returns>�G���j�t���O</returns>
    public bool IsEnemyDefeated()
    {
        if (_nowHP[CardScript.CharaID_Enemy] <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
