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
                        //X軸に力を足す
                        Vector2 force = new Vector2(moveForceX, 0);    // 力を設定
                        rigidBody.AddForce(force);
                    }
                }
                else
                {
                    //X軸に対してかかっている力を消す
                    Vector2 force = new Vector2(moveForceX, rigidBody.velocity.y);    // 力を設定
                    rigidBody.velocity = force;
                }
            }

            slimeController._SlimeAnimator.SetFloat("MoveSpeed", Mathf.Abs(rigidBody.velocity.x));
        }
    }

    //速度設定
    void SpeedSetting()
    {
        speed = slimeController._moveSpeed;

        //AIRの時の速度
        if (slimeController.s_state == State.AIR)
        {
            speed *= slimeController._stateAIRMoveSpeedMagnification;
        }
    }


    //X軸の速度が最大値より小さい時に最大値より大きくならないようにする
    float StateAIRMoveForceCheck(float _moveForceX)
    {
        //今進んでいる方向と力をかける方向が違うなら(rigidBody.velocity.xが0なら)
        if (Mathf.Sign(rigidBody.velocity.x) != Mathf.Sign(_moveForceX))
        {
            return _moveForceX;
        }

        //今の速度が最大速度以上なら
        if (Mathf.Abs(rigidBody.velocity.x) >= Mathf.Abs(slimeController._stateAIRMoveSpeedMax))
        {
            return 0;
        }

        //力の向きに応じて分岐
        if (_moveForceX > 0)
        {
            //速度を足した結果最大速度をこえてしまうなら最大速度になるように調整する
            if (rigidBody.velocity.x + _moveForceX > slimeController._stateAIRMoveSpeedMax)
            {
                return slimeController._stateAIRMoveSpeedMax - rigidBody.velocity.x;
            }
        }
        else
        {
            //速度を足した結果最大速度をこえてしまうなら最大速度になるように調整する
            if (rigidBody.velocity.x + _moveForceX < -slimeController._stateAIRMoveSpeedMax)
            {
                return -slimeController._stateAIRMoveSpeedMax - rigidBody.velocity.x;
            }
        }

        return _moveForceX;
    }


    //進もうとしている方向に壁はないか確認    
    bool StateAIRMoveWallCheck()
    {
        return !slimeController._triggerRight._onTrigger;
    }
}
