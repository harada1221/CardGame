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
using DG.Tweening;

public class BattleManagerScript : MonoBehaviour
{
    //フィールドの管理クラス
    private FieldAreaManagerScript _fieldAreaScript = default;
    [SerializeField, Header("キャラクターデータ管理クラス")]
    private CharacterManagerScript _characterManager = default;
    //カードデータを取得
    //[SerializeField, Header("カードリスト")]
    //private CardDataSO _cardDate = default;
    [SerializeField, Header("出現敵データ")]
    private EnemyStatusSO _enemyStatusSO = default;
    [SerializeField,Header("カード効果発動管理クラス")]
    private PlayBoardManagerScript _playBoardManager = default;


    public FieldAreaManagerScript GetFieldManager { get => _fieldAreaScript; }
    public CharacterManagerScript GetCharacterManager { get => _characterManager; }
    public PlayBoardManagerScript GetPlayBoardManager { get => _playBoardManager; }
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //FieldAreaManagerScriptを取得する
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //コンポーネント初期化
        _fieldAreaScript.InBattleManager(this);
        _characterManager.Init(this);
        _playBoardManager.Init(this);
        // (デバッグ用)敵を画面に出現させる
        DOVirtual.DelayedCall(
            1.0f, // 1秒遅延
            () =>
            {
                // 敵出現
                _characterManager.SpawnEnemy(_enemyStatusSO);
            }, false
        );
    }
    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        // (デバッグ用)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _characterManager.ChangeStatus_NowHP(CardScript.CharaID_Player, -5);
        }
    }
}
