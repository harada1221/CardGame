//-----------------------------------------------------------------------
/* プレイボードの処理を管理するクラス
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

public class PlayBoardManagerScript : MonoBehaviour
{
    [SerializeField, Header("プレイボードのカードゾーンエリア")]
    private CardZoneScript[] _boardCardZones = default;
    [SerializeField, Header("効果発動中カードのTransform")]
    private Transform _playingCardFrameTrs = default;
    //戦闘のマネージャースクリプト
    private BattleManagerScript _battleManager = default;
    //フィールドのマネージャースクリプト
    private FieldAreaManagerScript _fieldManager = default;
    //プレイヤーのマネージャースクリプト
    private CharacterManagerScript _characterManager = default;
    //カード効果実行Sequence
    private Sequence _playSequence = default;
    //カードゾーンのTransform
    private Transform[] _cardZonesTrs = default;

    //プレイボード内のカード枠数
    public const int PlayBoardCardNum = 5;
    //各カード実行の時間間隔
    private const float PlayIntervalTime = 0.2f;
    //フレームオブジェクトの初期位置
    private const float FrameObjPosition_FixX = 8.0f;
    //フレームオブジェクト移動アニメーション時間
    const float FrameAnimTime = 0.3f;
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="battleManager"></param>
    public void Init(BattleManagerScript battleManager)
    {
        // 参照取得
        _battleManager = battleManager;
        _fieldManager = _battleManager.GetFieldManager;
        _characterManager = _battleManager.GetCharacterManager;

        // 各カードゾーンのTransformを取得
        _cardZonesTrs = new Transform[PlayBoardCardNum];
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            _cardZonesTrs[i] = _boardCardZones[i].transform;
        }
        // フレームオブジェクトを開始位置に移動
        _playingCardFrameTrs.position = GetFrameObjectPos_Start();
    }
    /// <summary>
    /// プレイボード上のカードを左から順番に効果発動する
    /// </summary>
    /// <param name="boardCardList">発動対象となるカード配列</param>
    public void BoardCardsPlay(CardScript[] boardCards)
    {
        //フレームオブジェクトを開始位置に移動
        _playingCardFrameTrs.position = GetFrameObjectPos_Start();
        //順番にカードの効果を実行する(Sequence)
        _playSequence = DOTween.Sequence();
        //ボードの枚数分繰り返す
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            //現在実行中の配列内番号を記憶
            int index = i;
            // フレームオブジェクト移動
            _playSequence.Append(_playingCardFrameTrs
                .DOMove(GetPlayZonePos(i), FrameAnimTime) // 移動Tween
                .SetEase(Ease.OutQuart)); // 変化の仕方を指定

            //発動対象カードごとの処理(カードがないならスキップ)
            if (boardCards[index] != null)
            {
                //このカードの使用者のキャラクターIDを取得
                int useCharaID = boardCards[index].GetControllerCharaID;

                //カード効果発動
                _playSequence.AppendCallback(() =>
                {
                    //効果発動
                    PlayCard(boardCards[index], useCharaID, index);
                });
                // カードを半透明化Tween
                _playSequence.Append(boardCards[index].HideFadeTween());

                //戦闘終了条件を満たすか確認
                _playSequence.AppendCallback(() =>
                {
                    //プレイヤーの敗北か敵を撃破したか
                    if (_characterManager.IsPlayerDefeated() ||
                        _characterManager.IsEnemyDefeated())
                    {
                        //シーケンス強制終了
                        _playSequence.Complete();
                        _playSequence.Kill();
                        return;
                    }
                });
            }
            else
            {//カードが存在しない
            }

            //時間間隔を設定
            _playSequence.AppendInterval(PlayIntervalTime);
        }
        //フレームオブジェクトを終了位置に移動
        _playSequence.Append(_playingCardFrameTrs
            .DOMove(GetFrameObjectPos_End(), FrameAnimTime) // 移動Tween
            .SetEase(Ease.OutQuart)); // 変化の仕方を指定
                                      // Sequence終了時処理
        _playSequence.OnComplete(() =>
        {
            //ボード上のカードオブジェクトを全て削除
            foreach (CardScript card in boardCards)
            {
                if (card != null)
                {
                    _fieldManager.DestroyCardObject(card);
                }

            }
            //ターン終了処理呼出
            _battleManager.TurnEnd();
        });
    }
    private bool PlayCard(CardScript targetCard, int useCharaID, int boardIndex)
    {
        //相手キャラクターのID
        int targetCharaID = useCharaID ^ 1;

        int damagePoint = 0;//与ダメージ量
        int selfDamagePoint = 0;//自身への与ダメージ量
        int healPoint = 0;      //回復量
        int burnPoint = 0;      //最大HPへのダメージ量
        int selfBurnPoint = 0;  //自身の最大HPへのダメージ量
        int weakPoint = 0;      //ダメージ弱体化量
        int damageMulti = 1;    //ダメージ倍率
        bool isAbsorption = false;  //体力吸収フラグ
        //bool isBloodPact = false;   //回復⇔ダメージ変更フラグ
        //bool isReflection = false;  //反動ダメージフラグ

        //カード内のそれぞれの効果を実行
        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                #region 強度制限系
                case CardEffectDefineScript.CardEffect.ForceEqual: //強度n限定
                    if (effect.GetValue != targetCard.GetForcePoint)
                    {
                        //カード強度が指定の数値と異なるなら全ての効果を無効
                        return false;
                    }
                    break;

                case CardEffectDefineScript.CardEffect.ForceHigher: // 強度n以上
                    if (effect.GetValue > targetCard.GetForcePoint)
                    {
                        //カード強度が指定の範囲外なら全ての効果を無効
                        return false;
                    }
                    break;

                case CardEffectDefineScript.CardEffect.ForceLess: // 強度n以下
                    if (effect.GetValue < targetCard.GetForcePoint)
                    {
                        //カード強度が指定の範囲外なら全ての効果を無効
                        return false;
                    }
                    break;
                    #endregion
            }
        }
        //通常効果
        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                #region ダメージ系効果処理
                case CardEffectDefineScript.CardEffect.Damage: // ダメージ
                    damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.WeaponDmg: // 武器ダメージ
                    damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.Assault: // 突撃
                    if (boardIndex == 0)
                        damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.Pursuit: // 追撃
                    if (boardIndex == PlayBoardCardNum - 1)
                        damagePoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.CatchingUp: // 駆逐
                    if (_characterManager.GetNowHP[targetCharaID] * 2 <= _characterManager.GetMaxHP[targetCharaID])
                    {
                        damagePoint += effect.GetValue;
                    }
                    break;

                case CardEffectDefineScript.CardEffect.Burn: // 火傷
                    burnPoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.DoubleDamage: // ダメージ２倍
                    damageMulti *= 2;
                    break;

                case CardEffectDefineScript.CardEffect.TripleDamage: // ダメージ３倍
                    damageMulti *= 3;
                    break;

                case CardEffectDefineScript.CardEffect.Hypergravity: // 超重力
                    damagePoint += _characterManager.GetNowHP[targetCharaID] / 4;
                    break;

                case CardEffectDefineScript.CardEffect.SelfInjury: // 自傷
                    selfDamagePoint += effect.GetValue;
                    break;
                #endregion

                #region 回復系
                case CardEffectDefineScript.CardEffect.Heal: // 回復
                    healPoint += effect.GetValue;
                    break;

                case CardEffectDefineScript.CardEffect.SelfPredation: // 自己捕食
                                                                      // HP最大値半減
                    selfBurnPoint += _characterManager.GetMaxHP[useCharaID] / 2;
                    // 回復
                    healPoint += _characterManager.GetMaxHP[useCharaID] - _characterManager.GetNowHP[useCharaID];
                    break;

                case CardEffectDefineScript.CardEffect.Absorption: // 吸収
                    isAbsorption = true;
                    break;

                case CardEffectDefineScript.CardEffect.BloodPact: // 血の盟約
                    //isBloodPact = true;
                    break;
                #endregion

                #region 支援・妨害系
                case CardEffectDefineScript.CardEffect.Weakness: // 弱体化
                    weakPoint += effect.GetValue;
                    break;
                    #endregion
            }
        }
        //各種計算数値を対象ごとに適用
        //最大HPへのダメージ
        _characterManager.ChangeStatus_MaxHP(targetCharaID, -burnPoint);
        //ダメージ
        _characterManager.ChangeStatus_NowHP(targetCharaID, -damagePoint);
        //自分の最大HPへのダメージ
        _characterManager.ChangeStatus_MaxHP(useCharaID, -selfBurnPoint);
        //自分へのダメージ
        _characterManager.ChangeStatus_NowHP(useCharaID, -selfDamagePoint);
        //回復
        _characterManager.ChangeStatus_NowHP(useCharaID, healPoint);
        //体力吸収
        if (isAbsorption)
        {
            _characterManager.ChangeStatus_NowHP(useCharaID, damagePoint);
        }

        return true;
    }
    /// <summary>
	/// カードの効果実行中ならtrueを返す
	/// </summary>
	public bool IsPlayingCards()
    {
        if (_playSequence != null && _playSequence.IsActive() && _playSequence.IsPlaying())
        {
            return _playSequence.IsPlaying();
        }
        return false;
    }

    /// <summary>
	/// 対象プレイゾーンのVector2座標を取得
	/// </summary>
	/// <param name="areaID">ゾーンID</param>
	/// <returns>Position値</returns>
	public Vector2 GetPlayZonePos(int areaID)
    {
        return _cardZonesTrs[areaID].position;
    }

    /// <summary>
    /// 開始位置を取得
    /// </summary>
    /// <returns>Positionの開始位置</returns>
    private Vector2 GetFrameObjectPos_Start()
    {
        Vector2 res = _cardZonesTrs[0].position;
        res.x -= FrameObjPosition_FixX;
        return res;
    }
    /// <summary>
    /// 終了位置を取得
    /// </summary>
    /// <returns>Positionの終わる位置</returns>
    private Vector2 GetFrameObjectPos_End()
    {
        Vector2 res = _cardZonesTrs[PlayBoardCardNum - 1].position;
        res.x += FrameObjPosition_FixX;
        return res;
    }
}
