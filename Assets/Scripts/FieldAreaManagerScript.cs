//-----------------------------------------------------------------------
/* �t�B�[���h�̊Ǘ����s���N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldAreaManagerScript : MonoBehaviour
{
    //�퓬��ʃ}�l�[�W���[
    private BattleManagerScript _battleManager = default;

    /// <summary>
    /// BattleManagerScript�̏�����
    /// </summary>
    /// <param name="battleManager">�g��BattleManagerScript</param>
    public void InBattleManager(BattleManagerScript battleManager)
    {
        //�����Ă���BattleManagerScript���i�[����
        _battleManager = battleManager;
    }
}
