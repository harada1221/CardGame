using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageClearScript : MonoBehaviour
{
    //UI�I�u�W�F�N�g
    [SerializeField, Header("�I�u�W�F�N�g�̃O���[�v")]
    private CanvasGroup _canvasGroup = default;
    [SerializeField, Header("�e�L�X�g��RectTransform")]
    private RectTransform _logoRectTransform = default;
    [SerializeField, Header("�\������e�L�X�g")]
    private Text _clearText = default;
    [SerializeField, Header("�^�C�g���֖߂�{�^����RectTransform")]
    private RectTransform _titleButtonRectTransform = default;


    //���S�摜����Scale�l
    private const float LogoStartScale = 3.0f;
    //���S�摜�I��Scale�l
    private const float LogoEndScale = 1.0f;
    //�{�^���ړ���(Y����)
    private const float ButtonMovePosY = 200.0f;
    /// <summary>
    /// ����������
    /// </summary>
    public void Init()
    {
        // UI��\��
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// �X�e�[�W�N���A���ɌĂяo����鏈��
    /// </summary>
    public void StartAnimation()
    {
        //����UI�ݒ�
        gameObject.SetActive(true);
        //CanvasGroup�ݒ�
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _logoRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        _clearText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //�{�^���ݒ�
        Vector2 buttonPos = _titleButtonRectTransform.anchoredPosition;
        buttonPos.y -= ButtonMovePosY;
        _titleButtonRectTransform.anchoredPosition = buttonPos;

        //UI�\���A�j���[�V����
        Sequence sequence = DOTween.Sequence();
        //�p�l����\��
        sequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        //���S��\��
        sequence.Append(_logoRectTransform.DOScale(LogoEndScale, 0.6f)
            .SetEase(Ease.OutCubic));
        sequence.Join(_clearText.DOFade(1.0f, 0.6f));
        //���S��h�炷
        sequence.Append(_logoRectTransform.DOShakeAnchorPos(0.5f, 60, 30));
        //�҂�����
        sequence.AppendInterval(1.0f);
        //�{�^�����ړ�
        sequence.Join(_titleButtonRectTransform.DOAnchorPosY(ButtonMovePosY, 0.5f)
            .SetRelative());
    }

    /// <summary>
    /// �^�C�g����ʂֈړ�����
    /// </summary>
    public void GoTitleScene()
    {
        //�^�C�g���V�[���ɐ؂�ւ���
        SceneManager.LoadScene("TitleScene");
    }
}
