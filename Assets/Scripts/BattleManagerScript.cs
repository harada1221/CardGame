//-----------------------------------------------------------------------
/* バトルの処理を管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
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
    //[SerializeField, Header("出現敵データ")]
    //private EnemyStatusSO _enemyStatusSO = default;
    [SerializeField, Header("カード効果発動管理クラス")]
    private PlayBoardManagerScript _playBoardManager = default;

    [SerializeField, Header("ステージ名")]
    private Text _stageNameText = default;
    [SerializeField, Header("ステージアイコン")]
    private Image _stageIconImage = default;
    [SerializeField, Header("ステージ背景")]
    private Image _stageBackGroundImage = default;
    [SerializeField, Header("")]
    private Image _progressGageImage = default;
    [SerializeField, Header("攻略中ステージ")]
    private StageSO _stageSO = default;

    //現在の経過ターン
    private int _nowTurns = default;
    //現在のステージ進行度
    private int _nowProgress = default;
    //ボス出現進行度
    private int _battleNum = default;
    //ゲージ表示演出時間
    private const float GageAnimationTime = 2.0f;

    public FieldAreaManagerScript GetFieldManager { get => _fieldAreaScript; }
    public CharacterManagerScript GetCharacterManager { get => _characterManager; }
    public PlayBoardManagerScript GetPlayBoardManager { get => _playBoardManager; }
    public int GetNowTurns { get => _nowTurns; }
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //進行度初期化
        _nowProgress = -1;
        //ステージボスが出現する進行度を取得
        _battleNum = _stageSO.GetAppearEnemyTables.Count;
        //FieldAreaManagerScriptを取得する
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //コンポーネント初期化
        _fieldAreaScript.InBattleManager(this);
        _characterManager.Init(this);
        _playBoardManager.Init(this);
        //ステージ情報表示
        ApplyStageUIs();
        // (デバッグ用)敵を画面に出現させる
        DOVirtual.DelayedCall(
            1.0f, // 1秒遅延
            () =>
            {
                ProgressingStage();
            }, false
        );
    }
    /// <summary>
    /// 更新処理
    /// </summary>
   
    #region ステージ進行関連
    /// <summary>
	/// ステージの進行度を進めて戦闘を開始するかステージを終了する
	/// </summary>
	public void ProgressingStage()
    {
        //進行度を進める
        _nowProgress++;

        // 進行度を0.0f~1.0fで取得して表示
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
            //全ての敵との戦闘に勝利した
            Debug.Log("ステージクリア");
        }

        //敵キャラクター出現処理
        //出現する敵を決定
        List<EnemyStatusSO> appearEnemyTable = _stageSO.GetAppearEnemyTables[_nowProgress]._appearEnemys;
        int rand = Random.Range(0, appearEnemyTable.Count);
        _characterManager.SpawnEnemy(appearEnemyTable[rand]);

        //戦闘開始処理(遅延実行)
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
            //_fieldAreaScript.DestroyAllCards();

            //勝利キャラクター別処理(遅延実行)
            DOVirtual.DelayedCall(
                0.5f,
                () =>
                {
                    // プレイヤー敗北時
                    if (isPlayerLose)
                    {
                        Debug.Log("ゲームオーバー");
                    }
                    // プレイヤー勝利時
                    else if (isPlayerWin)
                    {
                        // 進行度増加
                        ProgressingStage();
                    }
                }
            );

            return;
        }

        // 次のターン開始
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
    #endregion
}
