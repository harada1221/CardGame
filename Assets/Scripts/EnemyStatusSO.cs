//-----------------------------------------------------------------------
/* �G�̃X�e�[�^�X���`����
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "EnemyStatusSO", menuName = " ScriptableObjects/EnemyStatusSO", order = 2)]
public class EnemyStatusSO : ScriptableObject
{
	[SerializeField,Header("�G�̖��O")]
	private string _enemyName = default;
	[SerializeField, Header("�G�摜")]
	private Sprite _charaSprite = default;
	[SerializeField, Header("�ő�HP(����HP)")]
	private int _maxHP = default;
	[SerializeField, Header("�e�^�[���Ɏg�p����J�[�h�Ɛݒu��̃��X�g")]
	private List<EnemyUseCardData> _useCardDatas = default;

	[SerializeField, Header("���j�{�[�i�X�F�I�����̌�")]
	private int _bonusOptions = default;
	[SerializeField, Header("���j�{�[�i�X�F�l���ł����")]
	private int _bonusPoint = default;
	[SerializeField, Header("���j�{�[�i�X�F�I�����ɏo������v���C���[�J�[�h")]
	private List<CardDataSO> _bonusCardList = default;


	public string GetEnemyName { get => _enemyName; }
	public Sprite GetCharaSprite { get => _charaSprite; }
	public int GetHP { get => _maxHP; }
	public List<EnemyUseCardData> GetUseCardDatas { get => _useCardDatas; }
	public int GetBonusOption { get => _bonusOptions; }
	public int GetBonusPoint { get => _bonusPoint; }
	public List<CardDataSO> GetBonusCardList { get => _bonusCardList; }
	/// <summary>
	/// �g�p����J�[�h�Ɛݒu��̃f�[�^�N���X
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
