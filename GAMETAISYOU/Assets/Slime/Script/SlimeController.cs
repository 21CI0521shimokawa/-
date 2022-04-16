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

    #region �L�яk�ݏ���
    [System.NonSerialized] public float stretchEndTime;   //�X���C�����k�ݎn�߂鎞��
    [System.NonSerialized] public float pullWideForceMax; //�X���C�����ɂǂꂾ�����������Ă��邩�i�ő�j
    #endregion

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

        _ifOperation = true;

        _scaleNow = _scaleMax;
    }

    // Update is called once per frame
    void Update()
    {
        AllFalse_FunctionProcessingFlag();

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
                        tearoffUpdate = true;
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
                if (IfHazikuUpdate(gamepad) && _airJump)
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

        Slime_SizeChange();

        //�X���C���̏d���ύX
        if (rigid2D.mass != _scaleNow * _slimeBuf._slimeMass)
        {
            rigid2D.mass = _scaleNow * _slimeBuf._slimeMass;
        }

        Slime_Direction();

        _SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
        _SlimeAnimator.SetBool("OnGround", _triggerUnder._onTrigger);

        //�n�ʂɂ��Ă�����
        if (_triggerUnder._onTrigger)
        {
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
    public bool Slime_Destroy()
    {
        if(!core)
        {
            #region �{�̂̑傫����߂�
            //�{�̂�T��
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            GameObject slimeCore = this.gameObject; //���Ŏ��g�����Ă���
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

            //�傫����ύX����
            if (successSearch)
            {
                slimeCore.GetComponent<SlimeController>()._scaleMax += this._scaleMax;
            }
            #endregion

            //�͂����̖�������
            _hazikuScript._ArrowDestroy();

            Destroy(this.gameObject);
        }
        return false;
    }


    //�X���C�����傫���Ȃ�邩�ǂ���
    private bool IfSlimeGrowInSize()
    {
        //���݂̑傫���ƍő�l�������������珈�����Ȃ�
        if(_scaleMax == _scaleNow)
        {
            return false;
        }

        //�㉺or���E�����̂��̂ƐڐG���Ă�����false
        return !(_triggerLeft._onTrigger && _triggerRight._onTrigger || _triggerTop._onTrigger && _triggerUnder._onTrigger);
    }
}
