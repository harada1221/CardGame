//-----------------------------------------------------------------------
/* �V���b�v���ڊǗ��N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@1��15��
 *--------------------------------------------------------------------------- 
*/
using UnityEngine;
using UnityEngine.UI;

public class ShoppingItemScript : MonoBehaviour
{
    // UI�I�u�W�F�N�g
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
    //�V���b�v�E�B���h�E�N���X
    private ShoppingWindowScript _shoppingWindow = default;
    //���̏��i�̃p�b�N�f�[�^
    private CardPackSO _cardpackData = default;
    //���ڂ̎擾�ɕK�v��GOLD��
    private int _price = default;

    public CardPackSO GetCardPack { get => _cardpackData; }
    public int GetPrice { get => _price; }


    /// <summary>
    /// ������
    /// </summary>
    /// <param name="shoppingWindow">ShoppingWindowScript�N���X</param>
    /// <param name="cardPackData">CardPackSO�f�[�^</param>
    public void Init(ShoppingWindowScript shoppingWindow, CardPackSO cardPackData)
    {
        //�Q�Ǝ擾
        _shoppingWindow = shoppingWindow;
        _cardpackData = cardPackData;
        //UI������
        _nameText.text = _cardpackData.GetName + "(" + _cardpackData.GetCardNum + "��)";
        _explainText.text = _cardpackData.GetExpLain;
        //�p�b�N�A�C�R��Image
        _iconImage.sprite = _cardpackData.GetIcon;
        //�K�vGold��
        _price = _cardpackData.GetPrice;
        _priceText.text = _price.ToString("#,0");
        CheckPrice();
    }

    /// <summary>
    /// ���ڂ̎擾�ɕK�v��EXP������Ă��邩�𔻒�
    /// </summary>
    public void CheckPrice()
    {
        //�S�[���h������Ă��邩
        if (DataScript._date.GetPlayerGold >= _price)
        {
            _obtainButton.interactable = true;
        }
        else
        {
            _obtainButton.interactable = false;
        }
    }
    /// <summary>
    /// �w���{�^������������
    /// </summary>
    public void ObtainButton()
    {
        _shoppingWindow.BuyItem(this);
    }
}
