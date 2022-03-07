using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearoff_Coalesecence : MonoBehaviour
{
    public PlayerController_Coalescence playerController;

    #region ちぎる変数宣言
    [SerializeField, Tooltip("分裂するまでの時間")]
    private float divisionTime;
    [SerializeField, Tooltip("分裂したオブジェクト")]
    private GameObject mini;

    private float power;//かかっている力
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //ちぎる
        power = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.tearoffUpdate)
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

                transform.localScale = new Vector2(1 + power, 1.0f / (1 + power));

                if (power >= 1)
                {
                    Debug.Log("きれたよ");

                    GetComponent<Renderer>().material.color = Color.red;

                    //右
                    {
                        Vector3 position = new Vector2((float)(transform.position.x + 0.8), transform.position.y);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;

                        cloneObject.GetComponent<PlayerController_Coalescence>().ifMiniPlayer = true;
                        cloneObject.GetComponent<PlayerHaziku_Coalescence>().modeLR = PlayerHaziku_Coalescence.LRMode.Right;
                    }

                    //左
                    {
                        Vector3 position = new Vector2((float)(transform.position.x - 0.8), transform.position.y);
                        GameObject cloneObject = Instantiate(mini);
                        cloneObject.transform.position = position;

                        cloneObject.GetComponent<PlayerController_Coalescence>().ifMiniPlayer = true;
                        cloneObject.GetComponent<PlayerHaziku_Coalescence>().modeLR = PlayerHaziku_Coalescence.LRMode.Left;
                    }

                    //自身を破壊
                    Destroy(this.gameObject);
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

                transform.localScale = new Vector2(1, 1);

                GetComponent<Renderer>().material.color = Color.green;
            }

        }
    }
}
