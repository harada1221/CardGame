//-----------------------------------------------------------------------
/* ステージ進行を管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageSO", menuName = " ScriptableObjects/StageSO")]
public class StageSO : ScriptableObject
{
	[SerializeField,Header("ステージ名")]
	private string _stageName = default;
	[Space(10)]
	[SerializeField, Header("難易度表示(日本語)")]
	private string _difficulty;

	[Space(10)]
	[SerializeField,Header("ステージアイコン画像")]
	private Sprite _stageIcon = default;
	[SerializeField, Header("ステージ背景画像")]
	private Sprite _stageBackGround = default;

	[Space(10)]
	[SerializeField,Header("各進行度別の敵の出現テーブル")]
	private List<appearEnemyTable> _appearEnemyTables = default;

	public List<appearEnemyTable> GetAppearEnemyTables { get => _appearEnemyTables; }
	public string GetStageName { get => _stageName; }
	public Sprite GetStageIcon { get => _stageIcon; }
	public Sprite GetStageBackGround { get => _stageBackGround; }
	public string GetDifficulty { get => _difficulty; }
}

/// <summary>
/// 各進行度別の敵の出現テーブルクラス
/// </summary>
[System.Serializable]
public class appearEnemyTable
{
	//敵出現テーブル
	[SerializeField,Header("敵の出現テーブル")]
	private List<EnemyStatusSO> _appearEnemys = default;
	public List<EnemyStatusSO> GetAppearEnemys { get => _appearEnemys; }
}