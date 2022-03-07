using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearoff_Coalesecence : MonoBehaviour
{
    public PlayerController_Coalescence playerController;

    #region ������ϐ��錾
    [SerializeField, Tooltip("���􂷂�܂ł̎���")]
    private float divisionTime;
    [SerializeField, Tooltip("���􂵂��I�u�W�F�N�g")]
    private GameObject mini;

    private float power;//�������Ă����
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //������
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.tearoffUpdate)
        {
            //������
            float stickLHorizontal = Input.GetAxis("L_Stick_Horizontal");
            float stickRHorizontal = Input.GetAxis("R_Stick_Horizontal");

            if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
            {
                //Debug.Log("���������Ă��:" + stickLHorizontal + "," + stickRHorizontal);
                power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //������͂𑝂₷

                if (power > (-stickLHorizontal + stickRHorizontal) / 2)
                {
                    power = (-stickLHorizontal + stickRHorizontal) / 2;
                }

                transform.localScale = new Vector2(1 + power, 1.0f / (1 + power));

                if (power >= 1)
                {
                    Debug.Log("���ꂽ��");

                    GetComponent<Renderer>().material.color = Color.red;

                    //�E
                    {
                        Vector3 position = new Vector2((float)(transform.position.x + 0.8), transform.position.y);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;

                        cloneObject.GetComponent<PlayerController_Coalescence>().ifMiniPlayer = true;
                        cloneObject.GetComponent<PlayerHaziku_Coalescence>().modeLR = PlayerHaziku_Coalescence.LRMode.Right;
                    }

                    //��
                    {
                        Vector3 position = new Vector2((float)(transform.position.x - 0.8), transform.position.y);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;

                        cloneObject.GetComponent<PlayerController_Coalescence>().ifMiniPlayer = true;
                        cloneObject.GetComponent<PlayerHaziku_Coalescence>().modeLR = PlayerHaziku_Coalescence.LRMode.Left;
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

                transform.localScale = new Vector2(1, 1);

                GetComponent<Renderer>().material.color = Color.green;
            }

        }
    }
}
