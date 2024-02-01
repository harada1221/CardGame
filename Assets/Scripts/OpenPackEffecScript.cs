//-----------------------------------------------------------------------
/* �p�b�N�J�����s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OpenPackEffecScript : MonoBehaviour
{
    [SerializeField, Header("UI�I�u�W�F�N�g")]
    private CanvasGroup _canvasGroup = default;
    // �J�[�h�֘A�Q��
    [SerializeField, Header(" �J�[�h�v���n�u")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header(" �J�[�h�I�u�W�F�N�g�̐eTransform")]
    private Transform _cardsParent = default;

    //�J�[�h�\�����oSequence
    private Sequence _effectSequence = default;
    //�V���b�v�E�B���h�E�N���X
    private ShoppingWindowScript _shoppingWindow = default;

    // �������֐�(ShoppingWindow.cs����ďo)
    public void Init(ShoppingWindowScript shoppingWindow)
    {
        //�Q�Ǝ擾
        _shoppingWindow = shoppingWindow;
        //UI������
        gameObject.SetActive(true);
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// �J�[�h���o
    /// </summary>
    /// <param name="cardDatas">�J�[�h�f�[�^</param>
    public void StartEffect(List<CardDataSO> cardDatas)
    {
        //TWEEN�ݒ�
        _effectSequence = DOTween.Sequence();

        //�w�i�摜�\�����o
        _effectSequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        //�J�[�h�����Ԃɕ\�����鉉�o
        foreach (CardDataSO cardData in cardDatas)
        {
            //�I�u�W�F�N�g�쐬
            GameObject obj = Instantiate(_cardPrefab, _cardsParent);
            CardScript objCard = obj.GetComponent<CardScript>();

            //�J�[�h�����ݒ�
            objCard.InitField(null, Vector2.zero);
            objCard.SetInitialCardData(cardData, CardScript.CharaID_Player);
        }
    }

    /// <summary>
    /// ��ʂ��\���ɂ���
    /// </summary>
    public void ClosePanel()
    {
        //UI��\��
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        //���o�I��
        if (_effectSequence != null)
        {
            _effectSequence.Kill();
        }
        //�������ꂽ�J�[�h������
        for (int i = 0; i < _cardsParent.childCount; i++)
        {
            Transform cardObject = _cardsParent.GetChild(i);
            Destroy(cardObject.gameObject);
        }
    }
}
