//-----------------------------------------------------------------------
/* �L�����̃X�e�[�^�X�\��UI���Ǘ�����N���X
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

public class StatusUIScript : MonoBehaviour
{
    [SerializeField, Header("HP�Q�[�WImage")]
    private Image _hpGageImage = default;
    [SerializeField, Header("HP�\��Text")]
    private Text _hpText = default;

    //�G�L�����N�^�[�p�p�����[�^
    [Space(10)]
    [Header("�G�L�����N�^�[�p")]
    [SerializeField, Header("CanvasGroup")]
    private CanvasGroup _enemyCanvasGroup = default;
    [SerializeField, Header("�L�����N�^�[��Text")]
    private Text _enemyCharaNameText = default;
    //�t�F�[�h���oTween
    private Tween _fadeTween = default;
    //�t�F�[�h���o����
    private const float _fadeTime = 0.8f;

    /// <summary>
    /// HP��\������
    /// </summary>
    /// <param name="nowHP">����HP</param>
    /// <param name="maxHP">�ő�HP</param>
    public void SetHPView(int nowHP, int maxHP)
    {
        //HP�\���̍ŏ��l��ݒ�
        if (nowHP < 0)
        {
            nowHP = 0;
        }
        if (maxHP < 0)
        {
            nowHP = 0;
        }

        //�Q�[�W�\��
        //�ő�HP�ɑ΂��錻��HP�̊���
        float ratio = 0.0f;
        // 0���Z�ɂȂ�Ȃ��悤��
        if (maxHP > 0)
        {
            ratio = (float)nowHP / maxHP;
            _hpGageImage.fillAmount = ratio;
        }
        // Text�\��
        _hpText.text = nowHP + " / " + maxHP;
    }

    #region �G�X�e�[�^�X�\����p����

    /// <summary>
    /// �L�������̕\��������
    /// </summary>
    /// <param name="charaName">�\�����閼�O</param>
    public void SetCharacterName(string charaName)
    {
        //�L�������\��
        if (_enemyCharaNameText != null)
        {
            _enemyCharaNameText.text = charaName;
        }
    }

    /// <summary>
    /// �SUI��\��
    /// </summary>
    public void ShowCanvasGroup()
    {
        //�A�j���[�V�������I��点��
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        //�SUI�\���A�j���[�V����
        _fadeTween = _enemyCanvasGroup.DOFade(1.0f, _fadeTime);
    }

    /// <summary>
    /// �SUI���\��
    /// </summary>
    /// <param name="isAnimation">�t�F�[�h���o���s�t���O</param>
    public void HideCanvasGroup(bool isAnimation)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        //isAnimation��true�̎��̂ݑSUI��\��
        if (isAnimation == true)
        {
            _fadeTween = _enemyCanvasGroup.DOFade(0.0f, _fadeTime);
        }
        else
        {
            _enemyCanvasGroup.alpha = 0.0f;
        }
    }
    #endregion
}
