using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManagerScript : MonoBehaviour
{
    [SerializeField, Header("ステージセレクトウィンドウクラス")]
    private StageSelectWindowScript _stageSelectWindow = default;

    //アニメーション用UIオブジェクト
    [SerializeField, Header("タイトルロゴRectTransform")]
    private RectTransform _titleLogoRectTransform = default;
    [SerializeField, Header("ボタン背景RectTransformリスト")]
    private List<RectTransform> _titleButtonUIs = default;

    [SerializeField, Header("ウィンドウ背景パネルオブジェクト")]
    private GameObject _windowBackObject = default;

    //各種ボタン関連UIのX方向移動量
    private const float ButtonsUIMoveLengthX = 400.0f;
    //タイトルロゴ演出時間
    private const float TitleLogoTweenTime = 3.0f;
    //各種ボタン関連UI演出時間
    private const float ButtonsUITweenTime = 1.0f;
    //各種ボタン関連UIに演出を実行する時間間隔
    private const float ButtonsUITweenDistance = 0.15f;
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        //管理下コンポーネント初期化
        _stageSelectWindow.Init(this);

        //ゲーム起動時のアニメーションを再生
        InitAnimation();

        //ウィンドウ背景オブジェクトを無効化
        SetWindowBackPanelActive(false);
    }

    /// <summary>
    /// ゲーム起動時アニメーション
    /// </summary>
    private void InitAnimation()
    {
        //タイトルロゴテキスト
        Text titleLogoText = _titleLogoRectTransform.GetComponent<Text>();
        //起動アニメーションSequence
        Sequence initSequence = DOTween.Sequence();

        //タイトルロゴを大きく・透明にする
        _titleLogoRectTransform.transform.localScale = new Vector2(1.8f, 1.8f);
        titleLogoText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        //各種ボタン関連UIを画面下側に退避
        int buttonUIsLength = _titleButtonUIs.Count;
        for (int i = 0; i < buttonUIsLength; i++)
        {
            _titleButtonUIs[i].anchoredPosition += new Vector2(0.0f, -ButtonsUIMoveLengthX);
        }

        //アニメーション設定
        //タイトルロゴの大きさ変更
        initSequence.Append(
            _titleLogoRectTransform.DOScale(1.0f, TitleLogoTweenTime));
        //タイトルロゴの非透明化
        initSequence.Join(
            titleLogoText.DOFade(1.0f, TitleLogoTweenTime));
        //各種ボタン関連UIを画面下側から移動
        for (int i = 0; i < buttonUIsLength; i++)
        {
            initSequence.Join(
                _titleButtonUIs[i].DOAnchorPosY(ButtonsUIMoveLengthX, ButtonsUITweenTime)
                .SetRelative() // 相対座標指定にする
                .SetDelay(ButtonsUITweenDistance * i)); // 少しずつ遅延していく
        }
    }

    /// <summary>
    /// ウィンドウ背景パネルオブジェクトを有効化・無効化する
    /// </summary>
    public void SetWindowBackPanelActive(bool mode)
    {
        _windowBackObject.SetActive(mode);
    }
}
