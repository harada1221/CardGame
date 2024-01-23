using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectIconScript : MonoBehaviour
{
    [SerializeField,Header("���ʕ\��Text")]
    private Text _valueText = default;
    //���݂̏�Ԉȏ�
	[SerializeField,Header("��Ԉȏ�")]
    private StatusEffectType _statusEffectType = default;
    public StatusEffectType GetStatusEffect { get => _statusEffectType; }
    //��Ԉُ�̎�ޒ�`
    public enum StatusEffectType
    {
        Poison, //��
        Flame,  //����
        MAX,    //�ő�l
    }
	/// <summary>
	/// �A�C�R���̕\����ݒ肷��
	/// </summary>
	/// <param name="value">���ʗ�</param>
	public void SetValue(int value)
	{
		//���ʒl��0�ȏォ
		if (value > 0)
		{
			//�A�C�R���\��
			gameObject.SetActive(true);
			//���ʗ�Text���f
			_valueText.text = value.ToString();
		}
		else
		{
			//�A�C�R����\��
			gameObject.SetActive(false);
		}
	}
}
