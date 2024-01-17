//-----------------------------------------------------------------------
/* データの管理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataScript : MonoBehaviour
{
    [SerializeField ,Header("シングルトン維持用")]
    private static DataScript _date = default;
	[SerializeField, Header("デッキ管理クラス")]
	private PlayerDeckDataScript _playerDeckData = default;
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
}
