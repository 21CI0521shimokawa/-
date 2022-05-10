using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard : MonoBehaviour
{
    [SerializeField] Animator animator;

    // 乗っているオブジェクトリスト
    public List<GameObject> onObjects = new List<GameObject>();
    
    public List<GameObject> onObjectsOll = new List<GameObject>();   //全てのオブジェクト

    public int quantity;    //乗っているモノの個数
    public float weight;    //乗っているモノの重さ

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

    //更新
    private void InformationUpdate()
    {
        InformationReset();

        #region onObjectsOllリストの作成
        //この板に乗っているオブジェクト全てを取得
        foreach (GameObject i in onObjects)
        {
            onObjectsOll.Add(i);
            WeighingBoard_ObjectOnObject buf = i.GetComponent<WeighingBoard_ObjectOnObject>();
            if (buf)
            {
                buf.ObjectOnObjectProsess(this);
            }
        }

        #region 念のため重複しているオブジェクトがあったら削除
        {
            List<GameObject> buf = new List<GameObject>();
            buf.AddRange(onObjectsOll);
            onObjectsOll.Clear();
            foreach (GameObject i in buf)
            {
                //リストに入ってなかったら追加
                if (!onObjectsOll.Contains(i))
                {
                    onObjectsOll.Add(i);
                }
            }
        }
        #endregion
        #endregion

        quantity = onObjectsOll.Count;

        //重さ計測
        foreach (GameObject i in onObjectsOll)
        {
            weight += i.GetComponent<Rigidbody2D>().mass;
        }

        if(animator)
        {
            animator.SetFloat("Weight", (int)weight);
        }
    }

    //初期化
    private void InformationReset()
    {
        quantity = 0;
        weight = 0.0f;
        onObjectsOll.Clear();
    }

    //リスト追加
    public void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D collisionRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();

        //リストに無かったら追加
        if (!onObjects.Contains(collision.gameObject) && collisionRigidbody)
        {
            onObjects.Add(collision.gameObject);

            //Scriptが無かったら Scriptアタッチ
            if (!collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>())
            {
                collision.gameObject.AddComponent<WeighingBoard_ObjectOnObject>();
            }
        }
    }

    //リスト削除
    public void OnCollisionExit2D(Collision2D collision)
    {
        //リストにあったら消去
        if (onObjects.Contains(collision.gameObject))
        {
            onObjects.Remove(collision.gameObject);

            //Scriptがあったら削除
            WeighingBoard_ObjectOnObject buf = collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>();
            if(buf != null)
            {
                Destroy(buf);
            }
        }
    }
}