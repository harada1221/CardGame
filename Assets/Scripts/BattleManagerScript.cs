//-----------------------------------------------------------------------
/* バトルの処理を管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManagerScript : MonoBehaviour
{
    //フィールドの管理クラス
    private FieldAreaManagerScript _fieldAreaScript = default;
    [SerializeField, Header("キャラクターデータ管理クラス")]
    private CharacterManagerScript _characterManager = default;
    [SerializeField, Header("報酬画面クラス")]
    private RewardScript _rewardPanel = default;
    [SerializeField, Header("カード効果発動管理クラス")]
    private PlayBoardManagerScript _playBoardManager = default;
    [SerializeField, Header("ステージ名")]
    private Text _stageNameText = default;
    [SerializeField, Header("ステージアイコン")]
    private Image _stageIconImage = default;
    [SerializeField, Header("ステージ背景")]
    private Image _stageBackGroundImage = default;
    [SerializeField, Header("ステージ進行度")]
    private Image _progressGageImage = default;
    [SerializeField, Header("攻略中ステージ")]
    private StageSO _stageSO = default;
    [SerializeField, Header("ボス表示")]
    private BossIncomingScript _bossIncoming = default;
    [SerializeField, Header("ステージクリアクラス")]
    private StageClearScript _stageClear = default;
    [SerializeField, Header("ゲームオーバークラス")]
    private GameOverScript _gameOver = default;
    //プレイヤーデータUI
    [SerializeField, Header("経験値量Text")]
    private Text _playerExpText = default;
    [SerializeField, Header("所持金貨Text")]
    private Text _playerGoldText = default;
    //経験値量Text表示用変数
    private int _playerExpDisp = default;
    //所持金貨Text表示用変数
    private int _playerGoldDisp = default;
    //現在の経過ターン
    private int _nowTurns = default;
    //現在のステージ進行度
    private int _nowProgress = default;
    //ボス出現進行度
    private int _battleNum = default;
    //定数定義
    //ゲージ表示演出時間
    private const float GageAnimationTime = 2.0f;
    private const float AnimationTime = 1.0f;
    //ボーナス量計算時のステージ進行度加算量
    private const int BonusValueBase = 4;
    //ボーナス量のランダム幅:最小
    private const float BonusRandomMulti_Min = 0.8f;
    //ボーナス量のランダム幅:最大
    private const float BonusRandomMulti_Max = 1.4f;
    public FieldAreaManagerScript GetFieldManager { get => _fieldAreaScript; }
    public CharacterManagerScript GetCharacterManager { get => _characterManager; }
    public PlayBoardManagerScript GetPlayBoardManager { get => _playBoardManager; }
    public int GetNowTurns { get => _nowTurns; }
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        _stageSO = DataScript._date.GetStageSOs[DataScript._date._nowStageID];
        //進行度初期化
        _nowProgress = -1;
        //ステージボスが出現する進行度を取得
        _battleNum = _stageSO.GetAppearEnemyTables.Count;
        //FieldAreaManagerScriptを取得する
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //各コンポーネント初期化
        _fieldAreaScript.Init(this);
        _characterManager.Init(this);
        _playBoardManager.Init(this);
        _bossIncoming.Init();
        _stageClear.Init();
        _gameOver.Init();
        _rewardPanel.Init(this);
        //経験値・金貨UI初期化
        ApplyEXPText();
        ApplyGoldText();
        //ステージ情報表示
        ApplyStageUIs();
        //敵を画面に出現させる
        DOVirtual.DelayedCall(
            1.0f, //1秒遅延
            () =>
            {
                ProgressingStage();
            }, false
        );
        //ステージBGM再生
        AudioSource audioSource = GetComponent<AudioSource>();
        //BGMクリップ設定
        audioSource.clip = _stageSO.GetBGM;
        //再生
        audioSource.Play(); 
    }

    #region ステージ進行関連
    /// <summary>
	/// ステージの進行度を進めて戦闘を開始するかステージを終了する
	/// </summary>
	public void ProgressingStage()
    {
        //進行度を進める
        _nowProgress++;

        //進行度を0.0f~1.0fで取得して表示
        float progressRatio = (float)(_nowProgress % _battleNum + 1) / _battleNum;
        progressRatio = Mathf.Clamp(progressRatio, 0.0f, 1.0f);
        if (progressRatio < 0)
        {
            progressRatio = 0;
        }
        ShowProgressGage(progressRatio);
        //次の敵出現
        DOVirtual.DelayedCall(
            0.5f,
            () =>
            {
                BattleStart();
            }
        );
    }
    /// <summary>
	/// 新しい敵との戦闘を開始する
	/// </summary>
	public void BattleStart()
    {
        //ターン数初期化
        _nowTurns = 0;

        //ステージクリアクリア確認
        if (_nowProgress >= _stageSO.GetAppearEnemyTables.Count)
        {
            //ステージクリア演出開始
            _stageClear.StartAnimation();
        }

        //敵キャラクター出現処理
        //出現する敵を決定
        List<EnemyStatusSO> appearEnemyTable = _stageSO.GetAppearEnemyTables[_nowProgress].GetAppearEnemys;
        int rand = Random.Range(0, appearEnemyTable.Count);
        _characterManager.SpawnEnemy(appearEnemyTable[rand]);
        //出現する敵が一種類しか居ないならボス用演出
        if (appearEnemyTable.Count == 1)
        {
            _bossIncoming.StartAnimation();
        }

        //戦闘開始処理
        DOVirtual.DelayedCall(
            1.0f,
            () =>
            {
                //フィールド側戦闘開始時処理
                _fieldAreaScript.OnBattleStarting();
                //ターン開始時処理
                TurnStart();
            }, false
        );
    }
    /// <summary>
	/// ターンを開始する
	/// </summary>
	public void TurnStart()
    {
        //FieldManager側ターン開始時処理
        _fieldAreaScript.OnTurnStarting();

        //ターン数カウント
        _nowTurns++;
    }
    /// <summary>
	/// ターンを終了する
	/// </summary>
	public void TurnEnd()
    {
        //FieldManager側終了時処理呼出
        _fieldAreaScript.OnTurnEnd();
        //CharacterManager側終了時処理呼出
        _characterManager.OnTurnEnd();

        // 戦闘終了を確認
        bool isPlayerWin = _characterManager.IsEnemyDefeated();
        bool isPlayerLose = _characterManager.IsPlayerDefeated();

        //戦闘終了処理
        if (isPlayerWin || isPlayerLose)
        {
            //フィールドのカードを消去
            _fieldAreaScript.DestroyAllCards();

            //勝利キャラクター別処理(遅延実行)
            DOVirtual.DelayedCall(
                0.5f,
                () =>
                {
                    // プレイヤー敗北時
                    if (isPlayerLose)
                    {
                        //ゲームオーバー演出開始
                        _gameOver.StartAnimation();
                    }
                    // プレイヤー勝利時
                    else if (isPlayerWin)
                    {
                        //勝利画面表示
                        _rewardPanel.OpenWindow(_characterManager.GetEnemyDate);
                    }
                }
            );
            return;
        }
        //次のターン開始
        TurnStart();
    }
    #endregion

    #region ステージUI
    /// <summary>
    /// 攻略中ステージデータを基に各種ステージ情報UIの表示を更新する
    /// </summary>
    private void ApplyStageUIs()
    {
        //ステージ名
        _stageNameText.text = _stageSO.GetStageName;
        //ステージアイコン
        _stageIconImage.sprite = _stageSO.GetStageIcon;
        //ステージ背景
        _stageBackGroundImage.sprite = _stageSO.GetStageBackGround;
        //ステージ進行度ゲージ初期化
        _progressGageImage.fillAmount = 0.0f;
    }
    /// <summary>
	/// ステージ進行度ゲージを表示する
	/// </summary>
	/// <param name="ratio">進行度割合(0.0f-1.0f)</param>
	public void ShowProgressGage(float ratio)
    {
        _progressGageImage.DOFillAmount(ratio, GageAnimationTime);
    }
    /// <summary>
	/// 経験値量Textの表示を更新する
	/// </summary>
	public void ApplyEXPText()
    {
        //少しずつ数字が変化する演出
        DOTween.To(() =>
            _playerExpDisp, (n) => _playerExpDisp = n, DataScript._date.GetPlayerExp, AnimationTime)
            .OnUpdate(() =>
            {
                _playerExpText.text = _playerExpDisp.ToString("#,0") + " EXP";
            });
    }
    /// <summary>
    /// 所持金貨Textの表示を更新する
    /// </summary>
    public void ApplyGoldText()
    {
        //少しずつ数字が変化する演出
        DOTween.To(() =>
            _playerGoldDisp, (n) => _playerGoldDisp = n, DataScript._date.GetPlayerGold, AnimationTime)
            .OnUpdate(() =>
            {
                _playerGoldText.text = _playerGoldDisp.ToString("#,0") + " G";
            });
    }
    #endregion

    #region ステージ戦闘報酬
    /// <summary>
	/// 敵撃破ボーナスの入手金貨量を返す
	/// </summary>
	public int GetBonusGoldValue()
    {
        //基本獲得量
        int value = _stageSO.GetGold * (BonusValueBase + _nowProgress);
        //ランダム幅適用
        value = (int)(value * Random.Range(BonusRandomMulti_Min, BonusRandomMulti_Max));
        return value;
    }
    /// <summary>
	/// 敵撃破ボーナスの入手経験値量を返す
	/// </summary>
	public int GetBonusEXPValue()
    {
        //基本獲得量
        int value = _stageSO.GetExp * (BonusValueBase + _nowProgress);
        //ランダム幅適用
        value = (int)(value * Random.Range(BonusRandomMulti_Min, BonusRandomMulti_Max));
        return value;
    }
    /// <summary>
    /// 敵撃破ボーナスのHP回復量を返す
    /// </summary>
    public int GetBonusHealValue()
    {
        //基本回復量
        int value = _stageSO.GetHeal;
        //ランダム幅適用
        value = (int)(value * Random.Range(BonusRandomMulti_Min, BonusRandomMulti_Max));
        return value;
    }
    #endregion
}
