//-----------------------------------------------------------------------
/* SEを管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManagerScript : MonoBehaviour
{
    //静的参照
    public static SEManagerScript instance { get; private set; }

    //SE再生用AudioSource
    private AudioSource _audioSource = default;
    [SerializeField, Header("登録効果音参照リスト")]
    private List<AudioClip> _seClips = default;

    //効果音定義リスト
    public enum SEName
    {
        DecideA,            //ボタン音A
        DecideB,            //ボタン音B
        DamageToEnemy,      //敵にダメージ
        DamageToPlayer,     //プレイヤーにダメージ
        DrawCard,           //カードドロー
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
        //参照取得
        instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

   /// <summary>
   /// SEを流す
   /// </summary>
   /// <param name="seName">流す音の名前</param>
    public void PlaySE(SEName seName)
    {
        //SE再生
        _audioSource.PlayOneShot(_seClips[(int)seName]);
    }
}
