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

    // 訓練場ウィンドウクラス
    private TrainingWindowScript trainingWindow;


    //訓練項目の種類定義
    public enum TrainingMode
    {
        MaxHPUp,   //最大体力上昇
        HandNumUp, //手札枚数上昇
    }
    /// <summary>
    /// 取得ボタン押下時処理
    /// </summary>
    public void ObtainButton()
    {

    }
}
