using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageSelectWindowScript : MonoBehaviour
{
    //タイトル管理クラス
    private TitleManagerScript _titleManager = default;
    //ウィンドウのRectTransform
    private RectTransform _windowRectTransform = default;
    //ウィンドウ用Tween;
    private Tween _windowTween = default;
    //ウィンドウ表示アニメーション時間
    private const float _WindowAnimTime = 0.3f;

    //初期化関数(TitleManager.csから呼出)
    public void Init(TitleManagerScript _titleManager)
    {
        //参照取得
        this._titleManager = _titleManager;
        _windowRectTransform = GetComponent<RectTransform>();

        //ウィンドウ非表示
        _windowRectTransform.transform.localScale = Vector2.zero;
        _windowRectTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// ウィンドウを表示する
    /// </summary>
    public void OpenWindow()
    {
        //アニメーション終了
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }

        //ウィンドウ表示Tween
        _windowTween =
            _windowRectTransform.DOScale(1.0f, _WindowAnimTime)
            .SetEase(Ease.OutBack);
        //ウィンドウ背景パネルを表示
        _titleManager.SetWindowBackPanelActive(true);
    }
    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        //アニメーション終了
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }

        //ウィンドウ非表示Tween
        _windowTween =
            _windowRectTransform.DOScale(0.0f, _WindowAnimTime)
            .SetEase(Ease.InBack);
        //ウィンドウ背景パネルを非表示
        _titleManager.SetWindowBackPanelActive(false);
    }
}
