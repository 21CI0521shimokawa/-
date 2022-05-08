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
    public Slime_Haziku _hazikuScript;
    public Slime_Tearoff _tearoff;
    public SlimeBuffer _slimeBuf;
    public SlimeTrampoline _trampoline;

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

    GameObject slimeCore;   //自身が本体ではないときにスライムの本体が入る変数
    SlimeController slimeCoreController;     //自身が本体ではないときにスライムの本体のコントローラーが入る変数

    bool Ontrigger;         //トリガーが押されているかどうか

    public bool _airJump;    //空中でもジャンプができるかどうか

    bool isDead;    //死んでるかどうか

    #region 伸び縮み処理
    [System.NonSerialized] public float stretchEndTime;   //スライムが縮み始める時間
    [System.NonSerialized] public float pullWideForceMax; //スライム横にどれだけ引っ張られているか（最大）
    #endregion

    [SerializeField] SpriteRenderer slimeImage;

    public ControllerVibrationScript _controllerVibrationScript;

    public RaycastHit2D _rayHitFoot = new RaycastHit2D();

    bool offTriggerUnderOneFlameBefore; //1フレーム前も地面に立っていなかったかどうか

    public bool _OnElevetor;    //スライムがエレベーターに乗っているか

    void Awake() 
    {
        _ifOperation = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        s_state = State.NORMAL;
        rigid2D = this.gameObject.GetComponent<Rigidbody2D>();

        _slimeBuf = GameObject.Find("SlimeBuffer").GetComponent<SlimeBuffer>();

        AllFalse_FunctionProcessingFlag();

        pullWideForce = 0;

        liveTime = 0;

        stretchEndTime = 0;

        if (_direction == _Direction.Non)
        {
            _direction = _Direction.Right;
        }

        _scaleNow = _scaleMax;

        isDead = false;

        _controllerVibrationScript = GameObject.Find("ControllerVibration").GetComponent<ControllerVibrationScript>();

        offTriggerUnderOneFlameBefore = false;

        _OnElevetor = false;
    }

    // Update is called once per frame
    void Update()
    {
        AllFalse_FunctionProcessingFlag();

        //死んでるなら既に死んだときの処理を行う
        if (isDead) 
        {
            DeadUpdate();
            return; 
        }

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
            _slimeBuf._ifTearOff = false;
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
                        //床が傾いていない場所なら
                        if(Quaternion.FromToRotation(new Vector3(0, 1, 0), _rayHitFoot.normal).eulerAngles.z == 0)
                        {
                            tearoffUpdate = true;
                        }
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
                if (IfHazikuUpdate(gamepad) && _airJump && _ifOperation)
                {
                    hazikuUpdate = true;
                }
                else
                {
                    moveUpdate = true;
                }
                break;
        }

        #region 仮スライムステータス変化
        //StateChange();
        #endregion

        liveTime += Time.deltaTime;

        //自身がコアでないなら
        if(!core)
        {
            if(!slimeCore)
            {
                //本体を探す
                GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
                //配列の要素一つ一つに対して処理を行う
                foreach (GameObject i in slimes)
                {
                    if (i.GetComponent<SlimeController>().core)  //本体だったら
                    {
                        slimeCore = i;
                        slimeCoreController = i.GetComponent<SlimeController>();
                        break;
                    }
                }
            }
            if(slimeCore)
            { 
                //生存時間の延長
                if((slimeCore.transform.position - this.gameObject.transform.position).magnitude <= _slimeBuf._doNotDeadAreaRadius)
                {
                    deadTime = liveTime + slimeCoreController._tearoff.deadEndTime;
                }
            }
        }

        //スライム点滅
        if(liveTime >= deadTime - 5 && deadTime != 0 && liveTime + _tearoff.deadEndTime - Time.deltaTime != deadTime)
        {
            Slime_Blinking();
        }
        else
        {
            slimeImage.enabled = true;
        }
        //スライム消滅
        if (liveTime >= deadTime && deadTime != 0)
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

        //Rayを飛ばす
        RayFly();

        Slime_SizeChange();

        //スライムの重さ変更
        if (rigid2D.mass != _scaleNow * _slimeBuf._slimeMass)
        {
            rigid2D.mass = _scaleNow * _slimeBuf._slimeMass;
        }

        Slime_Direction();

        //_SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
        _SlimeAnimator.SetBool("OnGround", _triggerUnder._onTrigger);

        //地面についていたら
        if (_triggerUnder._onTrigger/* || SlimeGetRayHit()*/)
        {
            offTriggerUnderOneFlameBefore = false;
            _SlimeAnimator.SetFloat("FallSpeed", 0);

            //アニメーションがFallだったら着地にする
            if (_SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Fall"))
            {
                _SlimeAnimator.SetTrigger("Landing");
            }

            //スライムのStateがAIRで固定されていなければMOVEに変える
            if (s_state == State.AIR && liveTime >= _hazikuScript._stateFixationTime)
            {
                s_state = State.MOVE;
                //rigid2D.velocity = Vector3.zero;
            }
        }
        else //ついていなかったら
        {
            //1フレームだけ待つ
            if (offTriggerUnderOneFlameBefore)
            {
                //スライムのStateがMOVEだったらAIRに変える
                if (s_state == State.MOVE)
                {
                    s_state = State.AIR;
                }

                _SlimeAnimator.SetFloat("FallSpeed", rigid2D.velocity.y);
            }
            else
            {
                offTriggerUnderOneFlameBefore = true;
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
        if(liveTime >= _hazikuScript._nextHazikuUpdateTime)  //クールタイム中だったら処理しない
        {
            if (_ifOperation)   //操作ができるなら
            {
                if (modeLR == LRMode.Left ? _gamepad.leftStickButton.isPressed : _gamepad.rightStickButton.isPressed)    //スティックが押されているなら
                {
                    return true;
                }
                else
                {
                    if (_hazikuScript._ifSlimeHazikuNow)
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
            pullWideForce = _tearoff.power;
        }
        else
        {
            pullWideForce = Mathf.Max(pullWideForce, _tearoff.power);
        }

        int slimeDirection = Slime_Direction();

        transform.localScale = new Vector2((1 + pullWideForce) * _scaleNow * slimeDirection, (1.0f / (1 + pullWideForce)) * _scaleNow);
    }

    //スライムの点滅
    private void Slime_Blinking()
    {
        bool buf = liveTime % 0.5f <= 0.25f;
        slimeImage.enabled = buf;

        //目の点滅
        if(_hazikuScript._eyeRenderer)
        {
            _hazikuScript._eyeRenderer.enabled = buf;
        }
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
    public void Slime_Destroy()
    {
        if(!core)
        {
            isDead = true;

            _ifOperation = false;   //操作不可
            _hazikuScript.enabled = false;
            _tearoff.enabled = false;
            _trampoline.enabled = false;
            rigid2D.simulated = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;

            //はじくの矢印を消す
            _hazikuScript._GuideDestroy();

            //はじくの目を消す
            _hazikuScript._AnimationEnd();
        }
    }

    //死んだときの更新処理
    private void DeadUpdate()
    {
        //本体がなかったら本体を探す
        if(!slimeCore)
        {
            #region 本体を探す
            //本体を探す
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
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
            #endregion

            //それでも見つからなかったら即破棄
            if (!successSearch)
            {
                Destroy(this.gameObject);
            }
        }

        //コアへのベクトルを計算
        Vector2 vec = slimeCore.transform.position - this.transform.position;   
        //移動
        this.transform.position += new Vector3(vec.x * Time.deltaTime, vec.y * Time.deltaTime) ;

        //少しずつ薄く
        slimeImage.color += new Color(0, 0, 0, -Time.deltaTime);

        //見えなくなったら
        if(slimeImage.color.a <= 0)
        {
            //大きさを変更する
            if (slimeCore)
            {
                slimeCore.GetComponent<SlimeController>()._scaleMax += this._scaleMax;
            }

            Destroy(this.gameObject);
        }
    }

    //スライムが大きくなれるかどうか
    private bool IfSlimeGrowInSize()
    {
        //エレベーターに乗っていたら処理しない
        if (_OnElevetor)
        {
            return false;
        }

        //現在の大きさと最大値が同じだったら処理しない
        if (_scaleMax == _scaleNow)
        {
            return false;
        }

        //上下or左右が他のものと接触していたらfalse
        return !(_triggerLeft._onTrigger && _triggerRight._onTrigger || _triggerTop._onTrigger && _triggerUnder._onTrigger);
    }


    //Rayを足元になげてぶつかったオブジェクトを検知
    private void RayFly()
    {
        _rayHitFoot = new RaycastHit2D();   //初期化

        //Rayを真下に飛ばす
        Ray ray = new Ray(transform.position, Vector3.down);
        float distance = 0.3f;

        Debug.DrawRay(ray.origin, ray.direction * (_scaleNow / 2 + distance), Color.red);
        RaycastHit2D[] rayHits = Physics2D.RaycastAll(ray.origin, ray.direction, _scaleNow / 2 + distance);
        List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

        //一部のオブジェクトを除外
        foreach (RaycastHit2D i in rayHits)
        {
            //トリガーは除外
            if(i.collider.isTrigger)
            {
                continue;
            }

            //自分自身は除外
            if(i.collider.gameObject == this.gameObject)
            {
                continue;
            }

            //一部のタグのついたオブジェクトは除外
            switch (i.collider.gameObject.tag)
            {
                case "SlimeTrigger": break;  //スライムトリガー
                case "Tracking": break;      //カメラトラッキング
                //ここに追加

                default:
                    raycastHits.Add(i);    //追加
                    break;
            }
        }

        //スライムに一番近いオブジェクトを取得
        RaycastHit2D nearestRaycastHit = new RaycastHit2D();
        foreach (RaycastHit2D i in raycastHits)
        {
            if(!nearestRaycastHit)
            {
                nearestRaycastHit = i;
            }
            else
            {
                if(nearestRaycastHit.distance > i.distance)
                {
                    nearestRaycastHit = i;
                }
            }
        }

        //当たったら
        if(nearestRaycastHit)
        {
            _rayHitFoot = nearestRaycastHit;
            //Debug.Log(_rayHitFoot.collider.name);
        }
    }


    bool SlimeGetRayHit()
    {
        return _rayHitFoot;
    }
}
