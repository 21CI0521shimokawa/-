using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTrakingChange : MonoBehaviour
{
    // バーチャルカメラ
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField] SlimeController SlimeDirectoin;
    public GameObject PlayerPosition;
    CinemachineTransposer artViewCameraTransposer;
    public bool IsSwitching;
    // Start is called before the first frame update
    void Start()
    {
        IsSwitching = false;
        artViewCameraTransposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSwitching)
        {
            if (SlimeDirectoin._direction == SlimeController._Direction.Right)
            {
                artViewCameraTransposer.m_FollowOffset.x = 5;
            }
            if (SlimeDirectoin._direction == SlimeController._Direction.Left)
            {
                artViewCameraTransposer.m_FollowOffset.x = -5;
            }
        }
    }
    private void LateUpdate()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            PlayerPosition = obj;
            _virtualCamera.Follow = PlayerPosition.transform;
        }
    }
}
