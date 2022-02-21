using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combine_PlayerController : MonoBehaviour
{
    //はじく===================================
    //float stickX;
    //float stickY;
    float[] stickX;
    float[] stickY;
    const int freamCntMax = 8;
    int freamCnt;
    float moveSpeed;
    //bool repelFlag;

    Rigidbody2D rigid2D;


    //ちぎる===================================
    public float divisionTime;    //分裂するまでの時間
    public GameObject mini;

    float power;    //かかっている力



    // Start is called before the first frame update
    void Start()
    {
        //はじく
        //stickX = 0.0f;
        //stickY = 0.0f;
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 10.0f;
        //repelFlag = false;

        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();


        //ちぎる
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //トリガーが押されているか確認
        if (IfLRTriggerOn())
        {
            //ちぎる

            float stickLHorizontal = Input.GetAxis("L_Stick_Horizontal");
            float stickRHorizontal = Input.GetAxis("R_Stick_Horizontal");

            if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
            {
                //Debug.Log("引っ張ってるよ:" + stickLHorizontal + "," + stickRHorizontal);

                power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //かかる力を増やす

                if (power > (-stickLHorizontal + stickRHorizontal) / 2)
                {
                    power = (-stickLHorizontal + stickRHorizontal) / 2;
                }

                transform.localScale = new Vector2(1 + power, 1.0f / (1 + power));

                if (power >= 1)
                {
                    Debug.Log("きれたよ");

                    GetComponent<Renderer>().material.color = Color.red;

                    //右
                    {
                        Vector3 position = new Vector2((float)(transform.position.x + 0.8), transform.position.y);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;
                    }

                    //左
                    {
                        Vector3 position = new Vector2((float)(transform.position.x - 0.8), transform.position.y);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;
                    }

                    //自身を破壊
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("きれてないよ:" + power);

                    GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
            {
                Debug.Log("つかんだよ");

                power = 0;

                transform.localScale = new Vector2(1, 1);

                GetComponent<Renderer>().material.color = Color.green;
            }

        }
        else
        {
            //はじく
            Debug.Log("つかんでないよ");

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
    }


    //両方のトリガーが押されているか確認
    bool IfLRTriggerOn()
    {
        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        return triggerL == 1 && triggerR == 1;
    }

}
