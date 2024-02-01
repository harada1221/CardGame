//-----------------------------------------------------------------------
/* データの管理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections.Generic;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    [SerializeField, Header("シングルトン維持用")]
    public static DataScript _date = default;
    [SerializeField, Header("デッキ管理クラス")]
    private PlayerDeckDataScript _playerDeckData = default;
    [SerializeField, Header("選択可能ステージ")]
    private List<StageSO> _stageSOs = default;
    [SerializeField, Header("プレイヤーの最大HP")]
    private int _playerMaxHP = 20;
    [SerializeField, Header("プレイヤーの各ターンの手札枚数")]
    private int _playerHandNum = 5;
    //進行中ステージID
    public int _nowStageID = default;
    //プレイヤーデータ
    //所持金貨
    private int _playerGold = default;
    //獲得済み経験値
    private int _playerEXP = default;



    public int GetNowStageID { get => _nowStageID; }
    public int SetNowStageID { set { _nowStageID = value; } }
    public List<StageSO> GetStageSOs { get => _stageSOs; }
    public int GetPlayerGold { get => _playerGold; }
    public int GetPlayerExp { get => _playerEXP; }
    public int GetPlayerMaxHP { get => _playerMaxHP; }
    public int GetPlayerHandNum { get => _playerHandNum; }
    /// <summary>
    /// 初期設定
    /// </summary>
    private void Awake()
    {
        //シングルトン用処理
        if (_date != null)
        {
            Destroy(gameObject);
            return;
        }
        _date = this;
        //消えないようにする
        DontDestroyOnLoad(gameObject);
        //ゲーム起動時処理
        InitialProcess();
    }
    /// <summary>
    /// ゲーム開始時に乱数生成
    /// </summary>
    private void InitialProcess()
    {
        //乱数シード値初期化
        Random.InitState(System.DateTime.Now.Millisecond);
        //プレイヤーデッキデータの初期処理
        _playerDeckData.Init();
        //プレイヤー所持カードデータ初期化
        _playerDeckData.DataInitialize();
    }
    #region 各種プレイヤーデータ変更処理
    /// <summary>
    /// プレイヤーの所持金貨を変更する
    /// </summary>
    /// <param name="value">変化量</param>
    public void ChangePlayerGold(int value)
    {
        _playerGold += value;
    }
    /// <summary>
    /// プレイヤーの経験値量を変更する
    /// </summary>
    /// <param name="value">変化量</param>
    public void ChangePlayerEXP(int value)
    {
        _playerEXP += value;
    }
    /// <summary>
	/// プレイヤーの最大HPを変更する
	/// </summary>
	/// <param name="value">変化量</param>
	public void ChangePlayerMaxHP(int value)
    {
        _playerMaxHP += value;
    }
    /// <summary>
    /// プレイヤーの各ターンの手札枚数を変更する
    /// </summary>
    /// <param name="value">変化量</param>
    public void ChangePlayerHandNum(int value)
    {
        _playerHandNum += value;
    }
    #endregion
}
