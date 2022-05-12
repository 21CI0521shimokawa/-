using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControll : MonoBehaviour
{
    private float FadeTime;
    // Start is called before the first frame update
    void Start()
    {
        FadeTime = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        DontDestroyOnLoad(gameObject);
        if(Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if(Input.GetKeyDown(KeyCode.F2))
        {
                //åªç›ÇÃsceneÇéÊìæ
                string SceneName = SceneManager.GetActiveScene().name;
                if (SceneName == "0-1")
                {
                    FadeManager.Instance.LoadScene("0-2", FadeTime);
                    return;
                }
                else if (SceneName == "0-2")
                {
                    FadeManager.Instance.LoadScene("0-3", FadeTime);
                    return;
                }
                else if (SceneName == "0-3")
                {
                    FadeManager.Instance.LoadScene("1-1", FadeTime);
                }
                else if (SceneName == "1-1")
                {
                    FadeManager.Instance.LoadScene("1-2", FadeTime);
                }
                else if (SceneName == "1-2")
                {
                    FadeManager.Instance.LoadScene("2-1", FadeTime);
                }
                else if (SceneName == "2-1")
                {
                    FadeManager.Instance.LoadScene("2-2", FadeTime);
                }
                else if (SceneName == "2-2")
                {
                    FadeManager.Instance.LoadScene("3-1", FadeTime);
                }
                else if (SceneName == "3-1")
                {
                    FadeManager.Instance.LoadScene("3-2", FadeTime);
                }
                else if (SceneName == "3-2")
                {
                    FadeManager.Instance.LoadScene("4-1", FadeTime);
                }
                else if (SceneName == "4-1")
                {
                    FadeManager.Instance.LoadScene("GameClear", FadeTime);
                }
            }
        }
    }
