//-----------------------------------------------------------------------
/* 敵のステータスを定義する
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStatusSO", menuName = " ScriptableObjects/EnemyStatusSO", order = 2)]
public class EnemyStatusSO : ScriptableObject
{
	[SerializeField,Header("敵の名前")]
	private string _enemyName = default;
	[SerializeField, Header("敵画像")]
	private Sprite _charaSprite = default;
	[SerializeField, Header("最大HP(初期HP)")]
	private int _maxHP = default;
	[SerializeField, Header("各ターンに使用するカードと設置先のリスト")]
	private List<EnemyUseCardData> _useCardDatas = default;

	[SerializeField, Header("撃破ボーナス：選択肢の個数")]
	private int _bonusOptions = default;
	[SerializeField, Header("撃破ボーナス：獲得できる個数")]
	private int _bonusPoint = default;
	[SerializeField, Header("撃破ボーナス：選択肢に出現するプレイヤーカード")]
	private List<CardDataSO> _bonusCardList = default;


	public string GetEnemyName { get => _enemyName; }
	public Sprite GetCharaSprite { get => _charaSprite; }
	public int GetHP { get => _maxHP; }
	public List<EnemyUseCardData> GetUseCardDatas { get => _useCardDatas; }
	public int GetBonusOption { get => _bonusOptions; }
	public int GetBonusPoint { get => _bonusPoint; }
	public List<CardDataSO> GetBonusCardList { get => _bonusCardList; }
	/// <summary>
	/// 使用するカードと設置先のデータクラス
	/// </summary>
	[System.Serializable]
	public class EnemyUseCardData
	{
		public CardDataSO placeCardData0 = default; 
		public CardDataSO placeCardData1 = default;
		public CardDataSO placeCardData2 = default;
		public CardDataSO placeCardData3 = default; 
		public CardDataSO placeCardData4 = default; 
	}
}
