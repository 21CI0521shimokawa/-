using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Division_PlayerController : MonoBehaviour
{
    public enum LRMode { Left, Right };   //左右スティックの分岐
    LRMode modeLR;

    float[] L_stickX;
    float[] L_stickY;
    float[] R_stickX;
    float[] R_stickY;
    const int freamCntMax = 8;
    int L_freamCnt;
    int R_freamCnt;
    float moveSpeed;

    Rigidbody2D mini_rigid2D;


    // Start is called before the first frame update
    void Start()
    {
        L_stickX = new float[freamCntMax];
        L_stickY = new float[freamCntMax];
        R_stickX = new float[freamCntMax];
        R_stickY = new float[freamCntMax];
        L_freamCnt = 0;
        R_freamCnt = 0;
        moveSpeed = 10.0f;
        //repelFlag = false;

        mini_rigid2D = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (modeLR)
        {
            case LRMode.Left:  //左スティック移動

                L_stickX[L_freamCnt % freamCntMax] = Input.GetAxis("L_Stick_Horizontal");
                L_stickY[L_freamCnt % freamCntMax] = Input.GetAxis("L_Stick_Vertical");

                if (L_freamCnt >= freamCntMax)
                {
                    Vector2 L_stickVectorNow = new Vector2(L_stickX[L_freamCnt % freamCntMax], L_stickY[L_freamCnt % freamCntMax]);
                    Vector2 L_stickVectorBefore = new Vector2(L_stickX[(L_freamCnt + 1) % freamCntMax], L_stickY[(L_freamCnt + 1) % freamCntMax]);

                    float L_stickVectorNowMagnitude = L_stickVectorNow.magnitude;
                    float L_stickVectorBeforeMagnitude = L_stickVectorBefore.magnitude;

                    if (L_stickVectorNowMagnitude <= 0.1f && L_stickVectorNowMagnitude < L_stickVectorBeforeMagnitude - 0.3f)
                    {
                        L_freamCnt = 0;
                        mini_rigid2D.velocity = new Vector2(-L_stickX[(L_freamCnt + 1) % freamCntMax] * moveSpeed, -L_stickY[(L_freamCnt + 1) % freamCntMax] * moveSpeed);
                    }
                }

                ++L_freamCnt;
                break;
            case LRMode.Right:  //右スティック移動
                R_stickX[R_freamCnt % freamCntMax] = Input.GetAxis("R_Stick_Horizontal");
                R_stickY[R_freamCnt % freamCntMax] = Input.GetAxis("R_Stick_Vertical");

                if (R_freamCnt >= freamCntMax)
                {
                    Vector2 R_stickVectorNow = new Vector2(R_stickX[R_freamCnt % freamCntMax], R_stickY[R_freamCnt % freamCntMax]);
                    Vector2 R_stickVectorBefore = new Vector2(R_stickX[(R_freamCnt + 1) % freamCntMax], R_stickY[(R_freamCnt + 1) % freamCntMax]);

                    float R_stickVectorNowMagnitude = R_stickVectorNow.magnitude;
                    float R_stickVectorBeforeMagnitude = R_stickVectorBefore.magnitude;

                    if (R_stickVectorNowMagnitude <= 0.1f && R_stickVectorNowMagnitude < R_stickVectorBeforeMagnitude - 0.3f)
                    {
                        R_freamCnt = 0;
                        mini_rigid2D.velocity = new Vector2(-R_stickX[(R_freamCnt + 1) % freamCntMax] * moveSpeed, -R_stickY[(R_freamCnt + 1) % freamCntMax] * moveSpeed);
                    }
                }

                ++R_freamCnt;
                break;
        }
    }

    public void Initialize(LRMode mode)
    {
        this.modeLR = mode;
    }
}
