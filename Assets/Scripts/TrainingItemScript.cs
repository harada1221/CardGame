using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingItemScript : MonoBehaviour
{
    //UI�I�u�W�F�N�g
    [SerializeField, Header("���ڃA�C�R��")]
    private Image _iconImage = default;
    [SerializeField, Header("���ږ�Text")]
    private Text _nameText = default;
    [SerializeField, Header("���ڐ���Text")]
    private Text _explainText = default;
    [SerializeField, Header("�擾�{�^��")]
    private Button _obtainButton = default;
    [SerializeField, Header("���iText")]
    private Text _priceText = default;
    [SerializeField, Header("���ڂ̎��")]
    private TrainingMode _trainingMode = default;
    [SerializeField, Header("�K�v�o���l��")]
    private int _price = default;
    //�P����E�B���h�E�N���X
    private TrainingWindowScript _trainingWindow = default;
    //�萔��`
    //�ő�̗͏㏸�̕K�v�o���l��(���݂̍ő�̗͂ɂ��̒萔��������)
    private const int TrainPriceMaxHpUp = 5;
    //��D�����㏸�̌P���񐔂��Ƃ̕K�v�o���l��
    public static readonly int[] TrainPrice_HandNum_UP = new int[] { 100, 1000, 5000, 10000, 50000 };

    public int GetPrice { get => _price; }
    public TrainingMode GetTraining { get => _trainingMode; }
    //�P�����ڂ̎�ޒ�`
    public enum TrainingMode
    {
        MaxHPUp,   //�ő�̗͏㏸
        HandNumUp, //��D�����㏸
    }
    /// <summary>
    ///���������� 
    /// </summary>
    /// <param name="trainingWindow">�g���[�j���O�Ǘ��p</param>
    /// <param name="trainingMode">�g���[�j���O���</param>
    public void Init(TrainingWindowScript trainingWindow, TrainingMode trainingMode)
    {
        //�Q�Ǝ擾
        _trainingWindow = trainingWindow;
        //�ϐ�������
        _trainingMode = trainingMode;
        //UI������
        RefreshUI();
    }
   	/// <summary>
	/// �擾�{�^������������
	/// </summary>
	public void ObtainButton()
    {
        _trainingWindow.ObtainItem(this);
    }
    /// <summary>
	/// UI�̕\�����X�V����
	/// </summary>
	public void RefreshUI()
    {
        switch (_trainingMode)
        {
            case TrainingMode.MaxHPUp:
                //�ő�HP�㏸
                _nameText.text = "�ő�̗͏㏸";
                _explainText.text = "�v���C���[�̍ő�̗͂��㏸���܂��B\n(���݂̍ő�̗�:" + DataScript._date.GetPlayerMaxHP + ")";


                //�K�vEXP��
                _price = DataScript._date.GetPlayerMaxHP * TrainPriceMaxHpUp;
                _priceText.text = _price.ToString("#,0");
                CheckPrice();
                break;

            case TrainingMode.HandNumUp:
                //��D�����㏸
                _nameText.text = "��D�����㏸";
                _explainText.text = "�^�[���J�n���̎�D�������������܂��B\n(����" + DataScript._date.GetPlayerHandNum + "��)";

                //�K�vEXP��
                _price = TrainPrice_HandNum_UP[_trainingWindow.GetHandNumTrainCount()];
                _priceText.text = _price.ToString("#,0");
                CheckPrice();
                break;
        }
    }
    /// <summary>
    /// �\������A�C�R�������߂�
    /// </summary>
    /// <param name="sprite">�\������A�C�R��</param>
    public void SetIcon(Sprite sprite)
    {
        //�A�C�R�������邩
        if (sprite == null)
        {
            _iconImage.color = Color.clear;
        }
        else
        {
            _iconImage.color = Color.white;
            _iconImage.sprite = sprite;
        }
    }
    /// <summary>
	/// ���ڂ̎擾�ɕK�v��EXP������Ă��邩�𔻒�
	/// </summary>
	public void CheckPrice()
    {
        //�v���C���[�̌o���l������Ă��邩
        if (DataScript._date.GetPlayerExp >= _price)
        {
            //�{�^����L����
            _obtainButton.interactable = true;
        }
        else
        {
            //�{�^���𖳌���
            _obtainButton.interactable = false;
        }
    }
}
