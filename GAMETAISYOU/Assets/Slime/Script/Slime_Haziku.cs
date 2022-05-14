using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Haziku : MonoBehaviour
{
    public SlimeController slimeController;
    [SerializeField] SlimeSE slimeSE;
    [SerializeField] GameObject ArrowPrefab;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject EyePrefab;
    #region �͂���
    [SerializeField] float[] stickX;
    [SerializeField] float[] stickY;
    public const int freamCntMax = 20;
    int freamCnt;
    float moveSpeed;
    #endregion

    public float _power;    //�p���[

    GameObject arrow;
    GameObject eye;
    public SpriteRenderer _eyeRenderer;

    public bool _ifSlimeHazikuNow;  //�͂����̃A�b�v�f�[�g���s���Ă���Œ����ǂ���
    bool isArrowBeing;  //��󂪑��݂��邩�ǂ���
    bool isEyeBeing;    //�ڂ����݂��邩�ǂ���

    [SerializeField] float coolTime;    //�͂������㉽�b�Ԃ͂������Ƃ��ł��Ȃ���
    public float _nextHazikuUpdateTime;  //�͂����A�b�v�f�[�g���s�����Ƃ��ł���悤�ɂȂ鎞��

    public float _stateFixationTime;    //�͂�������State��AIR�ŌŒ肷�鎞�ԁi�s��΍�j

    enum GuideType
    {
        Non,
        arrow,
        Ray
    };


    [SerializeField] GuideType guidetype;

    enum StickType
    {
        Normal,
        Reverse
    };

    [SerializeField] StickType stickType;

    [SerializeField] bool eyeReverse;

    // Start is called before the first frame update
    void Start()
    {
        #region �͂���
        stickX = new float[freamCntMax];
        stickY = new float[freamCntMax];
        freamCnt = 0;
        moveSpeed = 3.0f;
        #endregion

        _ifSlimeHazikuNow = false;
        isArrowBeing = false;
        isEyeBeing = false;

        _nextHazikuUpdateTime = 0.0f;

        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(1, 1, 1, 1);
        lineRenderer.endColor = new Color(1, 1, 1, 0);

        _stateFixationTime = 0;
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

        if (slimeController.hazikuUpdate && !slimeController._slimeBuf._ifTearOff)
        {
            stickX[freamCnt % freamCntMax] = horizontal * (stickType == StickType.Reverse ? 1 : -1);
            stickY[freamCnt % freamCntMax] = vertical * (stickType == StickType.Reverse ? 1 : -1);

            if (freamCnt >= freamCntMax)
            {
                Vector2 stickVectorNow = new Vector2(stickX[freamCnt % freamCntMax], stickY[freamCnt % freamCntMax]);

                #region �K�C�h
                if (guidetype == GuideType.arrow)
                {
                    Vector2 currentVector = new Vector2(horizontal, vertical);
                    float radian = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg + 90;
                    if (radian < 0) radian += 360;
                    if (currentVector.magnitude > 0.3f)
                    {
                        GuideInstanceate();
                        arrow.transform.position = transform.position;
                        arrow.transform.rotation = Quaternion.Euler(0, 0, radian);
                        arrow.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax * currentVector.magnitude * 5);
                    }
                    else
                    {
                        _GuideDestroy();
                    }
                }
                #endregion

                #region �K�C�h2
                if (guidetype == GuideType.Ray)
                {
                    Vector2 currentVector = new Vector2(horizontal, vertical);
                    if (currentVector.magnitude > 0.3f)
                    {
                        GuideInstanceate();
                        lineRenderer.SetPosition(0, this.gameObject.transform.position);
                        lineRenderer.SetPosition(1, new Vector2(this.gameObject.transform.position.x + currentVector.normalized.x * -3, this.gameObject.transform.position.y + currentVector.normalized.y * -3));
                    }
                    else
                    {
                        _GuideDestroy();
                    }
                }
                #endregion

                #region �����ύX
                {
                    Vector2 currentVector = new Vector2(horizontal, vertical);
                    float radian = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg + 90;
                    if (radian < 0) radian += 360;
                    
                    //�ڂ̈ʒu�ɉ����Č����ύX
                    if(slimeController._direction == SlimeController._Direction.Right && horizontal * (!eyeReverse ? -1 : 1) > 0.1f)
                    {
                        slimeController._direction = SlimeController._Direction.Left;
                    }
                    else if (slimeController._direction == SlimeController._Direction.Left && horizontal * (!eyeReverse ? -1 : 1) < -0.1f)
                    {
                        slimeController._direction = SlimeController._Direction.Right;
                    }
                }
                #endregion

                #region ��
                {
                    Vector2 currentVector = new Vector2(horizontal, vertical);
                    float radian = Mathf.Atan2(currentVector.y, currentVector.x) * Mathf.Rad2Deg + 90;
                    if (radian < 0) radian += 360;
                    AnimationInstanceate();
                    eye.transform.position = transform.position;

                    //�傫������
                    eye.transform.localScale = new Vector2(slimeController._scaleNow * 1.2f * (slimeController._direction == SlimeController._Direction.Right ? 1 : -1), slimeController._scaleNow * 1.2f);

                    //�ʒu����
                    eye.transform.position += new Vector3(slimeController._direction == SlimeController._Direction.Right ? 0.05f : -0.05f, 0, 0);

                    //�ڈړ�
                    float magnification = 0.07f * (!eyeReverse ? 1 : -1) * slimeController._scaleNow;
                    eye.transform.position += new Vector3(currentVector.x * magnification, currentVector.y * magnification);
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
                    if (stickVectorMost.magnitude > stickVectorNow.magnitude + 0.25f)
                    {
                        slimeSE._PlayJumpSE();

                        slimeController._SlimeAnimator.SetTrigger("Haziku");
                        StartCoroutine(vibrationCoroutine(Mathf.Abs(stickVectorMost.magnitude), Mathf.Abs(stickVectorMost.magnitude)));    //�R���[�`���̋N��

                        slimeController.rigid2D.velocity = stickVectorMost * -moveSpeed * _power;
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
            _GuideDestroy();
            _AnimationEnd();
        }

        ++freamCnt;
    }


    //�K�C�h�̐���
    private void GuideInstanceate()
    {
        switch (guidetype)
        {
            case GuideType.arrow:
                if (!isArrowBeing)
                {
                    isArrowBeing = true;

                    ArrowPrefab.SetActive(true);
                    GameObject buf = GameObject.Instantiate(ArrowPrefab);
                    arrow = buf;
                }
                break;

            case GuideType.Ray:
                lineRenderer.enabled = true;
                break;
        }
    }

    //�K�C�h�̔j��
    public void _GuideDestroy()
    {
        switch (guidetype)
        {
            case GuideType.arrow:
                if (isArrowBeing)
                {
                    isArrowBeing = false;

                    arrow.SetActive(false);
                    Destroy(arrow);
                }
                break;

            case GuideType.Ray:
                lineRenderer.enabled = false;
                break;
        }
    }


    void AnimationInstanceate()
    {
        slimeController._SlimeAnimator.SetBool("Haziku_prepare", true);

        if (!isEyeBeing)
        {
            isEyeBeing = true;

            EyePrefab.SetActive(true);
            GameObject buf = GameObject.Instantiate(EyePrefab);
            eye = buf;

            _eyeRenderer = eye.GetComponent<SpriteRenderer>();
        }
    }


    public void _AnimationEnd()
    {
        slimeController._SlimeAnimator.SetBool("Haziku_prepare", false);

        EyeDestroy();
    }

    void EyeDestroy()
    {
        if (isEyeBeing)
        {
            isEyeBeing = false;

            eye.SetActive(false);
            Destroy(eye);
        }
    }


    // �R���[�`���{��
    private IEnumerator vibrationCoroutine(float leftPower_, float rightPower_)
    {
        slimeController._controllerVibrationScript.Vibration("SlimeHaziku", leftPower_, rightPower_);

        yield return new WaitForSeconds(0.25f); //0.25�b�҂�
        slimeController._controllerVibrationScript.Destroy("SlimeHaziku");
    }
}
