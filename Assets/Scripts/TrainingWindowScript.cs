//-----------------------------------------------------------------------
/* 訓練場ウィンドウクラス
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

public class TrainingWindowScript : MonoBehaviour
{
    //タイトルマネージャー
    private TitleManagerScript _titleManager = default;
    //ウィンドウのRectTransform
    private RectTransform windowRectTransform = default;

    //ScrollView内のScrollbar
    [SerializeField]
    private Scrollbar _scrollbar = default;
    //所持EXP量Text
    [SerializeField] 
    private Text _statusValueText = default;

    //訓練項目：最大体力上昇 用のアイコン画像
    [SerializeField]
    private Sprite _itemIcon_MaxHPUp = default;
    //訓練項目：手札枚数上昇 用のアイコン画像
    [SerializeField]
    private Sprite _itemIcon_HandNumUp = default;

    //訓練項目関連
    [SerializeField] 
    private GameObject trainingItemPrefab = default; // 訓練項目UIプレハブ
    [SerializeField]
    private Transform trainingItemsParent = default; // 訓練項目UIの親Transform
    private List<TrainingItemScript> trainingItemInstances = default; // 生成済み訓練項目リスト

    //ウィンドウ表示・非表示Tween;
    private Tween windowTween;
    //ウィンドウ表示アニメーション時間
    private const float WindowAnimTime = 0.3f;
    //スクロールバー初期化フラグ
    private bool reservResetScrollBar;

    //定数定義
    public const int InitPlayerHandNum = 5; // プレイヤーの初期手札枚数

    // 初期化関数(TitleManager.csから呼出)
    //public void Init(TitleManagerScript titleManager)
    //{
    //    //参照取得
    //    _titleManager = titleManager;
    //    windowRectTransform = GetComponent<RectTransform>();

    //    // 変数初期化
    //    trainingItemInstances = new List<TrainingItemScript>();

    //    // 訓練項目作成
    //    // 最大体力上昇
    //    {
    //     // 項目作成
    //        GameObject obj = Instantiate(trainingItemPrefab, trainingItemsParent);
    //        TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
    //        trainingItemInstances.Add(trainingItem);
    //        //項目設定
    //        trainingItem.Init(this, TrainingItemScript.TrainingMode.MaxHP_UP);
    //        trainingItem.SetIcon(_itemIcon_MaxHPUp);
    //    }
    //    // 手札枚数上昇
    //    if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
    //    {// (この訓練を完了していないなら作成)
    //     // 項目作成
    //        GameObject obj = Instantiate(trainingItemPrefab, trainingItemsParent);
    //        TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
    //        trainingItemInstances.Add(trainingItem);
    //        // 項目設定
    //        trainingItem.Init(this, TrainingItemScript.TrainingMode.HandNum_UP);
    //        trainingItem.SetIcon(_itemIcon_HandNumUp);
    //    }

    //    // 所持EXP量表示
    //    _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");

    //    // ウィンドウ非表示
    //    windowRectTransform.transform.localScale = Vector2.zero;
    //    windowRectTransform.gameObject.SetActive(true);
    //}

    //// OnGUI (フレームごとに実行)
    //void OnGUI()
    //{
    //    // 画面スクロールを初期値に戻す処理
    //    if (reservResetScrollBar)
    //    {
    //        _scrollbar.value = 1.0f;
    //        reservResetScrollBar = false;
    //    }
    //}

    ///// <summary>
    ///// ウィンドウを表示する
    ///// </summary>
    //public void OpenWindow()
    //{
    //    if (windowTween != null)
    //        windowTween.Kill();
    //    // ウィンドウ表示Tween
    //    windowTween =
    //        windowRectTransform.DOScale(1.0f, WindowAnimTime)
    //        .SetEase(Ease.OutBack);
    //    // ウィンドウ背景パネルを有効化
    //   _titleManager.SetWindowBackPanelActive(true);
    //    // 画面スクロールを初期値に戻す(次のOnGUIで実行)
    //    reservResetScrollBar = true;
    //}
    ///// <summary>
    ///// ウィンドウを非表示にする
    ///// </summary>
    //public void CloseWindow()
    //{
    //    if (windowTween != null)
    //        windowTween.Kill();
    //    // ウィンドウ非表示Tween
    //    windowTween =
    //        windowRectTransform.DOScale(0.0f, WindowAnimTime)
    //        .SetEase(Ease.InBack);
    //    // ウィンドウ背景パネルを無効化
    //    _titleManager.SetWindowBackPanelActive(false);
    //}

    ///// <summary>
    ///// 該当の項目を取得(購入)する
    ///// </summary>
    //public void ObtainItem(TrainingItemScript targetItem)
    //{
    //    // EXP量減少
    //    DataScript._date.ChangePlayerEXP(-targetItem.price);
    //    OnChangePlayerEXP();

    //    // 項目が対応する物をプレイヤーが獲得する
    //    switch (targetItem.trainingMode)
    //    {
    //        case TrainingItemScript.TrainingMode.MaxHP_UP:
    //            // 最大体力上昇
    //            DataScript._date.ChangePlayerMaxHP(1);
    //            targetItem.RefreshUI();
    //            break;
    //        case TrainingItemScript.TrainingMode.HandNum_UP:
    //            // 手札枚数上昇
    //            Data.instance.ChangePlayerHandNum(1);
    //            if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
    //            {// まだ訓練可能
    //                targetItem.RefreshUI();
    //            }
    //            else
    //            {// もう訓練不可能
    //             // オブジェクト消去
    //                Destroy(targetItem.gameObject);
    //                // リストから削除
    //                trainingItemInstances.Remove(targetItem);
    //            }
    //            break;
    //    }
    //}

    ///// <summary>
    ///// プレイヤーのEXP量が変更された時に呼び出される
    ///// </summary>
    //public void OnChangePlayerEXP()
    //{
    //    // 所持EXP量表示
    //    _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");
    //    // 各項目の取得ボタン押下可否設定
    //    foreach (TrainingItemScript item in trainingItemInstances)
    //        item.CheckPrice();
    //}

    ///// <summary>
    ///// 手札枚数上昇の訓練を行った回数を返す
    ///// </summary>
    //public int GetHandNumTrainCount()
    //{
    //    return DataScript._date.GetPlayerHandNum - InitPlayerHandNum;
    //}
}
