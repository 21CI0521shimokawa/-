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


    //関数処理管理
    [SerializeField] Slime_Haziku hazikuScript;
    [SerializeField] Slime_Tearoff tearoff;
    public SlimeBuffer slimeBuf;

    public bool hazikuUpdate;
    public bool tearoffUpdate;
    public bool moveUpdate;

    #region スライム関連 変数宣言
    [SerializeField, Tooltip("ステータス")]
    public State s_state;
    public Rigidbody2D rigid2D;
    [System.NonSerialized] public float pullWideForce;    //スライムが横にどれだけ引っ張られているか
    public float _scaleMax;   //スライムの大きさ

    public enum LRMode { Left, Right };   //左右スティックの分岐
    public LRMode modeLR;

    public float liveTime; //スライムが生成されてから何秒経過するか

    public float deadTime;  //スライムが消える時間

    public bool core;  //このスライムが本体かどうか

    public bool _ifOperation;   //操作できるようにするかどうか

    public float _moveSpeed;    //スティックでの移動速度
    public float _stateAIRMoveSpeedMagnification;    //StateがAIRの時の移動速度倍率
    public float _stateAIRMoveSpeedMax; //StateがAIRの時の空中での最大速度
    #endregion

    public float _scaleNow; //今のスライムの大きさ
    float smoothCurrentVelocity;    //SmoothDamp関数用変数


    bool Ontrigger;         //トリガーが押されているかどうか

    #region 伸び縮み処理
    [System.NonSerialized] public float stretchEndTime;   //スライムが縮み始める時間
    [System.NonSerialized] public float pullWideForceMax; //スライム横にどれだけ引っ張られているか（最大）
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

        _scaleNow = _scaleMax;
    }

    // Update is called once per frame
    void Update()
    {
        AllFalse_FunctionProcessingFlag();

        Gamepad gamepad = Gamepad.current;

        //ゲームパッドが接続されていないなら処理しない
        if (gamepad == null)
        {
            Debug.Log("コントローラーが接続されていません");
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
                //通常
                s_state = State.MOVE;
                break;

            case State.MOVE:
                //移動可
                if (_ifOperation)
                {
                    //トリガーが押されているなら
                    if (Ontrigger)
                    {
                        tearoffUpdate = true;
                    }
                    else
                    {
                        //if ((modeLR ==　LRMode.Left ? /*Input.GetAxis("L_Trigger")*/gamepad.leftTrigger.ReadValue() : /*Input.GetAxis("R_Trigger")*/gamepad.rightTrigger.ReadValue()) >= 0.9f)
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

                moveUpdate = true;

                break;
        }

        #region 仮スライムステータス変化
        //StateChange();
        #endregion

        liveTime += Time.deltaTime;

        //スライム消滅
        if(liveTime >= deadTime && deadTime != 0)
        {
            Slime_Destroy();
        }

        if(_scaleMax <= 0)
        {
            Destroy(this.gameObject);
        }

        //スライムの縮み
        if (liveTime >= stretchEndTime && pullWideForce != 0)
        {
            pullWideForce = 0;
        }

        Slime_SizeChange();

        //スライムの重さ変更
        if (rigid2D.mass != _scaleNow * slimeBuf.slimeMass)
        {
            rigid2D.mass = _scaleNow * slimeBuf.slimeMass;
        }

        Slime_Direction();

        _SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
        _SlimeAnimator.SetBool("OnGround", _triggerUnder._onTrigger);

        //地面についていたら
        if (_triggerUnder._onTrigger)
        {
            //アニメーションがFallだったら着地にする
            if (_SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Fall"))
            {
                _SlimeAnimator.SetTrigger("Landing");
            }

            //スライムのStateがAIRで固定されていなければMOVEに変える
            if (s_state == State.AIR && liveTime >= hazikuScript._stateFixationTime)
            {
                s_state = State.MOVE;
                //rigid2D.velocity = Vector3.zero;
            }
        }
    }



    //全てのフラグをfalseに
    private void AllFalse_FunctionProcessingFlag()
    {
        hazikuUpdate = false;
        tearoffUpdate = false;
        moveUpdate = false;
    }

    //両方のトリガーが押されているか確認
    private bool IsLRTriggerPressed()
    {
        return Gamepad.current.leftTrigger.ReadValue() >= 0.9f && Gamepad.current.rightTrigger.ReadValue() >= 0.9f;
    }

    //はじくのアップデートを行うかどうか
    private bool IfHazikuUpdate(Gamepad _gamepad)
    {
        if(liveTime >= hazikuScript._nextHazikuUpdateTime)  //クールタイム中だったら処理しない
        {
            if (s_state == State.MOVE && _ifOperation)   //はじくのアップデートを行える状況なら
            {
                if (modeLR == LRMode.Left ? _gamepad.leftStickButton.isPressed : _gamepad.rightStickButton.isPressed)    //スティックが押されているなら
                {
                    return true;
                }
                else
                {
                    if (hazikuScript._ifSlimeHazikuNow)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }


    //スライムの大きさ変更
    private void Slime_SizeChange()
    {
        //スライムの大きさを大きくする
        if (IfSlimeGrowInSize())
        {
            float scaleBefore = _scaleNow;
            _scaleNow = Mathf.SmoothDamp(_scaleNow, _scaleMax, ref smoothCurrentVelocity, 0.5f);

            //大きさが変化しなかったら最大値と同じにする（不具合対策）
            if (scaleBefore == _scaleNow)
            {
                _scaleNow = _scaleMax;
            }
        }
        
        if (Ontrigger)   //伸ばす（ちぎれる）状態なら大きさを固定しない
        {
            pullWideForce = tearoff.power;
        }
        else
        {
            pullWideForce = Mathf.Max(pullWideForce, tearoff.power);
        }

        int slimeDirection = Slime_Direction();

        transform.localScale = new Vector2((1 + pullWideForce) * _scaleNow * slimeDirection, (1.0f / (1 + pullWideForce)) * _scaleNow);
    }

    //スライムの向き変更
    private int Slime_Direction()
    {
        //左→右
        if(rigid2D.velocity.x >= 0.01f && _direction == _Direction.Left) 
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
            _direction = _Direction.Right;
        }
        //右→左
        if (rigid2D.velocity.x <= -0.01f && _direction == _Direction.Right)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
            _direction = _Direction.Left;
        }

        return _direction == _Direction.Right ? 1 : -1;
    }

    //スライムの破棄
    public bool Slime_Destroy()
    {
        if(!core)
        {
            #region 本体の大きさを戻す
            //本体を探す
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            GameObject slimeCore = this.gameObject; //仮で自身を入れておく
            bool successSearch = false;
            //配列の要素一つ一つに対して処理を行う
            foreach (GameObject i in slimes)
            {
                if (i.GetComponent<SlimeController>().core)  //本体だったら
                {
                    slimeCore = i;
                    successSearch = true;
                    break;
                }
            }

            //大きさを変更する
            if (successSearch)
            {
                slimeCore.GetComponent<SlimeController>()._scaleMax += this._scaleMax;
            }
            #endregion

            //はじくの矢印を消す
            hazikuScript._ArrowDestroy();

            Destroy(this.gameObject);
        }
        return false;
    }


    //スライムが大きくなれるかどうか
    private bool IfSlimeGrowInSize()
    {
        //現在の大きさと最大値が同じだったら処理しない
        if(_scaleMax == _scaleNow)
        {
            return false;
        }

        //上下or左右が他のものと接触していたらfalse
        return !(_triggerLeft._onTrigger && _triggerRight._onTrigger || _triggerTop._onTrigger && _triggerUnder._onTrigger);
    }
}
