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
using DG.Tweening;

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform _rectTransform;
    // フィールド管理クラス
    [SerializeField,Header("フィールドを管理するスクリプト")]
    private FieldAreaManagerScript _fieldAreaScript = default;

    // 基本座標(ドラッグ終了後に戻ってくる座標)
    private Vector2 _basePos = default;
    // 移動用のTween
    private Tween _moveTween = default;

    // カード移動アニメーション時間
    private const float MoveTime = 0.4f;
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="_fieldManager">フィールドマネージャー</param>
    public void Init(FieldAreaManagerScript _fieldManager)
    {
        // 参照取得
        _fieldAreaScript = _fieldManager;
        // 変数初期化
        _basePos = this.gameObject.transform.position;
    }
    /// <summary>
	/// 基本座標までカードを移動させる
	/// </summary>
	public void BackToBasePos()
    {
        // 既に実行中の移動アニメーションがあれば停止
        if (_moveTween != null)
        {
            _moveTween.Kill();
        }

        // 指定地点まで移動するアニメーション(DOTween)
        this.transform.DOMove(_basePos, MoveTime) // 移動Tween
            .SetEase(Ease.OutQuart);   // 変化の仕方を指定
    }
    /// <summary>
    /// クリックしたときに実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // クリック下カードを代入する
        _fieldAreaScript.StartDragging(this);
        Debug.Log("クリック");
    }
    /// <summary>
    /// クリックが終わった時に実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //クリック終了処理
        BackToBasePos();
        Debug.Log("話した");
    }

    /// <summary>
    /// ドラック中の処理
    /// </summary>
    /// <param name="eventData">ドラック処理</param>
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("ドラッグ");
        Vector3 TargetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        TargetPos.z = 0;
        transform.position = TargetPos;
    }
}
