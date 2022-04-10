using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Haziku : MonoBehaviour
{
    public SlimeController slimeController;

    
    #region はじく
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float moveSpeed;
    #endregion

    public bool _ifSlimeHazikuNow;  //はじくのアップデートを行っている最中かどうか

    // Start is called before the first frame update
    void Start()
    {
        #region はじく
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 15.0f;
        #endregion

        _ifSlimeHazikuNow = false;
    }

    // Update is called once per frame
    void Update()
    {
        Gamepad gamepad = Gamepad.current;

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        float horizontal = 0.0f;
        float vertical = 0.0f;

        //ゲームパッドが接続されているなら
        if (!(gamepad == null))
        {
            Vector2 stickValue = slimeController.modeLR == SlimeController.LRMode.Left ? gamepad.leftStick.ReadValue() : gamepad.rightStick.ReadValue();

            horizontal = stickValue.x;
            vertical = stickValue.y;

            //horizontal = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Horizontal") : Input.GetAxis("R_Stick_Horizontal");
            //vertical = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Vertical") : Input.GetAxis("R_Stick_Vertical");
        }
        else
        {
            Debug.Log("コントローラーが接続されていません");
        }

        if (slimeController.hazikuUpdate && !slimeController.slimeBuf.ifTearOff)
        {
            stickX[freamCnt % freamCntMax] = horizontal;
            stickY[freamCnt % freamCntMax] = vertical;

            if (freamCnt >= freamCntMax)
            {
                Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);
                if (stickVectorNow.magnitude <= 0.1f)
                {
                    _ifSlimeHazikuNow = false;

                    Vector2 stickVectorMost = stickVectorNow;
                    for (int i = 1; i < freamCntMax; ++i)
                    {
                        Vector2 stickVectorCompare = new Vector2(stickX[(freamCnt + i) % freamCntMax], stickY[(freamCnt + i) % freamCntMax]);
                        if (stickVectorMost.magnitude < stickVectorCompare.magnitude)
                        {
                            stickVectorMost = stickVectorCompare;
                        }
                    }

                    //移動
                    if (stickVectorMost.magnitude > stickVectorNow.magnitude + 0.3f)
                    {
                        slimeController._SlimeAnimator.SetTrigger("Haziku");

                        slimeController.rigid2D.velocity = stickVectorMost * -moveSpeed;
                        freamCnt = 0;
                        slimeController.s_state = State.AIR;

                        for (int i = 0; i < freamCntMax; ++i)
                        {
                            stickX[i] = 0.0f;
                            stickY[i] = 0.0f;
                        }

                        _ifSlimeHazikuNow = false;
                    }
                }
                else
                {
                    _ifSlimeHazikuNow = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < freamCntMax; ++i)
            {
                stickX[i] = 0.0f;
                stickY[i] = 0.0f;
            }

            _ifSlimeHazikuNow = false;
        }

        ++freamCnt;
    }
}
