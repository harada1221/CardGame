//-----------------------------------------------------------------------
/* �P����E�B���h�E�N���X
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

public class TrainingWindowScript : MonoBehaviour
{
    [SerializeField, Header("Scrollbar")]
    private Scrollbar _scrollbar = default;
    [SerializeField, Header("����EXP��Text")]
    private Text _statusValueText = default;
    [SerializeField, Header("�̗͏㏸�A�C�R��")]
    private Sprite _itemIcon_MaxHPUp = default;
    [SerializeField, Header("��D�㏸�A�C�R��")]
    private Sprite _itemIcon_HandNumUp = default;
    [SerializeField, Header("�P������UI�v���n�u")]
    private GameObject _trainingItemPrefab = default;
    [SerializeField, Header("�P������UI�̐eTransform")]
    private Transform _trainingItemsParent = default;
    //�^�C�g���}�l�[�W���[
    private TitleManagerScript _titleManager = default;
    //�E�B���h�E��RectTransform
    private RectTransform _windowRectTransform = default;
    //�X�N���[���o�[�������t���O
    private bool isReservResetScrollBar;
    //�����ς݌P�����ڃ��X�g
    private List<TrainingItemScript> _trainingItemInstances = default;

    //�E�B���h�E�\���E��\��Tween;
    private Tween _windowTween = default;
    //�E�B���h�E�\���A�j���[�V��������
    private const float WindowAnimTime = 0.3f;

    //�萔��`
    public const int InitPlayerHandNum = 5; // �v���C���[�̏�����D����

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="titleManager">TitleManagerScript</param>
    public void Init(TitleManagerScript titleManager)
    {
        //�Q�Ǝ擾
        _titleManager = titleManager;
        _windowRectTransform = GetComponent<RectTransform>();

        // �ϐ�������
        _trainingItemInstances = new List<TrainingItemScript>();
        {
            //���ڍ쐬
            GameObject obj = Instantiate(_trainingItemPrefab, _trainingItemsParent);
            TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
            _trainingItemInstances.Add(trainingItem);
            //���ڐݒ�
            trainingItem.Init(this, TrainingItemScript.TrainingMode.MaxHPUp);
            trainingItem.SetIcon(_itemIcon_MaxHPUp);
        }
        //��D�����㏸
        if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
        {
            //���ڍ쐬
            GameObject obj = Instantiate(_trainingItemPrefab, _trainingItemsParent);
            TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
            _trainingItemInstances.Add(trainingItem);
            //���ڐݒ�
            trainingItem.Init(this, TrainingItemScript.TrainingMode.HandNumUp);
            trainingItem.SetIcon(_itemIcon_HandNumUp);
        }

        //����EXP�ʕ\��
        _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");

        //�E�B���h�E��\��
        _windowRectTransform.transform.localScale = Vector2.zero;
        _windowRectTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// �X�V����
    /// </summary>
    private void OnGUI()
    {
        //��ʃX�N���[���������l�ɖ߂�����
        if (isReservResetScrollBar)
        {
            _scrollbar.value = 1.0f;
            isReservResetScrollBar = false;
        }
    }

    /// <summary>
    /// �E�B���h�E��\������
    /// </summary>
    public void OpenWindow()
    {
        //�Đ��I��������
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }
        //�E�B���h�E�\��Tween
        _windowTween = _windowRectTransform.DOScale(1.0f, WindowAnimTime).SetEase(Ease.OutBack);
        //�E�B���h�E�w�i�p�l����L����
        _titleManager.SetWindowBackPanelActive(true);
        //��ʃX�N���[���������l�ɖ߂�(����OnGUI�Ŏ��s)
        isReservResetScrollBar = true;
    }
    /// <summary>
    /// �E�B���h�E���\���ɂ���
    /// </summary>
    public void CloseWindow()
    {
        //�Đ��I��������
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }
        //�E�B���h�E��\��Tween
        _windowTween = _windowRectTransform.DOScale(0.0f, WindowAnimTime).SetEase(Ease.InBack);
        //�E�B���h�E�w�i�p�l���𖳌���
        _titleManager.SetWindowBackPanelActive(false);
    }

    /// <summary>
    /// ���ڂ��擾����
    /// </summary>
    /// <param name="targetItem">�擾���鍀��</param>
    public void ObtainItem(TrainingItemScript targetItem)
    {
        //EXP�ʌ���
        DataScript._date.ChangePlayerEXP(-targetItem.GetPrice);
        OnChangePlayerEXP();

        //���ڂ��Ή����镨���v���C���[���l������
        switch (targetItem.GetTraining)
        {
            case TrainingItemScript.TrainingMode.MaxHPUp:
                //�ő�̗͏㏸
                DataScript._date.ChangePlayerMaxHP(1);
                targetItem.RefreshUI();
                break;
            case TrainingItemScript.TrainingMode.HandNumUp:
                //��D�����㏸
                DataScript._date.ChangePlayerHandNum(1);
                //�P���\��
                if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
                {
                    targetItem.RefreshUI();
                }
                else
                {
                    //�I�u�W�F�N�g����
                    Destroy(targetItem.gameObject);
                    //���X�g����폜
                    _trainingItemInstances.Remove(targetItem);
                }
                break;
        }
    }

    /// <summary>
    /// �v���C���[��EXP�ʂ��ύX���ꂽ���ɌĂяo�����
    /// </summary>
    public void OnChangePlayerEXP()
    {
        //����EXP�ʕ\��
        _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");
        //�e���ڂ̎擾�{�^�������ېݒ�
        foreach (TrainingItemScript item in _trainingItemInstances)
        {
            item.CheckPrice();
        }
    }

    /// <summary>
    /// ��D�����㏸�̌P�����s�����񐔂�Ԃ�
    /// </summary>
    public int GetHandNumTrainCount()
    {
        return DataScript._date.GetPlayerHandNum - InitPlayerHandNum;
    }
}
