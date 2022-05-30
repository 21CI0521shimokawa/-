using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Move : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;
    [SerializeField] Rigidbody2D rigidBody;

    float speed;
    [SerializeField] float airPowerMagnificationSlimeScale;    //スライムが1大きくなるごとに力が何倍になるか

    float noMoveTime;   //移動ができない時間

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

        //動けない時間カウント
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
                        //X軸に力を足す
                        Vector2 force = new Vector2(moveForceX, 0);    // 力を設定
                        //force *= Mathf.Pow(airPowerMagnificationSlimeScale, slimeController._scaleNow);
                        rigidBody.AddForce(force, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    //足元がトランポリンだったら処理しない
                    if(!IsFootTrampoline())
                    {
                        Vector2 force = new Vector2(moveForceX, 0);    // 力を設定
                        //移動ベクトルを床の角度に応じて回転
                        if (slimeController._rayHitFoot)
                        {
                            //足元がスライムor石だったら処理しない
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
                    
                    //かかっている力を消す
                    //Vector2 force = new Vector2(moveForceX, rigidBody.velocity.y);    // 力を設定
                    //rigidBody.velocity = force;
                }
            }
        }

        
        slimeController._SlimeAnimator.SetFloat("MoveSpeed", Mathf.Abs(rigidBody.velocity.x));
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
    bool StateAIRMoveWallCheck(float moveForceX)
    {
        int moveDirection = (int)Mathf.Sign(moveForceX);
        int slimeAngle = slimeController._direction == SlimeController._Direction.Right ? 1 : -1;

        return !(moveDirection * slimeAngle >= 0 ? slimeController._triggerRight._onTrigger : slimeController._triggerLeft._onTrigger);
    }

    //スライムがトランポリンを踏んでいるかどうか
    bool IsFootTrampoline()
    {
        if (slimeController._rayHitFoot)
        {
            if (slimeController._rayHitFoot.collider.gameObject.tag == "Trampoline")
            {
                noMoveTime = 0.2f;  //0.2秒間移動ができない
                return true;
            }
        }

        return false;
    }

    //足元のオブジェクトのタグが一致していたらtrue
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
