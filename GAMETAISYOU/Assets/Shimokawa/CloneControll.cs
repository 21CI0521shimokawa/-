using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneControll : MonoBehaviour
{
    float[] stickX;
    float[] stickY;
    const int freamCntMax = 8;
    int freamCnt;
    float moveSpeed;

    Rigidbody2D rigid2D;

    public CameraShake shake;
    // Start is called before the first frame update
    void Start()
    {
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 20.0f;

        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
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
}
