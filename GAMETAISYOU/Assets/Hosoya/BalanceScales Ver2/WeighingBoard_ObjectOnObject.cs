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

    //このオブジェクトの上に乗ってるオブジェクトの情報を本体に渡す処理
    public void ObjectOnObjectProsess(WeighingBoard _weighingBoard)
    {
        foreach(GameObject i in onObjects)
        {
            //リストに無かったら追加 処理
            if (!_weighingBoard.onObjectsOll.Contains(i) && !i.CompareTag("WeighingBoard"))
            {
                _weighingBoard.onObjectsOll.Add(i);

                //Scriptが無かったら Scriptアタッチ
                WeighingBoard_ObjectOnObject buf = i.GetComponent<WeighingBoard_ObjectOnObject>();
                if (!buf)
                {
                    buf = i.AddComponent<WeighingBoard_ObjectOnObject>();
                }

                //処理
                buf.ObjectOnObjectProsess(_weighingBoard);
            }
        }
    }

    //リスト追加
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //リストに無かったら追加
        if (!onObjects.Contains(collision.gameObject) && !collision.gameObject.CompareTag("WeighingBoard"))
        {
            onObjects.Add(collision.gameObject);

            //Scriptが無かったら Scriptアタッチ
            if(!collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>())
            {
                collision.gameObject.AddComponent<WeighingBoard_ObjectOnObject>();
            }
        }
    }

    //リスト削除
    private void OnCollisionExit2D(Collision2D collision)
    {
        //リストにあったら消去
        if (onObjects.Contains(collision.gameObject))
        {
            onObjects.Remove(collision.gameObject);

            //そのオブジェクトがこのオブジェクトに触れていないと板の上に乗ることができないならScriptを削除
            List<GameObject> exclusionObjects = new List<GameObject>();
            exclusionObjects.Add(this.gameObject);
            if (IfThatObjectToWeighingBoard(exclusionObjects))
            {
                //Scriptがあったら削除
                WeighingBoard_ObjectOnObject buf = collision.gameObject.GetComponent<WeighingBoard_ObjectOnObject>();
                if (buf != null)
                {
                    Destroy(buf);
                }
            } 
        }
    }


    //そのオブジェクトが板の上に乗っているかどうか
    public bool IfThatObjectToWeighingBoard(List<GameObject> _exclusionObjects)
    {
        foreach (GameObject i in onObjects)
        {
            //除外されていたら処理しない
            if(!_exclusionObjects.Contains(i))
            {
                //iが板ではないなら
                if (!i.CompareTag("WeighingBoard"))
                {
                    _exclusionObjects.Add(i);   //除外に入れる
                    return i.GetComponent<WeighingBoard_ObjectOnObject>().IfThatObjectToWeighingBoard(_exclusionObjects);  //除外したオブジェクトでもう一度処理をする
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
