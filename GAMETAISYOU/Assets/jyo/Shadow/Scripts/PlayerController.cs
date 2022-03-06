using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public GameObject[] _PlayCharacter;
    public GameObject[] _Fog;

    GameObject m_nowPlay;

    enum STATE{MAIN, SHADOW};
    STATE m_state;

    void Awake()
    {
        if(Instance != null){
            Destroy(gameObject);
            return;
        }
        Instance = this;

        m_state = STATE.MAIN;
    }

    public void SwitchPlayer(){
        switch(m_state){
            case STATE.MAIN:
                _PlayCharacter[0].GetComponent<Player_Main>()._IsNowPlaying = false;
                _PlayCharacter[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                _PlayCharacter[1].GetComponent<Player_Shadow>()._IsNowPlaying = true;
                _PlayCharacter[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                _Fog[0].SetActive(true);
                _Fog[1].SetActive(false);

                Physics2D.gravity *= -1;
                Camera.main.GetComponent<CameraController>()._Player = _PlayCharacter[1];
                m_state = STATE.SHADOW;
            break;

            case STATE.SHADOW:
                _PlayCharacter[0].GetComponent<Player_Main>()._IsNowPlaying = true;
                _PlayCharacter[0].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

                _PlayCharacter[1].GetComponent<Player_Shadow>()._IsNowPlaying = false;
                _PlayCharacter[1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                _Fog[0].SetActive(false);
                _Fog[1].SetActive(true);

                Physics2D.gravity *= -1;
                Camera.main.GetComponent<CameraController>()._Player = _PlayCharacter[0];
                m_state = STATE.MAIN;
            break;
        }
    }
}
