using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    //UI�I�u�W�F�N�g
    [SerializeField, Header("�Ή�����CanvasGroup")]
    private CanvasGroup _canvasGroup = null;
    [SerializeField, Header("�e�L�X�g��RectTransform ")]
    private RectTransform _logoRectTransform = null;
    [SerializeField, Header("�\������e�L�X�g")]
    private Text _logoImage = null;
    [SerializeField, Header("�{�^����RectTransform")]
    private RectTransform _titleButtonRectTransform = null;

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
    /// ���o�J�n����
    /// </summary>
    public void StartAnimation()
    {
        //����UI�ݒ�
        gameObject.SetActive(true);
        //CanvasGroup�ݒ�
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        //���S�ݒ�
        _logoRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        _logoImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //�{�^���ݒ�
        Vector2 buttonPos = _titleButtonRectTransform.anchoredPosition;
        buttonPos.y -= ButtonMovePosY;
        _titleButtonRectTransform.anchoredPosition = buttonPos;

        //UI�\���A�j���[�V����
        Sequence sequence = DOTween.Sequence();
        //�p�l����\��
        sequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        //���S��\��
        sequence.Append(_logoRectTransform.DOScale(LogoEndScale, 1.0f));
        sequence.Join(_logoImage.DOFade(1.0f, 1.0f));
        //�҂�����
        sequence.AppendInterval(1.0f);
        //�{�^�����ړ�
        sequence.Append(_titleButtonRectTransform.DOAnchorPosY(ButtonMovePosY, 0.5f)
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
