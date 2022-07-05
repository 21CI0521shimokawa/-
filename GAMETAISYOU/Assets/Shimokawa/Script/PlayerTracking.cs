using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracking : MonoBehaviour
{
    [SerializeField, Tooltip("カメラオブジェクト")] Camera MainCamera;
    [SerializeField, Tooltip("ターゲットの設定")] GameObject TargetPlayer;
    [SerializeField, Tooltip("プレイヤーポジションの取得")] GameObject PlayerPosition;
    [SerializeField, Tooltip("カメラの拡大する時間")] float ExpansionTime = 0.05f;
    private const int LimitExpansionrate = 11;
    private const int IminimumExpansionrate = 9;
    private Vector3 Offset;

    public bool TrackingFlag;

    void Start()
    {
        TrackingFlag = true;
        this.gameObject.transform.position = new Vector3(TargetPlayer.transform.position.x + 3f, TargetPlayer.transform.position.y + 4f, TargetPlayer.transform.position.z - 10);
        Offset = transform.position - TargetPlayer.transform.position;
    }

    void LateUpdate()
    {
        #region 主人公追跡
        if (TrackingFlag)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
            foreach (GameObject obj in objects)
            {
                PlayerPosition = obj;
            }
            if (PlayerPosition != null)
            {
                transform.position = Vector3.Lerp(transform.position, PlayerPosition.transform.position + Offset, 6.0f * Time.deltaTime);
            }
            #endregion
            #region カメラの拡大率
            if (objects.Length > 1)
            {
                MainCamera.orthographicSize = MainCamera.orthographicSize + 0.2f * ExpansionTime;
                if (MainCamera.orthographicSize >= LimitExpansionrate)
                {
                    MainCamera.orthographicSize = 11;
                }
            }
            else
            {
                MainCamera.orthographicSize = MainCamera.orthographicSize - 0.2f * ExpansionTime;
                if (MainCamera.orthographicSize <= IminimumExpansionrate)
                {
                    MainCamera.orthographicSize = 9;
                }
            }
        }
        else
        {
            return;
        }
        #endregion
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tracking")
        {
            TrackingFlag = true;
        }
    }
}
