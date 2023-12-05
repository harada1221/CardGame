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
		//Debug.Log("カードがタップされました");
		//クリックした座標を保存
		_cursorPosition = gameObject.transform.position - GetMouseWorldPos();
	}
	/// <summary>
	/// クリックが終わった時に実行する
	/// </summary>
	/// <param name="eventData">クリック情報</param>
	public void OnPointerUp(PointerEventData eventData)
	{
		//Debug.Log("カードへのタップを終了しました");
	}
	/// <summary>
	/// ドラッグしている間呼び出す
	/// </summary>
    public void OnMouseDrag()
    {
		Debug.Log("ドラッグ");
		//オブジェクトを移動させる
		transform.position = GetMouseWorldPos() + _cursorPosition;
	}
	/// <summary>
	/// スクリーン座標からワールド座標に変化する
	/// </summary>
	/// <returns>ワールド座標</returns>
    private Vector3 GetMouseWorldPos()
    {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos);
	}
}
