//-----------------------------------------------------------------------
/* �L�����N�^�[���Ǘ�����N���X
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
public class CharacterManagerScript : MonoBehaviour
{
    [SerializeField, Header("���������G�I�u�W�F�N�g�̐e")]
    private Transform _enemyPictureParent = default;
    [SerializeField, Header("�G�L������Prehub")]
    private GameObject _enemyPicturePrefab = default;
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
    //�o�����̓G�I�u�W�F�N�g�����N���X
    private EnemyPictureScript _enemyPicture = default;

    public int[] GetNowHP { get => _nowHP; }
    public int[] GetMaxHP { get => _maxHP; }
    public EnemyStatusSO GetEnemyDate { get => _enemyDate; }

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
	/// �^�[���I�����Ɏ��s����鏈��
	/// </summary>
	public void OnTurnEnd()
    {
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

        //�_���[�W���o
        if (charaID == CardScript.CharaID_Enemy)
        {
            //���j���o
            if (IsEnemyDefeated())
            {
                _enemyPicture.DefeatAnimation();
            }
            //��_���[�W���o
            else if (value < 0)
            {
                //_enemyPicture.DamageAnimation();
            }
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
        //�_���[�W���o
        if (charaID == CardScript.CharaID_Enemy)
        {
            //���j���o
            if (IsEnemyDefeated())
            {
                _enemyPicture.DefeatAnimation();
            }
            //��_���[�W���o
            else if (value < 0)
            {
                //_enemyPicture.DamageAnimation();
            }
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

        //�G�摜�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_enemyPicturePrefab, _enemyPictureParent);
        //�G�摜�����N���X�擾
        _enemyPicture = obj.GetComponent<EnemyPictureScript>();
        //�G�摜�����N���X������
        _enemyPicture.Init(this, _enemyDate.GetCharaSprite);

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
        //�I�u�W�F�N�g�폜
        _enemyPicture.gameObject.SetActive(false);
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
    /// <summary>
	/// �G�摜�̍��W��Ԃ�
	/// </summary>
	public Vector2 GetEnemyPosition()
    {
        return _enemyPicture.transform.position;
    }
    #endregion
    /// <summary>
	/// �v���C���[�̍ő�HP�����ɖ߂�
	/// </summary>
	public void RecoverMaxHP_Player()
    {
        //�J���I�yHP
        _maxHP[CardScript.CharaID_Player] = 30;
        //�傫��������ő�l�ɂ���
        if (_nowHP[CardScript.CharaID_Player] > _maxHP[CardScript.CharaID_Player])
        {
            _nowHP[CardScript.CharaID_Player] = _maxHP[CardScript.CharaID_Player];
        }
        //HP�\��
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
    }
}
