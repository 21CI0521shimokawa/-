using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTrakingChange : MonoBehaviour
{
    // バーチャルカメラ
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;
    public GameObject PlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
