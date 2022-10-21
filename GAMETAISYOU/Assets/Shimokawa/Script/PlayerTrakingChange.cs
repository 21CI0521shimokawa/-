using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTrakingChange : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera VirtualCamera; //�r�W���A���J�������擾 
    [SerializeField] GameObject PlayerPosition; //�v���C���[�|�W�V�������擾

    /// <summary>
    /// �X���C�������􂵂����ɐe�X���C���̕��ɒǐՃJ�������A�^�b�`
    /// </summary>
    private void LateUpdate()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            PlayerPosition = obj;
            VirtualCamera.Follow = PlayerPosition.transform;
        }
    }
}
