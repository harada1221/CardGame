//-----------------------------------------------------------------------
/* �J�[�h��UI��ύX����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@1���P5��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIScript : MonoBehaviour
{
    // ���̃J�[�h�̏����N���X
    private CardScript _card = default;

    [SerializeField, Header("�J�[�h���ʃe�L�X�gPrefab")]
    private GameObject _cardEffectTextPrefab = default;

    [SerializeField, Header("�J�[�h�w�iImage")]
    private Image _cardBackImage = default;

    [SerializeField, Header("�J�[�h��Text")]
    private Text _cardNameText = default;

    [SerializeField, Header("�J�[�h���ʃe�L�X�g���X�g�̐e")]
    private Transform _cardEffectTextParent = default;

    [SerializeField, Header("�J�[�h���xText")]
    private Text _cardForceText = default;

    [SerializeField, Header("�J�[�h���xText�̔w�iImage")]
    private Image _cardForceBackImage = default;

    [SerializeField, Header("�J�[�h����Text")]
    private Text _quantityText = default;

    [SerializeField, Header("�J�[�h����Text�̔w�iImage")]
    private Image _quantityBackImage = default;

    [SerializeField, Header("�I���������\���摜Object")]
    private GameObject _hilightImageObject = default;

    [SerializeField, Header("�v���C���[���J�[�h�w�i")]
    private Sprite _cardBackSpritePlayer = default;
    [SerializeField, Header(" �G���J�[�h�w�i")]
    private Sprite _cardBackSpriteEnemy = default;
    [SerializeField,Header("�{�[�i�X�J�[�h�w�i")] 
    private Sprite _cardBackSpriteBonus = null;

    //�쐬��������Text���X�g
    private Dictionary<CardEffectDefineScript, Text> _cardEffectTextDic;
    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="_card">�Ή�����J�[�h</param>
    public void Init(CardScript card)
    {
        //�g�p����J�[�h
        _card = card;
        //��������
        _cardEffectTextDic = new Dictionary<CardEffectDefineScript, Text>();
        // UI������
        _quantityText.text = "";
        _quantityBackImage.color = Color.clear;
    }
    /// <summary>
    /// �L�����ɍ��킹�Ĕw�i��ݒ�
    /// </summary>
    /// <param name="cardControllerChara">�L������ID</param>
    public void SetCardBackSprite(int cardControllerChara)
    {
        //�v���C���[�̎�
        if (cardControllerChara == CardScript.CharaID_Player)
        {
            _cardBackImage.sprite = _cardBackSpritePlayer;
        }
        //�G�̎�
        else if (cardControllerChara == CardScript.CharaID_Enemy)
        {
            _cardBackImage.sprite = _cardBackSpriteEnemy;
        }
        //�{�[�i�X�̎�
        else if (cardControllerChara == CardScript.CharaID_Bonus)
        {
            _cardBackImage.sprite = _cardBackSpriteBonus;
        }
    }
    /// <summary>
    /// ���O�����肷��
    /// </summary>
    /// <param name="name">�J�[�h�l�[��</param>
    public void SetCardNameText(string name)
    {
        _cardNameText.text = name;
    }
    /// <summary>
	/// �J�[�h����Text��ǉ�
	/// </summary>
	public void AddCardEffectText(CardEffectDefineScript effectData)
    {
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardEffectTextPrefab, _cardEffectTextParent);
        //TextUI�ƃJ�[�h���ʂ�R�Â���
        _cardEffectTextDic.Add(effectData, obj.GetComponent<Text>());
        //Text�̓��e���X�V
        ApplyCardEffectText(effectData);
    }
    /// <summary>
	/// �J�[�h����Text�̕\�����e���X�V
	/// </summary>
	public void ApplyCardEffectText(CardEffectDefineScript effectData)
    {
        //�Ώۂ�TextUI���擾
        Text targetText = _cardEffectTextDic[effectData];
        //���ʗʂ��擾
        int effectValue = effectData.GetValue;
        string effectValueMes = "";

        //���ʗʂ𕶎���
        effectValueMes = effectValue.ToString();

        //UI�\��
        targetText.text = string.Format(CardEffectDefineScript.Dic_EffectName[effectData.GetEffect], effectValueMes);
    }
   /// <summary>
   /// �J�[�h���x��Text�\��
   /// </summary>
   /// <param name="hardnessValue">�J�[�h�̋��x</param>
	public void SetForcePointText(int hardnessValue)
    {
        //0���傫���ƕ\��
        if (hardnessValue > 0)
        {
            _cardForceText.text = hardnessValue.ToString();
            _cardForceBackImage.color = Color.white;
        }
        //��\��
        else
        {
            _cardForceText.text = "";
            _cardForceBackImage.color = Color.clear;
        }
    }
    /// <summary>
    /// �J�[�h�����\��
    /// </summary>
    /// <param name="holdCard">��������</param>
	public void SetAmountText(int holdCard)
    {
        //������������
        _quantityText.text = "x" + holdCard;
        _quantityBackImage.color = Color.white;
    }
    /// <summary>
    /// �J�[�h�̕\�������߂�
    /// </summary>
    /// <param name="mode">�J�[�h�����Ă��邩</param>
	public void SetHilightImage(bool mode)
    {
        //�\���A��\��
        _hilightImageObject.SetActive(mode);
    }
    /// <summary>
	/// �A�C�R���ƌ��ʂ̕\�������Z�b�g����
	/// </summary>
	public void ClearIconsAndEffects()
    {
        // �A�C�R��������
        int length = _cardEffectTextParent.childCount;
        for (int i = 0; i < length; i++)
        {
            Destroy(_cardEffectTextParent.GetChild(i).gameObject);
        }
    }
}
