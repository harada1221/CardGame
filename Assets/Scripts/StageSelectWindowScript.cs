using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StageSelectWindowScript : MonoBehaviour
{
    [SerializeField, Header("ステージ名Text")]
    private Text _stageNameText = default;
    [SerializeField, Header("ステージ難易度Text")]
    private Text _stageDifficultyText = default;
    [SerializeField, Header("戦闘回数Text")]
    private Text _battleNumText = default;
    [SerializeField, Header("ステージアイコンImage")]
    private Image _stageIconImage = default;
    [SerializeField, Header("選択中ステージ番号ドット")]
    private List<Image> _stageOrderImages = null;
    [SerializeField, Header("戦闘シーン名")]
    private string _nextSceneName = default;

    //ステージの総数
    private int _stageListNum = default;
    //選択中ステージID
    private int _selectStageID = default;
    //タイトル管理クラス
    private TitleManagerScript _titleManager = default;
    //ウィンドウのRectTransform
    private RectTransform _windowRectTransform = default;
    //ウィンドウ用Tween;
    private Tween _windowTween = default;
    //ウィンドウ表示アニメーション時間
    private const float _WindowAnimTime = 0.3f;

    //初期化関数(TitleManager.csから呼出)
    public void Init(TitleManagerScript titleManager)
    {
        //参照取得
        this._titleManager = titleManager;
        _windowRectTransform = GetComponent<RectTransform>();
        // 変数初期化
        _stageListNum = DataScript._date.GetStageSOs.Count;
        _selectStageID = 0;

        //ウィンドウ非表示
        _windowRectTransform.transform.localScale = Vector2.zero;
        _windowRectTransform.gameObject.SetActive(true);
    }

    /// <summary>
    /// ウィンドウを表示する
    /// </summary>
    public void OpenWindow()
    {
        //アニメーション終了
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }

        //ウィンドウ表示Tween
        _windowTween =
            _windowRectTransform.DOScale(1.0f, _WindowAnimTime)
            .SetEase(Ease.OutBack);
        //ウィンドウ背景パネルを表示
        _titleManager.SetWindowBackPanelActive(true);
        //ステージ情報表示
        ShowStageDatas();
    }
    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        //アニメーション終了
        if (_windowTween != null)
        {
            _windowTween.Kill();
        }

        //ウィンドウ非表示Tween
        _windowTween =
            _windowRectTransform.DOScale(0.0f, _WindowAnimTime)
            .SetEase(Ease.InBack);
        //ウィンドウ背景パネルを非表示
        _titleManager.SetWindowBackPanelActive(false);
    }
    /// <summary>
	/// １つ左側(マイナス方向)のステージに切り替えるボタン
	/// </summary>
	public void LeftScrollButton()
    {
        //選択ステージ切り替え
        _selectStageID--;
        if (_selectStageID < 0)
        {
            _selectStageID = _stageListNum - 1;
        }
        //選択中ステージ情報表示
        ShowStageDatas();
    }
    /// <summary>
    /// １つ右側(プラス方向)のステージに切り替えるボタン
    /// </summary>
    public void RightScrollButton()
    {
        //選択ステージ切り替え
        _selectStageID++;
        if (_selectStageID >= _stageListNum)
        {
            _selectStageID = 0;
        }
        //選択中ステージ情報表示
        ShowStageDatas();
    }

    /// <summary>
    /// 選択中のステージ情報を表示する
    /// </summary>
    public void ShowStageDatas()
    {
        StageSO stageData = DataScript._date.GetStageSOs[_selectStageID];


        //ステージ名Text
        _stageNameText.text = stageData.GetStageName;
        //ステージ難易度Text
        _stageDifficultyText.text = stageData.GetDifficulty;
        //敵の数Text
        _battleNumText.text = "敵の数 " + stageData.GetAppearEnemyTables.Count;

        //ステージアイコンImage
        _stageIconImage.sprite = stageData.GetStageIcon;

        //ステージ番号Images
        for (int i = 0; i < _stageListNum; i++)
        {
            if (i == _selectStageID)
            {
                _stageOrderImages[i].transform.localScale = new Vector2(1.0f, 1.0f);
            }
            else
            {
                _stageOrderImages[i].transform.localScale = new Vector2(0.4f, 0.4f);
            }
        }
    }

    /// <summary>
    /// ステージ開始ボタン
    /// </summary>
    /// <param name="isWithTutorial">true:チュートリアル有りで開始する</param>
    public void StageStartButton(bool isWithTutorial)
    {
        // 選択したステージ番号を記憶
        DataScript._date._nowStageID = _selectStageID;
        // シーン切り替え
        SceneManager.LoadScene(_nextSceneName);


    }
}
