using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    static bool isPause_ = false;

    enum State { Non, Select }
    State state_;

    enum Select { Continue, Retry, Title }
    Select select_;

    float oldStickValueY_;

    [SerializeField] Image cursor_;
    [SerializeField] Vector3[] cursorPosTable_;

    [SerializeField] AudioClip decisionSE_;

    [SerializeField] AudioClip SelectionSE;
    [SerializeField] AudioClip StartSE;

    // Start is called before the first frame update
    void Start()
    {
        state_ = State.Select;
        select_ = Select.Continue;

        oldStickValueY_ = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state_)
        {
            case State.Select:
                if(SelectUpdate())
                {
                    Debug.Log(select_);
                    switch (select_)
                    {
                        case Select.Continue:
                            EndPause();
                            break;

                        case Select.Retry:
                            FadeManager.Instance.LoadScene(SceneManager.GetActiveScene().name, 1.0f);
                            EndPause();
                            break;

                        case Select.Title:
                            SceneManager.LoadSceneAsync("PauseWarningScene", LoadSceneMode.Additive);   //加算ロード
                            SceneManager.UnloadSceneAsync("PauseScene");
                            break;
                    }
                }
                break;
        }

        CursorMove();
    }

    bool SelectUpdate()
    {
        Gamepad gamepad = Gamepad.current;
        if(gamepad == null)
        {
            return false;
        }

        //選択
        {
            float stickY = gamepad.leftStick.ReadValue().y;

            if(stickY > 0.8f && oldStickValueY_ < 0.8f)
            {
                if (select_ != Select.Continue)
                {
                    PlayAudio.PlaySE(SelectionSE);
                    --select_;
                }
            }
            else if(stickY < -0.8f && oldStickValueY_ > -0.8f)
            {
                if (select_ != Select.Title)
                {
                    PlayAudio.PlaySE(SelectionSE);
                    ++select_;
                }
            }

            oldStickValueY_ = stickY;
        }

        //決定
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            PlayAudio.PlaySE(decisionSE_);

            return true;
        }

        return false;
    }

    public static void EndPause()
    {
        if(isPause_)
        {
            isPause_ = false;
            Time.timeScale = 1.0f;

            //現在読み込まれているシーン数だけループ
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if(SceneManager.GetSceneAt(i).name == "PauseWarningScene")
                {
                    SceneManager.UnloadSceneAsync("PauseWarningScene");   //アンロード
                }
                else if(SceneManager.GetSceneAt(i).name == "PauseScene")
                {
                    SceneManager.UnloadSceneAsync("PauseScene");   //アンロード
                }
            }     

            Resources.UnloadUnusedAssets(); //未使用アセットの解放
        }
    }


    void CursorMove()
    {
        cursor_.rectTransform.anchoredPosition = cursorPosTable_[(int)select_];
    }


    public static void Pause()
    {
        //ポーズ中でなかったら
        if(!isPause_)
        {
            isPause_ = true;
            SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);   //加算ロード
            Time.timeScale = 0.0f;
        }
    }
}
