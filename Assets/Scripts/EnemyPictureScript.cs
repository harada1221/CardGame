//-----------------------------------------------------------------------
/* �G�L�����N�^�[�̉摜�I�u�W�F�N�g���Ǘ��N���X
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

public class EnemyPictureScript : MonoBehaviour
{
    //�L�����N�^�[�}�l�[�W���[
    private CharacterManagerScript _characterManager = default;
    [SerializeField, Header("�o���ʒu")]
    private RectTransform _rectTransform = default;
    [SerializeField, Header("�o������G�̃C���[�W")]
    private Image _enemyImage = default;
    //�o���ʒu�̏������W
    private Vector2 _basePosition = default;
    //��_���[�W���̃����_���ړ�
    private Sequence _randomMoveSequence = default;

    //�ړ���Y���΍��W
    private const float TargetPositionYRelative = 200.0f;
    //���o����
    private const float AnimTime = 1.0f;
    //�G�����_���ړ��A�j���[�V����
    private const float JumpPosX_Width = 100.0f;    //�ړ����X�����͈�
    private const float JumpPosY_Height = 100.0f;   //�ړ����Y�����͈�
    private const float AnimTime_Move = 0.1f;       //�ړ�����
    private const float AnimTime_Back = 1.2f;   //�߂�̈ړ�����
                                               
    private const float JumpPosX_Relative = 100.0f;    //�W�����v�ړ����X���΍��W
    private const float JumpPosY_Relative = 50.0f;  //�W�����v�ړ����Y���΍��W
    private const float JumpPower = 30.0f;          //�W�����v�̋��x
    private const float EnemyAnimTime = 0.7f;    //���o����
    /// <summary>
    /// �����ݒ�
    /// </summary>
    /// <param name="characterManager">CharacterManagerScript</param>
    /// <param name="enemySprite">�ŏ��ɏo������G</param>
    public void Init(CharacterManagerScript characterManager, Sprite enemySprite)
    {
        //�Q�Ǝ擾
        _characterManager = characterManager;

        //�������W��ۑ�
        _basePosition = _rectTransform.anchoredPosition;

        //�G�摜�\��
        _enemyImage.sprite = enemySprite;
        _enemyImage.SetNativeSize(); // �I�u�W�F�N�g�̑傫������ʂ̑傫���ɍ��킹��

        //�G����ʏ㕔����~��Ă���A�j���[�V����
        //�����ʒu��ݒ�
        Vector2 pos = _basePosition;
        pos.y += TargetPositionYRelative;
        _rectTransform.anchoredPosition = pos;
        //Y�����ړ��A�j���[�V����
        _rectTransform.DOAnchorPosY(-TargetPositionYRelative, AnimTime)
            .SetRelative();
    }
    /// <summary>
	/// ��_���[�W�A�j���[�V�������Đ�����
	/// </summary>
	public void DamageAnimation()
    {
        //Sequence������
        if (_randomMoveSequence != null)
        {
            _randomMoveSequence.Kill();
            _randomMoveSequence = DOTween.Sequence();
        }
        //�����_���ړ����ݒ�
        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x += Random.Range(-JumpPosX_Width / 2.0f, JumpPosX_Width / 2.0f);
        pos.y += Random.Range(-JumpPosY_Height / 2.0f, JumpPosY_Height / 2.0f);
        //�W�����v�ړ��A�j���[�V����(Tween)
        _randomMoveSequence.Append(_rectTransform.DOAnchorPos(pos, AnimTime_Move));

        //���̈ʒu�ɖ߂�A�j���[�V����
        _randomMoveSequence.Append(_rectTransform.DOAnchorPos(_basePosition, AnimTime_Back));
    }
    /// <summary>
	/// ���j���̔�\�����A�j���[�V�������Đ�����
	/// </summary>
	public void DefeatAnimation()
    {
        //�Đ�����Sequence���~
        if (_randomMoveSequence != null)
        {
            _randomMoveSequence.Kill();
        }

        //���j�����o�V�[�P���X������
        Sequence defeatSequence = DOTween.Sequence();
        //�W�����v�ړ����ݒ�
        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x += JumpPosX_Relative;
        pos.y += JumpPosY_Relative;
        //�W�����v�ړ��A�j���[�V����
        defeatSequence.Append(_rectTransform.DOJumpAnchorPos(pos, JumpPower, 1, EnemyAnimTime)
            .SetEase(Ease.Linear)); // �ω��̎d�����w��

        //Scale�k��(Tween)
        defeatSequence.Join(_rectTransform.DOScale(0.0f, EnemyAnimTime)
            .SetEase(Ease.Linear)); // �ω��̎d�����w��

        //�A�j���[�V�����������ɃI�u�W�F�N�g���폜
        defeatSequence.OnComplete(() =>
        {
            _characterManager.DeleteEnemy();
        });
    }
}
