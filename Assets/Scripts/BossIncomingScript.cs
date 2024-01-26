using UnityEngine;
using DG.Tweening;

public class BossIncomingScript : MonoBehaviour
{
    [SerializeField, Header("UIオブジェクト    ")]
    private CanvasGroup _canvasGroup = default;
    [SerializeField, Header("テキストのRectTransform")]
    private RectTransform _textRectTransform = default;

    [SerializeField,Header("テキストの初期位置")]
    private Vector2 _textBasePosition = default;

    private const float LogoStartScale = 1.0f;
    private const float LogoEndScale = 3.0f;
    
    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        //UI非表示
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }
    public void StartAnimation()
    {
        //UI表示
        gameObject.SetActive(true);
        //CanvasGroup設定
        _canvasGroup.alpha = 0.0f;
        //テキスト表示
        _textRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        //テキストの初期座標
        _textRectTransform.anchoredPosition = _textBasePosition;

        //UI表示アニメーション
        Sequence sequence = DOTween.Sequence();
        //パネルを表示
        sequence.Append(_canvasGroup.DOFade(1.0f, 0.5f));
        //ロゴを移動
        sequence.Append(_textRectTransform.DOAnchorPos(Vector2.zero, 1.0f)
            .SetEase(Ease.OutQuint));
        //待ち時間
        sequence.AppendInterval(0.6f);
        //パネルを非表示
        sequence.Join(_canvasGroup.DOFade(0.0f, 1.0f));

    }
}
