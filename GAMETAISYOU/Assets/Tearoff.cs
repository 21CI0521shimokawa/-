using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tearoff : MonoBehaviour
{
    public float divisionTime;    //分裂するまでの時間
    public GameObject mini;

    public float walkSpeed = 3f;    //キャラの移動の速さ
    bool moveCheck = true;          //キャラ移動が可能か確認
    float power;    //かかっている力

    // Start is called before the first frame update
    void Start()
    {
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moveCheck = true;   //キャラ移動OK

        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        if(triggerL == 1 && triggerR == 1)
        {
            moveCheck = false;  //キャラ移動NG(ちぎる最中)

            float stickLHorizontal = Input.GetAxis("Horizontal");
            float stickRHorizontal = Input.GetAxis("R_Stick_H");

            if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
            {
                //Debug.Log("引っ張ってるよ:" + stickLHorizontal + "," + stickRHorizontal);

                power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //かかる力を増やす

                if(power > (-stickLHorizontal + stickRHorizontal) / 2)
                {
                    power = (-stickLHorizontal + stickRHorizontal) / 2;
                }

                transform.localScale = new Vector3(1 + power, 1.0f / (1 + power), 1);

                if (power >= 1)
                {
                    Debug.Log("きれたよ");

                    GetComponent<Renderer>().material.color = Color.red;

                    //右
                    {
                        Vector3 position = new Vector3 ((float)(transform.position.x + 0.8), transform.position.y, transform.position.z);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;
                    }

                    //左
                    {
                        Vector3 position = new Vector3((float)(transform.position.x - 0.8), transform.position.y, transform.position.z);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;
                    }

                    //自身を破壊
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("きれてないよ:" + power);

                    GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
            {
                Debug.Log("つかんだよ");

                power = 0;

                transform.localScale = new Vector3(1, 1, 1);

                GetComponent<Renderer>().material.color = Color.green;
            }
            
        }
        else
        {
            Debug.Log("つかんでないよ");

            power = 0;

            transform.localScale = new Vector3(1, 1, 1);

            GetComponent<Renderer>().material.color = Color.green;
        }

        //キャラの移動(左右)
        if (moveCheck == true)
        {
            if (Input.GetAxis("Horizontal") > 0) { transform.position += transform.right * walkSpeed * Time.deltaTime; }    //右
            if (Input.GetAxis("Horizontal") < 0) { transform.position -= transform.right * walkSpeed * Time.deltaTime; }    //左
        }
    }
}
