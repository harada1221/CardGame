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

    [SerializeField, Header("�J�[�h���ʃe�L�X�g")]
    private GameObject _cardEffectText = default;
    [SerializeField, Header("�J�[�h�w�iImage")]
    private Image _cardBackImage = default;
    [SerializeField, Header("�J�[�h��Text")]
    private Text _cardNameText = default;
    [SerializeField, Header("�J�[�h�A�C�R�����X�g�̐e")]
    private Transform _cardIconParent = default;
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
    private Image _cardBackSprite_Player = default;  
    [SerializeField, Header(" �G���J�[�h�w�i")]
    private Image _cardBackSprite_Enemy = default; 

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
}
