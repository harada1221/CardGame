using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIconScript : MonoBehaviour
{
    [SerializeField,Header("効果表示Text")]
    private Text _valueText = default;
    //現在の状態以上
	[SerializeField,Header("状態以上")]
    private StatusEffectType _statusEffectType = default;
    public StatusEffectType GetStatusEffect { get => _statusEffectType; }
    //状態異常の種類定義
    public enum StatusEffectType
    {
        Poison, //毒
        Flame,  //炎上
        MAX,    //最大値
    }
	/// <summary>
	/// アイコンの表示を設定する
	/// </summary>
	/// <param name="value">効果量</param>
	public void SetValue(int value)
	{
		//効果値が0以上か
		if (value > 0)
		{
			//アイコン表示
			gameObject.SetActive(true);
			//効果量Text反映
			_valueText.text = value.ToString();
		}
		else
		{
			//アイコン非表示
			gameObject.SetActive(false);
		}
	}
}
