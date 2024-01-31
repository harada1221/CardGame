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
    [SerializeField, Header("Scrollbar")]
    private Scrollbar _scrollbar = default;
    [SerializeField, Header("所持EXP量Text")]
    private Text _statusValueText = default;
    [SerializeField, Header("体力上昇アイコン")]
    private Sprite _itemIcon_MaxHPUp = default;
    [SerializeField, Header("手札上昇アイコン")]
    private Sprite _itemIcon_HandNumUp = default;
    [SerializeField, Header("訓練項目UIプレハブ")]
    private GameObject _trainingItemPrefab = default;
    [SerializeField, Header("訓練項目UIの親Transform")]
    private Transform _trainingItemsParent = default;
    //タイトルマネージャー
    private TitleManagerScript _titleManager = default;
    //ウィンドウのRectTransform
    private RectTransform _windowRectTransform = default;
    //スクロールバー初期化フラグ
    private bool isReservResetScrollBar;
    //生成済み訓練項目リスト
    private List<TrainingItemScript> _trainingItemInstances = default;

    //ウィンドウ表示・非表示Tween;
    private Tween _windowTween = default;
    //ウィンドウ表示アニメーション時間
    private const float WindowAnimTime = 0.3f;

    //定数定義
    public const int InitPlayerHandNum = 5; // プレイヤーの初期手札枚数

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <param name="titleManager">TitleManagerScript</param>
    public void Init(TitleManagerScript titleManager)
    {
        //参照取得
        _titleManager = titleManager;
        _windowRectTransform = GetComponent<RectTransform>();

        // 変数初期化
        _trainingItemInstances = new List<TrainingItemScript>();
        {
            //項目作成
            GameObject obj = Instantiate(_trainingItemPrefab, _trainingItemsParent);
            TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
            _trainingItemInstances.Add(trainingItem);
            //項目設定
            trainingItem.Init(this, TrainingItemScript.TrainingMode.MaxHPUp);
            trainingItem.SetIcon(_itemIcon_MaxHPUp);
        }
        //手札枚数上昇
        if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
        {
            //項目作成
            GameObject obj = Instantiate(_trainingItemPrefab, _trainingItemsParent);
            TrainingItemScript trainingItem = obj.GetComponent<TrainingItemScript>();
            _trainingItemInstances.Add(trainingItem);
            //項目設定
            trainingItem.Init(this, TrainingItemScript.TrainingMode.HandNumUp);
            trainingItem.SetIcon(_itemIcon_HandNumUp);
        }

        //所持EXP量表示
        _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");

        //ウィンドウ非表示
        _windowRectTransform.transform.localScale = Vector2.zero;
        _windowRectTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void OnGUI()
    {
        //画面スクロールを初期値に戻す処理
        if (isReservResetScrollBar)
        {
            _scrollbar.value = 1.0f;
            isReservResetScrollBar = false;
        }
    }

    /// <summary>
    /// ウィンドウを表示する
    /// </summary>
    public void OpenWindow()
    {
        //再生終了させる
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }
        //ウィンドウ表示Tween
        _windowTween = _windowRectTransform.DOScale(1.0f, WindowAnimTime).SetEase(Ease.OutBack);
        //ウィンドウ背景パネルを有効化
        _titleManager.SetWindowBackPanelActive(true);
        //画面スクロールを初期値に戻す(次のOnGUIで実行)
        isReservResetScrollBar = true;
    }
    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        //再生終了させる
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }
        //ウィンドウ非表示Tween
        _windowTween = _windowRectTransform.DOScale(0.0f, WindowAnimTime).SetEase(Ease.InBack);
        //ウィンドウ背景パネルを無効化
        _titleManager.SetWindowBackPanelActive(false);
    }

    /// <summary>
    /// 項目を取得する
    /// </summary>
    /// <param name="targetItem">取得する項目</param>
    public void ObtainItem(TrainingItemScript targetItem)
    {
        //EXP量減少
        DataScript._date.ChangePlayerEXP(-targetItem.GetPrice);
        OnChangePlayerEXP();

        //項目が対応する物をプレイヤーが獲得する
        switch (targetItem.GetTraining)
        {
            case TrainingItemScript.TrainingMode.MaxHPUp:
                //最大体力上昇
                DataScript._date.ChangePlayerMaxHP(1);
                targetItem.RefreshUI();
                break;
            case TrainingItemScript.TrainingMode.HandNumUp:
                //手札枚数上昇
                DataScript._date.ChangePlayerHandNum(1);
                //訓練可能か
                if (GetHandNumTrainCount() < TrainingItemScript.TrainPrice_HandNum_UP.Length)
                {
                    targetItem.RefreshUI();
                }
                else
                {
                    //オブジェクト消去
                    Destroy(targetItem.gameObject);
                    //リストから削除
                    _trainingItemInstances.Remove(targetItem);
                }
                break;
        }
    }

    /// <summary>
    /// プレイヤーのEXP量が変更された時に呼び出される
    /// </summary>
    public void OnChangePlayerEXP()
    {
        //所持EXP量表示
        _statusValueText.text = DataScript._date.GetPlayerExp.ToString("#,0");
        //各項目の取得ボタン押下可否設定
        foreach (TrainingItemScript item in _trainingItemInstances)
        {
            item.CheckPrice();
        }
    }

    /// <summary>
    /// 手札枚数上昇の訓練を行った回数を返す
    /// </summary>
    public int GetHandNumTrainCount()
    {
        return DataScript._date.GetPlayerHandNum - InitPlayerHandNum;
    }
}
