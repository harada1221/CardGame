using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelectWindowScript : MonoBehaviour
{
    //�^�C�g���Ǘ��N���X
    private TitleManagerScript _titleManager = default;
    //�E�B���h�E��RectTransform
    private RectTransform _windowRectTransform = default;
    //�E�B���h�E�pTween;
    private Tween _windowTween = default;
    //�E�B���h�E�\���A�j���[�V��������
    private const float _WindowAnimTime = 0.3f;

    //�������֐�(TitleManager.cs����ďo)
    public void Init(TitleManagerScript _titleManager)
    {
        //�Q�Ǝ擾
        this._titleManager = _titleManager;
        _windowRectTransform = GetComponent<RectTransform>();

        //�E�B���h�E��\��
        _windowRectTransform.transform.localScale = Vector2.zero;
        _windowRectTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// �E�B���h�E��\������
    /// </summary>
    public void OpenWindow()
    {
        //�A�j���[�V�����I��
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }

        //�E�B���h�E�\��Tween
        _windowTween =
            _windowRectTransform.DOScale(1.0f, _WindowAnimTime)
            .SetEase(Ease.OutBack);
        //�E�B���h�E�w�i�p�l����\��
        _titleManager.SetWindowBackPanelActive(true);
    }
    /// <summary>
    /// �E�B���h�E���\���ɂ���
    /// </summary>
    public void CloseWindow()
    {
        //�A�j���[�V�����I��
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }

        //�E�B���h�E��\��Tween
        _windowTween =
            _windowRectTransform.DOScale(0.0f, _WindowAnimTime)
            .SetEase(Ease.InBack);
        //�E�B���h�E�w�i�p�l�����\��
        _titleManager.SetWindowBackPanelActive(false);
    }
}
