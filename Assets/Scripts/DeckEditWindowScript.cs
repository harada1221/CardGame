using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckEditWindowScript : MonoBehaviour
{
    [SerializeField, Header("カードプレハブ")]
    private GameObject _cardPrefab = default;
    [SerializeField, Header("デッキ名,右上デッキ枚数文字列")]
    private Text _deckLogoText = default;
    [SerializeField, Header("デッキから外す")]
    private Button _backToStorageButton = default;
    [SerializeField, Header("デッキに入れる")]
    private Button _intoDeckButton = default;
    [SerializeField, Header("保管中カード一覧オブジェクトTransform")]
    private Transform _storageCardsAreaTransform = default;
    //生成済み保管中カードリスト
    private List<GameObject> _storageCardObjects = default;
    //所持カード
    private Dictionary<GameObject, CardScript> _dicStorageCardObjectByCard = default;
    [SerializeField, Header("デッキオブジェクトTransform")]
    private Transform _deckAreaTransform = default;
    //生成済みデッキ内カードリスト
    private List<GameObject> _deckCardObjects = default;
    private Dictionary<GameObject, CardScript> _dic_DeckCardObjectByCard = default;
    ////タイトル管理クラス
    //private TitleManagerScript _titleManager = default;
    //選択中カード情報
    private CardScript _selectCard = default;
    //定数定義
    //最小デッキ枚数
    public const int MinDeckNum = 1;
    //最大デッキ枚数
    public const int MaxDeckNum = 60;
    // 初期化関数(TitleManager.csから呼出)
    public void Init(TitleManagerScript _titleManager)
    {
        //変数初期化
        _deckCardObjects = new List<GameObject>();
        _dic_DeckCardObjectByCard = new Dictionary<GameObject, CardScript>();
        _storageCardObjects = new List<GameObject>();
        _dicStorageCardObjectByCard = new Dictionary<GameObject, CardScript>();

        //UI非表示
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ウィンドウを表示する
    /// </summary>
    public void OpenWindow()
    {
        //既に生成されているカードオブジェクトを削除
        if (_deckCardObjects != null)
        {
            foreach (GameObject cardObject in _deckCardObjects)
            {
                Destroy(cardObject.gameObject);
            }
            _deckCardObjects.Clear();
        }
        if (_storageCardObjects != null)
        {
            foreach (GameObject cardObject in _storageCardObjects)
            {
                Destroy(cardObject);
            }
            _storageCardObjects.Clear();
        }

        //UI初期化
        gameObject.SetActive(true);
        DeselectCard();
        RefreshDeckNumToUI();

        //保管中カード反映
        foreach (KeyValuePair<int, int> storageCard in PlayerDeckDataScript._storageCardList)
        {
            if (storageCard.Value > 0)
            {
                CreateStorageCardObject(PlayerDeckDataScript._cardDatasBySerialNum[storageCard.Key]);
            }
        }
        //デッキ反映
        foreach (int deckCard in PlayerDeckDataScript._deckCardList)
        {
            CreateDeckCardObject(PlayerDeckDataScript._cardDatasBySerialNum[deckCard]);
        }
        //カードリスト整列
        AlignDeckList();
        AlignStorageList();
    }
    /// <summary>
    /// ウィンドウを非表示にする
    /// </summary>
    public void CloseWindow()
    {
        //終了処理
        DeselectCard();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 保管中カードオブジェクトを作成する
    /// </summary>
    private void CreateStorageCardObject(CardDataSO cardData)
    {
        //オブジェクト作成
        GameObject obj = Instantiate(_cardPrefab, _storageCardsAreaTransform);
        //カード処理クラスを取得・リストに格納
        CardScript objCard = obj.GetComponent<CardScript>();
        _storageCardObjects.Add(obj);
        _dicStorageCardObjectByCard.Add(obj, objCard);

        //カード初期設定
        objCard.InitDeck(this, false);
        objCard.SetInitialCardData(cardData, CardScript.CharaID_Player);
        objCard.ShowCardAmountInStorage();
    }
    /// <summary>
    /// デッキ内カードオブジェクトを作成する
    /// </summary>
    private void CreateDeckCardObject(CardDataSO cardData)
    {
        //オブジェクト作成
        GameObject obj = Instantiate(_cardPrefab, _deckAreaTransform);
        //カード処理クラスを取得・リストに格納
        CardScript objCard = obj.GetComponent<CardScript>();
        _deckCardObjects.Add(obj);
        _dic_DeckCardObjectByCard.Add(obj, objCard);

        // カード初期設定
        objCard.InitDeck(this, true);
        objCard.SetInitialCardData(cardData, CardScript.CharaID_Player);

        //デッキ内カードオブジェクト整列処理
        int alignToIndex = 99;
        int cardObjectListLength = _deckAreaTransform.childCount;
        for (int i = 0; i < cardObjectListLength; i++)
        {
            Transform cardObject = _deckAreaTransform.GetChild(i);
            if (objCard.GetCardData.GetSerialNum <= cardObject.GetComponent<CardScript>().GetCardData.GetSerialNum)
            {
                alignToIndex = i;
                break;
            }
        }
        obj.transform.SetSiblingIndex(alignToIndex);
    }

    /// <summary>
    /// デッキカードオブジェクトの並びを変更する
    /// </summary>
    public void AlignDeckList()
    {
        //デッキカードオブジェクトのリストを整列させる
        _deckCardObjects.Sort((a, b) => _dic_DeckCardObjectByCard[a].GetCardData.GetSerialNum - _dic_DeckCardObjectByCard[b].GetCardData.GetSerialNum);
        //並び替えたリストを基にTransformの兄弟関係も整列させる
        int length = _deckCardObjects.Count;
        for (int i = 0; i < length; i++)
        {
            _deckCardObjects[i].transform.SetAsLastSibling();
        }
    }
    /// <summary>
    /// 保管中カードオブジェクトの並びを変更する
    /// </summary>
    public void AlignStorageList()
    {
        //オブジェクトのリストを整列させる
        _storageCardObjects.Sort((a, b) => _dicStorageCardObjectByCard[a].GetCardData.GetSerialNum - _dicStorageCardObjectByCard[b].GetCardData.GetSerialNum);
        //並び替えたリストを基にTransformの兄弟関係も整列させる
        int length = _storageCardObjects.Count;
        for (int i = 0; i < length; i++)
        {
            _storageCardObjects[i].transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// タップ先のカード選択処理
    /// </summary>
    public void SelectCard(CardScript targetCard)
    {
        //前回選択したカードの強調表示終了
        if (_selectCard != null)
        {
            _selectCard.SetCardHilight(false);
        }

        //選択情報取得
        _selectCard = targetCard;
        //選択カード強調表示
        targetCard.SetCardHilight(true);

        //各種ボタンの押下可否状態を変更
        if (_selectCard.isInDeckCard)
        {
            //編集中デッキ内カード選択時
            _backToStorageButton.interactable = true;
            _intoDeckButton.interactable = false;
        }
        else
        {
            //保管中リスト内カード選択時
            _backToStorageButton.interactable = false;
            _intoDeckButton.interactable = true;
        }
    }
    /// <summary>
    /// カードの選択を解除する
    /// </summary>
    public void DeselectCard()
    {
        //選択カード強調表示終了
        if (_selectCard != null)
        {
            _selectCard.SetCardHilight(false);
        }

        //選択情報クリア
        _selectCard = null;

        //各種ボタンの押下可否状態を変更
        _backToStorageButton.interactable = false;
        _intoDeckButton.interactable = false;
    }

    /// <summary>
    /// デッキ枚数の表示を更新する
    /// </summary>
    private void RefreshDeckNumToUI()
    {
        int nowDeckNum = PlayerDeckDataScript._deckCardList.Count;

        //デッキ枚数の表示を更新する
        _deckLogoText.text = "デッキ";
        _deckLogoText.text += "<size=20>(" + nowDeckNum + ")</size>";

        //デッキ枚数が最低枚数以下の時は強調表示
        if (nowDeckNum < MinDeckNum)
        {
            _deckLogoText.text = _deckLogoText.text.Insert(0, "デッキ<color=red>");
            _deckLogoText.text += "</color>";
        }
    }

    #region ボタン押下時処理
    /// <summary>
    /// ボタンを押した時
    /// </summary>
    public void Button_BackToStorage()
    {
        //デッキ枚数が下限に達している時は追加せず終了
        if (PlayerDeckDataScript._deckCardList.Count <= MinDeckNum)
        {
            return;
        }

        //選択カード記憶・選択解除
        if (_selectCard == null)
        {
            return;
        }
        CardScript targetCard = _selectCard;
        DeselectCard();

        //１つ右側にあるカードを記憶
        CardScript nextSelectCard = null;
        int selectCardIndexInList = _deckCardObjects.IndexOf(targetCard.gameObject);
        if (selectCardIndexInList + 1 < _deckCardObjects.Count)
        {
            nextSelectCard = _dic_DeckCardObjectByCard[_deckCardObjects[selectCardIndexInList + 1]];
        }

        //デッキからカードを削除
        PlayerDeckDataScript.RemoveCardFromDeck(targetCard.GetCardData.GetSerialNum);
        //保管中リストにカードを追加
        PlayerDeckDataScript.ChangeStorageCardNum(targetCard.GetCardData.GetSerialNum, 1);

        //保管中エリアのカードオブジェクト追加
        CardScript cardInstanceInStorageArea = null;
        for (int i = 0; i < _storageCardObjects.Count; i++)
        {
            CardScript targetStorageCard = _dicStorageCardObjectByCard[_storageCardObjects[i]];
            if (targetCard.GetCardData.GetSerialNum == targetStorageCard.GetCardData.GetSerialNum)
            {
                cardInstanceInStorageArea = targetStorageCard;
                break;
            }
        }
        if (cardInstanceInStorageArea != null)
        {
            //該当のカードオブジェクトが存在する
            //カードの表示を更新
            cardInstanceInStorageArea.ShowCardAmountInStorage();
        }
        else
        {
            //該当のカードオブジェクトが存在しない
            //カードオブジェクト作成
            CreateStorageCardObject(targetCard.GetCardData);
            //保管中カードリスト整列
            AlignStorageList();
        }
        //編集中デッキエリアのオブジェクト削除
        _deckCardObjects.Remove(targetCard.gameObject);
        _dic_DeckCardObjectByCard.Remove(targetCard.gameObject);
        Destroy(targetCard.gameObject);

        //デッキ枚数表示更新
        RefreshDeckNumToUI();
        //カード選択解除
        DeselectCard();
        //次のカードがあるなら自動的に選択
        if (nextSelectCard != null)
        {
            SelectCard(nextSelectCard);
        }

    }

    /// <summary>
    /// デッキに入れるボタン
    /// </summary>
    public void Button_IntoDeck()
    {
        //デッキ枚数が上限に達している時は追加せず終了
        if (PlayerDeckDataScript._deckCardList.Count >= MaxDeckNum)
        {
            return;
        }

        //選択カード記憶・選択解除
        if (_selectCard == null)
        {
            return;
        }
        CardScript targetCard = _selectCard;

        //デッキにカードを追加
        PlayerDeckDataScript.AddCardToDeck(targetCard.GetCardData.GetSerialNum);
        //保管中リストからカードを削除
        PlayerDeckDataScript.ChangeStorageCardNum(targetCard.GetCardData.GetSerialNum, -1);

        //保管中エリアのカードオブジェクト削除
        if (PlayerDeckDataScript._storageCardList[targetCard.GetCardData.GetSerialNum] > 0)
        {
            targetCard.ShowCardAmountInStorage();
        }
        else
        {
            //カード選択解除
            DeselectCard();
            //オブジェクト削除
            _storageCardObjects.Remove(targetCard.gameObject);
            _dicStorageCardObjectByCard.Remove(targetCard.gameObject);
            Destroy(targetCard.gameObject);
        }

        //デッキ枚数表示更新
        RefreshDeckNumToUI();
        //編集中デッキエリアのオブジェクト追加
        CreateDeckCardObject(targetCard.GetCardData);
        //デッキカードリスト整列
        AlignDeckList();
    }
    #endregion
}

