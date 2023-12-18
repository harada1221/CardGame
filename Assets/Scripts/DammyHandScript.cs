using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DammyHandScript : MonoBehaviour
{
    [SerializeField, Header("ダミー手札整列用")]
    private HorizontalLayoutGroup _layoutGroup = default;
    [SerializeField, Header("ダミー手札プレハブ")]
    private GameObject _dammyHand = default;
    [SerializeField, Header("生成する数")]
    private int _maxCount = default;
    //ダミー手札リスト
    private List<Transform> _dammyHandList = default;
    //生成するオブジェクトのQueue
    private Queue<GameObject> _handPrehub = default;

    private void Start()
    {
        //カード生成ループ
        for (int i = 0; i < _maxCount; i++)
        {
            // オブジェクト作成
            GameObject obj = Instantiate(_dammyHand, transform);
            //Queueに格納
            _handPrehub.Enqueue(obj);
            // オブジェクトをfalseにする
            obj.SetActive(false);
        }
    }
    /// <summary>
    /// 指定の枚数になるようダミー手札を作成または削除する
    /// </summary>
    /// <param name="value">設定枚数</param>
    public void SetHandNum(int value)
    {
        // 初回実行時
        if (_dammyHandList == null)
        {
            // リスト新規生成
            _dammyHandList = new List<Transform>();
            AddHandObj(value);
        }
        else
        {
            // 現在から変化する枚数を計算
            int differenceNum = value - _dammyHandList.Count;
            // ダミー手札作成・削除
            // 手札が増えるならダミー手札作成
            if (differenceNum > 0)
            {
                AddHandObj(differenceNum);
            }
            // 手札が減るならダミー手札削除
            else if (differenceNum < 0)
            {
                RemoveHandObj(differenceNum);
            }

        }
    }
    /// <summary>
    /// ダミー手札を指定枚数追加する
    /// </summary>
    private void AddHandObj(int value)
    {
        GameObject obj = _handPrehub.Dequeue();

        // 追加枚数分オブジェクト作成
        for (int i = 0; i < value; i++)
        {
            // リストになかったら生成
            if (obj == null)
            {
                // オブジェクト作成
                obj = Instantiate(_dammyHand, transform);
                //Queueに格納
                _handPrehub.Enqueue(obj);
            }
            obj.SetActive(true);
            // リストに追加
            _dammyHandList.Add(obj.transform);
        }
    }
    /// <summary>
    /// ダミー手札を指定枚数削除する
    /// </summary>
    private void RemoveHandObj(int value)
    {
        // 削除枚数を正数で取得
        value = Mathf.Abs(value);
        // 削除枚数分オブジェクト削除
        for (int i = 0; i < value; i++)
        {
            if (_dammyHandList.Count <= 0)
            {
                break;
            }
            // オブジェクト削除
            _dammyHandList[0].gameObject.SetActive(false);
            //Queueに格納
            _handPrehub.Enqueue(_dammyHandList[0].gameObject);
            // リストから削除
            _dammyHandList.RemoveAt(0);
        }
    }
    /// <summary>
    /// 該当番号のダミー手札の座標を返す
    /// </summary>
    public Vector2 GetHandPos(int index)
    {
        if (index < 0 || index >= _dammyHandList.Count)
        {
            return Vector2.zero;
        }
        // ダミー手札の座標を返す
        return _dammyHandList[index].position;
    }

    /// <summary>
    /// レイアウトの自動整列機能を即座に適用する
    /// </summary>
    public void ApplyLayout()
    {
        //整列させる
        _layoutGroup.CalculateLayoutInputHorizontal();
        _layoutGroup.SetLayoutHorizontal();
    }
}
