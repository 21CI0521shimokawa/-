using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrampoline : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [SerializeField] Rigidbody2D itemBody;
    [SerializeField] Vector2 normal; //法線
    [SerializeField] Vector2 startPoint; //通常位置
    [SerializeField] Vector2 objectSpeed;
    [SerializeField] float force = 10f; //反発力
    [SerializeField] float angle; //角度
    //[SerializeField] float ShakeTime = 0.2f;
    [SerializeField] float timer;
    //[SerializeField] bool shake;

    public bool _isOn;

    //外部
    [SerializeField, Tooltip("エレベーター")]
    private GameObject Elevator;
    [SerializeField, Tooltip("カメラ")]
    private GameObject Camera;


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
        startPoint = transform.position;
        angle = transform.eulerAngles.z + 90f;
        //角度から法線を求める
        normal = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    void Update()
    {
        OnValidate();
        ForceChange();
        Debug.DrawRay(transform.position, normal * 2f, Color.green);
    }

    void ForceChange()
    {
        if(Mathf.Abs(transform.localScale.x) > slimeController._scaleMax && _isOn)
        {
            force = 15;
        }
        else
        {
            force = 5;
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        //if(slimeController.pullWideForce > 0.1f)
        {
            if (collisionInfo.gameObject.tag == "Item")
            //if(collisionInfo.gameObject.GetComponent<Rigidbody2D>())
            {
                if (Vector3.Angle(normal, collisionInfo.transform.position - transform.position) < 90f)
                {
                    itemBody = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
                    objectSpeed = itemBody.velocity;
                    itemBody.AddForce(normal * force, ForceMode2D.Impulse);
                    //shake = true;
                    //GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                }
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="Area")
        {
            Elevator.GetComponent<ElevatoControll>().ElevatorStart();
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag== "Tracking")
        {
            Camera.GetComponent<PlayerTracking>().TrackingFlag = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tracking")
        {
            Camera.GetComponent<PlayerTracking>().TrackingFlag = false;
        }
        if (collision.gameObject.name == "Area")
        {
            Elevator.GetComponent<ElevatoControll>().ElevatorDown();
        }
    }
}