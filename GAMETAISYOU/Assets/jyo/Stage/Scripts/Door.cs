using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : StageGimmick
{
    [Header("Door Script")]
    [Tooltip("移動量"), SerializeField] Vector2 moveTo = new Vector2(0, 2);
    [Tooltip("移動速度"), SerializeField] float moveSpeed = 5f;

    Vector2 m_normalPos;//初期位置

    void Start()
    {
        //オブジェクトをゲームマネージャーのリストに登録する
        GameManager.Instance.RegisterDoor(this.gameObject); 
        m_normalPos = transform.position;
    }

    void Update()
    {
        Movement();
    }

    //状態による移動設定
    void Movement()
    {
        if (_IsOpen)
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos + moveTo, moveSpeed * Time.deltaTime);
        else
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 5 * Time.deltaTime);
    }


    //-------------------------------------------------------
    //起動する
    public override void Open()
    {
        _IsOpen = !_IsOpen;
    }
    //終了する
    public override void Close()
    {
        _IsOpen = !_IsOpen;
    }
}
