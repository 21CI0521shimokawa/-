using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorControll : MonoBehaviour
{
    private enum State { Open, Exit, Default };
    private Vector3 NowPosition;
    private Vector3 StartPositon;
    private State NowState;

    [SerializeField, Tooltip("�h�A�̊J�X�s�[�h")]
    private float Speed;
    void Start()
    {
        NowState = State.Default;
        StartPositon.y =this.transform.position.y;
    }


    void Update()
    {
        switch (NowState)
        {
            case State.Default:
                //�������Ȃ�
                break;
            case State.Open:
                //�h�A�I�[�v��
                while (NowPosition.y < 11.3f)
                {
                    NowPosition = transform.position;
                    transform.Translate(0, Speed, 0);
                }
                NowState = State.Default;
                    break;
            case State.Exit:
                while (NowPosition.y > StartPositon.y)
                {
                    NowPosition = transform.position;
                    transform.Translate(0, -Speed, 0);
                }
                break;
        }
        Debug.Log(NowState);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "Slime")
        {
            NowState = State.Open;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
            NowState = State.Exit;
        }
    }
}
