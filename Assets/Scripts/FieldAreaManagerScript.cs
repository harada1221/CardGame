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
using UnityEngine.EventSystems;

public class FieldAreaManagerScript : MonoBehaviour
{
    //�퓬��ʃ}�l�[�W���[
    private BattleManagerScript _battleManager = default;
    //�������J�[�h
    private CardScript _draggingCard = default;
    /// <summary>
    /// BattleManagerScript�̏�����
    /// </summary>
    /// <param name="battleManager">�g��BattleManagerScript</param>
    public void InBattleManager(BattleManagerScript battleManager)
    {
        //�����Ă���BattleManagerScript���i�[����
        _battleManager = battleManager;
    }
    public void StartDragging(CardScript card)
    {
        _draggingCard = card;
        _draggingCard.Init(this);
    }
}
