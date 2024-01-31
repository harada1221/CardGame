using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShoppingWindowScript : MonoBehaviour
{
    //[SerializeField, Header("ScrollView����Scrollbar")]
    //private Scrollbar _scrollbar = default;
    //[SerializeField, Header("����Gold��Text")]
    //private Text _goldText = default;
    //[SerializeField, Header("���i�̃J�[�h�p�b�N�̃��X�g")]
    //private List<CardPackSO> _cardPackSOs = default;
    ////���i���ڊ֘A
    //[SerializeField, Header("���i����UI�v���n�u")]
    //private GameObject _shoppingItemPrefab = default;
    //[SerializeField, Header("���i����UI�̐eTransform")]
    //private Transform _shoppingItemsParent = default;
    ////�^�C�g���Ǘ��N���X
    //private TitleManagerScript _titleManager = default;
    ////�E�B���h�E��RectTransform
    //private RectTransform _windowRectTransform = default;
    ////�����ςݏ��i���ڃ��X�g
    //private List<ShoppingItemScript> _shoppingItemInstances = default;
    ////�E�B���h�E�\���E��\��Tween;
    //private Tween _windowTween = default;
    ////�E�B���h�E�\���A�j���[�V��������
    //private const float _WindowAnimTime = 0.3f;
    ////�X�N���[���o�[�������t���O
    //private bool _reservResetScrollBar = default;
    ///// <summary>
    ///// ����������
    ///// </summary>
    ///// <param name="titleManager">TitleManagerScript</param>
    //public void Init(TitleManagerScript titleManager)
    //{
    //    //�Q�Ǝ擾
    //    _titleManager = titleManager;
    //    _windowRectTransform = GetComponent<RectTransform>();

    //    //�ϐ�������
    //    _shoppingItemInstances = new List<ShoppingItemScript>();
    //    //���i���ڍ쐬
    //    for (int i = 0; i < _cardPackSOs.Count; i++)
    //    {
    //        //���ڍ쐬
    //        GameObject obj = Instantiate(_shoppingItemPrefab, _shoppingItemsParent);
    //        ShoppingItemScript shoppingItem = obj.GetComponent<ShoppingItemScript>();
    //        _shoppingItemInstances.Add(shoppingItem);
    //        //���ڐݒ�
    //        shoppingItem.Init(this, _cardPackSOs[i]);
    //    }

    //    //����Gold�ʕ\��
    //    _goldText.text = DataScript._date.GetPlayerGold.ToString("#,0");

    //    //�E�B���h�E��\��
    //    _windowRectTransform.transform.localScale = Vector2.zero;
    //    _windowRectTransform.gameObject.SetActive(true);
    //}

    ////OnGUI (�t���[�����ƂɎ��s)
    //private void OnGUI()
    //{
    //    // ��ʃX�N���[���������l�ɖ߂�����
    //    if (_reservResetScrollBar)
    //    {
    //        _scrollbar.value = 1.0f;
    //        _reservResetScrollBar = false;
    //    }
    //}

    ///// <summary>
    ///// �E�B���h�E��\������
    ///// </summary>
    //public void OpenWindow()
    //{
    //    if (_windowTween != null)
    //    {
    //        _windowTween.Kill();
    //    }
    //    //�E�B���h�E�\��Tween
    //    _windowTween = _windowRectTransform.DOScale(1.0f, _WindowAnimTime).SetEase(Ease.OutBack);
    //    //�E�B���h�E�w�i�p�l����L����
    //    _titleManager.SetWindowBackPanelActive(true);
    //    //��ʃX�N���[���������l�ɖ߂�(����OnGUI�Ŏ��s)
    //    _reservResetScrollBar = true;
    //}
    ///// <summary>
    ///// �E�B���h�E���\���ɂ���
    ///// </summary>
    //public void CloseWindow()
    //{
    //    if (_windowTween != null)
    //    {
    //        _windowTween.Kill();
    //    }
    //    //�E�B���h�E��\��Tween
    //    _windowTween = _windowRectTransform.DOScale(0.0f, _WindowAnimTime).SetEase(Ease.InBack);
    //    //�E�B���h�E�w�i�p�l���𖳌���
    //    _titleManager.SetWindowBackPanelActive(false);
    //}

    ///// <summary>
    ///// �Y���̏��i���w������
    ///// </summary>
    //public void BuyItem(ShoppingItemScript targetItem)
    //{
    //    //�����_���Ɏw�薇���̃J�[�h�����
    //    //�p�b�N�f�[�^
    //    var targetPack = targetItem.cardpackData;
    //    //���肵���S�J�[�h�̃��X�g
    //    List<CardDataSO> obtainedCards = new List<CardDataSO>(); 
    //    for (int i = 0; i < targetPack.cardNum; i++)
    //    {
    //        //����J�[�h������
    //        var cardData = targetPack.includedCards[Random.Range(0, targetPack.includedCards.Count)];
    //        obtainedCards.Add(cardData);
    //        //���菈��
    //        PlayerDeckData.ChangeStorageCardNum(cardData.serialNum, 1);
    //    }

    //    //Gold�ʌ���
    //    DataScript._date.ChangePlayerGold(-targetItem.price);
    //    OnChangePlayerGold();
    //}

    ///// <summary>
    ///// �v���C���[��Gold�ʂ��ύX���ꂽ���ɌĂяo�����
    ///// </summary>
    //public void OnChangePlayerGold()
    //{
    //    //����GOLD�ʕ\��
    //    _goldText.text = DataScript._date.GetPlayerGold.ToString("#,0");
    //    //�e���ڂ̎擾�{�^�������ېݒ�
    //    foreach (ShoppingItemScript item in _shoppingItemInstances)
    //    {
    //        item.CheckPrice();
    //    }
    //}
}
