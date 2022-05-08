using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController: MonoBehaviour
{
    public enum _Direction { Non, Right, Left };
    public _Direction _direction;

    public Animator _SlimeAnimator;

    public SlimeTrigger _triggerLeft;
    public SlimeTrigger _triggerRight;
    public SlimeTrigger _triggerUnder;
    public SlimeTrigger _triggerTop;


    //�֐������Ǘ�
    public Slime_Haziku _hazikuScript;
    public Slime_Tearoff _tearoff;
    public SlimeBuffer _slimeBuf;
    public SlimeTrampoline _trampoline;

    public bool hazikuUpdate;
    public bool tearoffUpdate;
    public bool moveUpdate;

    #region �X���C���֘A �ϐ��錾
    [SerializeField, Tooltip("�X�e�[�^�X")]
    public State s_state;
    public Rigidbody2D rigid2D;
    [System.NonSerialized] public float pullWideForce;    //�X���C�������ɂǂꂾ�����������Ă��邩
    public float _scaleMax;   //�X���C���̑傫��

    public enum LRMode { Left, Right };   //���E�X�e�B�b�N�̕���
    public LRMode modeLR;

    public float liveTime; //�X���C������������Ă��牽�b�o�߂��邩

    public float deadTime;  //�X���C���������鎞��

    public bool core;  //���̃X���C�����{�̂��ǂ���

    public bool _ifOperation;   //����ł���悤�ɂ��邩�ǂ���

    public float _moveSpeed;    //�X�e�B�b�N�ł̈ړ����x
    public float _stateAIRMoveSpeedMagnification;    //State��AIR�̎��̈ړ����x�{��
    public float _stateAIRMoveSpeedMax; //State��AIR�̎��̋󒆂ł̍ő呬�x
    #endregion

    public float _scaleNow; //���̃X���C���̑傫��
    float smoothCurrentVelocity;    //SmoothDamp�֐��p�ϐ�

    GameObject slimeCore;   //���g���{�̂ł͂Ȃ��Ƃ��ɃX���C���̖{�̂�����ϐ�
    SlimeController slimeCoreController;     //���g���{�̂ł͂Ȃ��Ƃ��ɃX���C���̖{�̂̃R���g���[���[������ϐ�

    bool Ontrigger;         //�g���K�[��������Ă��邩�ǂ���

    public bool _airJump;    //�󒆂ł��W�����v���ł��邩�ǂ���

    bool isDead;    //����ł邩�ǂ���

    #region �L�яk�ݏ���
    [System.NonSerialized] public float stretchEndTime;   //�X���C�����k�ݎn�߂鎞��
    [System.NonSerialized] public float pullWideForceMax; //�X���C�����ɂǂꂾ�����������Ă��邩�i�ő�j
    #endregion

    [SerializeField] SpriteRenderer slimeImage;

    public ControllerVibrationScript _controllerVibrationScript;

    public RaycastHit2D _rayHitFoot = new RaycastHit2D();

    bool offTriggerUnderOneFlameBefore; //1�t���[���O���n�ʂɗ����Ă��Ȃ��������ǂ���

    public bool _OnElevetor;    //�X���C�����G���x�[�^�[�ɏ���Ă��邩

    void Awake() 
    {
        _ifOperation = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        s_state = State.NORMAL;
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        _slimeBuf = GameObject.Find("SlimeBuffer").GetComponent<SlimeBuffer>();

        AllFalse_FunctionProcessingFlag();

        pullWideForce = 0;

        liveTime = 0;

        stretchEndTime = 0;

        if (_direction == _Direction.Non)
        {
            _direction = _Direction.Right;
        }

        _scaleNow = _scaleMax;

        isDead = false;

        _controllerVibrationScript = GameObject.Find("ControllerVibration").GetComponent<ControllerVibrationScript>();

        offTriggerUnderOneFlameBefore = false;

        _OnElevetor = false;
    }

    // Update is called once per frame
    void Update()
    {
        AllFalse_FunctionProcessingFlag();

        //����ł�Ȃ���Ɏ��񂾂Ƃ��̏������s��
        if (isDead) 
        {
            DeadUpdate();
            return; 
        }

        Gamepad gamepad = Gamepad.current;

        //�Q�[���p�b�h���ڑ�����Ă��Ȃ��Ȃ珈�����Ȃ�
        if (gamepad == null)
        {
            Debug.Log("�R���g���[���[���ڑ�����Ă��܂���");
            return;
        }

        Ontrigger = IsLRTriggerPressed();

        if(!Ontrigger)
        {
            _slimeBuf._ifTearOff = false;
        }

        switch (s_state)
        {
            case State.NORMAL:
                //�ʏ�
                s_state = State.MOVE;
                break;

            case State.MOVE:
                //�ړ���
                if (_ifOperation)
                {
                    //�g���K�[��������Ă���Ȃ�
                    if (Ontrigger)
                    {
                        //�����X���Ă��Ȃ��ꏊ�Ȃ�
                        if(Quaternion.FromToRotation(new Vector3(0, 1, 0), _rayHitFoot.normal).eulerAngles.z == 0)
                        {
                            tearoffUpdate = true;
                        }
                    }
                    else
                    {
                        //if ((modeLR ==�@LRMode.Left ? /*Input.GetAxis("L_Trigger")*/gamepad.leftTrigger.ReadValue() : /*Input.GetAxis("R_Trigger")*/gamepad.rightTrigger.ReadValue()) >= 0.9f)
                        if(IfHazikuUpdate(gamepad))
                        {
                            hazikuUpdate = true;
                        }
                        else
                        {
                            moveUpdate = true;
                        }
                    }
                }
                break;

            case State.STOP:
                //��~
                //�������i���e�s���j
                break;

            case State.ATTACK:
                //�U��
                //�������i���e�s���j
                break;

            case State.DIVISION:
                //����

                break;

            case State.AIR:
                //�󒆂ɂ���Ƃ�
                if (IfHazikuUpdate(gamepad) && _airJump && _ifOperation)
                {
                    hazikuUpdate = true;
                }
                else
                {
                    moveUpdate = true;
                }
                break;
        }

        #region ���X���C���X�e�[�^�X�ω�
        //StateChange();
        #endregion

        liveTime += Time.deltaTime;

        //���g���R�A�łȂ��Ȃ�
        if(!core)
        {
            if(!slimeCore)
            {
                //�{�̂�T��
                GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
                //�z��̗v�f���ɑ΂��ď������s��
                foreach (GameObject i in slimes)
                {
                    if (i.GetComponent<SlimeController>().core)  //�{�̂�������
                    {
                        slimeCore = i;
                        slimeCoreController = i.GetComponent<SlimeController>();
                        break;
                    }
                }
            }
            if(slimeCore)
            { 
                //�������Ԃ̉���
                if((slimeCore.transform.position - this.gameObject.transform.position).magnitude <= _slimeBuf._doNotDeadAreaRadius)
                {
                    deadTime = liveTime + slimeCoreController._tearoff.deadEndTime;
                }
            }
        }

        //�X���C���_��
        if(liveTime >= deadTime - 5 && deadTime != 0 && liveTime + _tearoff.deadEndTime - Time.deltaTime != deadTime)
        {
            Slime_Blinking();
        }
        else
        {
            slimeImage.enabled = true;
        }
        //�X���C������
        if (liveTime >= deadTime && deadTime != 0)
        {
            Slime_Destroy();
        }

        if(_scaleMax <= 0)
        {
            Destroy(this.gameObject);
        }

        //�X���C���̏k��
        if (liveTime >= stretchEndTime && pullWideForce != 0)
        {
            pullWideForce = 0;
        }

        //Ray���΂�
        RayFly();

        Slime_SizeChange();

        //�X���C���̏d���ύX
        if (rigid2D.mass != _scaleNow * _slimeBuf._slimeMass)
        {
            rigid2D.mass = _scaleNow * _slimeBuf._slimeMass;
        }

        Slime_Direction();

        //_SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
        _SlimeAnimator.SetBool("OnGround", _triggerUnder._onTrigger);

        //�n�ʂɂ��Ă�����
        if (_triggerUnder._onTrigger/* || SlimeGetRayHit()*/)
        {
            offTriggerUnderOneFlameBefore = false;
            _SlimeAnimator.SetFloat("FallSpeed", 0);

            //�A�j���[�V������Fall�������璅�n�ɂ���
            if (_SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Fall"))
            {
                _SlimeAnimator.SetTrigger("Landing");
            }

            //�X���C����State��AIR�ŌŒ肳��Ă��Ȃ����MOVE�ɕς���
            if (s_state == State.AIR && liveTime >= _hazikuScript._stateFixationTime)
            {
                s_state = State.MOVE;
                //rigid2D.velocity = Vector3.zero;
            }
        }
        else //���Ă��Ȃ�������
        {
            //1�t���[�������҂�
            if (offTriggerUnderOneFlameBefore)
            {
                //�X���C����State��MOVE��������AIR�ɕς���
                if (s_state == State.MOVE)
                {
                    s_state = State.AIR;
                }

                _SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
            }
            else
            {
                offTriggerUnderOneFlameBefore = true;
            } 
        }

    }



    //�S�Ẵt���O��false��
    private void AllFalse_FunctionProcessingFlag()
    {
        hazikuUpdate = false;
        tearoffUpdate = false;
        moveUpdate = false;
    }

    //�����̃g���K�[��������Ă��邩�m�F
    private bool IsLRTriggerPressed()
    {
        return Gamepad.current.leftTrigger.ReadValue() >= 0.9f && Gamepad.current.rightTrigger.ReadValue() >= 0.9f;
    }

    //�͂����̃A�b�v�f�[�g���s�����ǂ���
    private bool IfHazikuUpdate(Gamepad _gamepad)
    {
        if(liveTime >= _hazikuScript._nextHazikuUpdateTime)  //�N�[���^�C�����������珈�����Ȃ�
        {
            if (_ifOperation)   //���삪�ł���Ȃ�
            {
                if (modeLR == LRMode.Left ? _gamepad.leftStickButton.isPressed : _gamepad.rightStickButton.isPressed)    //�X�e�B�b�N��������Ă���Ȃ�
                {
                    return true;
                }
                else
                {
                    if (_hazikuScript._ifSlimeHazikuNow)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    //�X���C���̑傫���ύX
    private void Slime_SizeChange()
    {
        //�X���C���̑傫����傫������
        if (IfSlimeGrowInSize())
        {
            float scaleBefore = _scaleNow;
            _scaleNow = Mathf.SmoothDamp(_scaleNow, _scaleMax, ref smoothCurrentVelocity, 0.5f);

            //�傫�����ω����Ȃ�������ő�l�Ɠ����ɂ���i�s��΍�j
            if (scaleBefore == _scaleNow)
            {
                _scaleNow = _scaleMax;
            }
        }
        
        if (Ontrigger)   //�L�΂��i�������j��ԂȂ�傫�����Œ肵�Ȃ�
        {
            pullWideForce = _tearoff.power;
        }
        else
        {
            pullWideForce = Mathf.Max(pullWideForce, _tearoff.power);
        }

        int slimeDirection = Slime_Direction();

        transform.localScale = new Vector2((1 + pullWideForce) * _scaleNow * slimeDirection, (1.0f / (1 + pullWideForce)) * _scaleNow);
    }

    //�X���C���̓_��
    private void Slime_Blinking()
    {
        bool buf = liveTime % 0.5f <= 0.25f;
        slimeImage.enabled = buf;

        //�ڂ̓_��
        if(_hazikuScript._eyeRenderer)
        {
            _hazikuScript._eyeRenderer.enabled = buf;
        }
    }

    //�X���C���̌����ύX
    private int Slime_Direction()
    {
        //�����E
        if(rigid2D.velocity.x >= 0.01f && _direction == _Direction.Left) 
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            _direction = _Direction.Right;
        }
        //�E����
        if (rigid2D.velocity.x <= -0.01f && _direction == _Direction.Right)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            _direction = _Direction.Left;
        }

        return _direction == _Direction.Right ? 1 : -1;
    }

    //�X���C���̔j��
    public void Slime_Destroy()
    {
        if(!core)
        {
            isDead = true;

            _ifOperation = false;   //����s��
            _hazikuScript.enabled = false;
            _tearoff.enabled = false;
            _trampoline.enabled = false;
            rigid2D.simulated = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            //�͂����̖�������
            _hazikuScript._GuideDestroy();

            //�͂����̖ڂ�����
            _hazikuScript._AnimationEnd();
        }
    }

    //���񂾂Ƃ��̍X�V����
    private void DeadUpdate()
    {
        //�{�̂��Ȃ�������{�̂�T��
        if(!slimeCore)
        {
            #region �{�̂�T��
            //�{�̂�T��
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            bool successSearch = false;
            //�z��̗v�f���ɑ΂��ď������s��
            foreach (GameObject i in slimes)
            {
                if (i.GetComponent<SlimeController>().core)  //�{�̂�������
                {
                    slimeCore = i;
                    successSearch = true;
                    break;
                }
            }
            #endregion

            //����ł�������Ȃ������瑦�j��
            if (!successSearch)
            {
                Destroy(this.gameObject);
            }
        }

        //�R�A�ւ̃x�N�g�����v�Z
        Vector2 vec = slimeCore.transform.position - this.transform.position;   
        //�ړ�
        this.transform.position += new Vector3(vec.x * Time.deltaTime, vec.y * Time.deltaTime) ;

        //����������
        slimeImage.color += new Color(0, 0, 0, -Time.deltaTime);

        //�����Ȃ��Ȃ�����
        if(slimeImage.color.a <= 0)
        {
            //�傫����ύX����
            if (slimeCore)
            {
                slimeCore.GetComponent<SlimeController>()._scaleMax += this._scaleMax;
            }

            Destroy(this.gameObject);
        }
    }

    //�X���C�����傫���Ȃ�邩�ǂ���
    private bool IfSlimeGrowInSize()
    {
        //�G���x�[�^�[�ɏ���Ă����珈�����Ȃ�
        if (_OnElevetor)
        {
            return false;
        }

        //���݂̑傫���ƍő�l�������������珈�����Ȃ�
        if (_scaleMax == _scaleNow)
        {
            return false;
        }

        //�㉺or���E�����̂��̂ƐڐG���Ă�����false
        return !(_triggerLeft._onTrigger && _triggerRight._onTrigger || _triggerTop._onTrigger && _triggerUnder._onTrigger);
    }


    //Ray�𑫌��ɂȂ��ĂԂ������I�u�W�F�N�g�����m
    private void RayFly()
    {
        _rayHitFoot = new RaycastHit2D();   //������

        //Ray��^���ɔ�΂�
        Ray ray = new Ray(transform.position, Vector3.down);
        float distance = 0.3f;

        Debug.DrawRay(ray.origin, ray.direction * (_scaleNow / 2 + distance), Color.red);
        RaycastHit2D[] rayHits = Physics2D.RaycastAll(ray.origin, ray.direction, _scaleNow / 2 + distance);
        List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

        //�ꕔ�̃I�u�W�F�N�g�����O
        foreach (RaycastHit2D i in rayHits)
        {
            //�g���K�[�͏��O
            if(i.collider.isTrigger)
            {
                continue;
            }

            //�������g�͏��O
            if(i.collider.gameObject == this.gameObject)
            {
                continue;
            }

            //�ꕔ�̃^�O�̂����I�u�W�F�N�g�͏��O
            switch (i.collider.gameObject.tag)
            {
                case "SlimeTrigger": break;  //�X���C���g���K�[
                case "Tracking": break;      //�J�����g���b�L���O
                //�����ɒǉ�

                default:
                    raycastHits.Add(i);    //�ǉ�
                    break;
            }
        }

        //�X���C���Ɉ�ԋ߂��I�u�W�F�N�g���擾
        RaycastHit2D nearestRaycastHit = new RaycastHit2D();
        foreach (RaycastHit2D i in raycastHits)
        {
            if(!nearestRaycastHit)
            {
                nearestRaycastHit = i;
            }
            else
            {
                if(nearestRaycastHit.distance > i.distance)
                {
                    nearestRaycastHit = i;
                }
            }
        }

        //����������
        if(nearestRaycastHit)
        {
            _rayHitFoot = nearestRaycastHit;
            //Debug.Log(_rayHitFoot.collider.name);
        }
    }


    bool SlimeGetRayHit()
    {
        return _rayHitFoot;
    }
}
