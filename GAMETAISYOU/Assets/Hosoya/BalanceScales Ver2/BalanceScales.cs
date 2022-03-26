using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScales : MonoBehaviour
{
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;

    [SerializeField] float distanceMax; //最大±いくつまで上下するか
    [SerializeField] float distancePerDifferenceWeight; //重さの差1につきどれだけ上下するか
    [SerializeField] float moveTimeMax; //何秒かけて移動するか

    WeighingBoard weighingBoardLeft, weighingBoardRight;
    float weightLeft, weightRight;  //重さ
    float differenceWeight;     //重さの差（右が重いと+ 左が重いと-）
    float differenceWeightOld;  //1フレーム前の値を保持

    float beforeYLeft, beforeYRight;    //最初の座標
    float moveYLeftEnd, moveYRightEnd;      //どこまで移動するか
    float moveYLeftStart, moveYRightStart;    //どこから移動するか
    float moveTime; //移動時間

    // Start is called before the first frame update
    void Start()
    {
        weighingBoardLeft = left.GetComponent<WeighingBoard>();
        weighingBoardRight = right.GetComponent<WeighingBoard>();

        weightLeft = 0;
        weightRight = 0;
        differenceWeight = 0;
        differenceWeightOld = 0;

        beforeYLeft = left.transform.position.y;
        beforeYRight = right.transform.position.y;

        moveTime = moveTimeMax;
    }

    // Update is called once per frame
    void Update()
    {
        //重さを取得
        weightLeft = weighingBoardLeft.weight;
        weightRight = weighingBoardRight.weight;

        //重さの差を計算
        differenceWeight = weightRight - weightLeft;


        //移動
        if(moveTime < moveTimeMax)
        {
            moveTime += Time.deltaTime;
            moveTime = Mathf.Min(moveTime, moveTimeMax);

            //移動量を取得
            float amountOfMovementLeft = Mathf.SmoothStep(moveYLeftStart, moveYLeftEnd, moveTime / moveTimeMax);
            float amountOfMovementRight = Mathf.SmoothStep(moveYRightStart, moveYRightEnd, moveTime / moveTimeMax);

            #region 乗っているオブジェクトの移動
            //左
            {
                Vector2 onObjectMove = new Vector2(0, amountOfMovementLeft - left.transform.position.y);
                foreach (GameObject i in weighingBoardLeft.onObjectsOll)
                {
                    i.transform.Translate(onObjectMove, Space.World);
                }
            }
            //右
            {
                Vector2 onObjectMove = new Vector2(0, amountOfMovementRight - right.transform.position.y);
                foreach (GameObject i in weighingBoardRight.onObjectsOll)
                {
                    i.transform.Translate(onObjectMove, Space.World);
                }
            }
            #endregion

            //移動
            left.transform.position = new Vector2(left.transform.position.x, amountOfMovementLeft);
            right.transform.position = new Vector2(right.transform.position.x, amountOfMovementRight);
        }

        //1フレーム前と重さの差が変わっていたら
        if (differenceWeightOld != differenceWeight)
        {
            differenceWeightOld = differenceWeight;
            moveTime = 0;

            //移動前の値を保持
            moveYLeftStart = left.transform.position.y;
            moveYRightStart = right.transform.position.y;

            //移動先の値を計算
            moveYLeftEnd = differenceWeight * distancePerDifferenceWeight + beforeYLeft;
            moveYRightEnd = -differenceWeight * distancePerDifferenceWeight + beforeYRight;

            moveYLeftEnd = Mathf.Min(moveYLeftEnd, beforeYLeft + distanceMax);
            moveYLeftEnd = Mathf.Max(moveYLeftEnd, beforeYLeft - distanceMax);

            moveYRightEnd = Mathf.Min(moveYRightEnd, beforeYRight + distanceMax);
            moveYRightEnd = Mathf.Max(moveYRightEnd, beforeYRight - distanceMax);
        }
    }
}
