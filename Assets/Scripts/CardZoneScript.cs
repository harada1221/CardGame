//-----------------------------------------------------------------------
/* カードゾーンを設定するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月20日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoneScript : MonoBehaviour
{
    // ゾーン種類定義
    public enum ZoneType
    {
        //手札
        Hand,
        //プレイボード0～4番目
        PlayBoard0,
        PlayBoard1,
        PlayBoard2,
        PlayBoard3,
        PlayBoard4,
        //トラッシュ
        Trash,
    }
    [SerializeField,Header("ゾーンの種類")]
    private ZoneType _zoneType = default;
    public ZoneType GetZoneType{ get => _zoneType; }
}
