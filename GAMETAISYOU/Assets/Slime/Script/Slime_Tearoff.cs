using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Tearoff : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [Tooltip("分裂できる大きさの下限")]
    public float _scaleLowerLimit;
    [SerializeField, Tooltip("ちぎる大きさ")]
    private float divisionScale;
    [SerializeField, Tooltip("分裂するまでの時間")]
    private float divisionTime;
    [SerializeField, Tooltip("分裂したオブジェクト")]
    private GameObject slimePrefab;

    public float stretchEndTime;    //何秒経過したら元の長さに戻るか
    public float deadEndTime;       //何秒経過したらスライムが消えるか

    public float power; //かかっている力

    bool oneFrameBefore_Update; //1フレーム前に処理を行ったかどうか

    bool IsDirecting;   //演出中かどうか

    public enum TearoffSlimeType
    {
        NON,
        RIGHT,
        LEFT
    }

    public TearoffSlimeType _tearoffSlimeType;

    // Start is called before the first frame update
    void Start()
    {
        //ちぎる
        power = 0;

        oneFrameBefore_Update = false;

        IsDirecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeController.tearoffUpdate && slimeController.core)  //updateを許可されていて本体だったら
        {
            if (!slimeController._slimeBuf._ifTearOff)    //既にちぎられていなかったら
            {

                //ちぎる
                //float stickLHorizontal = Input.GetAxis("L_Stick_Horizontal");
                //float stickRHorizontal = Input.GetAxis("R_Stick_Horizontal");

                var gamepadLeftStick = Gamepad.current.leftStick.ReadValue();
                var gamepadRightStick = Gamepad.current.rightStick.ReadValue();
                float stickLHorizontal = gamepadLeftStick.x;
                float stickRHorizontal = gamepadRightStick.x;

                //スティックの最大値にある程度余裕を持たせる（ちぎれなくなっちゃう）
                stickLHorizontal = Mathf.Min(stickLHorizontal / 0.90f, 1.0f);
                stickRHorizontal = Mathf.Min(stickRHorizontal / 0.90f, 1.0f);

                if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
                {
                    slimeController._SlimeAnimator.SetBool("Extend", true);
                    slimeController._SlimeAnimator.SetBool("SlimeScaleNo", slimeController._scaleNow <= _scaleLowerLimit);

                    //Debug.Log("引っ張ってるよ:" + stickLHorizontal + "," + stickRHorizontal);

                    ChangeDivisionTime();
                    power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //かかる力を増やす

                    if (power > (-stickLHorizontal + stickRHorizontal) / 2)
                    {
                        power = (-stickLHorizontal + stickRHorizontal) / 2;
                    }

                    //振動
                    slimeController._controllerVibrationScript.Vibration("SlimeTearoff", power * 0.3f, power * 0.3f);

                    slimeController._slimeSE._PlayStretchSE(3 * power);

                    //ちぎる
                    if (power >= 1)
                    {
                        //if (slimeController._scaleMax == slimeController._scaleNow) //スライムが大きくなりきっていたら
                        {
                            
                            //大きさがLimitより大きいならちぎる
                            if(slimeController._scaleNow > _scaleLowerLimit)
                            {
                                //スライムアニメーションがちぎれる状態だったら
                                if(slimeController._SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_ExtendIdle"))
                                {
                                    //ちぎる大きさと今の大きさからLimitを引いた値の内小さい方を代入する
                                    float tearOffSlimeScale = Mathf.Min(divisionScale, slimeController._scaleMax - _scaleLowerLimit);

                                    StartCoroutine(SlimeTearOffCoroutine(tearOffSlimeScale));    //コルーチンの起動
                                }
                            }
                            else
                            {
                                GetComponent<Renderer>().material.color = Color.red;
                                Debug.Log("大きさが足りません！！");
                                slimeController._CannotAction();
                            }

                            //大きさがLimitを下回らないなら
                            //if (_scaleLowerLimit <= slimeController._scaleMax - divisionScale)
                            //{

                            //    power = 0;

                            //    slimeController._ifOperation = false;

                            //    slimeController._SlimeAnimator.SetTrigger("Tearoff");
                            //    slimeController.slimeBuf.ifTearOff = true;

                            //    //Invoke("SlimeInstantiate", 0.55f);
                            //}
                            
                        }
                        //else
                        //{
                        //    GetComponent<Renderer>().material.color = Color.red;
                        //    Debug.Log("スライムが大きくなりきってません！！");
                        //    slimeController._CannotAction();
                        //}
                    }
                    else
                    {
                        //Debug.Log("きれてないよ:" + power);

                        GetComponent<Renderer>().material.color = Color.green;
                    }
                }
                else
                {
                    Debug.Log("つかんだよ");

                    power = 0;

                    GetComponent<Renderer>().material.color = Color.green;

                    slimeController._controllerVibrationScript.Vibration("SlimeTearoff", 0, 0);
                    slimeController._slimeSE._StopStretchSE();
                }
                oneFrameBefore_Update = true;
            }
        }
        else
        {
            if(oneFrameBefore_Update)
            {
                slimeController.pullWideForceMax = power;
                slimeController.stretchEndTime = slimeController.liveTime + stretchEndTime;
            }

            power = 0;

            if (!IsDirecting)
            {
                slimeController._controllerVibrationScript.Destroy("SlimeTearoff");
            }

            oneFrameBefore_Update = false;

            slimeController._SlimeAnimator.SetBool("Extend", false);

            slimeController._slimeSE._StopStretchSE();
        }
    }


    public void SlimeInstantiate()
    {
        Debug.Log("きれたよ");

        power = 0;

        //コアじゃないほう
        {
            float distance = slimeController._scaleMax * 1.0f;

            Vector3 position = new Vector2((float)(transform.position.x + distance * (slimeController._direction == SlimeController._Direction.Right ? 1 : -1)), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = divisionScale;
            cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
            buf.core = false;
            buf.deadTime = deadEndTime;
            buf._direction = slimeController._direction;

            //操作タイプ
            switch (_tearoffSlimeType)
            {
                //操作不可
                case TearoffSlimeType.NON:
                    buf._ifOperation = false;
                    break;

                //右
                case TearoffSlimeType.RIGHT:
                    buf.modeLR = SlimeController.LRMode.Right;
                    break;

                //左
                case TearoffSlimeType.LEFT:
                    buf.modeLR = SlimeController.LRMode.Left;
                    break;
            }
        }

        slimeController._scaleMax -= divisionScale;

        //コア
        {
            Vector3 position = new Vector2((float)(transform.position.x), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = slimeController._scaleMax;
            cloneObject.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax);

            buf.core = true;
            buf.modeLR = SlimeController.LRMode.Left;
            buf._direction = slimeController._direction;
        }

        //自身を破壊
        Destroy(this.gameObject);
    }

    // コルーチン本体
    private IEnumerator SlimeTearOffCoroutine(float slimeScale)
    {
        slimeController._slimeSE._PlayTearoffSE();
        slimeController._slimeSE._StopStretchSE();

        IsDirecting = true;

        power = 0;

        slimeController._ifOperation = false;

        slimeController._SlimeAnimator.SetTrigger("Tearoff");
        slimeController._slimeBuf._ifTearOff = true;

        slimeController._controllerVibrationScript.Vibration("SlimeTearoff", 1.0f, 1.0f);

        yield return new WaitForSeconds(0.25f); //0.25秒待つ
        slimeController._controllerVibrationScript.Destroy("SlimeTearoff");
        yield return new WaitForSeconds(0.2f); //0.2秒待つ

        Debug.Log("きれたよ");

        power = 0;

        //コアじゃないほう
        {
            float distance = slimeController._scaleMax * 1.0f;

            Vector3 position = new Vector2((float)(transform.position.x + SlimeNoCoreInstanceateDistance(distance) * (slimeController._direction == SlimeController._Direction.Right ? 1 : -1)), transform.position.y);

            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = slimeScale;
            cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
            buf.core = false;
            buf.deadTime = deadEndTime;
            buf._direction = slimeController._direction;

            //操作タイプ
            switch (_tearoffSlimeType)
            {
                //操作不可
                case TearoffSlimeType.NON:
                    buf._ifOperation = false;
                    break;

                //右
                case TearoffSlimeType.RIGHT:
                    buf.modeLR = SlimeController.LRMode.Right;
                    break;

                //左
                case TearoffSlimeType.LEFT:
                    buf.modeLR = SlimeController.LRMode.Left;
                    break;
            }
        }

        slimeController._scaleMax -= slimeScale;

        //コア
        {
            Vector3 position = new Vector2((float)(transform.position.x), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf._scaleMax = slimeController._scaleMax;
            cloneObject.transform.localScale = new Vector2(slimeController._scaleMax, slimeController._scaleMax);

            buf.core = true;
            buf.modeLR = SlimeController.LRMode.Left;
            buf._direction = slimeController._direction;
        }

        //自身を破壊
        Destroy(this.gameObject);

        IsDirecting = false;
    }


    float SlimeNoCoreInstanceateDistance(float distance_)
    {
        float rtv = distance_;

        //Rayを真横に飛ばす
        Ray ray = new Ray(transform.position, slimeController._direction == SlimeController._Direction.Right ? Vector2.right : Vector2.left);

        Debug.DrawRay(ray.origin, ray.direction * distance_, Color.red);
        RaycastHit2D[] rayHits = Physics2D.RaycastAll(ray.origin, ray.direction, distance_);
        List<RaycastHit2D> raycastHits = new List<RaycastHit2D>();

        //一番距離が近いオブジェクトとの距離をだす
        foreach (RaycastHit2D i in rayHits)
        {
            //トリガーは除外
            if (i.collider.isTrigger)
            {
                continue;
            }

            //自分自身は除外
            if (i.collider.gameObject == this.gameObject)
            {
                continue;
            }

            //一部のタグのついたオブジェクトは除外
            switch (i.collider.gameObject.tag)
            {
                case "Slime" : continue;        //スライム        
                case "SlimeTrigger": continue;  //スライムトリガー
                case "Tracking": continue;      //カメラトラッキング
                //ここに追加

                default:
                    raycastHits.Add(i);    //追加
                    break;
            }

            rtv = Mathf.Min(rtv, i.distance);   //距離が短い方を代入
        }

        return rtv;
    }

    void ChangeDivisionTime()
    {
        float sizeMin = 2.0f;
        float sizeMax = 5.0f;

        float divisionTimeMin = 1.0f;
        float divisionTimeMax = 2.5f;


        float buf = (slimeController._scaleMax - sizeMin) / sizeMax;

        float newDivisionTime = Mathf.Lerp(divisionTimeMin, divisionTimeMax, buf);

        divisionTime = newDivisionTime;
    }
}