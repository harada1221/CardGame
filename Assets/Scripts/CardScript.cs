//-----------------------------------------------------------------------
/* カードの処理を行うクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	//カーソルの位置
	private Vector3 _cursorPosition = default;


	/// <summary>
	/// クリックしたときに実行する
	/// </summary>
	/// <param name="eventData">クリック情報</param>
	public void OnPointerDown(PointerEventData eventData)
	{
		Debug.Log("カードがタップされました");
	}
	/// <summary>
	/// クリックが終わった時に実行する
	/// </summary>
	/// <param name="eventData">クリック情報</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		Debug.Log("カードへのタップを終了しました");
	}
}
