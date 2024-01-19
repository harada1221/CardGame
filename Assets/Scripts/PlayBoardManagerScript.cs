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
    //�퓬�̃}�l�[�W���[�X�N���v�g
    private BattleManagerScript _battleManager = default;
    //�t�B�[���h�̃}�l�[�W���[�X�N���v�g
    private FieldAreaManagerScript _fieldManager = default;
    //�v���C���[�̃}�l�[�W���[�X�N���v�g
    private CharacterManagerScript _characterManager = default;
    //�J�[�h���ʎ��sSequence
    private Sequence _playSequence = default;

    //�v���C�{�[�h���̃J�[�h�g��
    public const int PlayBoardCardNum = 5;
    //�e�J�[�h���s�̎��ԊԊu
    private const float PlayIntervalTime = 0.2f;

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
    }
    /// <summary>
    /// �v���C�{�[�h��̃J�[�h�������珇�ԂɌ��ʔ�������
    /// </summary>
    /// <param name="boardCardList">�����ΏۂƂȂ�J�[�h�z��</param>
    public void BoardCardsPlay(CardScript[] boardCards)
    {
        //���ԂɃJ�[�h�̌��ʂ����s����(Sequence)
        _playSequence = DOTween.Sequence();
        //�{�[�h�̖������J��Ԃ�
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            //���ݎ��s���̔z����ԍ����L��
            int index = i;

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
    }
    private bool PlayCard(CardScript targetCard, int useCharaID, int boardIndex)
    {
        //����L�����N�^�[��ID
        int targetCharaID = useCharaID ^ 1;

        //�J�[�h�̊e���ʗ�
        int damagePoint = 0;

        //�J�[�h���̂��ꂼ��̌��ʂ����s
        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                //���xn����
                case CardEffectDefineScript.CardEffect.ForceEqual:
                    //�J�[�h���x���w��̐��l�ƈقȂ�Ȃ�S�Ă̌��ʂ𖳌�
                    if (effect.GetValue != targetCard.GetForcePoint)
                    {
                        return false;
                    }

                    break;
            }
        }

        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                case CardEffectDefineScript.CardEffect.Damage:
                    //�_���[�W�ʌ���
                    damagePoint += effect.GetValue;
                    break;
            }
        }
        //�_���[�W��^����
        _characterManager.ChangeStatus_NowHP(targetCharaID, -damagePoint);

        return true;
    }
    /// <summary>
	/// �J�[�h�̌��ʎ��s���Ȃ�true��Ԃ�
	/// </summary>
	public bool IsPlayingCards()
    {
        if (_playSequence != null)
        {
            return _playSequence.IsPlaying();
        }
        return false;
    }
}
