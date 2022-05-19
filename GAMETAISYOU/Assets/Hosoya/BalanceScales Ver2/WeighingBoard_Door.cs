using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard_Door : MonoBehaviour
{
    [SerializeField] WeighingBoard weighingBoard;

    public bool _isMove;   //動くようにするかどうか

    [SerializeField] float onWeight;    //どの重さ以上でOnになるか
    [SerializeField] bool ifOnOpen;     //Onの状態だと開くようにするかどうか

    [SerializeField] float moveMaxY;    //どのくらい開くか
    [SerializeField] float moveTimeMax; //何秒かけて開いたり閉じたりするか

    bool ifOn;  //Onかどうか
    float beforePosY;   //移動前のY座標

    [SerializeField] Animator gear;

    // Start is called before the first frame update
    void Start()
    {
        ifOn = false;
        beforePosY = this.gameObject.transform.position.y;

        weighingBoard._changeCounter._goalWeight = onWeight;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMove) //動かすかどうか
        {
            ifOn = weighingBoard.weight >= onWeight;
            float moveY = 0;

            //開くなら
            if (ifOnOpen && ifOn || !ifOnOpen && !ifOn)
            {
                if (gear) { gear.SetBool("Open", true); }
                

                moveY = moveMaxY / moveTimeMax * Time.deltaTime;

                //上限
                if (transform.position.y + moveY > beforePosY + moveMaxY)
                {
                    moveY = beforePosY + moveMaxY - transform.position.y;
                }
            }
            else
            {
                if (gear) { gear.SetBool("Open", false); }

                moveY = -moveMaxY / moveTimeMax * Time.deltaTime;

                //下限
                if (transform.position.y + moveY < beforePosY)
                {
                    moveY = beforePosY - transform.position.y;
                }
            }

            transform.position = new Vector2(transform.position.x, transform.position.y + moveY);
        }
    }
}
