using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Tearoff : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [SerializeField, Tooltip("����ł���傫���̉���")]
    private float scaleLowerLimit;
    [SerializeField, Tooltip("������傫��")]
    private float divisionScale;
    [SerializeField, Tooltip("���􂷂�܂ł̎���")]
    private float divisionTime;
    [SerializeField, Tooltip("���􂵂��I�u�W�F�N�g")]
    private GameObject slimePrefab;

    public float stretchEndTime;    //���b�o�߂����猳�̒����ɖ߂邩
    public float deadEndTime;       //���b�o�߂�����X���C���������邩

    public float power; //�������Ă����

    bool oneFrameBefore_Update; //1�t���[���O�ɏ������s�������ǂ���

    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        //������
        power = 0;

        oneFrameBefore_Update = false;

        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeController.tearoffUpdate && slimeController.core)  //update��������Ă��Ė{�̂�������
        {
            if (!slimeController.slimeBuf.ifTearOff)    //���ɂ������Ă��Ȃ�������
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

                    gamepad.SetMotorSpeeds(power, power);

                    if (power >= 1)
                    {

                        if (scaleLowerLimit <= slimeController.scale - divisionScale)
                        {
                            Debug.Log("���ꂽ��");

                            slimeController.scale -= divisionScale;

                            power = 0;
                            slimeController.slimeBuf.ifTearOff = true;

                            //�E �R�A����Ȃ��ق�
                            {
                                Vector3 position = new Vector2((float)(transform.position.x + 0.1), transform.position.y);
                                GameObject cloneObject = Instantiate(slimePrefab);
                                cloneObject.transform.position = position;

                                SlimeController buf = cloneObject.GetComponent<SlimeController>();
                                buf.scale = divisionScale;
                                cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
                                buf.core = false;
                                buf.deadTime = deadEndTime;
                                buf.modeLR = SlimeController.LRMode.Right;
                            }

                            //�� �R�A
                            {
                                Vector3 position = new Vector2((float)(transform.position.x - 0.1), transform.position.y);
                                GameObject cloneObject = Instantiate(slimePrefab);
                                cloneObject.transform.position = position;

                                SlimeController buf = cloneObject.GetComponent<SlimeController>();
                                buf.scale = slimeController.scale;
                                cloneObject.transform.localScale = new Vector2(slimeController.scale, slimeController.scale);

                                buf.core = true;
                                buf.modeLR = SlimeController.LRMode.Left;
                            }

                            //���g��j��
                            Destroy(this.gameObject);

                            gamepad.SetMotorSpeeds(0.0f, 0.0f);
                        }
                        else
                        {
                            GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("����܂���I�I");
                        }
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

                    GetComponent<Renderer>().material.color = Color.green;

                    gamepad.SetMotorSpeeds(0.0f, 0.0f);
                }


                oneFrameBefore_Update = true;
            }
        }
        else
        {
            if(oneFrameBefore_Update)
            {
                slimeController.pullWideForceMax = power;
                slimeController.stretchEndTime = slimeController.liveTime + stretchEndTime;
            }

            power = 0;

            oneFrameBefore_Update = false;
        }
    }
}
