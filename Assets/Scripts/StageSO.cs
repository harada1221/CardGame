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
	[Header("ステージアイコン画像")]
	public Sprite _stageIcon = default;
	[Header("ステージ背景画像")]
	public Sprite _stageBackGround = default;

	[Space(10)]
	[SerializeField,Header("各進行度別の敵の出現テーブル")]
	private List<appearEnemyTable> _appearEnemyTables = default;

	public List<appearEnemyTable> GetAppearEnemyTables { get => _appearEnemyTables; }
	public string GetStageName { get => _stageName; }
	public Sprite GetStageIcon { get => _stageIcon; }
	public Sprite GetStageBackGround { get => _stageBackGround; }
}

/// <summary>
/// 各進行度別の敵の出現テーブルクラス
/// </summary>
[System.Serializable]
public class appearEnemyTable
{
	//敵出現テーブル(1体のみの指定でボス敵扱いにする)
	public List<EnemyStatusSO> _appearEnemys = default;
}