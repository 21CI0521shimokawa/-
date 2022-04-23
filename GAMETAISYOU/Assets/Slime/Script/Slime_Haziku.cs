using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Haziku : MonoBehaviour
{
    public SlimeController slimeController;
    [SerializeField] GameObject ArrowPrefab;
    [SerializeField] LineRenderer lineRenderer;

    #region はじく
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float moveSpeed;
    #endregion

    public float _power;    //パワー

    GameObject arrow;

    public bool _ifSlimeHazikuNow;  //はじくのアップデートを行っている最中かどうか
    bool isArrowBeing;  //矢印が存在するかどうか

    [SerializeField] float coolTime;    //はじいた後何秒間はじくことができないか
    public float _nextHazikuUpdateTime;  //はじくアップデートを行うことができるようになる時間

    public float _stateFixationTime;    //はじいた後StateをAIRで固定する時間（不具合対策）

    enum GuideType
    { 
        Non,
        arrow,
        Ray
    };


    [SerializeField] GuideType guidetype;

    // Start is called before the first frame update
    void Start()
    {
        #region はじく
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 3.0f;
        #endregion

        _ifSlimeHazikuNow = false;
        isArrowBeing = false;

        _nextHazikuUpdateTime = 0.0f;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 1, 1, 1);
        lineRenderer.endColor = new Color(1, 1, 1, 0);
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

        if (slimeController.hazikuUpdate && !slimeController._slimeBuf._ifTearOff)
        {
            stickX[freamCnt % freamCntMax] = horizontal;
            stickY[freamCnt % freamCntMax] = vertical;

            if (freamCnt >= freamCntMax)
            {
                Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);

                #region ガイド
                if(guidetype == GuideType.arrow)
                {
                    Vector2 currentVector = new Vector2(horizontal, vertical);
                    float radian = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg + 90;
                    if (radian < 0) radian += 360;
                    if (currentVector.magnitude > 0.3f)
                    {
                        GuideInstanceate();
                        arrow.transform.position = transform.position;
                        arrow.transform.rotation = Quaternion.Euler(0, 0, radian);
                        arrow.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax * currentVector.magnitude * 5);
                    }
                    else
                    {
                        _GuideDestroy();
                    }
                }
                #endregion

                #region ガイド2
                if (guidetype == GuideType.Ray)
                {
                    Vector2 currentVector = new Vector2(horizontal, vertical);
                    if (currentVector.magnitude > 0.3f)
                    {
                        GuideInstanceate();
                        lineRenderer.SetPosition(0, this.gameObject.transform.position);
                        lineRenderer.SetPosition(1, new Vector2(this.gameObject.transform.position.x + currentVector.normalized.x * -3, this.gameObject.transform.position.y + currentVector.normalized.y * -3));
                    }
                    else
                    {
                        _GuideDestroy();
                    }
                }
                #endregion

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

                        slimeController.rigid2D.velocity = stickVectorMost * -moveSpeed * _power;
                        freamCnt = 0;
                        slimeController.s_state = State.AIR;

                        for (int i = 0; i < freamCntMax; ++i)
                        {
                            stickX[i] = 0.0f;
                            stickY[i] = 0.0f;
                        }

                        _ifSlimeHazikuNow = false;

                        _nextHazikuUpdateTime = slimeController.liveTime + coolTime;
                        _stateFixationTime = slimeController.liveTime + 0.03f;
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
            _GuideDestroy();
        }

        ++freamCnt;
    }


    //ガイドの生成
    private void GuideInstanceate()
    {
        switch (guidetype)
        {
            case GuideType.arrow:
                if (!isArrowBeing)
                {
                    isArrowBeing = true;

                    ArrowPrefab.SetActive(true);
                    GameObject buf = GameObject.Instantiate(ArrowPrefab);
                    arrow = buf;
                }
                break;

            case GuideType.Ray:
                lineRenderer.enabled = true;
                break;
        }
    }

    //ガイドの破棄
    public void _GuideDestroy()
    {
        switch (guidetype)
        {
            case GuideType.arrow:
                if (isArrowBeing)
                {
                    isArrowBeing = false;

                    arrow.SetActive(false);
                    Destroy(arrow);
                }
                break;

            case GuideType.Ray:
                lineRenderer.enabled = false;
                break;
        }
    }
}
