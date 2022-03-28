using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracking: MonoBehaviour
{
    [SerializeField, Tooltip("�J�����I�u�W�F�N�g")]
    private Camera camera;
    public GameObject PlayerPosition;
    private float CameraPullingcondition;
    // Start is called before the first frame update
    void Start()
    {
        CameraPullingcondition = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        #region ��l���ǐ�
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            PlayerPosition = obj;
        }
        if (PlayerPosition != null)
        {
            transform.position = new Vector3(PlayerPosition.transform.position.x, PlayerPosition.transform.position.y + 5.0f, -10.0f);
        }
        #endregion
        #region �J�����̊g�嗦
        if(objects.Length>1)
        {
            camera.orthographicSize = 9;
        }
        else 
        {
            camera.orthographicSize = 7;
        }
        #endregion
    }
}
