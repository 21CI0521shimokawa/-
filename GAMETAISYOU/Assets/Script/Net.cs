using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Net : MonoBehaviour
{
    public GameObject veryMini;

    bool collisionDetectionErasingProcess;    //�����蔻����������ǂ����̏������������ǂ���

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
        //�����蔻����������������ɂ��Ă��邩�ǂ���
        if(collisionDetectionErasingProcess)
        {
            //�����蔻�肪���ɏ�����Ă�����
            if (!GetComponent<Collider2D>().enabled)
            {
                //����

                //����
                Vector3 playerPos = collision.GetComponent<Transform>().position;


                Vector3 position = new Vector3((transform.position.x - playerPos.x > 0 ? transform.position.x + 0.2f : transform.position.x - 0.2f), playerPos.y, playerPos.z);

                for (int i = 0; i < 10; ++i)
                {
                    GameObject cloneObject = Instantiate(veryMini);
                    cloneObject.transform.position = new Vector3(position.x, position.y, position.z);
                }

                //player��j��
                Destroy(collision.gameObject);
            }
        }
        else
        {
            //�����蔻��폜
            //���ȏ�̃X�s�[�h��������
            if (Mathf.Abs(collision.GetComponent<Rigidbody2D>().velocity.x) > 4.0f)
            {
                GetComponent<Collider2D>().enabled = false;
            }

            collisionDetectionErasingProcess = true;
        }
    }

    //�����蔻�蕜��
    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<Collider2D>().enabled = true;

        collisionDetectionErasingProcess = false;
    }
}
