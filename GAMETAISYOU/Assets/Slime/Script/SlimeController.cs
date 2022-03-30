using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController: MonoBehaviour
{
    public enum _Direction { Non, Right, Left };
    public _Direction _direction;

    public Animator _SlimeAnimator;

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
    public float scale;   //スライムの大きさ
    public enum LRMode { Left, Right };   //左右スティックの分岐
    public LRMode modeLR;

    public float liveTime; //スライムが生成されてから何秒経過するか

    public float deadTime;  //スライムが消える時間

    public bool core;  //このスライムが本体かどうか

    public bool _ifOperation;   //操作できるようにするかどうか
    #endregion

    bool Ontrigger;         //トリガーが押されているかどうか

    [SerializeField] bool Debug_Ontrigger;         //トリガーを押していることにする

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

        _direction = _Direction.Right;

        _ifOperation = true;
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
                        if ((modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Trigger") : Input.GetAxis("R_Trigger")) >= 0.5f)
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
                break;
        }

        #region 仮スライムステータス変化
        //StateChange();
        #endregion


        liveTime += Time.deltaTime;

        //スライム消滅
        if(liveTime >= deadTime && deadTime != 0)
        {
            #region 本体の大きさを戻す
            //本体を探す
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            GameObject slimeCore = this.gameObject; //仮で自身を入れておく
            bool successSearch = false;
            //配列の要素一つ一つに対して処理を行う
            foreach (GameObject i in slimes)
            {
                if(i.GetComponent<SlimeController>().core)  //本体だったら
                {
                    slimeCore = i;
                    successSearch = true;
                    break;
                }
            }

            //大きさを変更する
            if(successSearch)
            {
                slimeCore.GetComponent<SlimeController>().scale += this.scale;
            }
            #endregion

            Destroy(this.gameObject);
        }

        //スライムの縮み
        if (liveTime >= stretchEndTime && pullWideForce != 0)
        {
            pullWideForce = 0;
        }

        Slime_SizeChange();

        //スライムの重さ変更
        if (rigid2D.mass != scale * slimeBuf.slimeMass)
        {
            rigid2D.mass = scale * slimeBuf.slimeMass;
        }

        Slime_Direction();

        _SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
    }



    //関数
    private void AllFalse_FunctionProcessingFlag()
    {
        hazikuUpdate = false;
        tearoffUpdate = false;
        moveUpdate = false;
    }

    //両方のトリガーが押されているか確認
    private bool IfLRTriggerOn()
    {
        float triggerL = Input.GetAxis("L_Trigger");
        float triggerR = Input.GetAxis("R_Trigger");

        Debug_Ontrigger = Input.GetKey(KeyCode.Space);

        return triggerL == 1 && triggerR == 1 || Debug_Ontrigger;
    }


    //スライムの大きさ変更
    private void Slime_SizeChange()
    {
        if (Ontrigger)   //伸ばす（ちぎれる）状態なら大きさを固定しない
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

}
