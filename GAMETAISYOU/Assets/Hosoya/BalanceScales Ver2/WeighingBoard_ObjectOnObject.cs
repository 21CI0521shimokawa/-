using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard_ObjectOnObject : MonoBehaviour
{
    public List<GameObject> onObjects = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���̃I�u�W�F�N�g�̏�ɏ���Ă�I�u�W�F�N�g�̏���{�̂ɓn������
    public void ObjectOnObjectProsess(WeighingBoard _weighingBoard)
    {
        foreach(GameObject i in onObjects)
        {
            //���X�g�ɖ���������ǉ� ����
            if (!_weighingBoard.onObjectsOll.Contains(i) && !i.CompareTag("WeighingBoard"))
            {
                _weighingBoard.onObjectsOll.Add(i);

                //Script������������ Script�A�^�b�`
                WeighingBoard_ObjectOnObject buf = i.GetComponent<WeighingBoard_ObjectOnObject>();
                if (!buf)
                {
                    buf = i.AddComponent<WeighingBoard_ObjectOnObject>();
                }

                //����
                buf.ObjectOnObjectProsess(_weighingBoard);
            }
        }
    }

    //���X�g�ǉ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���X�g�ɖ���������ǉ�
        if (!onObjects.Contains(collision.gameObject) && !collision.gameObject.CompareTag("WeighingBoard"))
        {
            onObjects.Add(collision.gameObject);

            //Script������������ Script�A�^�b�`
            if(!collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>())
            {
                collision.gameObject.AddComponent<WeighingBoard_ObjectOnObject>();
            }
        }
    }

    //���X�g�폜
    private void OnCollisionExit2D(Collision2D collision)
    {
        //���X�g�ɂ����������
        if (onObjects.Contains(collision.gameObject))
        {
            onObjects.Remove(collision.gameObject);

            //���̃I�u�W�F�N�g�����̃I�u�W�F�N�g�ɐG��Ă��Ȃ��Ɣ̏�ɏ�邱�Ƃ��ł��Ȃ��Ȃ�Script���폜
            List<GameObject> exclusionObjects = new List<GameObject>();
            exclusionObjects.Add(this.gameObject);
            if (IfThatObjectToWeighingBoard(exclusionObjects))
            {
                //Script����������폜
                WeighingBoard_ObjectOnObject buf = collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>();
                if (buf != null)
                {
                    Destroy(buf);
                }
            } 
        }
    }


    //���̃I�u�W�F�N�g���̏�ɏ���Ă��邩�ǂ���
    public bool IfThatObjectToWeighingBoard(List<GameObject> _exclusionObjects)
    {
        foreach (GameObject i in onObjects)
        {
            //���O����Ă����珈�����Ȃ�
            if(!_exclusionObjects.Contains(i))
            {
                //i���ł͂Ȃ��Ȃ�
                if (!i.CompareTag("WeighingBoard"))
                {
                    _exclusionObjects.Add(i);   //���O�ɓ����
                    return i.GetComponent<WeighingBoard_ObjectOnObject>().IfThatObjectToWeighingBoard(_exclusionObjects);  //���O�����I�u�W�F�N�g�ł�����x����������
                }
                else
                {
                    return true;
                }
            }
        }

        return false;
    }
}
