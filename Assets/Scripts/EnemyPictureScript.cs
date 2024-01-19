//-----------------------------------------------------------------------
/* 敵キャラクターの画像オブジェクトを管理クラス
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

public class EnemyPictureScript : MonoBehaviour
{
    //キャラクターマネージャー
    private CharacterManagerScript _characterManager = default;
    [SerializeField, Header("出現位置")]
    private RectTransform _rectTransform = default;
    [SerializeField, Header("出現する敵のイメージ")]
    private Image _enemyImage = default;
    //出現位置の初期座標
    private Vector2 _basePosition = default;
    //被ダメージ時のランダム移動
    private Sequence _randomMoveSequence = default;

    //移動先Y相対座標
    private const float TargetPositionYRelative = 200.0f;
    //演出時間
    private const float AnimTime = 1.0f;
    //敵ランダム移動アニメーション
    private const float JumpPosX_Width = 100.0f;    //移動先のX方向範囲
    private const float JumpPosY_Height = 100.0f;   //移動先のY方向範囲
    private const float AnimTime_Move = 0.1f;       //移動時間
    private const float AnimTime_Back = 1.2f;   //戻りの移動時間
                                               
    private const float JumpPosX_Relative = 100.0f;    //ジャンプ移動先のX相対座標
    private const float JumpPosY_Relative = 50.0f;  //ジャンプ移動先のY相対座標
    private const float JumpPower = 30.0f;          //ジャンプの強度
    private const float EnemyAnimTime = 0.7f;    //演出時間
    /// <summary>
    /// 初期設定
    /// </summary>
    /// <param name="characterManager">CharacterManagerScript</param>
    /// <param name="enemySprite">最初に出現する敵</param>
    public void Init(CharacterManagerScript characterManager, Sprite enemySprite)
    {
        //参照取得
        _characterManager = characterManager;

        //初期座標を保存
        _basePosition = _rectTransform.anchoredPosition;

        //敵画像表示
        _enemyImage.sprite = enemySprite;
        _enemyImage.SetNativeSize(); // オブジェクトの大きさを画面の大きさに合わせる

        //敵が画面上部から降りてくるアニメーション
        //初期位置を設定
        Vector2 pos = _basePosition;
        pos.y += TargetPositionYRelative;
        _rectTransform.anchoredPosition = pos;
        //Y方向移動アニメーション
        _rectTransform.DOAnchorPosY(-TargetPositionYRelative, AnimTime)
            .SetRelative();
    }
    /// <summary>
	/// 被ダメージアニメーションを再生する
	/// </summary>
	public void DamageAnimation()
    {
        //Sequence初期化
        if (_randomMoveSequence != null)
        {
            _randomMoveSequence.Kill();
            _randomMoveSequence = DOTween.Sequence();
        }
        //ランダム移動先を設定
        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x += Random.Range(-JumpPosX_Width / 2.0f, JumpPosX_Width / 2.0f);
        pos.y += Random.Range(-JumpPosY_Height / 2.0f, JumpPosY_Height / 2.0f);
        //ジャンプ移動アニメーション(Tween)
        _randomMoveSequence.Append(_rectTransform.DOAnchorPos(pos, AnimTime_Move));

        //元の位置に戻るアニメーション
        _randomMoveSequence.Append(_rectTransform.DOAnchorPos(_basePosition, AnimTime_Back));
    }
    /// <summary>
	/// 撃破時の非表示化アニメーションを再生する
	/// </summary>
	public void DefeatAnimation()
    {
        //再生中のSequenceを停止
        if (_randomMoveSequence != null)
        {
            _randomMoveSequence.Kill();
        }

        //撃破時演出シーケンス初期化
        Sequence defeatSequence = DOTween.Sequence();
        //ジャンプ移動先を設定
        Vector2 pos = _rectTransform.anchoredPosition;
        pos.x += JumpPosX_Relative;
        pos.y += JumpPosY_Relative;
        //ジャンプ移動アニメーション
        defeatSequence.Append(_rectTransform.DOJumpAnchorPos(pos, JumpPower, 1, EnemyAnimTime)
            .SetEase(Ease.Linear)); // 変化の仕方を指定

        //Scale縮小(Tween)
        defeatSequence.Join(_rectTransform.DOScale(0.0f, EnemyAnimTime)
            .SetEase(Ease.Linear)); // 変化の仕方を指定

        //アニメーション完了時にオブジェクトを削除
        defeatSequence.OnComplete(() =>
        {
            _characterManager.DeleteEnemy();
        });
    }
}
