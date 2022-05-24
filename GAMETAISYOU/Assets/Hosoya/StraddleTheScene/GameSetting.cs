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

        //�J�[�\����\��
#if !UNITY_EDITOR
        Cursor.visible = false;
#endif

        //���̃I�u�W�F�N�g���V�[�����ׂ���悤��
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���I��
        GameEnd();
    }

    //�d���`�F�b�N
    bool IsDuplicate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameSetting");

        //�d�����Ă�����(��ȏ㌟�m������)true
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
