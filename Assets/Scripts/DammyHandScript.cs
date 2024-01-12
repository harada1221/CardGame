using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DammyHandScript : MonoBehaviour
{
    [SerializeField, Header("��D����p")]
    private HorizontalLayoutGroup _layoutGroup = default;
    [SerializeField, Header("��D�v���n�u")]
    private GameObject _dammyHand = default;
    //�_�~�[��D���X�g
    private List<Transform> _dammyHandList = default;
   
    /// <summary>
    /// �w��̖����ɂȂ�悤�_�~�[��D���쐬�܂��͍폜����
    /// </summary>
    /// <param name="value">�ݒ薇��</param>
    public void SetHandNum(int value)
    {
        //������s��
        if (_dammyHandList == null)
        {
            //���X�g�V�K����
            _dammyHandList = new List<Transform>();
            AddHandObj(value);
        }
        else
        {
            //���݂���ω����閇�����v�Z
            int differenceNum = value - _dammyHandList.Count;
            //��D��������Ȃ�_�~�[��D�쐬
            if (differenceNum > 0)
            {
                AddHandObj(differenceNum);
            }
            // ��D������Ȃ�_�~�[��D�폜
            else if (differenceNum < 0)
            {
                RemoveHandObj(differenceNum);
            }

        }
    }
    /// <summary>
    /// �_�~�[��D���w�薇���ǉ�����
    /// </summary>
    private void AddHandObj(int value)
    {

        //�ǉ��������I�u�W�F�N�g�쐬
        for (int i = 0; i < value; i++)
        {
            // �I�u�W�F�N�g�쐬
            GameObject obj = Instantiate(_dammyHand, transform);
            //���X�g�ɒǉ�
            _dammyHandList.Add(obj.transform);
        }
    }
    /// <summary>
    /// �_�~�[��D���w�薇���폜����
    /// </summary>
    private void RemoveHandObj(int value)
    {
        //�폜�����𐳐��Ŏ擾
        value = Mathf.Abs(value);
        //�폜�������I�u�W�F�N�g�폜
        for (int i = 0; i < value; i++)
        {
            if (_dammyHandList.Count <= 0)
            {
                break;
            }
            // �I�u�W�F�N�g�폜
            Destroy(_dammyHandList[0].gameObject);
            //���X�g����폜
            _dammyHandList.RemoveAt(0);
        }
    }
    /// <summary>
    /// �Y���ԍ��̃_�~�[��D�̍��W��Ԃ�
    /// </summary>
    public Vector2 GetHandPos(int index)
    {
        //�͈͊O��0�ɖ߂�
        if (index < 0 || index >= _dammyHandList.Count)
        {
            return Vector2.zero;
        }
        //�_�~�[��D�̍��W��Ԃ�
        return _dammyHandList[index].transform.position;
    }

    /// <summary>
    /// ���C�A�E�g�̎�������@�\��K�p����
    /// </summary>
    public void ApplyLayout()
    {
        //���񂳂���
        _layoutGroup.CalculateLayoutInputHorizontal();
        _layoutGroup.SetLayoutHorizontal();
    }
}
