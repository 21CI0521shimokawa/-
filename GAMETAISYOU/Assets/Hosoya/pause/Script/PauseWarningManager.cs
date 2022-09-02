using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseWarningManager : MonoBehaviour
{
    enum State { Non, Select }
    State state_;

    enum Select { Yes, No }
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
        select_ = Select.No;

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
                            SceneManager.LoadSceneAsync("PauseScene", LoadSceneMode.Additive);   //‰ÁŽZƒ[ƒh
                            SceneManager.UnloadSceneAsync("PauseWarningScene");
                            break;

                        case Select.Yes:
                            FadeManager.Instance.LoadScene("Title", 1.0f);
                            PauseManager.EndPause();
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

        //‘I‘ð
        {
            float stickY = gamepad.leftStick.ReadValue().y;

            if (stickY > 0.8f && oldStickValueY_ < 0.8f)
            {
                if (select_ != Select.Yes)
                {
                    PlayAudio.PlaySE(SelectionSE);
                    --select_;
                }
            }
            else if (stickY < -0.8f && oldStickValueY_ > -0.8f)
            {
                if (select_ != Select.No)
                {
                    PlayAudio.PlaySE(SelectionSE);
                    ++select_;
                }
            }

            oldStickValueY_ = stickY;
        }

        //Œˆ’è
        if (gamepad.buttonEast.wasPressedThisFrame)
        {
            PlayAudio.PlaySE(decisionSE_);

            return true;
        }

        return false;
    }


    void CursorMove()
    {
        cursor_.rectTransform.anchoredPosition = cursorPosTable_[(int)select_];
    }
}
