using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //float stickX;
    //float stickY;
    float[] stickX;
    float[] stickY;
    const int freamCntMax = 8;
    int freamCnt;
    float moveSpeed;
    //bool repelFlag;

    Rigidbody2D rigid2D;

    // Start is called before the first frame update
    void Start()
    {
        //stickX = 0.0f;
        //stickY = 0.0f;
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 10.0f;
        //repelFlag = false;

        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //‘æˆêˆÄ
        //if (TriggerCheck())
        //{
        //    if (ButtonCheck("L3Button"))
        //    {
        //        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        //        stickX = Input.GetAxis("Horizontal");
        //        stickY = Input.GetAxis("Vertical");
        //        repelFlag = true;
        //    }
        //    else if(repelFlag)/*if(rigid2D.velocity.magnitude < 0.01f)*/
        //    {
        //        repelFlag = false;
        //        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        //        rigid2D.velocity = new Vector2(-stickX * moveSpeed, -stickY * moveSpeed);
        //    }
        //}
        //else
        //{
        //    this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        //}

        
        //‘æ“ñˆÄ
        if (TriggerCheck())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            stickX[freamCnt % freamCntMax] = Input.GetAxis("L_Stick_Horizontal");
            stickY[freamCnt % freamCntMax] = Input.GetAxis("L_Stick_Vertical");
                        
            if (freamCnt >= freamCntMax)
            {
                Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);
                Vector2 stickVectorBefore = new Vector2(stickX[(freamCnt + 1) % freamCntMax], stickY[(freamCnt + 1) % freamCntMax]);

                float stickVectorNowMagnitude = stickVectorNow.magnitude;
                float stickVectorBeforeMagnitude = stickVectorBefore.magnitude;

                if (stickVectorNowMagnitude <= 0.1f && stickVectorNowMagnitude < stickVectorBeforeMagnitude - 0.3f)
                {
                    freamCnt = 0;
                    rigid2D.velocity = new Vector2(-stickX[(freamCnt + 1) % freamCntMax] * moveSpeed, -stickY[(freamCnt + 1) % freamCntMax] * moveSpeed);
                }
            }

            ++freamCnt;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }

    private bool TriggerCheck()
    {
        if (Input.GetAxis("L_Trigger") < 0.3) { return false; }
        if (Input.GetAxis("R_Trigger") < 0.3) { return false; }

        return true;
    }

    private bool ButtonCheck(string axisName)
    {
        if(Input.GetAxis(axisName) < 0.3f) { return false; }

        return true;
    }
}
