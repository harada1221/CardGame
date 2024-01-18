//-----------------------------------------------------------------------
/* キャラのステータス表示UIを管理するクラス
 * 
 * 制作者　原田　智大
 * 制作日　12月１日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatusUIScript : MonoBehaviour
{
    [SerializeField, Header("HPゲージImage")]
    private Image _hpGageImage = default;
    [SerializeField, Header("HP表示Text")]
    private Text _hpText = default;

    //敵キャラクター用パラメータ
    [Space(10)]
    [Header("敵キャラクター用")]
    [SerializeField, Header("CanvasGroup")]
    private CanvasGroup _enemyCanvasGroup = default;
    [SerializeField, Header("キャラクター名Text")]
    private Text _enemyCharaNameText = default;
    //フェード演出Tween
    private Tween _fadeTween = default;
    //フェード演出時間
    private const float _fadeTime = 0.8f;

    /// <summary>
    /// HPを表示する
    /// </summary>
    /// <param name="nowHP">現在HP</param>
    /// <param name="maxHP">最大HP</param>
    public void SetHPView(int nowHP, int maxHP)
    {
        //HP表示の最小値を設定
        if (nowHP < 0)
        {
            nowHP = 0;
        }
        if (maxHP < 0)
        {
            nowHP = 0;
        }

        //ゲージ表示
        //最大HPに対する現在HPの割合
        float ratio = 0.0f;
        // 0除算にならないように
        if (maxHP > 0)
        {
            ratio = (float)nowHP / maxHP;
            _hpGageImage.fillAmount = ratio;
        }
        // Text表示
        _hpText.text = nowHP + " / " + maxHP;
    }

    #region 敵ステータス表示専用処理

    /// <summary>
    /// キャラ名の表示をする
    /// </summary>
    /// <param name="charaName">表示する名前</param>
    public void SetCharacterName(string charaName)
    {
        //キャラ名表示
        if (_enemyCharaNameText != null)
        {
            _enemyCharaNameText.text = charaName;
        }
    }

    /// <summary>
    /// 全UIを表示
    /// </summary>
    public void ShowCanvasGroup()
    {
        //アニメーションを終わらせる
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        //全UI表示アニメーション
        _fadeTween = _enemyCanvasGroup.DOFade(1.0f, _fadeTime);
    }

    /// <summary>
    /// 全UIを非表示
    /// </summary>
    /// <param name="isAnimation">フェード演出実行フラグ</param>
    public void HideCanvasGroup(bool isAnimation)
    {
        if (_fadeTween != null)
        {
            _fadeTween.Kill();
        }
        //isAnimationがtrueの時のみ全UI非表示
        if (isAnimation == true)
        {
            _fadeTween = _enemyCanvasGroup.DOFade(0.0f, _fadeTime);
        }
        else
        {
            _enemyCanvasGroup.alpha = 0.0f;
        }
    }
    #endregion
}
