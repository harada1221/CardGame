using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingItemScript : MonoBehaviour
{
    // UI�I�u�W�F�N�g
    [SerializeField, Header("���ڃA�C�R��")]
    private Image iconImage = default;
    [SerializeField, Header("���ږ�Text")]
    private Text nameText = default;
    [SerializeField, Header("���ڐ���Text")]
    private Text explainText = default;
    [SerializeField, Header("�擾�{�^��")]
    private Button obtainButton = default;
    [SerializeField, Header("���iText")]
    private Text priceText = default;
    //�V���b�v�E�B���h�E�N���X
    private ShoppingWindowScript shoppingWindow = default;

    /// <summary>
    /// �w���{�^������������
    /// </summary>
    public void ObtainButton()
    {

    }
}
