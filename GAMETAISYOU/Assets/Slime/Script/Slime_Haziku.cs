using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Haziku : MonoBehaviour
{
    public SlimeController slimeController;

    
    #region ‚Í‚¶‚­
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float moveSpeed;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region ‚Í‚¶‚­
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 15.0f;
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeController.hazikuUpdate && !slimeController.slimeBuf.ifTearOff)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            float horizontal = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Horizontal") : Input.GetAxis("R_Stick_Horizontal");
            float vertical = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Vertical") : Input.GetAxis("R_Stick_Vertical");

            stickX[freamCnt % freamCntMax] = horizontal;
            stickY[freamCnt % freamCntMax] = vertical;

            if (freamCnt >= freamCntMax)
            {
                Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);
                if (stickVectorNow.magnitude <= 0.1f)
                {
                    Vector2 stickVectorMost = stickVectorNow;
                    for (int i = 1; i < freamCntMax; ++i)
                    {
                        Vector2 stickVectorCompare = new Vector2(stickX[(freamCnt + i) % freamCntMax], stickY[(freamCnt + i) % freamCntMax]);
                        if (stickVectorMost.magnitude < stickVectorCompare.magnitude)
                        {
                            stickVectorMost = stickVectorCompare;
                        }
                    }

                    //ˆÚ“®
                    if (stickVectorMost.magnitude > stickVectorNow.magnitude + 0.3f)
                    {
                        slimeController.rigid2D.velocity = stickVectorMost * -moveSpeed;
                        freamCnt = 0;
                        slimeController.s_state = State.AIR;
                    }
                }
            }

            ++freamCnt;
        }
    }
}
