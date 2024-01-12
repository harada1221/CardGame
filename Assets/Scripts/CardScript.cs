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

public class CardScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // フィールド管理クラス
    private FieldAreaManagerScript _fieldAreaScript = default;

    [SerializeField, Header("オブジェクトのRectTransform")]
    public RectTransform _rectTransform = default;

    // 各種変数
    // ドラッグ終了後に戻ってくる座標
    private Vector2 _basePos = default;
    // 移動Tween
    private Tween _moveTween = default;
    private CardZoneScript.ZoneType _nowZone = default;

    // カード移動アニメーション時間
    const float MoveTime = 0.4f;

    public CardZoneScript.ZoneType GetNowZone { get => _nowZone; }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="_fieldManager">フィールドマネージャー</param>
    /// <param name="initPos">初期座標</param>
    public void Init(FieldAreaManagerScript _fieldManager, Vector2 initPos)
    {
        //参照取得
        _fieldAreaScript = _fieldManager;
        //変数初期化
        //初期位置に移動
        _rectTransform.position = initPos;
        //初期位置固定
        _basePos = initPos;
        //初期の場所
        _nowZone = CardZoneScript.ZoneType.Hand;
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
        _moveTween = _rectTransform
            .DOMove(_basePos, MoveTime) // 移動Tween
            .SetEase(Ease.OutQuart);    // 変化の仕方を指定
    }
    /// <summary>
	/// カードを指定のゾーンに設置する
	/// </summary>
	/// <param name="zoneType">対象カードゾーン</param>
	/// <param name="targetPos">対象座標</param>
	public void PutToZone(CardZoneScript.ZoneType zoneType, Vector2 targetPos)
    {
        //座標を取得
        _basePos = targetPos;
        //初期位置に戻す
        BackToBasePos();
        //カードゾーンの種類を保存
        _nowZone = zoneType;
    }
    /// <summary>
    /// クリックしたときに実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerDown(PointerEventData eventData)
    {
        // クリック開始処理
        _fieldAreaScript.StartDragging(this);
    }
    /// <summary>
    /// クリックが終わった時に実行する
    /// </summary>
    /// <param name="eventData">クリック情報</param>
    public void OnPointerUp(PointerEventData eventData)
    {
        //クリック終了処理
        _fieldAreaScript.EndDragging();
    }
}
