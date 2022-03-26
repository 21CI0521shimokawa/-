using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScales : MonoBehaviour
{
    [SerializeField] GameObject left;
    [SerializeField] GameObject right;

    [SerializeField] float distanceMax; //�ő�}�����܂ŏ㉺���邩
    [SerializeField] float distancePerDifferenceWeight; //�d���̍�1�ɂ��ǂꂾ���㉺���邩
    [SerializeField] float moveTimeMax; //���b�����Ĉړ����邩

    WeighingBoard weighingBoardLeft, weighingBoardRight;
    float weightLeft, weightRight;  //�d��
    float differenceWeight;     //�d���̍��i�E���d����+ �����d����-�j
    float differenceWeightOld;  //1�t���[���O�̒l��ێ�

    float beforeYLeft, beforeYRight;    //�ŏ��̍��W
    float moveYLeftEnd, moveYRightEnd;      //�ǂ��܂ňړ����邩
    float moveYLeftStart, moveYRightStart;    //�ǂ�����ړ����邩
    float moveTime; //�ړ�����

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
        //�d�����擾
        weightLeft = weighingBoardLeft.weight;
        weightRight = weighingBoardRight.weight;

        //�d���̍����v�Z
        differenceWeight = weightRight - weightLeft;


        //�ړ�
        if(moveTime < moveTimeMax)
        {
            moveTime += Time.deltaTime;
            moveTime = Mathf.Min(moveTime, moveTimeMax);

            //�ړ��ʂ��擾
            float amountOfMovementLeft = Mathf.SmoothStep(moveYLeftStart, moveYLeftEnd, moveTime / moveTimeMax);
            float amountOfMovementRight = Mathf.SmoothStep(moveYRightStart, moveYRightEnd, moveTime / moveTimeMax);

            #region ����Ă���I�u�W�F�N�g�̈ړ�
            //��
            {
                Vector2 onObjectMove = new Vector2(0, amountOfMovementLeft - left.transform.position.y);
                foreach (GameObject i in weighingBoardLeft.onObjectsOll)
                {
                    i.transform.Translate(onObjectMove, Space.World);
                }
            }
            //�E
            {
                Vector2 onObjectMove = new Vector2(0, amountOfMovementRight - right.transform.position.y);
                foreach (GameObject i in weighingBoardRight.onObjectsOll)
                {
                    i.transform.Translate(onObjectMove, Space.World);
                }
            }
            #endregion

            //�ړ�
            left.transform.position = new Vector2(left.transform.position.x, amountOfMovementLeft);
            right.transform.position = new Vector2(right.transform.position.x, amountOfMovementRight);
        }

        //1�t���[���O�Əd���̍����ς���Ă�����
        if (differenceWeightOld != differenceWeight)
        {
            differenceWeightOld = differenceWeight;
            moveTime = 0;

            //�ړ��O�̒l��ێ�
            moveYLeftStart = left.transform.position.y;
            moveYRightStart = right.transform.position.y;

            //�ړ���̒l���v�Z
            moveYLeftEnd = differenceWeight * distancePerDifferenceWeight + beforeYLeft;
            moveYRightEnd = -differenceWeight * distancePerDifferenceWeight + beforeYRight;

            moveYLeftEnd = Mathf.Min(moveYLeftEnd, beforeYLeft + distanceMax);
            moveYLeftEnd = Mathf.Max(moveYLeftEnd, beforeYLeft - distanceMax);

            moveYRightEnd = Mathf.Min(moveYRightEnd, beforeYRight + distanceMax);
            moveYRightEnd = Mathf.Max(moveYRightEnd, beforeYRight - distanceMax);
        }
    }
}
