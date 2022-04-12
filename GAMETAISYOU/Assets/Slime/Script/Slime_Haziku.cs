using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Haziku : MonoBehaviour
{
    public SlimeController slimeController;
    [SerializeField] GameObject ArrowPrefab;

    #region �͂���
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float moveSpeed;
    #endregion

    GameObject arrow;

    public bool _ifSlimeHazikuNow;  //�͂����̃A�b�v�f�[�g���s���Ă���Œ����ǂ���
    bool isArrowBeing;  //��󂪑��݂��邩�ǂ���

    [SerializeField] float coolTime;    //�͂������㉽�b�Ԃ͂������Ƃ��ł��Ȃ���
    public float _nextHazikuUpdateTime;  //�͂����A�b�v�f�[�g���s�����Ƃ��ł���悤�ɂȂ鎞��

    public float _stateFixationTime;    //�͂�������State��AIR�ŌŒ肷�鎞�ԁi�s��΍�j

    // Start is called before the first frame update
    void Start()
    {
        #region �͂���
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 15.0f;
        #endregion

        _ifSlimeHazikuNow = false;
        isArrowBeing = false;

        _nextHazikuUpdateTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Gamepad gamepad = Gamepad.current;

        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
        float horizontal = 0.0f;
        float vertical = 0.0f;

        //�Q�[���p�b�h���ڑ�����Ă���Ȃ�
        if (!(gamepad == null))
        {
            Vector2 stickValue = slimeController.modeLR == SlimeController.LRMode.Left ? gamepad.leftStick.ReadValue() : gamepad.rightStick.ReadValue();

            horizontal = stickValue.x;
            vertical = stickValue.y;

            //horizontal = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Horizontal") : Input.GetAxis("R_Stick_Horizontal");
            //vertical = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Vertical") : Input.GetAxis("R_Stick_Vertical");
        }
        else
        {
            Debug.Log("�R���g���[���[���ڑ�����Ă��܂���");
        }

        if (slimeController.hazikuUpdate && !slimeController.slimeBuf.ifTearOff)
        {
            stickX[freamCnt % freamCntMax] = horizontal;
            stickY[freamCnt % freamCntMax] = vertical;

            if (freamCnt >= freamCntMax)
            {
                Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);

                #region �K�C�h
                Vector2 currentVector = new Vector2(horizontal, vertical);
                float radian = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg + 90;
                if (radian < 0) radian += 360;
                if (currentVector.magnitude > 0.3f)
                {
                    ArrowInstanceate();
                    arrow.transform.position = transform.position;
                    arrow.transform.rotation = Quaternion.Euler(0, 0, radian);
                    arrow.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax * currentVector.magnitude * 5);
                }
                else
                {
                    _ArrowDestroy();
                }
                #endregion

                if (stickVectorNow.magnitude <= 0.1f)
                {
                    _ifSlimeHazikuNow = false;

                    Vector2 stickVectorMost = stickVectorNow;
                    for (int i = 1; i < freamCntMax; ++i)
                    {
                        Vector2 stickVectorCompare = new Vector2(stickX[(freamCnt + i) % freamCntMax], stickY[(freamCnt + i) % freamCntMax]);
                        if (stickVectorMost.magnitude < stickVectorCompare.magnitude)
                        {
                            stickVectorMost = stickVectorCompare;
                        }
                    }

                    //�ړ�
                    if (stickVectorMost.magnitude > stickVectorNow.magnitude + 0.3f)
                    {
                        slimeController._SlimeAnimator.SetTrigger("Haziku");

                        slimeController.rigid2D.velocity = stickVectorMost * -moveSpeed;
                        freamCnt = 0;
                        slimeController.s_state = State.AIR;

                        for (int i = 0; i < freamCntMax; ++i)
                        {
                            stickX[i] = 0.0f;
                            stickY[i] = 0.0f;
                        }

                        _ifSlimeHazikuNow = false;

                        _nextHazikuUpdateTime = slimeController.liveTime + coolTime;
                        _stateFixationTime = slimeController.liveTime + 0.03f;
                    }
                }
                else
                {
                    _ifSlimeHazikuNow = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < freamCntMax; ++i)
            {
                stickX[i] = 0.0f;
                stickY[i] = 0.0f;
            }

            _ifSlimeHazikuNow = false;
            _ArrowDestroy();
        }

        ++freamCnt;
    }


    //���̐���
    private void ArrowInstanceate()
    {
        if (!isArrowBeing)
        {
            isArrowBeing = true;

            ArrowPrefab.SetActive(true);
            GameObject buf = GameObject.Instantiate(ArrowPrefab);
            arrow = buf;
        }
    }

    //���̔j��
    public void _ArrowDestroy()
    {
        if (isArrowBeing)
        {
            isArrowBeing = false;

            arrow.SetActive(false);
            Destroy(arrow);
        }
    }
}
