using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController_Coalescence : MonoBehaviour
{
    //public enum State { MOVE, STOP, ATTACK, NORMAL, DIVISION, AIR };

    //�֐������Ǘ�
    public PlayerHaziku_Coalescence playerHazikuScript;
    public PlayerTearoff_Coalesecence playerTearoff;

    public bool hazikuUpdate;
    public bool tearoffUpdate;

    #region �X���C���֘A �ϐ��錾
    [SerializeField, Tooltip("�X�e�[�^�X")]
    public State s_state;
    public Rigidbody2D rigid2D;
    #endregion

    public bool ifMiniPlayer;  //�v���C�������������ǂ���

    // Start is called before the first frame update
    void Start()
    {
        s_state = State.NORMAL;
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        AllFalse_FunctionProcessingFlag();


        //�傫���v���C���̏�����
        if (!ifMiniPlayer)
        {
            playerHazikuScript.modeLR = PlayerHaziku_Coalescence.LRMode.Left;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AllFalse_FunctionProcessingFlag();

        switch (s_state)
        {
            case State.NORMAL:
                //�ʏ�
                s_state = State.MOVE;
                break;

            case State.MOVE:
                //�ړ���

                //�g���K�[��������Ă���+Player���������Ă��Ȃ��Ȃ�
                if (IfLRTriggerOn() && !ifMiniPlayer)
                {
                    tearoffUpdate = true;
                }
                else
                {
                    hazikuUpdate = true;
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
    }



    //�֐�
    private void AllFalse_FunctionProcessingFlag()
    {
        hazikuUpdate = false;
        tearoffUpdate = false;
    }

    //�����̃g���K�[��������Ă��邩�m�F
    private bool IfLRTriggerOn()
    {
        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        return triggerL == 1 && triggerR == 1;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        //rigid2D.velocity = Vector3.zero;
    }
}
