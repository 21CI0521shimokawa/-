using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSticking : MonoBehaviour
{
    #region はじく変数宣言
    //float stickX;
    //float stickY;
    private float[] stickX;
    private float[] stickY;
    const int freamCntMax = 8;
    private int freamCnt;
    private float moveSpeed;
    //bool repelFlag;

    private Rigidbody2D rigid2D;
    #endregion


    #region ちぎる変数宣言
    [SerializeField, Tooltip("分裂するまでの時間")]
    private float divisionTime;
    [SerializeField, Tooltip("分裂したオブジェクト")]
    private GameObject mini;

    private float power;//かかっている力
    #endregion

    #region スライム関連 変数宣言
    [SerializeField, Tooltip("ステータス")]
    private State s_state;
    #endregion



    #region Unityfunction
    void Start()
    {
        //はじく
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 20.0f;
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();
        //ちぎる
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (s_state)
        {
            case State.DIVISION:
                DivisionMove();
                break;
            case State.MOVE:
                RepelMove();
                break;
            case State.STOP:
                break;
            case State.ATTACK:
                break;
            case State.AIR:
                break;
        }

        #region 仮スライムステータス変化
        StateChange();
        #endregion
    }
    #endregion

    #region private function
    //両方のトリガーが押されているか確認
    private bool IfLRTriggerOn()
    {
        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        return triggerL == 1 && triggerR == 1;
    }
    private void StateChange()
    {
        if (IfLRTriggerOn())
        {
            s_state = State.DIVISION;
        }
    }
    private void DivisionMove()
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
                        cloneObject.AddComponent<CloneControll>();
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
            s_state = State.MOVE;
        }
    }
    private void RepelMove()
    {
        //はじく
        Debug.Log("つかんでないよ");

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        stickX[freamCnt % freamCntMax] = Input.GetAxis("L_Stick_Horizontal");
        stickY[freamCnt % freamCntMax] = Input.GetAxis("L_Stick_Vertical");

        if (freamCnt >= freamCntMax)
        {
            Debug.Log("入力されています");
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {   
            Vector2 velocityCopy = rigid2D.velocity;
            float vectorCos = Vector2.Dot(new Vector2(0.0f, 1.0f), velocityCopy);

            if(rigid2D.velocity.magnitude > 2.5f && vectorCos < -0.95f)
            {
                
                velocityCopy.Normalize();
                rigid2D.velocity = velocityCopy * 4.0f;
            }

            this.s_state = State.MOVE;
        }
        else if(collision.gameObject.tag == "Ground")
        {
            this.s_state = State.MOVE;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            this.s_state = State.AIR;
        }
        if(collision.gameObject.tag == "Ground")
        {
            this.s_state = State.AIR;
        }
    }
    #endregion
    #region public function
    #endregion
}
