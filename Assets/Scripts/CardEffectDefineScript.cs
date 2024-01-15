//-----------------------------------------------------------------------
/* カードの効果の種類などを定義するクラス
 * 
 * 制作者　原田　智大
 * 制作日　1月１5日
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffectDefineScript
{
    #region パラメータ
    [SerializeField, Header("効果の種類")]
    private CardEffect _cardEffect = default;
    [SerializeField, Header("効果値")]
    private int _value = default;

    public CardEffect GetEffect 
    {
        get { return _cardEffect; }
        set { _cardEffect = value; }
    }
    public int GetValue
    { 
        get { return _value; }
        set { _value = value; }
    }
    #endregion

    #region 効果の種類定義部
    // カード効果定義
    public enum CardEffect
    {
        Damage,     // ダメージ
        WeaponDmg,  // 武器ダメージ
        Assault,    // 突撃
        Pursuit,    // 追撃
        CatchingUp, // 追い討ち
        Burn,       // 火傷
        DoubleDamage,// ダメージ２倍
        TripleDamage,// ダメージ３倍
        Hypergravity,// 超重力
        SelfInjury, // 自傷
        Rush,       // ラッシュ

        Heal,       // 回復
        SelfPredation,// 自己捕食
        Absorption, // 吸収
        BloodPact,  // 血の盟約

        HealRush,   // ヒールラッシュ
        Weakness,   // 弱体化
        Electrocution,// 感電
        Leakage,    // 漏電
        Defense,    // 防衛
        Augment,    // 増強
        Kiai,       // 気合溜め
        NoHeal,     // 回復停止
        Seal,       // 封印
        Stun,       // スタン
        DeckRegen,  // 山札再生
        Reverse,    // 反転

        Poison,     // 毒
        Detox,      // 解毒
        Flame,      // 炎上

        ForceEqual, // 強度n限定
        ForceHigher,// 強度n以上
        ForceLess,  // 強度n以下
        BaseOnly,   // 本体限定
        PartsOnly,  // 素材限定
        NoCompo,    // 合成不可

        Reaction,   // 反動
        NoWeapon,   // 武器無効

        Bonus_Gold, // (ボーナス用)お金
        Bonus_EXP,  // (ボーナス用)経験値
        Bonus_Heal, // (ボーナス用)体力回復

        _MAX,
    }

    //効果名
    readonly public static Dictionary<CardEffect, string> Dic_EffectName = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "ダメージ {0}" },
        {CardEffect.Heal,
            "回復 {0}" },
        {CardEffect.Weakness,
            "弱体化 {0}" },
        {CardEffect.DoubleDamage,
            "ダメージ二倍" },
        {CardEffect.Absorption,
            "体力吸収" },
        {CardEffect.ForceEqual,
            "強度 {0} 限定" },
        {CardEffect.Assault,
            "突撃 {0}" },
        {CardEffect.Pursuit,
            "追撃 {0}" },
        {CardEffect.WeaponDmg,
            "武器ダメージ {0}" },
        {CardEffect.Kiai,
            "気合溜め" },
        {CardEffect.Rush,
            "ラッシュ" },
        {CardEffect.HealRush,
            "ヒールラッシュ" },
        {CardEffect.SelfPredation,
            "自己捕食" },
        {CardEffect.BloodPact,
            "血の盟約" },
        {CardEffect.Poison,
            "毒 {0}" },
        {CardEffect.Detox,
            "解毒" },
        {CardEffect.Defense,
            "防衛 {0}" },
        {CardEffect.Seal,
            "封印" },
        {CardEffect.Burn,
            "火傷 {0}" },
        {CardEffect.Flame,
            "炎上 {0}" },
        {CardEffect.CatchingUp,
            "駆逐 {0}" },
        {CardEffect.Electrocution,
            "感電 {0}" },
        {CardEffect.Leakage,
            "漏電 {0}" },
        {CardEffect.Augment,
            "増強 {0}" },
        {CardEffect.TripleDamage,
            "ダメージ三倍" },
        {CardEffect.Reverse,
            "反転" },
        {CardEffect.Hypergravity,
            "超重力" },
        {CardEffect.ForceHigher,
            "強度 {0} 以上" },
        {CardEffect.ForceLess,
            "強度 {0} 以下" },
        {CardEffect.BaseOnly,
            "本体限定" },
        {CardEffect.PartsOnly,
            "素材限定" },
        {CardEffect.NoCompo,
            "合成無効" },
        {CardEffect.SelfInjury,
            "自傷 {0}" },
        {CardEffect.DeckRegen,
            "山札再生 {0}" },
        {CardEffect.NoHeal,
            "回復停止" },
        {CardEffect.Stun,
            "スタン" },
        {CardEffect.Reaction,
            "反動" },
        {CardEffect.NoWeapon,
            "武器無効" },
        {CardEffect.Bonus_Gold,
            "金貨 {0}" },
        {CardEffect.Bonus_EXP,
            "経験値 {0}" },
        {CardEffect.Bonus_Heal,
            "回復 {0}" },
    };


    //効果説明
    readonly public static Dictionary<CardEffect, string> Dic_EffectExplain = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "相手に {0} のダメージを与える" },
        {CardEffect.Heal,
            "自分の体力を {0} 回復する" },
        {CardEffect.Weakness,
            "ダメージの効果を {0} 減少させる\n(マイナスにはならない)" },
        {CardEffect.DoubleDamage,
            "相手に与えるダメージを２倍にする\n(状態異常には無効)(同名効果と重複なし)" },
        {CardEffect.Absorption,
            "相手に与えたダメージ分自身を回復" },
        {CardEffect.ForceEqual,
            "カードの強度が {0} の場合のみ発動" },
        {CardEffect.Assault,
            "左端でこのカードが発動された時\n{0} のダメージを与える" },
        {CardEffect.Pursuit,
            "右端でこのカードが発動された時\n{0} のダメージを与える" },
        {CardEffect.WeaponDmg,
            "相手に {0} のダメージを与える" },
        {CardEffect.Kiai,
            "右隣のカードで自分が与える武器ダメージを2倍にする" },
        {CardEffect.Rush,
            "武器ダメージ効果を持つカードの使用回数×3ポイントのダメージ\n (現在 {0} 回・戦闘後にリセット)" },
        {CardEffect.HealRush,
            "回復効果を持つカードの使用回数×3ポイント体力を回復\n (現在 {0} 回・戦闘後にリセット)" },
        {CardEffect.SelfPredation,
            "自分の体力を全回復するが、最大体力を半分にする" },
        {CardEffect.BloodPact,
            "このカードの体力回復量を\n相手に与えるダメージ量に変更する" },
        {CardEffect.Poison,
            "相手に毒を {0} 与える\n(毒：ターン終了時にダメージ)" },
        {CardEffect.Detox,
            "自分の毒を取り除く" },
        {CardEffect.Defense,
            "ターン終了まで相手が与えるダメージを {0} 減らす" },
        {CardEffect.Seal,
            "右隣のカードの発動を封じる" },
        {CardEffect.Burn,
            "相手の最大体力を {0} 減らす" },
        {CardEffect.Flame,
            "相手に炎上を {0} 与える\n(炎上：最大体力減少時に追加ダメージ)" },
        {CardEffect.CatchingUp,
            "発動時に相手の体力が半分以下の時\n{0} の追加ダメージを与える" },
        {CardEffect.Electrocution,
            "右隣のカードが与えるダメージを {0} 減らす" },
        {CardEffect.Leakage,
            "次以降の全てのカードが与えるダメージを {0} 減らす\n(ターン終了まで)" },
        {CardEffect.Augment,
            "次以降の全てのカードが与えるダメージを {0} 増やす\n(ターン終了まで)(ダメージ効果が無くても対象)" },
        {CardEffect.TripleDamage,
            "相手に与えるダメージを３倍にする\n(状態異常には無効)(同名効果と重複なし)" },
        {CardEffect.Reverse,
            "このカードが相手に与える効果を自分に、\n自分が受ける効果を相手に与える" },
        {CardEffect.Hypergravity,
            "相手の残り体力の4分の1のダメージを与える" },
        {CardEffect.ForceHigher,
            "カードの強度が {0} 以上の場合のみ発動" },
        {CardEffect.ForceLess,
            "カードの強度が {0} 以下の場合のみ発動" },
        {CardEffect.BaseOnly,
            "このカードの合成は本体としてのみ可能" },
        {CardEffect.PartsOnly,
            "このカードの合成は素材としてのみ可能" },
        {CardEffect.NoCompo,
            "このカードは合成できない" },
        {CardEffect.SelfInjury,
            "自分は {0} の体力を失う" },
        {CardEffect.DeckRegen,
            "デッキの残りカード枚数を {0} 枚追加する" },
        {CardEffect.NoHeal,
            "ターン終了まで相手の体力回復を封じる" },
        {CardEffect.Stun,
            "右隣の自分のカードは発動できない" },
        {CardEffect.Reaction,
            "相手に与えたダメージの半分だけ自分の残り体力を減らす\n（端数切り捨て）" },
        {CardEffect.NoWeapon,
            "ターン終了まで相手から武器ダメージを受けない" },
        {CardEffect.Bonus_Gold,
            "(ボーナス専用)\nお金を{0}得る" },
        {CardEffect.Bonus_EXP,
            "(ボーナス専用)\n経験値を{0}得る" },
        {CardEffect.Bonus_Heal,
            "(ボーナス専用)\n体力を{0}回復する" },
    };

    //合成モード
    readonly public static Dictionary<CardEffect, EffectCompoMode> Dic_EffectCompoMode = new Dictionary<CardEffect, EffectCompoMode>()
    {
        {CardEffect.Damage,
            EffectCompoMode.Possible },
        {CardEffect.Heal,
            EffectCompoMode.Possible },
        {CardEffect.Weakness,
            EffectCompoMode.Possible },
        {CardEffect.DoubleDamage,
            EffectCompoMode.Possible },
        {CardEffect.Absorption,
            EffectCompoMode.Possible },
        {CardEffect.ForceEqual,
            EffectCompoMode.OnlyOwn_New },
        {CardEffect.Assault,
            EffectCompoMode.Possible },
        {CardEffect.Pursuit,
            EffectCompoMode.Possible },
        {CardEffect.WeaponDmg,
            EffectCompoMode.Possible },
        {CardEffect.Kiai,
            EffectCompoMode.Possible },
        {CardEffect.Rush,
            EffectCompoMode.Possible },
        {CardEffect.HealRush,
            EffectCompoMode.Possible },
        {CardEffect.SelfPredation,
            EffectCompoMode.Possible },
        {CardEffect.BloodPact,
            EffectCompoMode.Possible },
        {CardEffect.Poison,
            EffectCompoMode.Possible },
        {CardEffect.Detox,
            EffectCompoMode.Possible },
        {CardEffect.Defense,
            EffectCompoMode.Possible },
        {CardEffect.Seal,
            EffectCompoMode.Possible },
        {CardEffect.Burn,
            EffectCompoMode.Possible },
        {CardEffect.Flame,
            EffectCompoMode.Possible },
        {CardEffect.CatchingUp,
            EffectCompoMode.Possible },
        {CardEffect.Electrocution,
            EffectCompoMode.Possible },
        {CardEffect.Leakage,
            EffectCompoMode.Possible },
        {CardEffect.Augment,
            EffectCompoMode.Possible },
        {CardEffect.TripleDamage,
            EffectCompoMode.Possible },
        {CardEffect.Reverse,
            EffectCompoMode.Possible },
        {CardEffect.Hypergravity,
            EffectCompoMode.Possible },
        {CardEffect.ForceHigher,
            EffectCompoMode.OnlyOwn_New },
        {CardEffect.ForceLess,
            EffectCompoMode.OnlyOwn_New },
        {CardEffect.BaseOnly,
            EffectCompoMode.Impossible },
        {CardEffect.PartsOnly,
            EffectCompoMode.Possible },
        {CardEffect.NoCompo,
            EffectCompoMode.Impossible },
        {CardEffect.SelfInjury,
            EffectCompoMode.Possible },
        {CardEffect.DeckRegen,
            EffectCompoMode.Possible },
        {CardEffect.NoHeal,
            EffectCompoMode.Possible },
        {CardEffect.Stun,
            EffectCompoMode.Possible },
        {CardEffect.Reaction,
            EffectCompoMode.Possible },
        {CardEffect.NoWeapon,
            EffectCompoMode.Impossible },
        {CardEffect.Bonus_Gold,
            EffectCompoMode.Impossible },
        {CardEffect.Bonus_EXP,
            EffectCompoMode.Impossible },
        {CardEffect.Bonus_Heal,
            EffectCompoMode.Impossible },
    };
    #endregion

    #region 合成モード定義
    //合成モード定義
    public enum EffectCompoMode
    {
        Possible,   // 合成可能
        Impossible, // 合成不可能
        OnlyNew,    // 新規追加のみ
        OnlyOwn,    // 自分とカードとのみ合成可能
        OnlyOwn_New,// 自分とカードとのみ合成可能(新規追加のみ)
    }

    //効果名(JP)
    readonly public static Dictionary<EffectCompoMode, string> Dic_CompoModeName = new Dictionary<EffectCompoMode, string>()
    {
        {EffectCompoMode.Possible,
            "合成可能" },
        {EffectCompoMode.Impossible,
            "合成不可能" },
        {EffectCompoMode.OnlyNew,
            "追加限定" },
        {EffectCompoMode.OnlyOwn,
            "自カード限定" },
        {EffectCompoMode.OnlyOwn_New,
            "追加・自カード限定" },
    };
    #endregion
}
