using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Move : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;
    [SerializeField] Rigidbody2D rigidBody;

    float speed;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        SpeedSetting();

        if (slimeController.moveUpdate)
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
                    moveForceX = StateAIRMoveForceCheck(moveForceX);

                    if (StateAIRMoveWallCheck())
                    {
                        //X���ɗ͂𑫂�
                        Vector2 force = new Vector2(moveForceX, 0);    // �͂�ݒ�
                        rigidBody.AddForce(force);
                    }
                }
                else
                {
                    //X���ɑ΂��Ă������Ă���͂�����
                    Vector2 force = new Vector2(moveForceX, rigidBody.velocity.y);    // �͂�ݒ�
                    rigidBody.velocity = force;
                }
            }

            slimeController._SlimeAnimator.SetFloat("MoveSpeed", Mathf.Abs(rigidBody.velocity.x));
        }
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
    bool StateAIRMoveWallCheck()
    {
        return !slimeController._triggerRight._onTrigger;
    }
}
