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
using DG.Tweening;

public class BattleManagerScript : MonoBehaviour
{
    //�t�B�[���h�̊Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;
    [SerializeField, Header("�L�����N�^�[�f�[�^�Ǘ��N���X")]
    private CharacterManagerScript _characterManager = default;
    //�J�[�h�f�[�^���擾
    //[SerializeField, Header("�J�[�h���X�g")]
    //private CardDataSO _cardDate = default;
    [SerializeField, Header("�o���G�f�[�^")]
    private EnemyStatusSO _enemyStatusSO = default;
    [SerializeField,Header("�J�[�h���ʔ����Ǘ��N���X")]
    private PlayBoardManagerScript _playBoardManager = default;


    public FieldAreaManagerScript GetFieldManager { get => _fieldAreaScript; }
    public CharacterManagerScript GetCharacterManager { get => _characterManager; }
    public PlayBoardManagerScript GetPlayBoardManager { get => _playBoardManager; }
    /// <summary>
    /// ����������
    /// </summary>
    private void Start()
    {
        //FieldAreaManagerScript���擾����
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //�R���|�[�l���g������
        _fieldAreaScript.InBattleManager(this);
        _characterManager.Init(this);
        _playBoardManager.Init(this);
        // (�f�o�b�O�p)�G����ʂɏo��������
        DOVirtual.DelayedCall(
            1.0f, // 1�b�x��
            () =>
            {
                // �G�o��
                _characterManager.SpawnEnemy(_enemyStatusSO);
            }, false
        );
    }
    /// <summary>
    /// �X�V����
    /// </summary>
    private void Update()
    {
        // (�f�o�b�O�p)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _characterManager.ChangeStatus_NowHP(CardScript.CharaID_Player, -5);
        }
    }
}
