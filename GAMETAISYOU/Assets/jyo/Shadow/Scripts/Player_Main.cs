using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    public GameObject _Shadow;
    public float _MoveSpeed;
    public float _DivitionTime;
    public bool _IsNowPlaying;

    enum STATE { NORMAL, DIVIDE, DEAD };
    STATE m_state;

    //入力
    Vector2 m_stickL;
    Vector2 m_stickR;
    float m_triggerL;
    float m_triggerR;
    

    float m_timer;
    bool m_canDivide;

    void Start()
    {
        m_state = STATE.NORMAL;
        _IsNowPlaying = true;
    }

    void Update()
    {
        if(!_IsNowPlaying) return;

        GetInput();
        StateUpdate();

        //適当のもの
        if(m_canDivide){
            _Shadow.SetActive(true);
            _Shadow.GetComponent<SpriteRenderer>().color = Color.black;
            _Shadow.transform.position = new Vector3(transform.position.x, _Shadow.transform.position.y, _Shadow.transform.position.z);
        }else{
            _Shadow.SetActive(false);
        }
    }

    void GetInput(){
        m_stickL = new Vector2(Input.GetAxis("L_Stick_Horizontal"), Input.GetAxis("L_Stick_Vertical"));
        m_stickR = new Vector2(Input.GetAxis("R_Stick_Horizontal"), Input.GetAxis("R_Stick_Vertical"));
        m_triggerL = Input.GetAxis("L_Trigger");
        m_triggerR = Input.GetAxis("R_Trigger");
    }

    void StateUpdate(){
        switch(m_state){
            case STATE.NORMAL:
                Movement();

                if(CheckedTrigger() && m_canDivide)
                    m_state = STATE.DIVIDE;
            break;
            case STATE.DIVIDE:
                if(CheckedStick()){
                    m_timer += Time.deltaTime;
                    Camera.main.orthographicSize -= Time.deltaTime;
                    if(m_timer >= _DivitionTime){
                        m_timer = 0;
                        m_state = STATE.NORMAL;
                        Camera.main.orthographicSize = 7;
                        PlayerController.Instance.SwitchPlayer();
                    }
                }else{
                    m_timer = 0;
                    Camera.main.orthographicSize = 7;
                }

                if(!CheckedTrigger() || !m_canDivide){
                    m_timer = 0;
                    Camera.main.orthographicSize = 7;
                    m_state = STATE.NORMAL;
                }
            break;
            case STATE.DEAD:
            break;
        }
    }

    void Movement(){
        transform.position += transform.right * _MoveSpeed * m_stickL.x * Time.deltaTime;
    }

    bool CheckedTrigger(){
        return m_triggerL == 1 && m_triggerR == 1;
    }

    bool CheckedStick(){
        return m_stickL.x < 0 && m_stickR.x > 0; 
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "DivideArea"){
            m_canDivide = true;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "DivideArea"){
            m_canDivide = false;
            GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
