using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoorControll : MonoBehaviour
{
    private enum State { Open, Exit, Default };
    private State NowState;
    private Vector2 StartPosition;
    private bool StayFlag;
    private int PlaySECnt;
    [SerializeField, Tooltip("�h�A�̊J�X�s�[�h")]
    private float Speed;
    [SerializeField, Tooltip("�ړ���"),]
    Vector2 moveTo = new Vector2(0, 4);
    [SerializeField, Tooltip("�I�[�f�B�Isource")]
    AudioSource audioSource = null;
    [SerializeField, Tooltip("�h�A�J��SE")]
    AudioClip DoorSE;
    [SerializeField, Tooltip("Animation")]
    Animator AutoDoorAnimator;

    void Start()
    {
        StayFlag = false;
        PlaySECnt = 0;
        StartPosition = this.transform.position;
        NowState = State.Default;
    }


    void Update()
    {
        if (NowState == State.Open)
        {
            StartCoroutine("Up");
        }
        else if (NowState == State.Exit&& !StayFlag)
        {
            StartCoroutine("Down");
        }
    }
    #region public fanction
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
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
            if (NowState == State.Default)
            {
                NowState = State.Open;
            }
            else
            {
                return;
            }
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slime" && NowState == State.Default)
        {
            NowState = State.Open;
        }
        if (collision.gameObject.tag == "Slime" && NowState == State.Exit)
        {
            StayFlag = true;
            StopCoroutine("Down");
            StartCoroutine("Up");
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Slime" && NowState == State.Exit)
        {
            StayFlag = false;
        }
    }
    #endregion
    #region �R���[�`��
    private IEnumerator Up()//�h�A�㏸
    {
        if (PlaySECnt == 0)
        {
            PlaySE(DoorSE);
            PlaySECnt++;
        }
        AutoDoorAnimator.SetBool("Start", true);
        StopCoroutine("Down");
        transform.position = Vector3.MoveTowards(transform.position, StartPosition + moveTo, 5 * Time.deltaTime);
        yield return new WaitForSeconds(3f);
        NowState = State.Exit;
        PlaySECnt = 0;
        yield break;
    }
    private IEnumerator Down()//�h�A���~
    {
        StopCoroutine("Up");
        AutoDoorAnimator.SetBool("Start", true);
        transform.position = Vector3.MoveTowards(transform.position, StartPosition, 5 * Time.deltaTime);
        if (this.transform.position.y == StartPosition.y)
        {
            AutoDoorAnimator.SetBool("Start", false);
            if (NowState != State.Open)
            {
                NowState = State.Default;
            }
        }
        yield break;
    }
    #endregion
}
