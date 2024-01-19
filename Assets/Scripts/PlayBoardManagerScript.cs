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
    //戦闘のマネージャースクリプト
    private BattleManagerScript _battleManager = default;
    //フィールドのマネージャースクリプト
    private FieldAreaManagerScript _fieldManager = default;
    //プレイヤーのマネージャースクリプト
    private CharacterManagerScript _characterManager = default;
    //カード効果実行Sequence
    private Sequence _playSequence = default;

    //プレイボード内のカード枠数
    public const int PlayBoardCardNum = 5;
    //各カード実行の時間間隔
    private const float PlayIntervalTime = 0.2f;

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
    }
    /// <summary>
    /// プレイボード上のカードを左から順番に効果発動する
    /// </summary>
    /// <param name="boardCardList">発動対象となるカード配列</param>
    public void BoardCardsPlay(CardScript[] boardCards)
    {
        //順番にカードの効果を実行する(Sequence)
        _playSequence = DOTween.Sequence();
        //ボードの枚数分繰り返す
        for (int i = 0; i < PlayBoardCardNum; i++)
        {
            //現在実行中の配列内番号を記憶
            int index = i;

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
    }
    private bool PlayCard(CardScript targetCard, int useCharaID, int boardIndex)
    {
        //相手キャラクターのID
        int targetCharaID = useCharaID ^ 1;

        //カードの各効果量
        int damagePoint = 0;

        //カード内のそれぞれの効果を実行
        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                //強度n限定
                case CardEffectDefineScript.CardEffect.ForceEqual:
                    //カード強度が指定の数値と異なるなら全ての効果を無効
                    if (effect.GetValue != targetCard.GetForcePoint)
                    {
                        return false;
                    }

                    break;
            }
        }

        foreach (CardEffectDefineScript effect in targetCard.GetCardEffects)
        {
            switch (effect.GetEffect)
            {
                case CardEffectDefineScript.CardEffect.Damage:
                    //ダメージ量決定
                    damagePoint += effect.GetValue;
                    break;
            }
        }
        //ダメージを与える
        _characterManager.ChangeStatus_NowHP(targetCharaID, -damagePoint);

        return true;
    }
    /// <summary>
	/// カードの効果実行中ならtrueを返す
	/// </summary>
	public bool IsPlayingCards()
    {
        if (_playSequence != null)
        {
            return _playSequence.IsPlaying();
        }
        return false;
    }
}
