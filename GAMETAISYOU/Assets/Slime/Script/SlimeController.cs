using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeController: MonoBehaviour
{
    public enum _Direction { Non, Right, Left };
    public _Direction _direction;

    public Animator _SlimeAnimator;

    //�֐������Ǘ�
    [SerializeField] Slime_Haziku hazikuScript;
    [SerializeField] Slime_Tearoff tearoff;
    public SlimeBuffer slimeBuf;

    public bool hazikuUpdate;
    public bool tearoffUpdate;
    public bool moveUpdate;

    #region �X���C���֘A �ϐ��錾
    [SerializeField, Tooltip("�X�e�[�^�X")]
    public State s_state;
    public Rigidbody2D rigid2D;
    [System.NonSerialized] public float pullWideForce;    //�X���C�������ɂǂꂾ�����������Ă��邩
    public float scale;   //�X���C���̑傫��
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

    bool Ontrigger;         //�g���K�[��������Ă��邩�ǂ���

    #region �L�яk�ݏ���
    [System.NonSerialized] public float stretchEndTime;   //�X���C�����k�ݎn�߂鎞��
    [System.NonSerialized] public float pullWideForceMax; //�X���C�����ɂǂꂾ�����������Ă��邩�i�ő�j
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        s_state = State.NORMAL;
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        slimeBuf = GameObject.Find("SlimeBuffer").GetComponent<SlimeBuffer>();

        AllFalse_FunctionProcessingFlag();

        pullWideForce = 0;

        liveTime = 0;

        stretchEndTime = 0;

        if (_direction == _Direction.Non)
        {
            _direction = _Direction.Right;
        }

        _ifOperation = true;
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
            slimeBuf.ifTearOff = false;
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
                        if(IsHazikuUpdate(gamepad))
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

                moveUpdate = true;

                break;
        }

        #region ���X���C���X�e�[�^�X�ω�
        //StateChange();
        #endregion

        liveTime += Time.deltaTime;

        //�X���C������
        if(liveTime >= deadTime && deadTime != 0)
        {
            Slime_Destroy();
        }

        if(scale <= 0)
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
        if (rigid2D.mass != scale * slimeBuf.slimeMass)
        {
            rigid2D.mass = scale * slimeBuf.slimeMass;
        }

        Slime_Direction();

        _SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
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
    private bool IsHazikuUpdate(Gamepad _gamepad)
    {
        if(s_state == State.MOVE && _ifOperation)   //�͂����̃A�b�v�f�[�g���s����󋵂Ȃ�
        {
            if(modeLR == LRMode.Left ? _gamepad.leftStickButton.isPressed : _gamepad.rightStickButton.isPressed)    //�X�e�B�b�N��������Ă���Ȃ�
            {
                return true;
            }
            else
            {
                if(hazikuScript._ifSlimeHazikuNow)
                {
                    return true;
                }
            }
        }
        return false;
    }


    //�X���C���̑傫���ύX
    private void Slime_SizeChange()
    {
        if (Ontrigger)   //�L�΂��i�������j��ԂȂ�傫�����Œ肵�Ȃ�
        {
            pullWideForce = tearoff.power;
        }
        else
        {
            pullWideForce = Mathf.Max(pullWideForce, tearoff.power);
        }

        int slimeDirection = Slime_Direction();

        transform.localScale = new Vector2((1 + pullWideForce) * scale * slimeDirection, (1.0f / (1 + pullWideForce)) * scale);
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
                slimeCore.GetComponent<SlimeController>().scale += this.scale;
            }
            #endregion

            Destroy(this.gameObject);
        }
        return false;
    }

}
