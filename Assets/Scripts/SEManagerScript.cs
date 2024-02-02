//-----------------------------------------------------------------------
/* SE���Ǘ�����N���X
 * 
 * ����ҁ@���c�@�q��
 * ������@12���P��
 *--------------------------------------------------------------------------- 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SEManagerScript : MonoBehaviour
{
    //�ÓI�Q��
    public static SEManagerScript instance { get; private set; }

    //SE�Đ��pAudioSource
    private AudioSource _audioSource = default;
    [SerializeField, Header("�o�^���ʉ��Q�ƃ��X�g")]
    private List<AudioClip> _seClips = default;

    //���ʉ���`���X�g
    public enum SEName
    {
        DecideA,            //�{�^����A
        DecideB,            //�{�^����B
        DamageToEnemy,      //�G�Ƀ_���[�W
        DamageToPlayer,     //�v���C���[�Ƀ_���[�W
        DrawCard,           //�J�[�h�h���[
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void Start()
    {
        //�Q�Ǝ擾
        instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

   /// <summary>
   /// SE�𗬂�
   /// </summary>
   /// <param name="seName">�������̖��O</param>
    public void PlaySE(SEName seName)
    {
        //SE�Đ�
        _audioSource.PlayOneShot(_seClips[(int)seName]);
    }
}
