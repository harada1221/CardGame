using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    //UIオブジェクト
    [SerializeField, Header("対応するCanvasGroup")]
    private CanvasGroup _canvasGroup = null;
    [SerializeField, Header("テキストのRectTransform ")]
    private RectTransform _logoRectTransform = null;
    [SerializeField, Header("表示するテキスト")]
    private Text _logoImage = null;
    [SerializeField, Header("ボタンのRectTransform")]
    private RectTransform _titleButtonRectTransform = null;

    //ロゴ画像初期Scale値
    private const float LogoStartScale = 3.0f;
    //ロゴ画像終了Scale値
    private const float LogoEndScale = 1.0f;
    //ボタン移動量(Y方向)
    private const float ButtonMovePosY = 200.0f;

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Init()
    {
        // UI非表示
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 演出開始処理
    /// </summary>
    public void StartAnimation()
    {
        //初期UI設定
        gameObject.SetActive(true);
        //CanvasGroup設定
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        //ロゴ設定
        _logoRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        _logoImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //ボタン設定
        Vector2 buttonPos = _titleButtonRectTransform.anchoredPosition;
        buttonPos.y -= ButtonMovePosY;
        _titleButtonRectTransform.anchoredPosition = buttonPos;

        //UI表示アニメーション
        Sequence sequence = DOTween.Sequence();
        //パネルを表示
        sequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        //ロゴを表示
        sequence.Append(_logoRectTransform.DOScale(LogoEndScale, 1.0f));
        sequence.Join(_logoImage.DOFade(1.0f, 1.0f));
        //待ち時間
        sequence.AppendInterval(1.0f);
        //ボタンを移動
        sequence.Append(_titleButtonRectTransform.DOAnchorPosY(ButtonMovePosY, 0.5f)
            .SetRelative());
    }

    /// <summary>
    /// タイトル画面へ移動する
    /// </summary>
    public void GoTitleScene()
    {
        //タイトルシーンに切り替える
        SceneManager.LoadScene("TitleScene");
    }
}
