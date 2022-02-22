using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("対応番号")]
    public int _Number;

    [SerializeField]Vector3 m_normalPos;　//初期位置
    [SerializeField]bool isOpen;

    void Start()
    {
        //オブジェクトをゲームマネージャーのリストに登録する
        GameManager.Instance.RegisterDoor(this.gameObject); 
        m_normalPos = transform.position;
        isOpen = false;
    }

    void Update()
    {
        Movement();
    }

    //状態による移動設定
    void Movement()
    {
        if (isOpen)
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos + new Vector3(0, 2, 0), 5 * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 5 * Time.deltaTime);
    }


    //-------------------------------------------------------
    //起動する
    public void Open()
    {
        isOpen = true;
    }
    //終了する
    public void Close()
    {
        isOpen = false;
    }
}
