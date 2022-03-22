using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrampoline : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [SerializeField] Rigidbody2D itemBody;
    [SerializeField] Vector2 normal; //ñ@ê¸
    [SerializeField] Vector2 startPoint; //í èÌà íu
    [SerializeField] Vector2 objectSpeed;
    [SerializeField] float force = 10f; //îΩî≠óÕ
    [SerializeField] float angle; //äpìx
    //[SerializeField] float ShakeTime = 0.2f;
    [SerializeField] float timer;
    //[SerializeField] bool shake;

    void OnValidate()
    {
        angle = transform.eulerAngles.z + 90f;
        //äpìxÇ©ÇÁñ@ê¸ÇãÅÇﬂÇÈ
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
        //äpìxÇ©ÇÁñ@ê¸ÇãÅÇﬂÇÈ
        normal = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
    }

    void Update()
    {
        OnValidate();
        Shake();

        Debug.DrawRay(transform.position, normal * 2f, Color.green);
    }

    //èuä‘êUìÆ
    void Shake()
    {
        //if (shake)
        //{
        //    timer += Time.deltaTime;
        //    transform.position -= new Vector3(normal.x, normal.y, 0f) * Time.deltaTime;
        //    if (timer > ShakeTime)
        //    {
        //        transform.position = startPoint;
        //        shake = false;
        //    }
        //}
        //else
        //{
        //    //transform.position = startPoint;
        //    GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
        //    timer = 0;
        //}
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (slimeController.pullWideForce > 0.1f)
        {
            if (collisionInfo.gameObject.tag == "Target")
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
}