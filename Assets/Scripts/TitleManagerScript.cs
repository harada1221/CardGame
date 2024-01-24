using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManagerScript : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W�Z���N�g�E�B���h�E�N���X")]
    private StageSelectWindowScript _stageSelectWindow = default;

    //�A�j���[�V�����pUI�I�u�W�F�N�g
    [SerializeField, Header("�^�C�g�����SRectTransform")]
    private RectTransform _titleLogoRectTransform = default;
    [SerializeField, Header("�{�^���w�iRectTransform���X�g")]
    private List<RectTransform> _titleButtonUIs = default;

    [SerializeField, Header("�E�B���h�E�w�i�p�l���I�u�W�F�N�g")]
    private GameObject _windowBackObject = default;

    //�e��{�^���֘AUI��X�����ړ���
    private const float ButtonsUIMoveLengthX = 400.0f;
    //�^�C�g�����S���o����
    private const float TitleLogoTweenTime = 3.0f;
    //�e��{�^���֘AUI���o����
    private const float ButtonsUITweenTime = 1.0f;
    //�e��{�^���֘AUI�ɉ��o�����s���鎞�ԊԊu
    private const float ButtonsUITweenDistance = 0.15f;
    /// <summary>
    /// ����������
    /// </summary>
    void Start()
    {
        //�Ǘ����R���|�[�l���g������
        _stageSelectWindow.Init(this);

        //�Q�[���N�����̃A�j���[�V�������Đ�
        InitAnimation();

        //�E�B���h�E�w�i�I�u�W�F�N�g�𖳌���
        SetWindowBackPanelActive(false);
    }

    /// <summary>
    /// �Q�[���N�����A�j���[�V����
    /// </summary>
    private void InitAnimation()
    {
        //�^�C�g�����S�e�L�X�g
        Text titleLogoText = _titleLogoRectTransform.GetComponent<Text>();
        //�N���A�j���[�V����Sequence
        Sequence initSequence = DOTween.Sequence();

        //�^�C�g�����S��傫���E�����ɂ���
        _titleLogoRectTransform.transform.localScale = new Vector2(1.8f, 1.8f);
        titleLogoText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //�e��{�^���֘AUI����ʉ����ɑޔ�
        int buttonUIsLength = _titleButtonUIs.Count;
        for (int i = 0; i < buttonUIsLength; i++)
        {
            _titleButtonUIs[i].anchoredPosition += new Vector2(0.0f, -ButtonsUIMoveLengthX);
        }

        //�A�j���[�V�����ݒ�
        //�^�C�g�����S�̑傫���ύX
        initSequence.Append(
            _titleLogoRectTransform.DOScale(1.0f, TitleLogoTweenTime));
        //�^�C�g�����S�̔񓧖���
        initSequence.Join(
            titleLogoText.DOFade(1.0f, TitleLogoTweenTime));
        //�e��{�^���֘AUI����ʉ�������ړ�
        for (int i = 0; i < buttonUIsLength; i++)
        {
            initSequence.Join(
                _titleButtonUIs[i].DOAnchorPosY(ButtonsUIMoveLengthX, ButtonsUITweenTime)
                .SetRelative() // ���΍��W�w��ɂ���
                .SetDelay(ButtonsUITweenDistance * i)); // �������x�����Ă���
        }
    }

    /// <summary>
    /// �E�B���h�E�w�i�p�l���I�u�W�F�N�g��L�����E����������
    /// </summary>
    public void SetWindowBackPanelActive(bool mode)
    {
        _windowBackObject.SetActive(mode);
    }
}
