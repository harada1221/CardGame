//-----------------------------------------------------------------------
/* �o�g���̏������Ǘ�����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManagerScript : MonoBehaviour
{
    //�t�B�[���h�̊Ǘ��N���X
    private FieldAreaManagerScript _fieldAreaScript = default;
    // Start is called before the first frame update
    void Start()
    {
        _fieldAreaScript = GameObject.FindAnyObjectByType<FieldAreaManagerScript>();
        // �Ǘ����R���|�[�l���g������
        _fieldAreaScript.InBattleManager(this);
    }

}
