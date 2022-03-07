using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController_Coalescence : MonoBehaviour
{
    //public enum State { MOVE, STOP, ATTACK, NORMAL, DIVISION, AIR };

    //関数処理管理
    public PlayerHaziku_Coalescence playerHazikuScript;
    public PlayerTearoff_Coalesecence playerTearoff;

    public bool hazikuUpdate;
    public bool tearoffUpdate;

    #region スライム関連 変数宣言
    [SerializeField, Tooltip("ステータス")]
    public State s_state;
    public Rigidbody2D rigid2D;
    #endregion

    public bool ifMiniPlayer;  //プレイヤが小さいかどうか

    // Start is called before the first frame update
    void Start()
    {
        s_state = State.NORMAL;
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        AllFalse_FunctionProcessingFlag();


        //大きいプレイヤの初期化
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
                //通常
                s_state = State.MOVE;
                break;

            case State.MOVE:
                //移動可

                //トリガーが押されている+Playerがちぎられていないなら
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
                //停止
                //未実装（内容不明）
                break;

            case State.ATTACK:
                //攻撃
                //未実装（内容不明）
                break;

            case State.DIVISION:
                //分裂

                break;

            case State.AIR:
                //空中にいるとき

                break;
        }

        #region 仮スライムステータス変化
        //StateChange();
        #endregion
    }



    //関数
    private void AllFalse_FunctionProcessingFlag()
    {
        hazikuUpdate = false;
        tearoffUpdate = false;
    }

    //両方のトリガーが押されているか確認
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
