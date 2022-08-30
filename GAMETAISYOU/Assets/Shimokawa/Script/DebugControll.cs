using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControll : MonoBehaviour
{
    private float FadeTime;
    void Start()
    {
        if (IsDuplicate())
        {
            Destroy(gameObject);
        }

        FadeTime = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);

        //���݂̃V�[������蒼��
        if(Input.GetKeyDown(KeyCode.F1))
        {
            //���݂�scene���擾
            string SceneName = SceneManager.GetActiveScene().name;
            FadeManager.Instance.LoadScene(SceneName, FadeTime);
        }
        //��i��
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            //���݂�scene���擾
            string SceneName = SceneManager.GetActiveScene().name;
            if (SceneName == "Title")
            {
                FadeManager.Instance.LoadScene("S0-1", FadeTime);
                return;
            }
            if (SceneName == "S0-1")
            {
                FadeManager.Instance.LoadScene("S0-2", FadeTime);
                return;
            }
            else if (SceneName == "S0-2")
            {
                FadeManager.Instance.LoadScene("S0-3", FadeTime);
                return;
            }
            else if (SceneName == "S0-3")
            {
                FadeManager.Instance.LoadScene("S1-1", FadeTime);
            }
            else if (SceneName == "S1-1")
            {
                FadeManager.Instance.LoadScene("S1-2", FadeTime);
            }
            else if (SceneName == "S1-2")
            {
                FadeManager.Instance.LoadScene("S1-3", FadeTime);
            }
            else if (SceneName == "S1-3")
            {
                FadeManager.Instance.LoadScene("S2-1", FadeTime);
            }
            else if (SceneName == "S2-1")
            {
                FadeManager.Instance.LoadScene("S2-2", FadeTime);
            }
            else if (SceneName == "S2-2")
            {
                FadeManager.Instance.LoadScene("S2-3", FadeTime);
            }
            else if (SceneName == "S2-3")
            {
                FadeManager.Instance.LoadScene("S2-4", FadeTime);
            }
            else if (SceneName == "S2-4")
            {
                FadeManager.Instance.LoadScene("S3-1", FadeTime);
            }
            else if (SceneName == "S3-1")
            {
                FadeManager.Instance.LoadScene("S3-2", FadeTime);
            }
            else if (SceneName == "S3-2")
            {
                FadeManager.Instance.LoadScene("S3-3", FadeTime);
            }
            else if (SceneName == "S3-3")
            {
                FadeManager.Instance.LoadScene("S3-4", FadeTime);
            }
            else if (SceneName == "S3-4")
            {
                FadeManager.Instance.LoadScene("S4-1", FadeTime);
            }
            else if (SceneName == "S4-1")
            {
                FadeManager.Instance.LoadScene("GameClear", FadeTime+2f);
            }
            else if(SceneName=="TGS-1")
            {
                FadeManager.Instance.LoadScene("TGS-2", FadeTime);
            }
        }
    }

    //�d���`�F�b�N
    bool IsDuplicate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameSetting");

        //�d�����Ă�����(��ȏ㌟�m������)true
        return gameObjects.Length >= 2;
    }
}
