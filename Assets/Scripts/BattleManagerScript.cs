//-----------------------------------------------------------------------
/* �o�g���̏������Ǘ�����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerScript : MonoBehaviour
{
    //�t�B�[���h�̊Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;
    //�J�[�h�f�[�^���擾
    [SerializeField,Header("�J�[�h���X�g")]
    private CardDataSO _cardDate = default;

    /// <summary>
    /// ����������
    /// </summary>
    private void Start()
    {
        //FieldAreaManagerScript���擾����
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //�R���|�[�l���g������
        _fieldAreaScript.InBattleManager(this);
        //�J�[�h���ʖ��\��
        foreach (CardEffectDefineScript cardEffect in _cardDate.GetEffectList)
        {
            //���ʂ̕������擾
            string nameText = CardEffectDefineScript.Dic_EffectName[cardEffect.GetEffect];
            //���ʒl�ϐ��𕶎���ɑ��
            nameText = string.Format(nameText, cardEffect.GetValue);

            Debug.Log(nameText);
        }
    }

}
