using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingItemScript : MonoBehaviour
{
    //UIオブジェクト
    [SerializeField, Header("項目アイコン")]
    private Image _iconImage = default;
    [SerializeField, Header("項目名Text")]
    private Text _nameText = default;
    [SerializeField, Header("項目説明Text")]
    private Text _explainText = default;
    [SerializeField, Header("取得ボタン")]
    private Button _obtainButton = default;
    [SerializeField, Header("価格Text")]
    private Text _priceText = default;
    [SerializeField, Header("項目の種類")]
    private TrainingMode _trainingMode = default;
    [SerializeField, Header("必要経験値量")]
    private int _price = default;
    //訓練場ウィンドウクラス
    private TrainingWindowScript _trainingWindow = default;
    //定数定義
    //最大体力上昇の必要経験値量(現在の最大体力にこの定数をかける)
    private const int TrainPriceMaxHpUp = 5;
    //手札枚数上昇の訓練回数ごとの必要経験値量
    public static readonly int[] TrainPrice_HandNum_UP = new int[] { 100, 1000, 5000, 10000, 50000 };

    public int GetPrice { get => _price; }
    public TrainingMode GetTraining { get => _trainingMode; }
    //訓練項目の種類定義
    public enum TrainingMode
    {
        MaxHPUp,   //最大体力上昇
        HandNumUp, //手札枚数上昇
    }
    /// <summary>
    ///初期化処理 
    /// </summary>
    /// <param name="trainingWindow">トレーニング管理用</param>
    /// <param name="trainingMode">トレーニング状態</param>
    public void Init(TrainingWindowScript trainingWindow, TrainingMode trainingMode)
    {
        //参照取得
        _trainingWindow = trainingWindow;
        //変数初期化
        _trainingMode = trainingMode;
        //UI初期化
        RefreshUI();
    }
   	/// <summary>
	/// 取得ボタン押下時処理
	/// </summary>
	public void ObtainButton()
    {
        _trainingWindow.ObtainItem(this);
    }
    /// <summary>
	/// UIの表示を更新する
	/// </summary>
	public void RefreshUI()
    {
        switch (_trainingMode)
        {
            case TrainingMode.MaxHPUp:
                //最大HP上昇
                _nameText.text = "最大体力上昇";
                _explainText.text = "プレイヤーの最大体力が上昇します。\n(現在の最大体力:" + DataScript._date.GetPlayerMaxHP + ")";


                //必要EXP量
                _price = DataScript._date.GetPlayerMaxHP * TrainPriceMaxHpUp;
                _priceText.text = _price.ToString("#,0");
                CheckPrice();
                break;

            case TrainingMode.HandNumUp:
                //手札枚数上昇
                _nameText.text = "手札枚数上昇";
                _explainText.text = "ターン開始時の手札枚数が増加します。\n(現在" + DataScript._date.GetPlayerHandNum + "枚)";

                //必要EXP量
                _price = TrainPrice_HandNum_UP[_trainingWindow.GetHandNumTrainCount()];
                _priceText.text = _price.ToString("#,0");
                CheckPrice();
                break;
        }
    }
    /// <summary>
    /// 表示するアイコンを決める
    /// </summary>
    /// <param name="sprite">表示するアイコン</param>
    public void SetIcon(Sprite sprite)
    {
        //アイコンがあるか
        if (sprite == null)
        {
            _iconImage.color = Color.clear;
        }
        else
        {
            _iconImage.color = Color.white;
            _iconImage.sprite = sprite;
        }
    }
    /// <summary>
	/// 項目の取得に必要なEXPが足りているかを判定
	/// </summary>
	public void CheckPrice()
    {
        //プレイヤーの経験値が足りているか
        if (DataScript._date.GetPlayerExp >= _price)
        {
            //ボタンを有効化
            _obtainButton.interactable = true;
        }
        else
        {
            //ボタンを無効化
            _obtainButton.interactable = false;
        }
    }
}
