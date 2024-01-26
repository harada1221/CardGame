using UnityEngine;
using DG.Tweening;

public class BossIncomingScript : MonoBehaviour
{
    [SerializeField, Header("UI�I�u�W�F�N�g    ")]
    private CanvasGroup _canvasGroup = default;
    [SerializeField, Header("�e�L�X�g��RectTransform")]
    private RectTransform _textRectTransform = default;

    [SerializeField,Header("�e�L�X�g�̏����ʒu")]
    private Vector2 _textBasePosition = default;

    private const float LogoStartScale = 1.0f;
    private const float LogoEndScale = 3.0f;
    
    /// <summary>
    /// ����������
    /// </summary>
    public void Init()
    {
        //UI��\��
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
    public void StartAnimation()
    {
        //UI�\��
        gameObject.SetActive(true);
        //CanvasGroup�ݒ�
        _canvasGroup.alpha = 0.0f;
        //�e�L�X�g�\��
        _textRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        //�e�L�X�g�̏������W
        _textRectTransform.anchoredPosition = _textBasePosition;

        //UI�\���A�j���[�V����
        Sequence sequence = DOTween.Sequence();
        //�p�l����\��
        sequence.Append(_canvasGroup.DOFade(1.0f, 0.5f));
        //���S���ړ�
        sequence.Append(_textRectTransform.DOAnchorPos(Vector2.zero, 1.0f)
            .SetEase(Ease.OutQuint));
        //�҂�����
        sequence.AppendInterval(0.6f);
        //�p�l�����\��
        sequence.Join(_canvasGroup.DOFade(0.0f, 1.0f));

    }
}
