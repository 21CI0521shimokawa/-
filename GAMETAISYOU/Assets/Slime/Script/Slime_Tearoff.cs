using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Tearoff : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [Tooltip("����ł���傫���̉���")]
    public float _scaleLowerLimit;
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

    bool IsDirecting;   //���o�����ǂ���

    public enum TearoffSlimeType
    {
        NON,
        RIGHT,
        LEFT
    }

    public TearoffSlimeType _tearoffSlimeType;

    // Start is called before the first frame update
    void Start()
    {
        //������
        power = 0;

        oneFrameBefore_Update = false;

        IsDirecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeController.tearoffUpdate && slimeController.core)  //update��������Ă��Ė{�̂�������
        {
            if (!slimeController._slimeBuf._ifTearOff)    //���ɂ������Ă��Ȃ�������
            {

                //������
                //float stickLHorizontal = Input.GetAxis("L_Stick_Horizontal");
                //float stickRHorizontal = Input.GetAxis("R_Stick_Horizontal");

                var gamepadLeftStick = Gamepad.current.leftStick.ReadValue();
                var gamepadRightStick = Gamepad.current.rightStick.ReadValue();
                float stickLHorizontal = gamepadLeftStick.x;
                float stickRHorizontal = gamepadRightStick.x;

                //�X�e�B�b�N�̍ő�l�ɂ�����x�]�T����������i������Ȃ��Ȃ����Ⴄ�j
                stickLHorizontal = Mathf.Min(stickLHorizontal / 0.90f, 1.0f);
                stickRHorizontal = Mathf.Min(stickRHorizontal / 0.90f, 1.0f);

                if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
                {
                    slimeController._SlimeAnimator.SetBool("Extend", true);
                    slimeController._SlimeAnimator.SetBool("SlimeScaleNo", slimeController._scaleNow <= _scaleLowerLimit);

                    //Debug.Log("���������Ă��:" + stickLHorizontal + "," + stickRHorizontal);

                    ChangeDivisionTime();
                    power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //������͂𑝂₷

                    if (power > (-stickLHorizontal + stickRHorizontal) / 2)
                    {
                        power = (-stickLHorizontal + stickRHorizontal) / 2;
                    }

                    //�U��
                    slimeController._controllerVibrationScript.Vibration("SlimeTearoff", power * 0.3f, power * 0.3f);

                    slimeController._slimeSE._PlayStretchSE(3 * power);

                    //������
                    if (power >= 1)
                    {
                        //if (slimeController._scaleMax == slimeController._scaleNow) //�X���C�����傫���Ȃ肫���Ă�����
                        {
                            
                            //�傫����Limit���傫���Ȃ炿����
                            if(slimeController._scaleNow > _scaleLowerLimit)
                            {
                                //�X���C���A�j���[�V��������������Ԃ�������
                                if(slimeController._SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_ExtendIdle"))
                                {
                                    //������傫���ƍ��̑傫������Limit���������l�̓�����������������
                                    float tearOffSlimeScale = Mathf.Min(divisionScale, slimeController._scaleMax - _scaleLowerLimit);

                                    StartCoroutine(SlimeTearOffCoroutine(tearOffSlimeScale));    //�R���[�`���̋N��
                                }
                            }
                            else
                            {
                                GetComponent<Renderer>().material.color = Color.red;
                                Debug.Log("�傫��������܂���I�I");
                                slimeController._CannotAction();
                            }

                            //�傫����Limit�������Ȃ��Ȃ�
                            //if (_scaleLowerLimit <= slimeController._scaleMax - divisionScale)
                            //{

                            //    power = 0;

                            //    slimeController._ifOperation = false;

                            //    slimeController._SlimeAnimator.SetTrigger("Tearoff");
                            //    slimeController.slimeBuf.ifTearOff = true;

                            //    //Invoke("SlimeInstantiate", 0.55f);
                            //}
                            
                        }
                        //else
                        //{
                        //    GetComponent<Renderer>().material.color = Color.red;
                        //    Debug.Log("�X���C�����傫���Ȃ肫���Ă܂���I�I");
                        //    slimeController._CannotAction();
                        //}
                    }
                    else
                    {
                        //Debug.Log("����ĂȂ���:" + power);

                        GetComponent<Renderer>().material.color = Color.green;
                    }
                }
                else
                {
                    Debug.Log("���񂾂�");

                    power = 0;

                    GetComponent<Renderer>().material.color = Color.green;

                    slimeController._controllerVibrationScript.Vibration("SlimeTearoff", 0, 0);
                    slimeController._slimeSE._StopStretchSE();
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

            if (!IsDirecting)
            {
                slimeController._controllerVibrationScript.Destroy("SlimeTearoff");
            }

            oneFrameBefore_Update = false;

            slimeController._SlimeAnimator.SetBool("Extend", false);

            slimeController._slimeSE._StopStretchSE();
        }
    }


    public void SlimeInstantiate()
    {
        Debug.Log("���ꂽ��");

        power = 0;

        //�R�A����Ȃ��ق�
        {
            float distance = slimeController._scaleMax * 1.0f;

            Vector3 position = new Vector2((float)(transform.position.x + distance * (slimeController._direction == SlimeController._Direction.Right ? 1 : -1)), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = divisionScale;
            cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
            buf.core = false;
            buf.deadTime = deadEndTime;
            buf._direction = slimeController._direction;

            //����^�C�v
            switch (_tearoffSlimeType)
            {
                //����s��
                case TearoffSlimeType.NON:
                    buf._ifOperation = false;
                    break;

                //�E
                case TearoffSlimeType.RIGHT:
                    buf.modeLR = SlimeController.LRMode.Right;
                    break;

                //��
                case TearoffSlimeType.LEFT:
                    buf.modeLR = SlimeController.LRMode.Left;
                    break;
            }
        }

        slimeController._scaleMax -= divisionScale;

        //�R�A
        {
            Vector3 position = new Vector2((float)(transform.position.x), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = slimeController._scaleMax;
            cloneObject.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax);

            buf.core = true;
            buf.modeLR = SlimeController.LRMode.Left;
            buf._direction = slimeController._direction;
        }

        //���g��j��
        Destroy(this.gameObject);
    }

    // �R���[�`���{��
    private IEnumerator SlimeTearOffCoroutine(float slimeScale)
    {
        slimeController._slimeSE._PlayTearoffSE();
        slimeController._slimeSE._StopStretchSE();

        IsDirecting = true;

        power = 0;

        slimeController._ifOperation = false;

        slimeController._SlimeAnimator.SetTrigger("Tearoff");
        slimeController._slimeBuf._ifTearOff = true;

        slimeController._controllerVibrationScript.Vibration("SlimeTearoff", 1.0f, 1.0f);

        yield return new WaitForSeconds(0.25f); //0.25�b�҂�
        slimeController._controllerVibrationScript.Destroy("SlimeTearoff");
        yield return new WaitForSeconds(0.2f); //0.2�b�҂�

        Debug.Log("���ꂽ��");

        power = 0;

        //�R�A����Ȃ��ق�
        {
            float distance = slimeController._scaleMax * 1.0f;

            Vector3 position = new Vector2((float)(transform.position.x + SlimeNoCoreInstanceateDistance(distance) * (slimeController._direction == SlimeController._Direction.Right ? 1 : -1)), transform.position.y);

            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = slimeScale;
            cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
            buf.core = false;
            buf.deadTime = deadEndTime;
            buf._direction = slimeController._direction;

            //����^�C�v
            switch (_tearoffSlimeType)
            {
                //����s��
                case TearoffSlimeType.NON:
                    buf._ifOperation = false;
                    break;

                //�E
                case TearoffSlimeType.RIGHT:
                    buf.modeLR = SlimeController.LRMode.Right;
                    break;

                //��
                case TearoffSlimeType.LEFT:
                    buf.modeLR = SlimeController.LRMode.Left;
                    break;
            }
        }

        slimeController._scaleMax -= slimeScale;

        //�R�A
        {
            Vector3 position = new Vector2((float)(transform.position.x), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = slimeController._scaleMax;
            cloneObject.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax);

            buf.core = true;
            buf.modeLR = SlimeController.LRMode.Left;
            buf._direction = slimeController._direction;
        }

        //���g��j��
        Destroy(this.gameObject);

        IsDirecting = false;
    }


    float SlimeNoCoreInstanceateDistance(float distance_)
    {
        float rtv = distance_;

        //Ray��^���ɔ�΂�
        Ray ray = new Ray(transform.position, slimeController._direction == SlimeController._Direction.Right ? Vector2.right : Vector2.left);

        Debug.DrawRay(ray.origin, ray.direction * distance_, Color.red);
        RaycastHit2D[] rayHits = Physics2D.RaycastAll(ray.origin, ray.direction, distance_);
        List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

        //��ԋ������߂��I�u�W�F�N�g�Ƃ̋���������
        foreach (RaycastHit2D i in rayHits)
        {
            //�g���K�[�͏��O
            if (i.collider.isTrigger)
            {
                continue;
            }

            //�������g�͏��O
            if (i.collider.gameObject == this.gameObject)
            {
                continue;
            }

            //�ꕔ�̃^�O�̂����I�u�W�F�N�g�͏��O
            switch (i.collider.gameObject.tag)
            {
                case "Slime" : continue;        //�X���C��        
                case "SlimeTrigger": continue;  //�X���C���g���K�[
                case "Tracking": continue;      //�J�����g���b�L���O
                //�����ɒǉ�

                default:
                    raycastHits.Add(i);    //�ǉ�
                    break;
            }

            rtv = Mathf.Min(rtv, i.distance);   //�������Z��������
        }

        return rtv;
    }

    void ChangeDivisionTime()
    {
        float sizeMin = 2.0f;
        float sizeMax = 5.0f;

        float divisionTimeMin = 1.0f;
        float divisionTimeMax = 2.5f;


        float buf = (slimeController._scaleMax - sizeMin) / sizeMax;

        float newDivisionTime = Mathf.Lerp(divisionTimeMin, divisionTimeMax, buf);

        divisionTime = newDivisionTime;
    }
}