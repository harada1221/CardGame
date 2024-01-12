//-----------------------------------------------------------------------
/* �J�[�h�]�[����ݒ肷��N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12��20��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoneScript : MonoBehaviour
{
    // �]�[����ޒ�`
    public enum ZoneType
    {
        //��D
        Hand,
        //�v���C�{�[�h0�`4�Ԗ�
        PlayBoard0,
        PlayBoard1,
        PlayBoard2,
        PlayBoard3,
        PlayBoard4,
        //�g���b�V��
        Trash,
    }
    [SerializeField,Header("�]�[���̎��")]
    private ZoneType _zoneType = default;
    public ZoneType GetZoneType{ get => _zoneType; }
}
