using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracking : MonoBehaviour
{

    public GameObject PlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        #region 強引なので今後ブラッシュアップ予定
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            PlayerPosition = obj;
        }
        #endregion
        if (PlayerPosition != null)
        {
            transform.position = new Vector3(PlayerPosition.transform.position.x, PlayerPosition.transform.position.y, -10.0f);
        }
    }
}
