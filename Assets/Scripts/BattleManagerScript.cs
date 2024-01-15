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
    //カードデータを取得
    [SerializeField,Header("カードリスト")]
    private CardDataSO _cardDate = default;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //FieldAreaManagerScriptを取得する
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        //コンポーネント初期化
        _fieldAreaScript.InBattleManager(this);
        //カード効果名表示
        foreach (CardEffectDefineScript cardEffect in _cardDate.GetEffectList)
        {
            //効果の文字を取得
            string nameText = CardEffectDefineScript.Dic_EffectName[cardEffect.GetEffect];
            //効果値変数を文字列に代入
            nameText = string.Format(nameText, cardEffect.GetValue);

            Debug.Log(nameText);
        }
    }

}
