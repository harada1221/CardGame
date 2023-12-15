//-----------------------------------------------------------------------
/* フィールドの管理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldAreaManagerScript : MonoBehaviour
{
    //戦闘画面マネージャー
    private BattleManagerScript _battleManager = default;
    //動かすカード
    private CardScript _draggingCard = default;
    /// <summary>
    /// BattleManagerScriptの初期化
    /// </summary>
    /// <param name="battleManager">使うBattleManagerScript</param>
    public void InBattleManager(BattleManagerScript battleManager)
    {
        //送られてきたBattleManagerScriptを格納する
        _battleManager = battleManager;
    }
    public void StartDragging(CardScript card)
    {
        _draggingCard = card;
        _draggingCard.Init(this);
    }
}
