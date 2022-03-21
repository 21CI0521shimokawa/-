using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        //ちぎる
        power = 0;

        oneFrameBefore_Update = false;
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
                    //Debug.Log("引っ張ってるよ:" + stickLHorizontal + "," + stickRHorizontal);
                    power += (-stickLHorizontal + stickRHorizontal) / 2 * Time.deltaTime / divisionTime;    //かかる力を増やす

                    if (power > (-stickLHorizontal + stickRHorizontal) / 2)
                    {
                        power = (-stickLHorizontal + stickRHorizontal) / 2;
                    }



                    if (power >= 1)
                    {

                        if (scaleLowerLimit <= slimeController.scale - divisionScale)
                        {
                            Debug.Log("きれたよ");

                            slimeController.scale -= divisionScale;

                            power = 0;
                            slimeController.slimeBuf.ifTearOff = true;

                            //右 コアじゃないほう
                            {
                                Vector3 position = new Vector2((float)(transform.position.x + 0.1), transform.position.y);
                                GameObject cloneObject = Instantiate(slimePrefab);
                                cloneObject.transform.position = position;

                                SlimeController buf = cloneObject.GetComponent<SlimeController>();
                                buf.scale = divisionScale;
                                cloneObject.transform.localScale = new Vector2(divisionScale, divisionScale);
                                buf.core = false;
                                buf.deadTime = deadEndTime;
                                buf.modeLR = SlimeController.LRMode.Right;
                            }

                            //左 コア
                            {
                                Vector3 position = new Vector2((float)(transform.position.x - 0.1), transform.position.y);
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
        }
    }
}
