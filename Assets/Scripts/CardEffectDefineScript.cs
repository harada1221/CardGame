//-----------------------------------------------------------------------
/* �J�[�h�̌��ʂ̎�ނȂǂ��`����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@1���P5��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardEffectDefineScript
{
    #region �p�����[�^
    [SerializeField, Header("���ʂ̎��")]
    private CardEffect _cardEffect = default;
    [SerializeField, Header("���ʒl")]
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

    #region ���ʂ̎�ޒ�`��
    // �J�[�h���ʒ�`
    public enum CardEffect
    {
        Damage,     // �_���[�W
        WeaponDmg,  // ����_���[�W
        Assault,    // �ˌ�
        Pursuit,    // �ǌ�
        CatchingUp, // �ǂ�����
        Burn,       // �Ώ�
        DoubleDamage,// �_���[�W�Q�{
        TripleDamage,// �_���[�W�R�{
        Hypergravity,// ���d��
        SelfInjury, // ����
        Rush,       // ���b�V��

        Heal,       // ��
        SelfPredation,// ���ȕߐH
        Absorption, // �z��
        BloodPact,  // ���̖���

        HealRush,   // �q�[�����b�V��
        Weakness,   // ��̉�
        Electrocution,// ���d
        Leakage,    // �R�d
        Defense,    // �h�q
        Augment,    // ����
        Kiai,       // �C������
        NoHeal,     // �񕜒�~
        Seal,       // ����
        Stun,       // �X�^��
        DeckRegen,  // �R�D�Đ�
        Reverse,    // ���]

        Poison,     // ��
        Detox,      // ���
        Flame,      // ����

        ForceEqual, // ���xn����
        ForceHigher,// ���xn�ȏ�
        ForceLess,  // ���xn�ȉ�
        BaseOnly,   // �{�̌���
        PartsOnly,  // �f�ތ���
        NoCompo,    // �����s��

        Reaction,   // ����
        NoWeapon,   // ���햳��

        Bonus_Gold, // (�{�[�i�X�p)����
        Bonus_EXP,  // (�{�[�i�X�p)�o���l
        Bonus_Heal, // (�{�[�i�X�p)�̗͉�

        _MAX,
    }

    //���ʖ�
    readonly public static Dictionary<CardEffect, string> Dic_EffectName = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "�_���[�W {0}" },
        {CardEffect.Heal,
            "�� {0}" },
        {CardEffect.Weakness,
            "��̉� {0}" },
        {CardEffect.DoubleDamage,
            "�_���[�W��{" },
        {CardEffect.Absorption,
            "�̗͋z��" },
        {CardEffect.ForceEqual,
            "���x {0} ����" },
        {CardEffect.Assault,
            "�ˌ� {0}" },
        {CardEffect.Pursuit,
            "�ǌ� {0}" },
        {CardEffect.WeaponDmg,
            "����_���[�W {0}" },
        {CardEffect.Kiai,
            "�C������" },
        {CardEffect.Rush,
            "���b�V��" },
        {CardEffect.HealRush,
            "�q�[�����b�V��" },
        {CardEffect.SelfPredation,
            "���ȕߐH" },
        {CardEffect.BloodPact,
            "���̖���" },
        {CardEffect.Poison,
            "�� {0}" },
        {CardEffect.Detox,
            "���" },
        {CardEffect.Defense,
            "�h�q {0}" },
        {CardEffect.Seal,
            "����" },
        {CardEffect.Burn,
            "�Ώ� {0}" },
        {CardEffect.Flame,
            "���� {0}" },
        {CardEffect.CatchingUp,
            "�쒀 {0}" },
        {CardEffect.Electrocution,
            "���d {0}" },
        {CardEffect.Leakage,
            "�R�d {0}" },
        {CardEffect.Augment,
            "���� {0}" },
        {CardEffect.TripleDamage,
            "�_���[�W�O�{" },
        {CardEffect.Reverse,
            "���]" },
        {CardEffect.Hypergravity,
            "���d��" },
        {CardEffect.ForceHigher,
            "���x {0} �ȏ�" },
        {CardEffect.ForceLess,
            "���x {0} �ȉ�" },
        {CardEffect.BaseOnly,
            "�{�̌���" },
        {CardEffect.PartsOnly,
            "�f�ތ���" },
        {CardEffect.NoCompo,
            "��������" },
        {CardEffect.SelfInjury,
            "���� {0}" },
        {CardEffect.DeckRegen,
            "�R�D�Đ� {0}" },
        {CardEffect.NoHeal,
            "�񕜒�~" },
        {CardEffect.Stun,
            "�X�^��" },
        {CardEffect.Reaction,
            "����" },
        {CardEffect.NoWeapon,
            "���햳��" },
        {CardEffect.Bonus_Gold,
            "���� {0}" },
        {CardEffect.Bonus_EXP,
            "�o���l {0}" },
        {CardEffect.Bonus_Heal,
            "�� {0}" },
    };


    //���ʐ���
    readonly public static Dictionary<CardEffect, string> Dic_EffectExplain = new Dictionary<CardEffect, string>()
    {
        {CardEffect.Damage,
            "����� {0} �̃_���[�W��^����" },
        {CardEffect.Heal,
            "�����̗̑͂� {0} �񕜂���" },
        {CardEffect.Weakness,
            "�_���[�W�̌��ʂ� {0} ����������\n(�}�C�i�X�ɂ͂Ȃ�Ȃ�)" },
        {CardEffect.DoubleDamage,
            "����ɗ^����_���[�W���Q�{�ɂ���\n(��Ԉُ�ɂ͖���)(�������ʂƏd���Ȃ�)" },
        {CardEffect.Absorption,
            "����ɗ^�����_���[�W�����g����" },
        {CardEffect.ForceEqual,
            "�J�[�h�̋��x�� {0} �̏ꍇ�̂ݔ���" },
        {CardEffect.Assault,
            "���[�ł��̃J�[�h���������ꂽ��\n{0} �̃_���[�W��^����" },
        {CardEffect.Pursuit,
            "�E�[�ł��̃J�[�h���������ꂽ��\n{0} �̃_���[�W��^����" },
        {CardEffect.WeaponDmg,
            "����� {0} �̃_���[�W��^����" },
        {CardEffect.Kiai,
            "�E�ׂ̃J�[�h�Ŏ������^���镐��_���[�W��2�{�ɂ���" },
        {CardEffect.Rush,
            "����_���[�W���ʂ����J�[�h�̎g�p�񐔁~3�|�C���g�̃_���[�W\n (���� {0} ��E�퓬��Ƀ��Z�b�g)" },
        {CardEffect.HealRush,
            "�񕜌��ʂ����J�[�h�̎g�p�񐔁~3�|�C���g�̗͂���\n (���� {0} ��E�퓬��Ƀ��Z�b�g)" },
        {CardEffect.SelfPredation,
            "�����̗̑͂�S�񕜂��邪�A�ő�̗͂𔼕��ɂ���" },
        {CardEffect.BloodPact,
            "���̃J�[�h�̗͉̑񕜗ʂ�\n����ɗ^����_���[�W�ʂɕύX����" },
        {CardEffect.Poison,
            "����ɓł� {0} �^����\n(�ŁF�^�[���I�����Ƀ_���[�W)" },
        {CardEffect.Detox,
            "�����̓ł���菜��" },
        {CardEffect.Defense,
            "�^�[���I���܂ő��肪�^����_���[�W�� {0} ���炷" },
        {CardEffect.Seal,
            "�E�ׂ̃J�[�h�̔����𕕂���" },
        {CardEffect.Burn,
            "����̍ő�̗͂� {0} ���炷" },
        {CardEffect.Flame,
            "����ɉ���� {0} �^����\n(����F�ő�̗͌������ɒǉ��_���[�W)" },
        {CardEffect.CatchingUp,
            "�������ɑ���̗̑͂������ȉ��̎�\n{0} �̒ǉ��_���[�W��^����" },
        {CardEffect.Electrocution,
            "�E�ׂ̃J�[�h���^����_���[�W�� {0} ���炷" },
        {CardEffect.Leakage,
            "���ȍ~�̑S�ẴJ�[�h���^����_���[�W�� {0} ���炷\n(�^�[���I���܂�)" },
        {CardEffect.Augment,
            "���ȍ~�̑S�ẴJ�[�h���^����_���[�W�� {0} ���₷\n(�^�[���I���܂�)(�_���[�W���ʂ������Ă��Ώ�)" },
        {CardEffect.TripleDamage,
            "����ɗ^����_���[�W���R�{�ɂ���\n(��Ԉُ�ɂ͖���)(�������ʂƏd���Ȃ�)" },
        {CardEffect.Reverse,
            "���̃J�[�h������ɗ^������ʂ������ɁA\n�������󂯂���ʂ𑊎�ɗ^����" },
        {CardEffect.Hypergravity,
            "����̎c��̗͂�4����1�̃_���[�W��^����" },
        {CardEffect.ForceHigher,
            "�J�[�h�̋��x�� {0} �ȏ�̏ꍇ�̂ݔ���" },
        {CardEffect.ForceLess,
            "�J�[�h�̋��x�� {0} �ȉ��̏ꍇ�̂ݔ���" },
        {CardEffect.BaseOnly,
            "���̃J�[�h�̍����͖{�̂Ƃ��Ă̂݉\" },
        {CardEffect.PartsOnly,
            "���̃J�[�h�̍����͑f�ނƂ��Ă̂݉\" },
        {CardEffect.NoCompo,
            "���̃J�[�h�͍����ł��Ȃ�" },
        {CardEffect.SelfInjury,
            "������ {0} �̗̑͂�����" },
        {CardEffect.DeckRegen,
            "�f�b�L�̎c��J�[�h������ {0} ���ǉ�����" },
        {CardEffect.NoHeal,
            "�^�[���I���܂ő���̗͉̑񕜂𕕂���" },
        {CardEffect.Stun,
            "�E�ׂ̎����̃J�[�h�͔����ł��Ȃ�" },
        {CardEffect.Reaction,
            "����ɗ^�����_���[�W�̔������������̎c��̗͂����炷\n�i�[���؂�̂āj" },
        {CardEffect.NoWeapon,
            "�^�[���I���܂ő��肩�畐��_���[�W���󂯂Ȃ�" },
        {CardEffect.Bonus_Gold,
            "(�{�[�i�X��p)\n������{0}����" },
        {CardEffect.Bonus_EXP,
            "(�{�[�i�X��p)\n�o���l��{0}����" },
        {CardEffect.Bonus_Heal,
            "(�{�[�i�X��p)\n�̗͂�{0}�񕜂���" },
    };

    //�������[�h
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

    #region �������[�h��`
    //�������[�h��`
    public enum EffectCompoMode
    {
        Possible,   // �����\
        Impossible, // �����s�\
        OnlyNew,    // �V�K�ǉ��̂�
        OnlyOwn,    // �����ƃJ�[�h�Ƃ̂ݍ����\
        OnlyOwn_New,// �����ƃJ�[�h�Ƃ̂ݍ����\(�V�K�ǉ��̂�)
    }

    //���ʖ�(JP)
    readonly public static Dictionary<EffectCompoMode, string> Dic_CompoModeName = new Dictionary<EffectCompoMode, string>()
    {
        {EffectCompoMode.Possible,
            "�����\" },
        {EffectCompoMode.Impossible,
            "�����s�\" },
        {EffectCompoMode.OnlyNew,
            "�ǉ�����" },
        {EffectCompoMode.OnlyOwn,
            "���J�[�h����" },
        {EffectCompoMode.OnlyOwn_New,
            "�ǉ��E���J�[�h����" },
    };
    #endregion
}
