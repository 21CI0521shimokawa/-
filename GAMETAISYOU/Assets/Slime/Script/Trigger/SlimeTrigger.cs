using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrigger : MonoBehaviour
{
    List<GameObject> onObjects = new List<GameObject>();    //触れているオブジェクト一覧
    [SerializeField] List<GameObject> exclusionObjects = new List<GameObject>(); //除外するオブジェクト一覧

    public bool _onTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _onTrigger = false;
        onObjects.Clear();

        //このスライム自身を除外
        exclusionObjects.Add(transform.root.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _onTrigger = false;

        //オブジェクトが範囲に入っているか検知
        foreach (GameObject i in onObjects)
        {
            //一部のオブジェクトは除外
            if (exclusionObjects.Contains(i))
            {
                continue;
            }

            //一部のタグのついたオブジェクトは除外
            switch (i.tag)
            {
                case "SlimeTrigger":            break;  //スライムトリガー
                case "Tracking":                break;  //カメラトラッキング
                case "AutomaticDoor":           break;  //自動ドア
                    //ここに追加

                default:
                    _onTrigger = true;
                    Debug.Log(i.gameObject.name);
                    break;
            }

            //トリガーがOnになったら処理終了
            if (_onTrigger)
            {
                break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //トリガーではなかったら
        if (!collision.isTrigger)
        {
            //リストに無かったら追加
            if (!onObjects.Contains(collision.gameObject))
            {
                onObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //リストにあったら消去
        if (onObjects.Contains(collision.gameObject))
        {
            onObjects.Remove(collision.gameObject);
        }
    }
}
