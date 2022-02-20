using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tearoff : MonoBehaviour
{
    public float divisionTime;    //���􂷂�܂ł̎���
    public GameObject mini;

    public float walkSpeed = 3f;    //�L�����̈ړ��̑���
    bool moveCheck = true;          //�L�����ړ����\���m�F
    float power;    //�������Ă����

    // Start is called before the first frame update
    void Start()
    {
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moveCheck = true;   //�L�����ړ�OK

        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        if(triggerL == 1 && triggerR == 1)
        {
            moveCheck = false;  //�L�����ړ�NG(������Œ�)

            float stickLHorizontal = Input.GetAxis("Horizontal");
            float stickRHorizontal = Input.GetAxis("R_Stick_H");

            if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
            {
                //Debug.Log("���������Ă��:" + stickLHorizontal + "," + stickRHorizontal);

                power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //������͂𑝂₷

                if(power > (-stickLHorizontal + stickRHorizontal) / 2)
                {
                    power = (-stickLHorizontal + stickRHorizontal) / 2;
                }

                transform.localScale = new Vector3(1 + power, 1.0f / (1 + power), 1);

                if (power >= 1)
                {
                    Debug.Log("���ꂽ��");

                    GetComponent<Renderer>().material.color = Color.red;

                    //�E
                    {
                        Vector3 position = new Vector3 ((float)(transform.position.x + 0.8), transform.position.y, transform.position.z);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;
                    }

                    //��
                    {
                        Vector3 position = new Vector3((float)(transform.position.x - 0.8), transform.position.y, transform.position.z);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;
                    }

                    //���g��j��
                    Destroy(this.gameObject);
                }
                else
                {
                    Debug.Log("����ĂȂ���:" + power);

                    GetComponent<Renderer>().material.color = Color.green;
                }
            }
            else
            {
                Debug.Log("���񂾂�");

                power = 0;

                transform.localScale = new Vector3(1, 1, 1);

                GetComponent<Renderer>().material.color = Color.green;
            }
            
        }
        else
        {
            Debug.Log("����łȂ���");

            power = 0;

            transform.localScale = new Vector3(1, 1, 1);

            GetComponent<Renderer>().material.color = Color.green;
        }

        //�L�����̈ړ�(���E)
        if (moveCheck == true)
        {
            if (Input.GetAxis("Horizontal") > 0) { transform.position += transform.right * walkSpeed * Time.deltaTime; }    //�E
            if (Input.GetAxis("Horizontal") < 0) { transform.position -= transform.right * walkSpeed * Time.deltaTime; }    //��
        }
    }
}
