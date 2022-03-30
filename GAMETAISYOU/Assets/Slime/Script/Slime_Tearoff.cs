using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Slime_Tearoff : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;

    [SerializeField, Tooltip("分裂できる大きさの下限")]
    private float scaleLowerLimit;
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

    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        //ちぎる
        power = 0;

        oneFrameBefore_Update = false;

        gamepad = Gamepad.current;
    }

    // Update is called once per frame
    void Update()
    {
        if (slimeController.tearoffUpdate && slimeController.core)  //updateを許可されていて本体だったら
        {
            if (!slimeController.slimeBuf.ifTearOff)    //既にちぎられていなかったら
            {

                //ちぎる
                float stickLHorizontal = Input.GetAxis("L_Stick_Horizontal");
                float stickRHorizontal = Input.GetAxis("R_Stick_Horizontal");

                if ((stickLHorizontal < 0) && (stickRHorizontal > 0))
                {
                    slimeController._SlimeAnimator.SetBool("Extend", true);

                    //Debug.Log("引っ張ってるよ:" + stickLHorizontal + "," + stickRHorizontal);
                    power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //かかる力を増やす

                    if (power > (-stickLHorizontal + stickRHorizontal) / 2)
                    {
                        power = (-stickLHorizontal + stickRHorizontal) / 2;
                    }

                    //gamepad.SetMotorSpeeds(power, power);

                    //ちぎる
                    if (power >= 1)
                    {

                        //大きさがLimitを下回らないなら
                        if (scaleLowerLimit <= slimeController.scale - divisionScale)
                        {
                            power = 0;

                            slimeController._ifOperation = false;

                            slimeController._SlimeAnimator.SetTrigger("Tearoff");
                            slimeController.slimeBuf.ifTearOff = true;

                            Invoke("SlimeInstantiate", 0.55f);

                            //gamepad.SetMotorSpeeds(0.0f, 0.0f);
                        }
                        else
                        {
                            GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("きれません！！");
                        }
                    }
                    else
                    {
                        Debug.Log("きれてないよ:" + power);

                        GetComponent<Renderer>().material.color = Color.green;
                    }
                }
                else
                {
                    Debug.Log("つかんだよ");

                    power = 0;

                    GetComponent<Renderer>().material.color = Color.green;

                    //gamepad.SetMotorSpeeds(0.0f, 0.0f);
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

            oneFrameBefore_Update = false;

            slimeController._SlimeAnimator.SetBool("Extend", false);
        }
    }


    public void SlimeInstantiate()
    {
        Debug.Log("きれたよ");

        power = 0;

        //コアじゃないほう
        {
            float distance = slimeController.scale * 1.0f;

            Vector3 position = new Vector2((float)(transform.position.x + distance * (slimeController._direction == SlimeController._Direction.Right ? 1 : -1)), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf.scale = divisionScale;
            cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
            buf.core = false;
            buf.deadTime = deadEndTime;
            buf.modeLR = SlimeController.LRMode.Right;
        }

        slimeController.scale -= divisionScale;

        //コア
        {
            Vector3 position = new Vector2((float)(transform.position.x), transform.position.y);
            GameObject cloneObject = Instantiate(slimePrefab);
            cloneObject.transform.position = position;

            SlimeController buf = cloneObject.GetComponent<SlimeController>();
            buf.scale = slimeController.scale;
            cloneObject.transform.localScale = new Vector2(slimeController.scale, slimeController.scale);

            buf.core = true;
            buf.modeLR = SlimeController.LRMode.Left;
        }

        //自身を破壊
        Destroy(this.gameObject);
    }
}
