using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorControll : MonoBehaviour
{
    private enum State { Open, Exit, Default };
    private State NowState;
    private Vector2 StartPosition;
    [SerializeField, Tooltip("�h�A�̊J�X�s�[�h")]
    private float Speed;
    [SerializeField, Tooltip("�ړ���"),] 
    Vector2 moveTo = new Vector2(0, 4);
    [SerializeField,Tooltip("�I�[�f�B�Isource")]
    AudioSource audioSource = null;
    [SerializeField, Tooltip("�h�A�J��SE")]
    AudioClip DoorSE;

    void Start()
    {
        StartPosition = this.transform.position;
        NowState = State.Default;
    }


    void Update()
    {
        if (NowState == State.Open |NowState==State.Exit)
        {
            StartCoroutine(Move());
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
            PlaySE(DoorSE);
            NowState = State.Open;
        }
        if(collision.gameObject.tag=="Ground")
        {
            NowState = State.Default;
        }
    }
    private IEnumerator Up()//�h�A�㏸
    {
        if (NowState == State.Open)
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPosition + moveTo, 5 * Time.deltaTime);
        }
        yield break;
    }
    private IEnumerator Down()//�h�A���~
    {
        if (NowState == State.Exit)
        {
            transform.position = Vector3.MoveTowards(transform.position, StartPosition, 5 * Time.deltaTime);
        }
        yield break;
    }
    private IEnumerator Move()
    {
        StartCoroutine(Up());
        yield return new WaitForSeconds(3f);
        NowState = State.Exit;
        StartCoroutine(Down());
        NowState = State.Default;
        yield break;
    }
    public void PlaySE(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ肳��ĂȂ�");
        }
    }
}
