using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CharacterManagerScript : MonoBehaviour
{
    [SerializeField, Header("プレイヤーステータス")]
    private StatusUIScript _playerStatusUI = default;
    [SerializeField, Header("敵ステータス")]
    private StatusUIScript _enemyStatusUI = default;
    [SerializeField, Header("初期HP")]
    private int _fastHP = 30;
    //戦闘のマネージャー
    private BattleManagerScript _battleManager = default;
    //敵の定義データ
    private EnemyStatusSO _enemyDate = default;
    //現在のHPデータ
    private int[] _nowHP = default;
    //最大HPデータ
    private int[] _maxHP = default;

    //振動演出強度
    public const float _shakeAnimPower = 18.0f;
    //振動演出時間
    public const float _shakeAnimTime = 0.4f;
    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="battleManeger">バトルマネージャー</param>
    public void Init(BattleManagerScript battleManeger)
    {
        //BattleManagerScript参照取得
        _battleManager = battleManeger;

        // 変数初期化
        _nowHP = new int[CardScript.CharaNum];
        _maxHP = new int[CardScript.CharaNum];
        ResetHP_Player();

        //UI初期化
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
        // 敵ステータスを非表示
        _enemyStatusUI.HideCanvasGroup(false);
    }
    /// <summary>
	/// プレイヤーのHPを初期化する
	/// </summary>
	public void ResetHP_Player()
    {
        //HP初期化
        _maxHP[CardScript.CharaID_Player] = _fastHP;
        _nowHP[CardScript.CharaID_Player] = _maxHP[CardScript.CharaID_Player];
        //HP表示
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
    }
    /// <summary>
    /// 現在HPを変更する
    /// </summary>
    /// <param name="charaID">キャラID</param>
    /// <param name="value">変化量</param>
    public void ChangeStatus_NowHP(int charaID, int value)
    {
        //既に現在HP0以下なら処理しない
        if (_nowHP[charaID] <= 0)
        {
            return;
        }
        //現在HP変更
        _nowHP[charaID] += value;
        //最大HPを越えないようにする
        if (_nowHP[charaID] > _maxHP[charaID])
        {
            _nowHP[charaID] = _maxHP[charaID];
        }
        //UI反映
        if (charaID == CardScript.CharaID_Player)
        {
            //プレイヤーのHP
            _playerStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }
        else
        {
            //敵のHP
            _enemyStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }

    }/// <summary>
     /// 最大HPを変更する
     /// </summary>
     /// <param name="charaID">キャラID</param>
     /// <param name="value">変化量</param>
    public void ChangeStatus_MaxHP(int charaID, int value)
    {
        //既に最大HP0なら処理しない
        if (_maxHP[charaID] <= 0)
        {
            return;
        }

        //最大HP変更
        _maxHP[charaID] += value;
        //現在HPの上限・下限を反映
        _nowHP[charaID] = Mathf.Clamp(_nowHP[charaID], 0, _maxHP[charaID]);

        // UI反映
        if (charaID == CardScript.CharaID_Player)
        {
            //プレイヤーのHP
            _playerStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }
        else
        {
            //敵のHP
            _enemyStatusUI.SetHPView(_nowHP[charaID], _maxHP[charaID]);
        }
    }
    #region 敵への処理
    /// <summary>
    /// 敵の出現を行う
    /// </summary>
    /// <param name="spawnEnemyData">出現させる敵のデータ</param>
    public void SpawnEnemy(EnemyStatusSO spawnEnemyData)
    {
        //敵データ取得
        _enemyDate = spawnEnemyData;

        //敵ステータス初期化
        _nowHP[CardScript.CharaID_Enemy] = _enemyDate.GetHP;
        _maxHP[CardScript.CharaID_Enemy] = _enemyDate.GetHP;

        //敵ステータスUI表示
        _enemyStatusUI.ShowCanvasGroup();
        _enemyStatusUI.SetCharacterName(_enemyDate.GetEnemyName);
        _enemyStatusUI.SetHPView(_nowHP[CardScript.CharaID_Enemy], _maxHP[CardScript.CharaID_Enemy]);
    }
    /// <summary>
	/// 敵を消去する
	/// </summary>
	public void DeleteEnemy()
    {

    }
    #endregion

    #region 各種判定
    /// <summary>
    /// プレイヤーのHPが0以下か
    /// </summary>
    /// <returns>プレイヤー敗北フラグ</returns>
    public bool IsPlayerDefeated()
    {
        if (_nowHP[CardScript.CharaID_Player] <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 敵のHPが0以下か
    /// </summary>
    /// <returns>敵撃破フラグ</returns>
    public bool IsEnemyDefeated()
    {
        if (_nowHP[CardScript.CharaID_Enemy] <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
