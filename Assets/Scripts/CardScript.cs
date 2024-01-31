//-----------------------------------------------------------------------
/* �J�[�h�̏������s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region �ϐ��錾
    //�t�B�[���h�Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;

    [SerializeField, Header("�I�u�W�F�N�g��RectTransform")]
    public RectTransform _rectTransform = default;
    [SerializeField, Header("�J�[�hUI�\��")]
    private CardUIScript _cardUI = default;
    [SerializeField, Header("�I�u�W�F�N�g��CanvasGroup")]
    private CanvasGroup _canvasGroup = default;

    //�f�b�L�ҏW��ʗp
    private DeckEditWindowScript _deckEditWindow = default;
    //�h���b�O�I����ɖ߂��Ă�����W
    private Vector2 _basePos = default;
    //�ړ�Tween
    private Tween _moveTween = default;
    //�J�[�h�]�[���̎��
    private CardZoneScript.ZoneType _nowZone = default;
    public bool isInDeckCard = false;

    //��ɂȂ�J�[�h�f�[�^
    private CardDataSO _cardDate = default;
    //�J�[�h���ʂ̃��X�g
    private List<CardEffectDefineScript> _cardEffects = default;
    //�J�[�h�̋��x
    private int _forcePoint = default;
    //�J�[�h���g�p�L�����N�^�[
    private int _controllerCharaID = default;
    // �퓬��V��ʗp
    private RewardScript _rewardPanel = default;

    //�J�[�h�ړ��A�j���[�V��������
    private const float MoveTime = 0.4f;
    //�L�����N�^�[ID�萔
    //�퓬���̃L�����N�^�[�l��
    public const int CharaNum = 2;
    //�L�����N�^�[ID:��l��(�v���C���[�L����)
    public const int CharaID_Player = 0;
    //�L�����N�^�[ID:�G
    public const int CharaID_Enemy = 1;
    //�L�����N�^�[ID:(����)
    public const int CharaID_None = -1;
    //�L�����N�^�[ID:�{�[�i�X�p
    public const int CharaID_Bonus = 2;
    //�J�[�h�A�C�R���̍ő吔
    private const int MaxIcons = 6;
    //�J�[�h���ʂ̍ő吔
    private const int MaxEffects = 6;
    //�J�[�h���x�̏���l(����𒴂���Ɣj��)
    private const int MaxForcePoint = 9;
    //CanvasGroup�̕ύX��Alpha�l
    private const float TargetAlpha = 0.5f;
    //���o����
    private const float AnimTime = 0.3f;
    //�ړ���X���΍��W
    private const float TargetPositionX_Relative = -300.0f;
    //���o����
    private const float OutAnimTime = 1.0f;
    #endregion

    #region �v���p�e�B
    public CardZoneScript.ZoneType GetNowZone { get => _nowZone; }
    public int GetControllerCharaID { get => _controllerCharaID; }
    public List<CardEffectDefineScript> GetCardEffects { get => _cardEffects; }
    public int GetForcePoint { get => _forcePoint; }
    public CardDataSO GetCardData { get => _cardDate; }
    #endregion

    #region ��������
    /// <summary>
    /// �����������t�B�[���h�}�l�[�W���[����Ăяo��
    /// </summary>
    /// <param name="_fieldManager">�t�B�[���h�}�l�[�W���[</param>
    /// <param name="initPos">�������W</param>
    public void InitField(FieldAreaManagerScript _fieldManager, Vector2 initPos)
    {
        //FieldAreaManagerScript�Q�Ǝ擾
        _fieldAreaScript = _fieldManager;
        //UI�����ݒ�
        _cardUI.Init(this);
        //�ϐ�������
        //�����ʒu�Ɉړ�
        _rectTransform.position = initPos;
        //�����ʒu�Œ�
        _basePos = initPos;
        //�����̏ꏊ
        _nowZone = CardZoneScript.ZoneType.Hand;
        //���X�g����
        _cardEffects = new List<CardEffectDefineScript>();
    }
    /// <summary>
	/// �����������f�b�L�ҏW�N���X����ďo�p
	/// </summary>
	/// <param name="deckEditWindow">DeckEditWindow�Q��</param>
	/// <param name="_isInDeckCard">�f�b�L���J�[�h�t���O</param>
	public void InitDeck(DeckEditWindowScript deckEditWindow, bool _isInDeckCard)
    {
        _deckEditWindow = deckEditWindow;
        isInDeckCard = _isInDeckCard;
        InitField(null, Vector2.zero);
    }
    /// <summary>
	/// ����������RewardScript��ʂ���ďo��
	/// </summary>
	public void InitReward(RewardScript rewardPanel)
    {
        _rewardPanel = rewardPanel;
        InitField(null, Vector2.zero);
    }

    /// <summary>
    /// �J�[�h�̃p�����[�^���擾����
    /// </summary>
    /// <param name="cardData"></param>
    /// <param name="ControllerCharaID"></param>
    public void SetInitialCardData(CardDataSO cardData, int ControllerCharaID)
    {
        //�J�[�h�f�[�^�擾
        _cardDate = cardData;
        //�J�[�h��
        _cardUI.SetCardNameText(cardData.GetCardName);

        //�J�[�h���ʃ��X�g
        foreach (CardEffectDefineScript item in cardData.GetEffectList)
        {
            AddCardEffect(item);
        }
        //���x��ݒ�
        SetForcePoint(cardData.GetForce);
        //�J�[�h�g�p�҃f�[�^
        _controllerCharaID = ControllerCharaID;
        //�J�[�h�w�iUI�ɓK�p
        _cardUI.SetCardBackSprite(ControllerCharaID);
    }
    #endregion

    #region �ړ�����
    /// <summary>
    /// ��{���W�܂ŃJ�[�h���ړ�������
    /// </summary>
    public void BackToBasePos()
    {
        //���Ɏ��s���̈ړ��A�j���[�V����������Β�~
        if (_moveTween != null)
        {
            _moveTween.Kill();
        }
        //�w��n�_�܂ňړ�����A�j���[�V����(DOTween)
        _moveTween = _rectTransform
            .DOMove(_basePos, MoveTime) //�ړ�Tween
            .SetEase(Ease.OutQuart);    //�ω��̎d�����w��
    }
    /// <summary>
	/// �J�[�h���w��̃]�[���ɐݒu����
	/// </summary>
	/// <param name="zoneType">�ΏۃJ�[�h�]�[��</param>
	/// <param name="targetPos">�Ώۍ��W</param>
	public void PutToZone(CardZoneScript.ZoneType zoneType, Vector2 targetPos)
    {
        //���W���擾
        _basePos = targetPos;
        //�����ʒu�ɖ߂�
        BackToBasePos();
        //�J�[�h�]�[���̎�ނ�ۑ�
        _nowZone = zoneType;
    }

    /// <summary>
	/// �J�[�h����������������Tween�����s����
	/// </summary>
	/// <returns>���sTween</returns>
	public Tween HideFadeTween()
    {
        return _canvasGroup.DOFade(TargetAlpha, AnimTime);
    }
    /// <summary>
	/// �J�[�h����ʊO�Ɉړ�������Tween�����s����
	/// </summary>
	/// <returns>���sTween</returns>
	public void HideMoveTween()
    {
        _rectTransform.DOAnchorPosX(TargetPositionX_Relative, OutAnimTime)
            .SetRelative();
    }
    #endregion

    #region �N���b�N����
    /// <summary>
    /// �N���b�N�����Ƃ��Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //�h���b�O�J�n����
        if (_fieldAreaScript != null)
        {
            _fieldAreaScript.StartDragging(this);
        }
        else if (_deckEditWindow != null)
        {
            _deckEditWindow.SelectCard(this);
        }
        else if (_rewardPanel != null)
        {
            _rewardPanel.SelectCard(this);
        }
    }
    /// <summary>
    /// �N���b�N���I��������Ɏ��s����
    /// </summary>
    /// <param name="eventData">�N���b�N���</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //�N���b�N�I������
        if (_fieldAreaScript != null)
        {
            _fieldAreaScript.EndDragging();
        }

    }
    /// <summary>
	/// �v���C���[���ۊǒ��̃J�[�h���ʂ�\��(�f�b�L�ҏW���p)
	/// </summary>
	public void ShowCardAmountInStorage()
    {
        int amount = PlayerDeckDataScript._storageCardList[_cardDate.GetSerialNum];
        _cardUI.SetAmountText(amount);
    }
    /// <summary>
    /// �J�[�h�̋����\����Ԃ�ύX����
    /// </summary>
    public void SetCardHilight(bool mode)
    {
        _cardUI.SetHilightImage(mode);
    }
    #endregion

    #region �p�����[�^�ύX
    /// <summary>
    /// �J�[�h�̌��ʒl�𑝌�����
    /// </summary>
    /// <param name="effectType">�Ώی��ʃf�[�^</param>
    /// <param name="value">�ω���</param>
    public void EnhanceCardEffect(CardEffectDefineScript.CardEffect effectType, int value)
    {
        for (int i = 0; i < _cardEffects.Count; i++)
        {
            //�Ώی��ʂ����݂��邩
            if (_cardEffects[i].GetEffect == effectType)
            {
                //���ʒl��ύX
                _cardEffects[i].GetValue += value;
                //UI�\��
                _cardUI.ApplyCardEffectText(_cardEffects[i]);
                return;
            }
        }
    }
    /// <summary>
	/// �����ɂ���ăJ�[�h���ʂ�ǉ�����
	/// </summary>
	/// <param name="newEffect">���ʂ̎�ށE���l�f�[�^</param>
	public void CompoCardEffect(CardEffectDefineScript newEffect)
    {
        //���ʂ̍����ۂ��擾
        CardEffectDefineScript.EffectCompoMode compoMode = CardEffectDefineScript.Dic_EffectCompoMode[newEffect.GetEffect];
        //���̃J�[�h���G���̃J�[�h�����擾
        bool isEnemyCard = false;
        if (_controllerCharaID == CharaID_Enemy)
        {
            isEnemyCard = true;
        }

        //�����s�Ȃ�I��
        switch (compoMode)
        {
            case CardEffectDefineScript.EffectCompoMode.Impossible:
                //�����s�\
                return;

            case CardEffectDefineScript.EffectCompoMode.OnlyOwn:
                //�����ƃJ�[�h�Ƃ̂ݍ����\
                if (isEnemyCard)
                {
                    return;
                }
                else
                {
                    break;
                }
            case CardEffectDefineScript.EffectCompoMode.OnlyOwn_New:
                // �����ƃJ�[�h�Ƃ̂ݍ����\(�V�K�̂�)
                if (isEnemyCard)
                {
                    return;
                }
                else
                {
                    break;
                }

        }

        //�ǉ��������ʂƓ�����ނ̌��ʂ����ɑ��݂��邩�𒲂ׂ�
        //���݂̌��ʂ̎�ސ�
        int length = _cardEffects.Count;
        //�������ʃf�[�^�̔z����ԍ�
        int index;
        for (index = 0; index < length; index++)
        {
            //���݂����ꍇ�F�ԍ���ۑ����ă��[�v�I��
            if (_cardEffects[index].GetEffect == newEffect.GetEffect)
            {
                break;
            }

        }

        //���ʂ̍����E�ǉ�����
        if (index < length)
        {
            //������ނ̌��ʂ�����F��������
            //�V�K�ǉ�����̌��ʂȂ獇�����Ȃ�
            if (compoMode == CardEffectDefineScript.EffectCompoMode.OnlyNew ||
                compoMode == CardEffectDefineScript.EffectCompoMode.OnlyOwn_New)
            {
                return;
            }
            //���ʒl����
            EnhanceCardEffect(index, newEffect.GetValue);
        }
        else
        {
            //������ނ̌��ʂ��Ȃ�
            AddCardEffect(newEffect);
        }
    }
    /// <summary>
    /// �J�[�h���ʂ�V�K�ǉ�����
    /// </summary>
    /// <param name="newEffect">���ʂ̎�ށE���l�f�[�^</param>
    private void AddCardEffect(CardEffectDefineScript newEffect)
    {
        //�J�[�h���ʐ�����Ȃ�I��
        if (_cardEffects.Count >= MaxEffects)
        {
            return;
        }

        //���ʃf�[�^��V�K�쐬����
        CardEffectDefineScript effectData = new CardEffectDefineScript();
        effectData.GetEffect = newEffect.GetEffect;
        effectData.GetValue = newEffect.GetValue;
        //���ʃ��X�g�ɒǉ�
        _cardEffects.Add(effectData);
        //UI�\��
        _cardUI.AddCardEffectText(effectData);
    }
    /// <summary>
	/// �J�[�h�̌��ʒl�𑝌�����
	/// </summary>
	/// <param name="index">�Ώی��ʃf�[�^�̔z����ԍ�</param>
	/// <param name="value">�ω���</param>
	public void EnhanceCardEffect(int index, int value)
    {
        //���ʒl��ύX
        CardEffectDefineScript effectData = _cardEffects[index];
        effectData.GetValue += value;
        //UI�\��
        _cardUI.ApplyCardEffectText(_cardEffects[index]);
    }
    /// <summary>
    /// �J�[�h�̋��x���Z�b�g����
    /// </summary>
    /// <param name="value">���x�̕ω���</param>
    /// <returns>�j��\��</returns>
	public bool SetForcePoint(int value)
    {
        //�p�����[�^���Z�b�g
        _forcePoint = value;
        //UI�\��
        _cardUI.SetForcePointText(_forcePoint);

        //�j��\��
        if (value > MaxForcePoint)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    #endregion

    #region ���̑�Get
    /// <summary>
    /// ���ʃ��X�g�̒��ɊY��������ʂ����邩
    /// </summary>
    /// <param name="effectType">�����������</param>
    /// <returns>���݂��邩</returns>
    public bool CheckContainEffect(CardEffectDefineScript.CardEffect effectType)
    {
        //�Y��������ʂ������true��Ԃ�
        foreach (CardEffectDefineScript effect in _cardEffects)
        {
            if (effect.GetEffect == effectType)
            {
                return true;
            }
        }
        //�Ȃ��ꍇ��false��Ԃ�
        return false;
    }
    /// <summary>
	/// ���ʃ��X�g�̒��ɊY���̌��ʎ킪���݂���ꍇ���̌��ʗʂ�Ԃ�
	/// </summary>
	public int GetEffectValue(CardEffectDefineScript.CardEffect effectType)
    {
        //�Y��������ʂ�����Ό��ʗʂ�Ԃ�
        foreach (CardEffectDefineScript effect in _cardEffects)
        {
            if (effect.GetEffect == effectType)
            {
                return effect.GetValue;
            }
        }
        //�Ȃ��ꍇ��-1��Ԃ�
        return -1;
    }
    #endregion
}
