using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevator : MonoBehaviour
{
    private Vector3 pos;
    [SerializeField, Tooltip("Yの位置")]
    private float DestinationPosY;
    [SerializeField]
    private float Speed;
    public Vector3 DestinationPos;
    public bool is2ndFloor;

    [SerializeField] Animator StartElevatorAnimator;
    [SerializeField] SlimeController SlimeController;
    [SerializeField] AudioSource StartElevatorAudioSource;
    [SerializeField] AudioClip SE;
    void Start()
    {
        SlimeController._ifOperation = false;
        DestinationPos = new Vector3(0, DestinationPosY, 0);
        pos = this.transform.position;
        is2ndFloor = false;
        MoveUp();
    }
    public void MoveUp()
    {
        StartCoroutine("MoveUpStart");
    }

    IEnumerator MoveUpStart()
    {
        while (pos.y < DestinationPos.y)
        {
            pos = transform.position;
            transform.Translate(0, Speed*Time.deltaTime, 0);
            yield return new WaitForSeconds(0.01f);
        }
        SlimeController._ifOperation = true;
        PlaySE(SE);
        StartElevatorAnimator.SetTrigger("Open");
        is2ndFloor = true;
    }
    public void MoveDown()
    {
        StartCoroutine("MoveDownStart");

    }
    public void PlaySE(AudioClip audio)
    {
        if (StartElevatorAudioSource != null)
        {
            StartElevatorAudioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("オーディオソースが設定されてない");
        }
    }
    IEnumerator MoveDownStart()
    {
        while (pos.y > 0.0f)
        {
            pos = transform.position;
            transform.Translate(0, -0.02f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        is2ndFloor = false;
    }
}