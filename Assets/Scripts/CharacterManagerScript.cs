//-----------------------------------------------------------------------
/* キャラクターを管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using UnityEngine;

public class CharacterManagerScript : MonoBehaviour
{
    [SerializeField, Header("生成した敵オブジェクトの親")]
    private Transform _enemyPictureParent = default;
    [SerializeField, Header("敵キャラのPrehub")]
    private GameObject _enemyPicturePrefab = default;
    [SerializeField, Header("プレイヤーステータス")]
    private StatusUIScript _playerStatusUI = default;
    [SerializeField, Header("敵ステータス")]
    private StatusUIScript _enemyStatusUI = default;

    //戦闘のマネージャー
    private BattleManagerScript _battleManager = default;
    //敵の定義データ
    private EnemyStatusSO _enemyDate = default;
    //現在のHPデータ
    private int[] _nowHP = default;
    //最大HPデータ
    private int[] _maxHP = default;
    //各状態異常ポイント 
    private int[,] _statusEffectsPoints = default;
    //出現中の敵オブジェクト処理クラス
    private EnemyPictureScript _enemyPicture = default;

    public int[] GetNowHP { get => _nowHP; }
    public int[] GetMaxHP { get => _maxHP; }
    public EnemyStatusSO GetEnemyDate { get => _enemyDate; }
    public int[,] GetStatusEffect { get => _statusEffectsPoints; }

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

        //変数初期化
        _nowHP = new int[CardScript.CharaNum];
        _maxHP = new int[CardScript.CharaNum];
        ResetHP_Player();
        //状態異常ポイント初期化
        _statusEffectsPoints = new int[CardScript.CharaNum, (int)StatusEffectIconScript.StatusEffectType.MAX];

        //UI初期化
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
        // 敵ステータスを非表示
        _enemyStatusUI.HideCanvasGroup(false);
    }
    /// <summary>
	/// ターン終了時に実行される処理
	/// </summary>
	public void OnTurnEnd()
    {
        //キャラクターごとの状態異常処理
        for (int i = 0; i < CardScript.CharaNum; i++)
        {
            //毒の効果発動
            int poisonDamage = _statusEffectsPoints[i, (int)StatusEffectIconScript.StatusEffectType.Poison];
            if (poisonDamage > 0)
            {
                ChangeStatus_NowHP(i, -poisonDamage);
            }
            //残り効果量減少
            ChangeStatusEffect(i, StatusEffectIconScript.StatusEffectType.Poison, -1);
            ChangeStatusEffect(i, StatusEffectIconScript.StatusEffectType.Flame, -1);
        }
    }
    /// <summary>
	/// プレイヤーのHPを初期化する
	/// </summary>
	public void ResetHP_Player()
    {
        //HP初期化
        //プレイヤーの最大HP
        _maxHP[CardScript.CharaID_Player] = DataScript._date.GetPlayerMaxHP; 
        _nowHP[CardScript.CharaID_Player] = _maxHP[CardScript.CharaID_Player];
        //HP表示
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
        //各種状態異常初期化
        _statusEffectsPoints = new int[CardScript.CharaNum, (int)StatusEffectIconScript.StatusEffectType.MAX];
        RefleshStatusEffectUI();
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

        //ダメージ演出
        if (charaID == CardScript.CharaID_Enemy)
        {
            //撃破演出
            if (IsEnemyDefeated())
            {
                _enemyPicture.DefeatAnimation();
            }
            //被ダメージ演出
            else if (value < 0)
            {
                //_enemyPicture.DamageAnimation();
            }
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

        //状態異常：炎上の効果を適用
        if (value < 0)
        {
            //最大HP減少時
            int flameDamage = _statusEffectsPoints[charaID, (int)StatusEffectIconScript.StatusEffectType.Flame];
            _nowHP[charaID] -= flameDamage;
        }

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
        //ダメージ演出
        if (charaID == CardScript.CharaID_Enemy)
        {
            //撃破演出
            if (IsEnemyDefeated())
            {
                _enemyPicture.DefeatAnimation();
            }
            //被ダメージ演出
            else if (value < 0)
            {
                //_enemyPicture.DamageAnimation();
            }
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

        //敵画像オブジェクト作成
        GameObject obj = Instantiate(_enemyPicturePrefab, _enemyPictureParent);
        //敵画像処理クラス取得
        _enemyPicture = obj.GetComponent<EnemyPictureScript>();
        //敵画像処理クラス初期化
        _enemyPicture.Init(this, _enemyDate.GetCharaSprite);

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
        //オブジェクト削除
        _enemyPicture.gameObject.SetActive(false);
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
    /// <summary>
	/// 敵画像の座標を返す
	/// </summary>
	public Vector2 GetEnemyPosition()
    {
        return _enemyPicture.transform.position;
    }
    #endregion
    /// <summary>
	/// プレイヤーの最大HPを元に戻す
	/// </summary>
	public void RecoverMaxHP_Player()
    {
        //プレイヤーの最大HP
        _maxHP[CardScript.CharaID_Player] = DataScript._date.GetPlayerMaxHP;
        //大きかったら最大値にする
        if (_nowHP[CardScript.CharaID_Player] > _maxHP[CardScript.CharaID_Player])
        {
            _nowHP[CardScript.CharaID_Player] = _maxHP[CardScript.CharaID_Player];
        }
        //HP表示
        _playerStatusUI.SetHPView(_nowHP[CardScript.CharaID_Player], _maxHP[CardScript.CharaID_Player]);
    }
    #region 状態異常関連
    /// <summary>
	/// 指定キャラクターの状態異常の効果量を変更する
	/// </summary>
	public void ChangeStatusEffect(int charaID, StatusEffectIconScript.StatusEffectType effectType, int value)
    {
        //効果量変更
        _statusEffectsPoints[charaID, (int)effectType] += value;
        if (_statusEffectsPoints[charaID, (int)effectType] < 0)
        {
            _statusEffectsPoints[charaID, (int)effectType] = 0;
        }

        //UI反映
        RefleshStatusEffectUI();
    }
    /// <summary>
	/// 全ての状態異常表示UIの表示を更新する
	/// </summary>
	public void RefleshStatusEffectUI()
    {
        //プレイヤー側状態異常表示
        _playerStatusUI.SetStatusEffectUI(StatusEffectIconScript.StatusEffectType.Poison, _statusEffectsPoints[CardScript.CharaID_Player, (int)StatusEffectIconScript.StatusEffectType.Poison]);
        _playerStatusUI.SetStatusEffectUI(StatusEffectIconScript.StatusEffectType.Flame, _statusEffectsPoints[CardScript.CharaID_Player, (int)StatusEffectIconScript.StatusEffectType.Flame]);
        //敵側状態異常表示
        _enemyStatusUI.SetStatusEffectUI(StatusEffectIconScript.StatusEffectType.Poison, _statusEffectsPoints[CardScript.CharaID_Enemy, (int)StatusEffectIconScript.StatusEffectType.Poison]);
        _enemyStatusUI.SetStatusEffectUI(StatusEffectIconScript.StatusEffectType.Flame, _statusEffectsPoints[CardScript.CharaID_Enemy, (int)StatusEffectIconScript.StatusEffectType.Flame]);
    }

    #endregion
}
