//-----------------------------------------------------------------------
/* �P����E�B���h�E�N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TrainingWindowScript : MonoBehaviour
{
    //�^�C�g���}�l�[�W���[
    private TitleManagerScript _titleManager = default;
    //�E�B���h�E��RectTransform
    private RectTransform windowRectTransform = default;

    //ScrollView����Scrollbar
    [SerializeField]
    private Scrollbar _scrollbar = default;
    //����EXP��Text
    [SerializeField] 
    private Text _statusValueText = default;

    //�P�����ځF�ő�̗͏㏸ �p�̃A�C�R���摜
    [SerializeField]
    private Sprite _itemIcon_MaxHPUp = default;
    //�P�����ځF��D�����㏸ �p�̃A�C�R���摜
    [SerializeField]
    private Sprite _itemIcon_HandNumUp = default;

    //�P�����ڊ֘A
    [SerializeField] 
    private GameObject trainingItemPrefab = default; // �P������UI�v���n�u
    [SerializeField]
    private Transform trainingItemsParent = default; // �P������UI�̐eTransform
    private List<TrainingItemScript> trainingItemInstances = default; // �����ς݌P�����ڃ��X�g

    //�E�B���h�E�\���E��\��Tween;
    private Tween windowTween;
    //�E�B���h�E�\���A�j���[�V��������
    private const float WindowAnimTime = 0.3f;
    //�X�N���[���o�[�������t���O
    private bool reservResetScrollBar;

    //�萔��`
    public const int InitPlayerHandNum = 5; // �v���C���[�̏�����D����

    // �������֐�(TitleManager.cs����ďo)
    //public void Init(TitleManagerScript titleManager)
    //{
    //    //�Q�Ǝ擾
    //    _titleManager = titleManager;
    //    windowRectTransform = GetComponent<RectTransform>();

    //    // �ϐ�������
    //    trainingItemInstances = new List<TrainingItemScript>();

    //    // �P�����ڍ쐬
    //    // �ő�̗͏㏸
    //    {
    //     // ���ڍ쐬
    //        GameObject obj = Instantiate(trainingItemPrefab, trainingItemsParent);
    //        TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
    //        trainingItemInstances.Add(trainingItem);
    //        //���ڐݒ�
    //        trainingItem.Init(this, TrainingItemScript.TrainingMode.MaxHP_UP);
    //        trainingItem.SetIcon(_itemIcon_MaxHPUp);
    //    }
    //    // ��D�����㏸
    //    if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
    //    {// (���̌P�����������Ă��Ȃ��Ȃ�쐬)
    //     // ���ڍ쐬
    //        GameObject obj = Instantiate(trainingItemPrefab, trainingItemsParent);
    //        TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
    //        trainingItemInstances.Add(trainingItem);
    //        // ���ڐݒ�
    //        trainingItem.Init(this, TrainingItemScript.TrainingMode.HandNum_UP);
    //        trainingItem.SetIcon(_itemIcon_HandNumUp);
    //    }

    //    // ����EXP�ʕ\��
    //    _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");

    //    // �E�B���h�E��\��
    //    windowRectTransform.transform.localScale = Vector2.zero;
    //    windowRectTransform.gameObject.SetActive(true);
    //}

    //// OnGUI (�t���[�����ƂɎ��s)
    //void OnGUI()
    //{
    //    // ��ʃX�N���[���������l�ɖ߂�����
    //    if (reservResetScrollBar)
    //    {
    //        _scrollbar.value = 1.0f;
    //        reservResetScrollBar = false;
    //    }
    //}

    ///// <summary>
    ///// �E�B���h�E��\������
    ///// </summary>
    //public void OpenWindow()
    //{
    //    if (windowTween != null)
    //        windowTween.Kill();
    //    // �E�B���h�E�\��Tween
    //    windowTween =
    //        windowRectTransform.DOScale(1.0f, WindowAnimTime)
    //        .SetEase(Ease.OutBack);
    //    // �E�B���h�E�w�i�p�l����L����
    //   _titleManager.SetWindowBackPanelActive(true);
    //    // ��ʃX�N���[���������l�ɖ߂�(����OnGUI�Ŏ��s)
    //    reservResetScrollBar = true;
    //}
    ///// <summary>
    ///// �E�B���h�E���\���ɂ���
    ///// </summary>
    //public void CloseWindow()
    //{
    //    if (windowTween != null)
    //        windowTween.Kill();
    //    // �E�B���h�E��\��Tween
    //    windowTween =
    //        windowRectTransform.DOScale(0.0f, WindowAnimTime)
    //        .SetEase(Ease.InBack);
    //    // �E�B���h�E�w�i�p�l���𖳌���
    //    _titleManager.SetWindowBackPanelActive(false);
    //}

    ///// <summary>
    ///// �Y���̍��ڂ��擾(�w��)����
    ///// </summary>
    //public void ObtainItem(TrainingItemScript targetItem)
    //{
    //    // EXP�ʌ���
    //    DataScript._date.ChangePlayerEXP(-targetItem.price);
    //    OnChangePlayerEXP();

    //    // ���ڂ��Ή����镨���v���C���[���l������
    //    switch (targetItem.trainingMode)
    //    {
    //        case TrainingItemScript.TrainingMode.MaxHP_UP:
    //            // �ő�̗͏㏸
    //            DataScript._date.ChangePlayerMaxHP(1);
    //            targetItem.RefreshUI();
    //            break;
    //        case TrainingItemScript.TrainingMode.HandNum_UP:
    //            // ��D�����㏸
    //            Data.instance.ChangePlayerHandNum(1);
    //            if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
    //            {// �܂��P���\
    //                targetItem.RefreshUI();
    //            }
    //            else
    //            {// �����P���s�\
    //             // �I�u�W�F�N�g����
    //                Destroy(targetItem.gameObject);
    //                // ���X�g����폜
    //                trainingItemInstances.Remove(targetItem);
    //            }
    //            break;
    //    }
    //}

    ///// <summary>
    ///// �v���C���[��EXP�ʂ��ύX���ꂽ���ɌĂяo�����
    ///// </summary>
    //public void OnChangePlayerEXP()
    //{
    //    // ����EXP�ʕ\��
    //    _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");
    //    // �e���ڂ̎擾�{�^�������ېݒ�
    //    foreach (TrainingItemScript item in trainingItemInstances)
    //        item.CheckPrice();
    //}

    ///// <summary>
    ///// ��D�����㏸�̌P�����s�����񐔂�Ԃ�
    ///// </summary>
    //public int GetHandNumTrainCount()
    //{
    //    return DataScript._date.GetPlayerHandNum - InitPlayerHandNum;
    //}
}
