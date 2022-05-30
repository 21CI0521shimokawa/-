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

    //�d���`�F�b�N
    bool IsDuplicate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameSetting");

        //�d�����Ă�����(��ȏ㌟�m������)true
        return gameObjects.Length >= 2;
    }
}
