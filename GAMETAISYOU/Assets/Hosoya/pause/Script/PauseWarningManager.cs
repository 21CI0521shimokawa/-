using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWarningManager : MonoBehaviour
{
    static bool isPause_ = false;

    enum State { Non, Select }
    State state_;

    enum Select { Yes, No }
    Select select_;

    float oldStickValueY_;

    [SerializeField] Image cursor_;
    [SerializeField] Vector3[] cursorPosTable_;

    [SerializeField] AudioClip selectSE_;

    // Start is called before the first frame update
    void Start()
    {
        state_ = State.Select;
        select_ = Select.No;
        Time.timeScale = 0.0f;
        oldStickValueY_ = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state_)
        {
            case State.Select:
                if (SelectUpdate())
                {
                    Debug.Log(select_);
                    switch (select_)
                    {
                        case Select.No:
                            SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);   //加算ロード
                            SceneManager.UnloadSceneAsync("PauseWarningScene");
                            break;

                        case Select.Yes:
                            FadeManager.Instance.LoadScene("Title", 1.0f);
                            EndPause();
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
        if (gamepad == null)
        {
            return false;
        }

        //選択
        {
            float stickY = gamepad.leftStick.ReadValue().y;

            if (stickY > 0.8f && oldStickValueY_ < 0.8f)
            {
                if (select_ != Select.Yes)
                {
                    --select_;
                }
            }
            else if (stickY < -0.8f && oldStickValueY_ > -0.8f)
            {
                if (select_ != Select.No)
                {
                    ++select_;
                }
            }

            oldStickValueY_ = stickY;
        }

        //決定
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            PlayAudio.PlaySE(selectSE_);

            return true;
        }

        return false;
    }

    void EndPause()
    {
        if (isPause_)
        {
            isPause_ = false;
            Time.timeScale = 1.0f;
            SceneManager.UnloadSceneAsync("PauseScene");   //アンロード
            SceneManager.UnloadSceneAsync("PauseWarningScene");

            Resources.UnloadUnusedAssets(); //未使用アセットの解放
        }
    }


    void CursorMove()
    {
        cursor_.rectTransform.anchoredPosition = cursorPosTable_[(int)select_];
    }
}
