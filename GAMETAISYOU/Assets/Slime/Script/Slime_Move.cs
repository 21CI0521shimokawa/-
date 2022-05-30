using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Move : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;
    [SerializeField] Rigidbody2D rigidBody;

    float speed;
    [SerializeField] float airPowerMagnificationSlimeScale;    //�X���C����1�傫���Ȃ邲�Ƃɗ͂����{�ɂȂ邩

    float noMoveTime;   //�ړ����ł��Ȃ�����

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        speed = 0;
        noMoveTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedSetting();

        //�����Ȃ����ԃJ�E���g
        if (noMoveTime > 0)
        {
            noMoveTime = Mathf.Max(noMoveTime - Time.deltaTime, 0);
        }

        if (slimeController.moveUpdate && noMoveTime == 0)
        {
            float horizontal = slimeController.modeLR == SlimeController.LRMode.Left ? /*Input.GetAxisRaw("L_Stick_Horizontal")*/Gamepad.current.leftStick.x.ReadValue() : /*Input.GetAxisRaw("R_Stick_Horizontal")*/Gamepad.current.rightStick.x.ReadValue();
            if (horizontal > 0.2f) { horizontal = 1; }
            else if (horizontal < -0.2f) { horizontal = -1; }
            else { horizontal = 0; }

            float moveForceX = horizontal * speed;

            if (moveForceX != 0)
            {
                if (slimeController.s_state == State.AIR)
                {

                    moveForceX *= slimeController._scaleNow;
                    moveForceX *= Time.deltaTime;

                    moveForceX = StateAIRMoveForceCheck(moveForceX);

                    if (StateAIRMoveWallCheck(moveForceX))
                    {
                        //X���ɗ͂𑫂�
                        Vector2 force = new Vector2(moveForceX, 0);    // �͂�ݒ�
                        //force *= Mathf.Pow(airPowerMagnificationSlimeScale, slimeController._scaleNow);
                        rigidBody.AddForce(force, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    //�������g�����|�����������珈�����Ȃ�
                    if(!IsFootTrampoline())
                    {
                        Vector2 force = new Vector2(moveForceX, 0);    // �͂�ݒ�
                        //�ړ��x�N�g�������̊p�x�ɉ����ĉ�]
                        if (slimeController._rayHitFoot)
                        {
                            //�������X���C��or�΂������珈�����Ȃ�
                            if (!(IsFootObjectCheckTag("Slime") || IsFootObjectCheckTag("Item")))
                            {
                                Debug.Log(slimeController._rayHitFoot.normal);
                                //force = Quaternion.Euler(Quaternion.FromToRotation(transform.up, slimeController._rayHitFoot.normal).eulerAngles) * force;
                                //force = Quaternion.Euler(Quaternion.FromToRotation(Vector3.up, slimeController._rayHitFoot.normal).eulerAngles) * force;
                                force = Quaternion.Euler(slimeController._FloorAngle()) * force;
                            }
                        }

                        rigidBody.velocity = force;

                    }
                    
                    //�������Ă���͂�����
                    //Vector2 force = new Vector2(moveForceX, rigidBody.velocity.y);    // �͂�ݒ�
                    //rigidBody.velocity = force;
                }
            }
        }

        
        slimeController._SlimeAnimator.SetFloat("MoveSpeed", Mathf.Abs(rigidBody.velocity.x));
    }

    //���x�ݒ�
    void SpeedSetting()
    {
        speed = slimeController._moveSpeed;

        //AIR�̎��̑��x
        if (slimeController.s_state == State.AIR)
        {
            speed *= slimeController._stateAIRMoveSpeedMagnification;
        }
    }


    //X���̑��x���ő�l��菬�������ɍő�l���傫���Ȃ�Ȃ��悤�ɂ���
    float StateAIRMoveForceCheck(float _moveForceX)
    {
        //���i��ł�������Ɨ͂�������������Ⴄ�Ȃ�(rigidBody.velocity.x��0�Ȃ�)
        if (Mathf.Sign(rigidBody.velocity.x) != Mathf.Sign(_moveForceX))
        {
            return _moveForceX;
        }

        //���̑��x���ő呬�x�ȏ�Ȃ�
        if (Mathf.Abs(rigidBody.velocity.x) >= Mathf.Abs(slimeController._stateAIRMoveSpeedMax))
        {
            return 0;
        }

        //�͂̌����ɉ����ĕ���
        if (_moveForceX > 0)
        {
            //���x�𑫂������ʍő呬�x�������Ă��܂��Ȃ�ő呬�x�ɂȂ�悤�ɒ�������
            if (rigidBody.velocity.x + _moveForceX > slimeController._stateAIRMoveSpeedMax)
            {
                return slimeController._stateAIRMoveSpeedMax - rigidBody.velocity.x;
            }
        }
        else
        {
            //���x�𑫂������ʍő呬�x�������Ă��܂��Ȃ�ő呬�x�ɂȂ�悤�ɒ�������
            if (rigidBody.velocity.x + _moveForceX < -slimeController._stateAIRMoveSpeedMax)
            {
                return -slimeController._stateAIRMoveSpeedMax - rigidBody.velocity.x;
            }
        }

        return _moveForceX;
    }


    //�i�����Ƃ��Ă�������ɕǂ͂Ȃ����m�F    
    bool StateAIRMoveWallCheck(float moveForceX)
    {
        int moveDirection = (int)Mathf.Sign(moveForceX);
        int slimeAngle = slimeController._direction == SlimeController._Direction.Right ? 1 : -1;

        return !(moveDirection * slimeAngle >= 0 ? slimeController._triggerRight._onTrigger : slimeController._triggerLeft._onTrigger);
    }

    //�X���C�����g�����|�����𓥂�ł��邩�ǂ���
    bool IsFootTrampoline()
    {
        if (slimeController._rayHitFoot)
        {
            if (slimeController._rayHitFoot.collider.gameObject.tag == "Trampoline")
            {
                noMoveTime = 0.2f;  //0.2�b�Ԉړ����ł��Ȃ�
                return true;
            }
        }

        return false;
    }

    //�����̃I�u�W�F�N�g�̃^�O����v���Ă�����true
    bool IsFootObjectCheckTag(string tag)
    {
        if (slimeController._rayHitFoot)
        {
            if (slimeController._rayHitFoot.collider.gameObject.tag == tag)
            {
                return true;
            }
        }

        return false;
    }
}
