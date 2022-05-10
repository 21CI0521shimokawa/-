using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard : MonoBehaviour
{
    [SerializeField] Animator animator;

    // ����Ă���I�u�W�F�N�g���X�g
    public List<GameObject> onObjects = new List<GameObject>();
    
    public List<GameObject> onObjectsOll = new List<GameObject>();   //�S�ẴI�u�W�F�N�g

    public int quantity;    //����Ă��郂�m�̌�
    public float weight;    //����Ă��郂�m�̏d��

    // Start is called before the first frame update
    void Start()
    {
        InformationReset();
    }

    // Update is called once per frame
    void Update()
    {
        InformationUpdate();
    }

    //�X�V
    private void InformationUpdate()
    {
        InformationReset();

        #region onObjectsOll���X�g�̍쐬
        //���̔ɏ���Ă���I�u�W�F�N�g�S�Ă��擾
        foreach (GameObject i in onObjects)
        {
            onObjectsOll.Add(i);
            WeighingBoard_ObjectOnObject buf = i.GetComponent<WeighingBoard_ObjectOnObject>();
            if (buf)
            {
                buf.ObjectOnObjectProsess(this);
            }
        }

        #region �O�̂��ߏd�����Ă���I�u�W�F�N�g����������폜
        {
            List<GameObject> buf = new List<GameObject>();
            buf.AddRange(onObjectsOll);
            onObjectsOll.Clear();
            foreach (GameObject i in buf)
            {
                //���X�g�ɓ����ĂȂ�������ǉ�
                if (!onObjectsOll.Contains(i))
                {
                    onObjectsOll.Add(i);
                }
            }
        }
        #endregion
        #endregion

        quantity = onObjectsOll.Count;

        //�d���v��
        foreach (GameObject i in onObjectsOll)
        {
            weight += i.GetComponent<Rigidbody2D>().mass;
        }

        if(animator)
        {
            animator.SetFloat("Weight", (int)weight);
        }
    }

    //������
    private void InformationReset()
    {
        quantity = 0;
        weight = 0.0f;
        onObjectsOll.Clear();
    }

    //���X�g�ǉ�
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D collisionRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        //���X�g�ɖ���������ǉ�
        if (!onObjects.Contains(collision.gameObject) && collisionRigidbody)
        {
            onObjects.Add(collision.gameObject);

            //Script������������ Script�A�^�b�`
            if (!collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>())
            {
                collision.gameObject.AddComponent<WeighingBoard_ObjectOnObject>();
            }
        }
    }

    //���X�g�폜
    public void OnCollisionExit2D(Collision2D collision)
    {
        //���X�g�ɂ����������
        if (onObjects.Contains(collision.gameObject))
        {
            onObjects.Remove(collision.gameObject);

            //Script����������폜
            WeighingBoard_ObjectOnObject buf = collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>();
            if(buf != null)
            {
                Destroy(buf);
            }
        }
    }
}