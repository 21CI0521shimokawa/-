using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTrakingChange : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera VirtualCamera; //ビジュアルカメラを取得 
    [SerializeField] GameObject PlayerPosition; //プレイヤーポジションを取得

    /// <summary>
    /// スライムが分裂した時に親スライムの方に追跡カメラをアタッチ
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
