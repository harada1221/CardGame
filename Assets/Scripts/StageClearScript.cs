using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StageClearScript : MonoBehaviour
{
    //UIオブジェクト
    [SerializeField, Header("オブジェクトのグループ")]
    private CanvasGroup _canvasGroup = default;
    [SerializeField, Header("テキストのRectTransform")]
    private RectTransform _logoRectTransform = default;
    [SerializeField, Header("表示するテキスト")]
    private Text _clearText = default;
    [SerializeField, Header("タイトルへ戻るボタンのRectTransform")]
    private RectTransform _titleButtonRectTransform = default;


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
    /// ステージクリア時に呼び出される処理
    /// </summary>
    public void StartAnimation()
    {
        //初期UI設定
        gameObject.SetActive(true);
        //CanvasGroup設定
        _canvasGroup.alpha = 0.0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _logoRectTransform.localScale = new Vector3(LogoStartScale, LogoStartScale, 1.0f);
        _clearText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //ボタン設定
        Vector2 buttonPos = _titleButtonRectTransform.anchoredPosition;
        buttonPos.y -= ButtonMovePosY;
        _titleButtonRectTransform.anchoredPosition = buttonPos;

        //UI表示アニメーション
        Sequence sequence = DOTween.Sequence();
        //パネルを表示
        sequence.Append(_canvasGroup.DOFade(1.0f, 1.0f));
        //ロゴを表示
        sequence.Append(_logoRectTransform.DOScale(LogoEndScale, 0.6f)
            .SetEase(Ease.OutCubic));
        sequence.Join(_clearText.DOFade(1.0f, 0.6f));
        //ロゴを揺らす
        sequence.Append(_logoRectTransform.DOShakeAnchorPos(0.5f, 60, 30));
        //待ち時間
        sequence.AppendInterval(1.0f);
        //ボタンを移動
        sequence.Join(_titleButtonRectTransform.DOAnchorPosY(ButtonMovePosY, 0.5f)
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
