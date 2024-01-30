using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardScript : MonoBehaviour
{
    [SerializeField, Header("親CanvasGroup")]
    private CanvasGroup _canvasGroup = default;
    //UIオブジェクト
    [SerializeField, Header("報酬Text")]
    private Text _rewardText = default;
    [SerializeField, Header("決定Button")]
    private Button _decideButton = default;
    [SerializeField, Header("決定ボタン内Text")]
    private Text _decideButtonText = default;

    //カード関連参照
    [SerializeField, Header("カードプレハブ")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("ボーナスカードオブジェクトの親")]
    private Transform _cardsParent = default;
    [SerializeField, Header("ボーナス専用カードデータのリスト")]
    private List<CardDataSO> _bonusCardSOsList = default;
    //戦闘画面マネージャ
    private BattleManagerScript _battleManager = default;
    //ボーナスカードリスト
    private List<CardScript> _cardInstances = default;
    //選択中のボーナスカード情報
    private List<CardScript> _selectingBonus = default;
    //戦闘した敵のデータ
    private EnemyStatusSO _enemySO = default;

    //画面のフェードイン・アウト時間
    private const float FadeTime = 1.0f;
    //ボーナス専用カードの出現率
    private const float Percentage_BonusOnlyCards = 0.5f;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="gameManager">使用するBattleManagerScript</param>
    public void Init(BattleManagerScript gameManager)
    {
        //参照取得
        _battleManager = gameManager;

        //変数初期化
        _selectingBonus = new List<CardScript>();
        _cardInstances = new List<CardScript>();

        //UI初期化
        gameObject.SetActive(false);
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 画面を表示する
    /// </summary>
    /// <param name="enemyData">倒した敵のデータ</param>
    public void OpenWindow(EnemyStatusSO enemySO)
    {
        //敵のデータ取得
        this._enemySO = enemySO;

        //CanvasGroup初期設定
        gameObject.SetActive(true);
        _canvasGroup.alpha = 1.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        //ボーナスカードオブジェクトを必要分作成する
        CreateBonusCards();
        //戦闘結果Text表示
        string resultMes = this._enemySO.GetEnemyName;
        resultMes += "を撃破した！\n" + "<size=30>獲得ボーナスを" + this._enemySO.GetBonusPoint + "つ選んでください</size>";
        _rewardText.text = resultMes;
        //ボタンUI初期設定
        ShowRemainingSelections();
    }
    /// <summary>
    /// 指定の枚数だけボーナスカードを表示する
    /// </summary>
    private void CreateBonusCards()
    {
        //連続でカードを出現
        int nowHandNum = 0;
        for (int i = 0; i < _enemySO.GetBonusOption; i++)
        {
            //１枚引く処理
            CreateCard(nowHandNum);
            nowHandNum++;
        }
    }
    /// <summary>
    /// ボーナスカードを１枚作成する
    /// </summary>
    /// <param name="handID">対象手札番号</param>
    private void CreateCard(int handID)
    {
        //オブジェクト作成
        GameObject obj = Instantiate(_cardPrefab, _cardsParent);
        //カード処理クラスを取得・リストに格納
        CardScript objCard = obj.GetComponent<CardScript>();
        _cardInstances.Add(objCard);

        //出現するカードをランダムに決定する
        CardDataSO targetCard = null;
        //ボーナス専用カード出現時フラグ
        bool isBonusOnlyCard;
        if (Random.value < Percentage_BonusOnlyCards)
        {
            //ボーナス専用カード
            isBonusOnlyCard = true;
            //カードの種類を決定
            int rand = Random.Range(0, _bonusCardSOsList.Count);
            targetCard = _bonusCardSOsList[rand];
        }
        else
        {
            //プレイヤーが獲得できるカード
            targetCard = _enemySO.GetBonusCardList[Random.Range(0, _enemySO.GetBonusCardList.Count)];
            isBonusOnlyCard = false;
        }

        //カード初期設定
        objCard.InitReward(this);
        //ボーナス専用カードの場合は背景に別のものを使用する
        if (isBonusOnlyCard)
        {
            objCard.SetInitialCardData(targetCard, CardScript.CharaID_Bonus);
        }
        else
        {
            objCard.SetInitialCardData(targetCard, CardScript.CharaID_Player);
        }
        // ボーナスカードの場合各種効果量を適用する
        objCard.EnhanceCardEffect(CardEffectDefineScript.CardEffect.Bonus_EXP, _battleManager.GetBonusEXPValue());
        objCard.EnhanceCardEffect(CardEffectDefineScript.CardEffect.Bonus_Gold, _battleManager.GetBonusGoldValue());
        objCard.EnhanceCardEffect(CardEffectDefineScript.CardEffect.Bonus_Heal, _battleManager.GetBonusHealValue());
    }

    /// <summary>
    /// クリック先のカード取得
    /// </summary>
    /// <param name="targetCard">選んだカード</param>
    public void SelectCard(CardScript targetCard)
    {
        if (_selectingBonus.Contains(targetCard))
        {
            //既に選択済みのカードをタップ時
            //カード強調解除
            targetCard.SetCardHilight(false);

            //選択を解除
            _selectingBonus.Remove(targetCard);
        }
        else
        {
            //既に選択数上限なら処理しない
            if (_selectingBonus.Count >= _enemySO.GetBonusPoint)
            {
                return;
            }

            //カード強調表示
            targetCard.SetCardHilight(true);

            //選択カード情報を記憶
            _selectingBonus.Add(targetCard);
        }
        //ボタンUIに反映
        ShowRemainingSelections();
    }
    /// <summary>
    /// 残り選択可能なボーナス数をボタン内Textに表示
    /// 選択完了ならボタン有効化
    /// </summary>
    private void ShowRemainingSelections()
    {
        //現在の選択状況を取得
        int selectingNum = _enemySO.GetBonusPoint - _selectingBonus.Count;

        string mes = "";

        //表示内容を設定・Button押下可否に反映
        if (selectingNum == 0)
        {
            //選択完了時
            mes = "獲 得";
            //ボタン有効化
            _decideButton.interactable = true;
        }
        else
        {
            //選択未完了時
            mes = "あと" + selectingNum + "つ選択";
            //ボタン無効化
            _decideButton.interactable = false;
        }
        //Text反映
        _decideButtonText.text = mes;
    }

    /// <summary>
    /// Decideボタン押下時処理
    /// </summary>
    public void Button_Decide()
    {

        //ボーナス適用
        foreach (CardScript card in _selectingBonus)
        {
            ApplyBonus(card);
        }
        //カードオブジェクト消去
        foreach (CardScript card in _cardInstances)
        {
            Destroy(card.gameObject);
        }
        //各リストを初期化
        _cardInstances.Clear();
        _selectingBonus.Clear();

        //戦闘結果画面オブジェクト非アクティブ化
        gameObject.SetActive(false);
        //ステージ進行再開
        _battleManager.ProgressingStage();


    }
    /// <summary>
    /// 選択したカードのボーナス効果の適用および入手を行う
    /// </summary>
    /// <param name="targetCard">獲得対象カード</param>
    private void ApplyBonus(CardScript targetCard)
    {
        //経験値獲得
        int valueEXP = targetCard.GetEffectValue(CardEffectDefineScript.CardEffect.Bonus_EXP);
        if (valueEXP > 0)
        {
            DataScript._date.ChangePlayerEXP(valueEXP);
            _battleManager.ApplyEXPText();
        }
        //金貨獲得
        int valueGold = targetCard.GetEffectValue(CardEffectDefineScript.CardEffect.Bonus_Gold);
        if (valueGold > 0)
        {
            DataScript._date.ChangePlayerGold(valueGold);
            _battleManager.ApplyGoldText();
        }
        //体力回復
        int valueHeal = targetCard.GetEffectValue(CardEffectDefineScript.CardEffect.Bonus_Heal);
        if (valueHeal > 0)
        {
            _battleManager.GetCharacterManager.ChangeStatus_NowHP(CardScript.CharaID_Player, valueHeal);
        }
        //上記のいずれの効果も持っていない場合はプレイヤーが入手するカードとして扱う
        if (valueEXP < 0 && valueGold < 0 && valueHeal < 0)
        {
            PlayerDeckDataScript.ChangeStorageCardNum(targetCard.GetCardData.GetSerialNum, 1);
        }
    }
}
