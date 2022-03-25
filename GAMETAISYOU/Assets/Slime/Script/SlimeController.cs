using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController: MonoBehaviour
{
    //�֐������Ǘ�
    [SerializeField] Slime_Haziku hazikuScript;
    [SerializeField] Slime_Tearoff Tearoff;
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

    [System.NonSerialized] public float deadTime;  //�X���C���������鎞��

    public bool core;  //���̃X���C�����{�̂��ǂ���
    #endregion

    bool Ontrigger;         //�g���K�[��������Ă��邩�ǂ���

    [SerializeField] bool Debug_Ontrigger;         //�g���K�[�������Ă��邱�Ƃɂ���

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
    }

    // Update is called once per frame
    void Update()
    {
        AllFalse_FunctionProcessingFlag();
        Ontrigger = IfLRTriggerOn();

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

                //�g���K�[��������Ă���Ȃ�
                if (Ontrigger)
                {
                    tearoffUpdate = true;
                }
                else
                {
                    if((modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Trigger") : Input.GetAxis("R_Trigger")) >= 0.5f)
                    {
                        hazikuUpdate = true;
                    }
                    else
                    {
                        moveUpdate = true;
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
                break;
        }

        #region ���X���C���X�e�[�^�X�ω�
        //StateChange();
        #endregion


        liveTime += Time.deltaTime;

        //�X���C������
        if(liveTime >= deadTime && deadTime != 0)
        {
            #region �{�̂̑傫����߂�
            //�{�̂�T��
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            GameObject slimeCore = this.gameObject; //���Ŏ��g�����Ă���
            bool successSearch = false;
            //�z��̗v�f���ɑ΂��ď������s��
            foreach (GameObject i in slimes)
            {
                if(i.GetComponent<SlimeController>().core)  //�{�̂�������
                {
                    slimeCore = i;
                    successSearch = true;
                    break;
                }
            }

            //�傫����ύX����
            if(successSearch)
            {
                slimeCore.GetComponent<SlimeController>().scale += this.scale;
            }
            #endregion

            Destroy(this.gameObject);
        }

        //�X���C���̏k��
        if (liveTime >= stretchEndTime && pullWideForce != 0)
        {
            pullWideForce = 0;
        }

        //�X���C���̑傫���ύX
        if (Ontrigger)   //�L�΂��i�������j��ԂȂ�傫�����Œ肵�Ȃ�
        {
            pullWideForce = Tearoff.power;
        }
        else
        {
            pullWideForce = Mathf.Max(pullWideForce, Tearoff.power);
        }
        transform.localScale = new Vector2((1 + pullWideForce) * scale, (1.0f / (1 + pullWideForce)) * scale);

        //�X���C���̏d���ύX
        if(rigid2D.mass != scale * slimeBuf.slimeMass)
        {
            rigid2D.mass = scale * slimeBuf.slimeMass;
        }
    }



    //�֐�
    private void AllFalse_FunctionProcessingFlag()
    {
        hazikuUpdate = false;
        tearoffUpdate = false;
        moveUpdate = false;
    }

    //�����̃g���K�[��������Ă��邩�m�F
    private bool IfLRTriggerOn()
    {
        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        Debug_Ontrigger = Input.GetKey(KeyCode.Space);

        return triggerL == 1 && triggerR == 1 || Debug_Ontrigger;
    }


    private float easeOutExpo(float _x)
    {
        return _x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * _x);
    }

}
