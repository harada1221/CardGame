//-----------------------------------------------------------------------
/* バトルの処理を管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerScript : MonoBehaviour
{
    //フィールドの管理クラス
    private FieldAreaManagerScript _fieldAreaScript = default;
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        //FieldAreaManagerScriptを取得する
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        // 管理下コンポーネント初期化
        _fieldAreaScript.InBattleManager(this);
    }

}
