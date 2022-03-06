using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region はじく
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float moveSpeed;

    Rigidbody2D rigid2D;
    #endregion

    //----------------------------------------
    public GameObject _Arrow;
    //public GameObject _DivideBody;
    public TimeManager _TimeManager;
    public float _RecoveryCT;
    public float _LimitedTime;

    float m_timer;
    bool m_intoSlowMotion;
    [SerializeField] bool m_isGround;


    //----------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        #region はじく
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 15.0f;

        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isGround)
        {
            SizeRecovery();
        }

        #region はじく
        if (TriggerCheck() && SizeCheck())
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
            float horizontal = Input.GetAxis("L_Stick_Horizontal");
            float vertical = Input.GetAxis("L_Stick_Vertical");
            stickX[freamCnt % freamCntMax] = horizontal;
            stickY[freamCnt % freamCntMax] = vertical;

            //----------------------------------------スローモーション
            if (m_intoSlowMotion)
            {
                m_intoSlowMotion = false;
                _TimeManager.DoSlowMotion();
                _TimeManager._IsNormalSpeed = false;
            }
            //----------------------------------------

            //----------------------------------------ガイド
            Vector2 currentVector = new Vector2(horizontal, vertical);
            float radian = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg + 90;
            if (radian < 0) radian += 360;
            if (currentVector.magnitude > 0.3f)
            {
                _Arrow.SetActive(true);
                _Arrow.transform.position = transform.position;
                _Arrow.transform.rotation = Quaternion.Euler(0, 0, radian);
            }
            //----------------------------------------


            if (freamCnt >= freamCntMax)
            {
                /*  Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);
                  Vector2 stickVectorBefore = new Vector2(stickX[(freamCnt + 1) % freamCntMax], stickY[(freamCnt + 1) % freamCntMax]);

                  float stickVectorNowMagnitude = stickVectorNow.magnitude;
                  float stickVectorBeforeMagnitude = stickVectorBefore.magnitude;


                  if (stickVectorNowMagnitude <= 0.1f && stickVectorNowMagnitude < stickVectorBeforeMagnitude - 0.3f)
                  {
                      rigid2D.velocity = new Vector2(-stickX[(freamCnt + 1) % freamCntMax] * moveSpeed, -stickY[(freamCnt + 1) % freamCntMax] * moveSpeed);
                      freamCnt = 0;

                      //----------------------------------------スローモーション、分裂
                      _Arrow.SetActive(false);
                     // GameObject obj = Instantiate(_DivideBody, transform.position , Quaternion.identity);
                      //obj.GetComponent<Rigidbody2D>().velocity = new Vector2(stickX[(freamCnt + 1) % freamCntMax] * moveSpeed, stickY[(freamCnt + 1) % freamCntMax] * moveSpeed) * 2;
                      _TimeManager._IsNormalSpeed = true;
                      transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
                      //----------------------------------------
                  }*/
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

                    if (stickVectorMost.magnitude > stickVectorNow.magnitude + 0.3f)
                    {
                        _TimeManager._IsNormalSpeed = true;
                        rigid2D.velocity = stickVectorMost * -moveSpeed;
                        freamCnt = 0;

                        //----------------------------------------スローモーション、分裂
                        _Arrow.SetActive(false);
                        //GameObject obj = Instantiate(_DivideBody, transform.position , Quaternion.identity);
                        //obj.GetComponent<Rigidbody2D>().velocity = new Vector2(stickX[(freamCnt + 1) % freamCntMax] * moveSpeed, stickY[(freamCnt + 1) % freamCntMax] * moveSpeed) * 2;
                        _TimeManager._IsNormalSpeed = true;
                        transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, transform.localScale.z - 0.1f);
                        //----------------------------------------
                    }
                }
            }

            ++freamCnt;
        }
        else
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0.5f, 1);
            for (int i = 0; i < stickX.Length; ++i)
            {
                stickX[i] = 0;
                stickY[i] = 0;
            }

            //----------------------------------------スローモーション
            _Arrow.SetActive(false);
            _TimeManager._IsNormalSpeed = true;
            m_intoSlowMotion = true;
            //----------------------------------------
        }
        #endregion 
    }

    #region はじく
    private bool TriggerCheck()
    {
        if (Input.GetAxis("L_Trigger") < 0.3) { return false; }
        if (Input.GetAxis("R_Trigger") < 0.3) { return false; }

        return true;
    }
    #endregion

    bool SizeCheck()
    {
        if (transform.localScale.x < 0.5f)
        {
            return false;
        }
        return true;
    }

    void SizeRecovery()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            if (transform.localScale.x < 1)
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            m_timer = _RecoveryCT;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Target")
        {
            //transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            //Instantiate(_DivideBody, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            m_isGround = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground")
            m_isGround = false;
    }
}
