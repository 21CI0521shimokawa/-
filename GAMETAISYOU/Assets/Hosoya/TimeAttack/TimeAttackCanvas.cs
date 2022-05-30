using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAttackCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (IsDuplicate())
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //重複チェック
    bool IsDuplicate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameSetting");

        //重複していたら(二つ以上検知したら)true
        return gameObjects.Length >= 2;
    }
}
