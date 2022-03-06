using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shadow : MonoBehaviour
{
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float m_moveSpeed;

    Rigidbody2D m_rigidbody;

    //----------------------------------------
    public GameObject _Arrow;
    public TimeManager _TimeManager;
    public bool _IsNowPlaying;
    bool m_intoSlowMotion;
    bool m_canCombine;
    //----------------------------------------

    void Start()
    {
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        m_moveSpeed = 15.0f;   

        m_rigidbody = this.gameObject.GetComponent<Rigidbody2D>();

        _IsNowPlaying = false;
    }

    void Update()
    {
        if(!_IsNowPlaying) return;

        Launch();
        Combine();
    }

    void Launch(){
        #region はじく
        if (TriggerCheck())
        {
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
                        m_rigidbody.velocity = stickVectorMost * -m_moveSpeed;
                        freamCnt = 0;

                        //----------------------------------------スローモーション、分裂
                        _Arrow.SetActive(false);
                        _TimeManager._IsNormalSpeed = true;
                        //----------------------------------------
                    }
                }
            }

            ++freamCnt;
        }
        else
        {
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

    void Combine(){
        if(!m_canCombine) return;
        
        if(Input.GetButtonDown("Submit")){
            PlayerController.Instance.SwitchPlayer();
        }
    }

    private bool TriggerCheck()
    {
        if (Input.GetAxis("L_Trigger") < 0.3) { return false; }
        if (Input.GetAxis("R_Trigger") < 0.3) { return false; }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "DivideArea"){
            m_canCombine = true;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "DivideArea"){
            m_canCombine = false;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
