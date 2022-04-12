using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard_Door : MonoBehaviour
{
    [SerializeField] WeighingBoard weighingBoard;

    public bool _isMove;   //�����悤�ɂ��邩�ǂ���

    [SerializeField] float onWeight;    //�ǂ̏d���ȏ��On�ɂȂ邩
    [SerializeField] bool ifOnOpen;     //On�̏�Ԃ��ƊJ���悤�ɂ��邩�ǂ���

    [SerializeField] float moveMaxY;    //�ǂ̂��炢�J����
    [SerializeField] float moveTimeMax; //���b�����ĊJ����������肷�邩

    bool ifOn;  //On���ǂ���
    float beforePosY;   //�ړ��O��Y���W

    // Start is called before the first frame update
    void Start()
    {
        ifOn = false;
        beforePosY = this.gameObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isMove) //���������ǂ���
        {
            ifOn = weighingBoard.weight >= onWeight;
            float moveY = 0;

            //�J���Ȃ�
            if (ifOnOpen && ifOn || !ifOnOpen && !ifOn)
            {
                moveY = moveMaxY / moveTimeMax * Time.deltaTime;

                //���
                if (transform.position.y + moveY > beforePosY + moveMaxY)
                {
                    moveY = beforePosY + moveMaxY - transform.position.y;
                }
            }
            else
            {
                moveY = -moveMaxY / moveTimeMax * Time.deltaTime;

                //����
                if (transform.position.y + moveY < beforePosY)
                {
                    moveY = beforePosY - transform.position.y;
                }
            }

            transform.position = new Vector2(transform.position.x, transform.position.y + moveY);
        }
    }
}
