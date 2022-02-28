using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    public GameObject veryMini;

    bool collisionDetectionErasingProcess;    //当たり判定を消すかどうかの処理をしたかどうか

    // Start is called before the first frame update
    void Start()
    {
        collisionDetectionErasingProcess = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //当たり判定を消す処理を既にしているかどうか
        if(collisionDetectionErasingProcess)
        {
            //当たり判定が既に消されていたら
            if (!GetComponent<Collider2D>().enabled)
            {
                //分裂

                //生成
                Vector3 playerPos = collision.GetComponent<Transform>().position;


                Vector3 position = new Vector3((transform.position.x - playerPos.x > 0 ? transform.position.x + 0.2f : transform.position.x - 0.2f), playerPos.y, playerPos.z);

                for (int i = 0; i < 10; ++i)
                {
                    GameObject cloneObject = Instantiate(veryMini);
                    cloneObject.transform.position = new Vector3(position.x, position.y, position.z);
                }

                //playerを破壊
                Destroy(collision.gameObject);
            }
        }
        else
        {
            //当たり判定削除
            //一定以上のスピードだったら
            if (Mathf.Abs(collision.GetComponent<Rigidbody2D>().velocity.x) > 4.0f)
            {
                GetComponent<Collider2D>().enabled = false;
            }

            collisionDetectionErasingProcess = true;
        }
    }

    //当たり判定復活
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Collider2D>().enabled = true;

        collisionDetectionErasingProcess = false;
    }
}
