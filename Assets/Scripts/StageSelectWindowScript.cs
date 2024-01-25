using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageSelectWindowScript : MonoBehaviour
{
    [SerializeField, Header("�X�e�[�W��Text")]
    private Text _stageNameText = default;
    [SerializeField, Header("�X�e�[�W��ՓxText")]
    private Text _stageDifficultyText = default;
    [SerializeField, Header("�퓬��Text")]
    private Text _battleNumText = default;
    [SerializeField, Header("�X�e�[�W�A�C�R��Image")]
    private Image _stageIconImage = default;
    [SerializeField, Header("�I�𒆃X�e�[�W�ԍ��h�b�g")]
    private List<Image> _stageOrderImages = null;
    [SerializeField, Header("�퓬�V�[����")]
    private string _nextSceneName = default;

    //�X�e�[�W�̑���
    private int _stageListNum = default;
    //�I�𒆃X�e�[�WID
    private int _selectStageID = default;
    //�^�C�g���Ǘ��N���X
    private TitleManagerScript _titleManager = default;
    //�E�B���h�E��RectTransform
    private RectTransform _windowRectTransform = default;
    //�E�B���h�E�pTween;
    private Tween _windowTween = default;
    //�E�B���h�E�\���A�j���[�V��������
    private const float _WindowAnimTime = 0.3f;

    //�������֐�(TitleManager.cs����ďo)
    public void Init(TitleManagerScript titleManager)
    {
        //�Q�Ǝ擾
        this._titleManager = titleManager;
        _windowRectTransform = GetComponent<RectTransform>();
        // �ϐ�������
        _stageListNum = DataScript._date.GetStageSOs.Count;
        _selectStageID = 0;

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
        //�X�e�[�W���\��
        ShowStageDatas();
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
    /// <summary>
	/// �P����(�}�C�i�X����)�̃X�e�[�W�ɐ؂�ւ���{�^��
	/// </summary>
	public void LeftScrollButton()
    {
        //�I���X�e�[�W�؂�ւ�
        _selectStageID--;
        if (_selectStageID < 0)
        {
            _selectStageID = _stageListNum - 1;
        }
        //�I�𒆃X�e�[�W���\��
        ShowStageDatas();
    }
    /// <summary>
    /// �P�E��(�v���X����)�̃X�e�[�W�ɐ؂�ւ���{�^��
    /// </summary>
    public void RightScrollButton()
    {
        //�I���X�e�[�W�؂�ւ�
        _selectStageID++;
        if (_selectStageID >= _stageListNum)
        {
            _selectStageID = 0;
        }
        //�I�𒆃X�e�[�W���\��
        ShowStageDatas();
    }

    /// <summary>
    /// �I�𒆂̃X�e�[�W����\������
    /// </summary>
    public void ShowStageDatas()
    {
        StageSO stageData = DataScript._date.GetStageSOs[_selectStageID];


        //�X�e�[�W��Text
        _stageNameText.text = stageData.GetStageName;
        //�X�e�[�W��ՓxText
        _stageDifficultyText.text = stageData.GetDifficulty;
        //�G�̐�Text
        _battleNumText.text = "�G�̐� " + stageData.GetAppearEnemyTables.Count;

        //�X�e�[�W�A�C�R��Image
        _stageIconImage.sprite = stageData.GetStageIcon;

        //�X�e�[�W�ԍ�Images
        for (int i = 0; i < _stageListNum; i++)
        {
            if (i == _selectStageID)
            {
                _stageOrderImages[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
            else
            {
                _stageOrderImages[i].transform.localScale = new Vector2(0.4f, 0.4f);
            }
        }
    }

    /// <summary>
    /// �X�e�[�W�J�n�{�^��
    /// </summary>
    /// <param name="isWithTutorial">true:�`���[�g���A���L��ŊJ�n����</param>
    public void StageStartButton(bool isWithTutorial)
    {
        // �I�������X�e�[�W�ԍ����L��
        DataScript._date._nowStageID = _selectStageID;
        // �V�[���؂�ւ�
        SceneManager.LoadScene(_nextSceneName);


    }
}
