using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] Rigidbody2D itemBody;
    [SerializeField] Vector2 normal; //法線
    [SerializeField] Vector2 startPoint; //通常位置
    [SerializeField] Vector2 objectSpeed;
    [SerializeField] float force = 10f; //反発力
    [SerializeField] float angle; //角度
    [SerializeField] float ShakeTime = 0.2f;
    [SerializeField] float timer;
    [SerializeField] bool shake;

    [SerializeField, Tooltip("オーディオsource")]
    AudioSource audioSource = null;
    [SerializeField]
    AudioClip SE;

    Animator animator;

    void OnValidate()
    {
        angle = transform.eulerAngles.z + 90f;
        //角度から法線を求める
        normal = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        if (angle < 0)
        {
            angle += 360f;
        }
        else if (angle > 360)
        {
            angle -= 360f;
        }
    }

    void Start()
    {
        TryGetComponent(out animator);

        startPoint = transform.position;
        angle = transform.eulerAngles.z + 90f;
        //角度から法線を求める
        normal = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    void Update()
    {
        OnValidate();
        Shake();

        Debug.DrawRay(transform.position, normal * 2f, Color.green);
    }

    //瞬間振動
    void Shake()
    {
        if (shake)
        {
            timer += Time.deltaTime;
            transform.position -= new Vector3(normal.x, normal.y, 0f) * Time.deltaTime;
            if (timer > ShakeTime)
            {
                transform.position = startPoint;
                shake = false;
            }
        }
        else
        {
            transform.position = startPoint;
            //GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
            timer = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        // Debug.Log(collisionInfo.transform.position - transform.position);
        // Debug.Log(Vector3.Angle(normal, collisionInfo.transform.position - transform.position));

        if (Vector3.Angle(normal, collisionInfo.transform.position - transform.position) < 90f)
        {
            PlaySE(SE);
            itemBody = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
            objectSpeed = itemBody.velocity;
            //itemBody.velocity =Vector2.zero;
            //itemBody.AddForce(normal.normalized * force, ForceMode2D.Impulse);
            itemBody.velocity = normal.normalized * force;
            //shake = true;
            //GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            animator.SetTrigger("Touch");
        }
        //CaculateStopAndShotPoint();
    }
    public void PlaySE(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("オーディオソースが設定されてない");
        }
    }
}
