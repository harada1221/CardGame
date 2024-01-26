using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScript : MonoBehaviour
{
    [SerializeField, Header("�eCanvasGroup")]
    private CanvasGroup _canvasGroup = default;
    // UI�I�u�W�F�N�g
    [SerializeField, Header("��VText")]
    private Text _rewardText = default;
    [SerializeField, Header("����Button")]
    private Button _decideButton = default;
    [SerializeField, Header("����{�^����Text")]
    private Text _decideButtonText = default;

    //�J�[�h�֘A�Q��
    [SerializeField, Header("�J�[�h�v���n�u")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("�{�[�i�X�J�[�h�I�u�W�F�N�g�̐e")]
    private Transform _cardsParent = default;
    //�퓬��ʃ}�l�[�W��
    private BattleManagerScript _battleManager = default;
    //�{�[�i�X�J�[�h���X�g
    private List<CardScript> _cardInstances = default;

    //�I�𒆂̃{�[�i�X�J�[�h���
    private List<CardScript> _selectingBonus = default;

    //�퓬�����G�̃f�[�^
    private EnemyStatusSO _enemySO = default;

    //�萔��`
    //��ʂ̃t�F�[�h�C���E�A�E�g����
    private const float FadeTime = 1.0f;

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="gameManager">�g�p����BattleManagerScript</param>
    public void Init(BattleManagerScript gameManager)
    {
        //�Q�Ǝ擾
        _battleManager = gameManager;

        //�ϐ�������
        _selectingBonus = new List<CardScript>();
        _cardInstances = new List<CardScript>();

        //UI������
        gameObject.SetActive(false);
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ��ʂ�\������
    /// </summary>
    /// <param name="enemyData">�|�����G�̃f�[�^</param>
    public void OpenWindow(EnemyStatusSO enemySO)
    {
        //�G�̃f�[�^�擾
        this._enemySO = enemySO;

        //CanvasGroup�����ݒ�
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        //�{�[�i�X�J�[�h�I�u�W�F�N�g��K�v���쐬����
        CreateBonusCards();
        //�퓬����Text�\��
        string resultMes = this._enemySO.GetEnemyName;
        resultMes += "�����j�����I\n" +"<size=30>�l���{�[�i�X��" +this._enemySO.GetBonusPoint +"�I��ł�������</size>";
        _rewardText.text = resultMes;
        //�{�^��UI�����ݒ�
        ShowRemainingSelections();
    }
    /// <summary>
    /// �w��̖��������{�[�i�X�J�[�h��\������
    /// </summary>
    private void CreateBonusCards()
    {
        //�A���ŃJ�[�h���o��
        int nowHandNum = 0;
        for (int i = 0; i < _enemySO.GetBonusOption; i++)
        {
            //�P����������
            CreateCard(nowHandNum);
            nowHandNum++;
        }
    }
    /// <summary>
    /// �{�[�i�X�J�[�h���P���쐬����
    /// </summary>
    /// <param name="handID">�Ώێ�D�ԍ�</param>
    private void CreateCard(int handID)
    {
        //�I�u�W�F�N�g�쐬
        GameObject obj = Instantiate(_cardPrefab, _cardsParent);
        //�J�[�h�����N���X���擾�E���X�g�Ɋi�[
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        // �o������J�[�h�������_���Ɍ��肷��
        CardDataSO targetCard = _enemySO.GetBonusCardList[Random.Range(0, _enemySO.GetBonusCardList.Count)];

        //�J�[�h�����ݒ�
        objCard.InitReward(this);
        objCard.SetInitialCardData(targetCard, CardScript.CharaID_Player);
    }

    /// <summary>
    /// �N���b�N��̃J�[�h�擾
    /// </summary>
    /// <param name="targetCard">�I�񂾃J�[�h</param>
    public void SelectCard(CardScript targetCard)
    {
        if (_selectingBonus.Contains(targetCard))
        {
            //���ɑI���ς݂̃J�[�h���^�b�v��
            //�J�[�h��������
            targetCard.SetCardHilight(false);

            //�I��������
            _selectingBonus.Remove(targetCard);
        }
        else
        {
            //���ɑI�𐔏���Ȃ珈�����Ȃ�
            if (_selectingBonus.Count >= _enemySO.GetBonusPoint)
            {
                return;
            }

            //�J�[�h�����\��
            targetCard.SetCardHilight(true);

            //�I���J�[�h�����L��
            _selectingBonus.Add(targetCard);
        }
        //�{�^��UI�ɔ��f
        ShowRemainingSelections();
    }
    /// <summary>
    /// �c��I���\�ȃ{�[�i�X�����{�^����Text�ɕ\��
    /// �I�������Ȃ�{�^���L����
    /// </summary>
    private void ShowRemainingSelections()
    {
        //���݂̑I���󋵂��擾
        int selectingNum = _enemySO.GetBonusPoint - _selectingBonus.Count;

        string mes = "";

        //�\�����e��ݒ�EButton�����ۂɔ��f
        if (selectingNum == 0)
        {
            //�I��������
            mes = "�l ��";
            //�{�^���L����
            _decideButton.interactable = true;
        }
        else
        {
            //�I�𖢊�����
            mes = "����" + selectingNum +"�I��";
            //�{�^��������
            _decideButton.interactable = false;
        }
        //Text���f
        _decideButtonText.text = mes;
    }

    /// <summary>
    /// Decide�{�^������������
    /// </summary>
    public void Button_Decide()
    {
      
            //�{�[�i�X�K�p
            foreach (CardScript card in _selectingBonus)
            {
                ApplyBonus(card);
            }
            //�J�[�h�I�u�W�F�N�g����
            foreach (CardScript card in _cardInstances)
            {
                Destroy(card.gameObject);
            }
            //�e���X�g��������
            _cardInstances.Clear();
            _selectingBonus.Clear();

            //�퓬���ʉ�ʃI�u�W�F�N�g��A�N�e�B�u��
            gameObject.SetActive(false);
            //�X�e�[�W�i�s�ĊJ
            _battleManager.ProgressingStage();
       
       
    }
    /// <summary>
    /// �I�������J�[�h�̃{�[�i�X���ʂ̓K�p����ѓ�����s��
    /// </summary>
    /// <param name="targetCard">�l���ΏۃJ�[�h</param>
    private void ApplyBonus(CardScript targetCard)
    {
        PlayerDeckDataScript.ChangeStorageCardNum(targetCard.GetCardData.GetSerialNum, 1);
    }
}
