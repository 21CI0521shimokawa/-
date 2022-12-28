using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugController : MonoBehaviour
{
    private float FadeTime;

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x����Start�֐�����Ă΂��֐�
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(instance);
    }

    /// <summary>
    /// �Q�[�����n�܂鎞�Ɉ�x�����Ă΂��֐�
    /// </summary>
    void Start()
    {
        if (IsDuplicate())
        { DontDestroyOnLoad(instance);
            Destroy(gameObject);
        }

        FadeTime = 1.0f;

    }

    /// <summary>
    /// ���t���[���Ă΂��֐�
    /// </summary>
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1)) //���݂̃V�[������蒼��
        {
            string SceneName = SceneManager.GetActiveScene().name;//���݂�scene���擾
            FadeManager.Instance.LoadScene(SceneName, FadeTime);
        }
        else if (Input.GetKeyDown(KeyCode.F2)) //��V�[����i�܂���
        {
            string SceneName = SceneManager.GetActiveScene().name; //���݂�scene���擾
            if (SceneName == "Title")
            {
                FadeManager.Instance.LoadScene("TGS-1", FadeTime);
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
        else if(Input.GetKeyDown(KeyCode.F3)) //�ЂƂO�ɖ߂�
        {
            string SceneName = SceneManager.GetActiveScene().name;
            if(SceneName=="TGS-2")
            {
                FadeManager.Instance.LoadScene("TGS-1", FadeTime);
            }
        }
    }

    /// <summary>
    /// �d���`�F�b�N
    /// </summary>
    /// <returns></returns>
    bool IsDuplicate()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("GameSetting");
        return gameObjects.Length >= 2; //�d�����Ă�����(��ȏ㌟�m������)true
    }
}
