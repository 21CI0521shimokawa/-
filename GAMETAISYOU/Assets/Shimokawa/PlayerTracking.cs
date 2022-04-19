using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracking : MonoBehaviour
{
    [SerializeField, Tooltip("カメラオブジェクト")]
    private Camera camera;
    public GameObject PlayerPosition;
    private float CameraPullingcondition;
    public float ExpansionTime = 0.05f;

    private const int LimitExpansionrate = 9;
    private const int IminimumExpansionrate = 7;

    private GameObject player;
    private Vector3 offset;

    void Start()
    {
        CameraPullingcondition = 1.0f;
        player = GameObject.Find("Slime");
        this.gameObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 10);
        offset = transform.position - player.transform.position;

    }

    void LateUpdate()
    {
        #region 主人公追跡
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            PlayerPosition = obj;
        }
        if (PlayerPosition != null)
        {
            // transform.position = new Vector3(PlayerPosition.transform.position.x, PlayerPosition.transform.position.y + 4.0f, -10.0f);
            transform.position = Vector3.Lerp(transform.position, PlayerPosition.transform.position + offset, 6.0f * Time.deltaTime);
        }
        #endregion
        #region カメラの拡大率
        if (objects.Length > 1)
        {
            camera.orthographicSize = camera.orthographicSize + 0.2f * ExpansionTime;
            if (camera.orthographicSize >= LimitExpansionrate)
            {
                camera.orthographicSize = 9;
            }
        }
        else
        {
            camera.orthographicSize = camera.orthographicSize - 0.2f * ExpansionTime;
            if (camera.orthographicSize <= IminimumExpansionrate)
            {
                camera.orthographicSize = 7;
            }
        }
        #endregion
    }
}
