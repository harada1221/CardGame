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

    // �P����E�B���h�E�N���X
    private TrainingWindowScript trainingWindow;


    //�P�����ڂ̎�ޒ�`
    public enum TrainingMode
    {
        MaxHPUp,   //�ő�̗͏㏸
        HandNumUp, //��D�����㏸
    }
    /// <summary>
    /// �擾�{�^������������
    /// </summary>
    public void ObtainButton()
    {

    }
}
