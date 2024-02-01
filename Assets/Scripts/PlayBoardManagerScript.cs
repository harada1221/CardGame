//-----------------------------------------------------------------------
/* �v���C�{�[�h�̏������Ǘ�����N���X
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

public class PlayBoardManagerScript : MonoBehaviour
{
    [SerializeField, Header("�v���C�{�[�h�̃J�[�h�]�[���G���A")]
    private CardZoneScript[] _boardCardZones = default;
    [SerializeField, Header("���ʔ������J�[�h��Transform")]
    private Transform _playingCardFrameTrs = default;
    //�퓬�̃}�l�[�W���[�X�N���v�g
    private BattleManagerScript _battleManager = default;
    //�t�B�[���h�̃}�l�[�W���[�X�N���v�g
    private FieldAreaManagerScript _fieldManager = default;
    //�v���C���[�̃}�l�[�W���[�X�N���v�g
    private CharacterManagerScript _characterManager = default;
    //�J�[�h���ʎ��sSequence
    private Sequence _playSequence = default;
    //�J�[�h�]�[����Transform
    private Transform[] _cardZonesTrs = default;

    //�v���C�{�[�h���̃J�[�h�g��
    public const int PlayBoardCardNum = 5;
    //�e�J�[�h���s�̎��ԊԊu
    private const float PlayIntervalTime = 0.2f;
    //�t���[���I�u�W�F�N�g�̏����ʒu
    private const float FrameObjPosition_FixX = 8.0f;
    //�t���[���I�u�W�F�N�g�ړ��A�j���[�V��������
    const float FrameAnimTime = 0.3f;
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="battleManager"></param>
    public void Init(BattleManagerScript battleManager)
    {
        // �Q�Ǝ擾
        _battleManager = battleManager;
        _fieldManager = _battleManager.GetFieldManager;
        _characterManager = _battleManager.GetCharacterManager;

        // �e�J�[�h�]�[����Transform���擾
        _cardZonesTrs = new Transform[PlayBoardCardNum];
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            _cardZonesTrs[i] = _boardCardZones[i].transform;
        }
        // �t���[���I�u�W�F�N�g���J�n�ʒu�Ɉړ�
        _playingCardFrameTrs.position = GetFrameObjectPos_Start();
    }
    /// <summary>
    /// �v���C�{�[�h��̃J�[�h�������珇�ԂɌ��ʔ�������
    /// </summary>
    /// <param name="boardCardList">�����ΏۂƂȂ�J�[�h�z��</param>
    public void BoardCardsPlay(CardScript[] boardCards)
    {
        //�t���[���I�u�W�F�N�g���J�n�ʒu�Ɉړ�
        _playingCardFrameTrs.position = GetFrameObjectPos_Start();
        //���ԂɃJ�[�h�̌��ʂ����s����(Sequence)
        _playSequence = DOTween.Sequence();
        //�{�[�h�̖������J��Ԃ�
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            //���ݎ��s���̔z����ԍ����L��
            int index = i;
            // �t���[���I�u�W�F�N�g�ړ�
            _playSequence.Append(_playingCardFrameTrs
                .DOMove(GetPlayZonePos(i), FrameAnimTime) // �ړ�Tween
                .SetEase(Ease.OutQuart)); // �ω��̎d�����w��

            //�����ΏۃJ�[�h���Ƃ̏���(�J�[�h���Ȃ��Ȃ�X�L�b�v)
            if (boardCards[index] != null)
            {
                //���̃J�[�h�̎g�p�҂̃L�����N�^�[ID���擾
                int useCharaID = boardCards[index].GetControllerCharaID;

                //�J�[�h���ʔ���
                _playSequence.AppendCallback(() =>
                {
                    //���ʔ���
                    PlayCard(boardCards[index], useCharaID, index);
                });
                // �J�[�h�𔼓�����Tween
                _playSequence.Append(boardCards[index].HideFadeTween());

                //�퓬�I�������𖞂������m�F
                _playSequence.AppendCallback(() =>
                {
                    //�v���C���[�̔s�k���G�����j������
                    if (_characterManager.IsPlayerDefeated() ||
                        _characterManager.IsEnemyDefeated())
                    {
                        //�V�[�P���X�����I��
                        _playSequence.Complete();
                        _playSequence.Kill();
                        return;
                    }
                });
            }
            else
            {//�J�[�h�����݂��Ȃ�
            }

            //���ԊԊu��ݒ�
            _playSequence.AppendInterval(PlayIntervalTime);
        }
        //�t���[���I�u�W�F�N�g���I���ʒu�Ɉړ�
        _playSequence.Append(_playingCardFrameTrs
            .DOMove(GetFrameObjectPos_End(), FrameAnimTime) // �ړ�Tween
            .SetEase(Ease.OutQuart)); // �ω��̎d�����w��
                                      // Sequence�I��������
        _playSequence.OnComplete(() =>
        {
            //�{�[�h��̃J�[�h�I�u�W�F�N�g��S�č폜
            foreach (CardScript card in boardCards)
            {
                if (card != null)
                {
                    _fieldManager.DestroyCardObject(card);
                }

            }
            //�^�[���I�������ďo
            _battleManager.TurnEnd();
        });
    }
    private bool PlayCard(CardScript targetCard, int useCharaID, int boardIndex)
    {
        //����L�����N�^�[��ID
        int targetCharaID = useCharaID ^ 1;

        int damagePoint = 0;//�^�_���[�W��
        int selfDamagePoint = 0;//���g�ւ̗^�_���[�W��
        int healPoint = 0;      //�񕜗�
        int burnPoint = 0;      //�ő�HP�ւ̃_���[�W��
        int selfBurnPoint = 0;  //���g�̍ő�HP�ւ̃_���[�W��
        int weakPoint = 0;      //�_���[�W��̉���
        int damageMulti = 1;    //�_���[�W�{��
        bool isAbsorption = false;  //�̗͋z���t���O
        //bool isBloodPact = false;   //�񕜁̃_���[�W�ύX�t���O
        //bool isReflection = false;  //�����_���[�W�t���O

        //�J�[�h���̂��ꂼ��̌��ʂ����s
        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                #region ���x�����n
                case CardEffectDefineScript.CardEffect.ForceEqual: //���xn����
                    if (effect.GetValue != targetCard.GetForcePoint)
                    {
                        //�J�[�h���x���w��̐��l�ƈقȂ�Ȃ�S�Ă̌��ʂ𖳌�
                        return false;
                    }
                    break;

                case CardEffectDefineScript.CardEffect.ForceHigher: // ���xn�ȏ�
                    if (effect.GetValue > targetCard.GetForcePoint)
                    {
                        //�J�[�h���x���w��͈̔͊O�Ȃ�S�Ă̌��ʂ𖳌�
                        return false;
                    }
                    break;

                case CardEffectDefineScript.CardEffect.ForceLess: // ���xn�ȉ�
                    if (effect.GetValue < targetCard.GetForcePoint)
                    {
                        //�J�[�h���x���w��͈̔͊O�Ȃ�S�Ă̌��ʂ𖳌�
                        return false;
                    }
                    break;
                    #endregion
            }
        }
        //�ʏ����
        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                #region �_���[�W�n���ʏ���
                case CardEffectDefineScript.CardEffect.Damage: // �_���[�W
                    damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.WeaponDmg: // ����_���[�W
                    damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.Assault: // �ˌ�
                    if (boardIndex == 0)
                        damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.Pursuit: // �ǌ�
                    if (boardIndex == PlayBoardCardNum - 1)
                        damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.CatchingUp: // �쒀
                    if (_characterManager.GetNowHP[targetCharaID] * 2 <= _characterManager.GetMaxHP[targetCharaID])
                    {
                        damagePoint += effect.GetValue;
                    }
                    break;

                case CardEffectDefineScript.CardEffect.Burn: // �Ώ�
                    burnPoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.DoubleDamage: // �_���[�W�Q�{
                    damageMulti *= 2;
                    break;

                case CardEffectDefineScript.CardEffect.TripleDamage: // �_���[�W�R�{
                    damageMulti *= 3;
                    break;

                case CardEffectDefineScript.CardEffect.Hypergravity: // ���d��
                    damagePoint += _characterManager.GetNowHP[targetCharaID] / 4;
                    break;

                case CardEffectDefineScript.CardEffect.SelfInjury: // ����
                    selfDamagePoint += effect.GetValue;
                    break;
                #endregion

                #region �񕜌n
                case CardEffectDefineScript.CardEffect.Heal: // ��
                    healPoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.SelfPredation: // ���ȕߐH
                                                                      // HP�ő�l����
                    selfBurnPoint += _characterManager.GetMaxHP[useCharaID] / 2;
                    // ��
                    healPoint += _characterManager.GetMaxHP[useCharaID] - _characterManager.GetNowHP[useCharaID];
                    break;

                case CardEffectDefineScript.CardEffect.Absorption: // �z��
                    isAbsorption = true;
                    break;

                case CardEffectDefineScript.CardEffect.BloodPact: // ���̖���
                    //isBloodPact = true;
                    break;
                #endregion

                #region �x���E�W�Q�n
                case CardEffectDefineScript.CardEffect.Weakness: // ��̉�
                    weakPoint += effect.GetValue;
                    break;
                    #endregion
            }
        }
        //�e��v�Z���l��Ώۂ��ƂɓK�p
        //�ő�HP�ւ̃_���[�W
        _characterManager.ChangeStatus_MaxHP(targetCharaID, -burnPoint);
        //�_���[�W
        _characterManager.ChangeStatus_NowHP(targetCharaID, -damagePoint);
        //�����̍ő�HP�ւ̃_���[�W
        _characterManager.ChangeStatus_MaxHP(useCharaID, -selfBurnPoint);
        //�����ւ̃_���[�W
        _characterManager.ChangeStatus_NowHP(useCharaID, -selfDamagePoint);
        //��
        _characterManager.ChangeStatus_NowHP(useCharaID, healPoint);
        //�̗͋z��
        if (isAbsorption)
        {
            _characterManager.ChangeStatus_NowHP(useCharaID, damagePoint);
        }

        return true;
    }
    /// <summary>
	/// �J�[�h�̌��ʎ��s���Ȃ�true��Ԃ�
	/// </summary>
	public bool IsPlayingCards()
    {
        if (_playSequence != null && _playSequence.IsActive() && _playSequence.IsPlaying())
        {
            return _playSequence.IsPlaying();
        }
        return false;
    }

    /// <summary>
	/// �Ώۃv���C�]�[����Vector2���W���擾
	/// </summary>
	/// <param name="areaID">�]�[��ID</param>
	/// <returns>Position�l</returns>
	public Vector2 GetPlayZonePos(int areaID)
    {
        return _cardZonesTrs[areaID].position;
    }

    /// <summary>
    /// �J�n�ʒu���擾
    /// </summary>
    /// <returns>Position�̊J�n�ʒu</returns>
    private Vector2 GetFrameObjectPos_Start()
    {
        Vector2 res = _cardZonesTrs[0].position;
        res.x -= FrameObjPosition_FixX;
        return res;
    }
    /// <summary>
    /// �I���ʒu���擾
    /// </summary>
    /// <returns>Position�̏I���ʒu</returns>
    private Vector2 GetFrameObjectPos_End()
    {
        Vector2 res = _cardZonesTrs[PlayBoardCardNum - 1].position;
        res.x += FrameObjPosition_FixX;
        return res;
    }
}
