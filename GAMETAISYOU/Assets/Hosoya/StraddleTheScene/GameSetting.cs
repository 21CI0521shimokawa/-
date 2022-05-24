using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameSetting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(IsDuplicate())
        {
            Destroy(gameObject);
        }

        //カーソル非表示
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

        //このオブジェクトがシーンを跨げるように
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //ゲーム終了
        GameEnd();
    }

    //重複チェック
    bool IsDuplicate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameSetting");

        //重複していたら(二つ以上検知したら)true
        return gameObjects.Length >= 2;
    }

    void GameEnd()
    {
        Keyboard keyboard = Keyboard.current;

        if(keyboard != null)
        {
            if (keyboard.escapeKey.isPressed)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
    }
}
