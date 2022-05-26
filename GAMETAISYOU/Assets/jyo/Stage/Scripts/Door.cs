using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : StageGimmick
{
    [Header("Door Script")]
    [Tooltip("歯車アニメーター"), SerializeField] Animator gear;
    [Tooltip("移動量"), SerializeField] Vector2 moveTo = new Vector2(0, 2);
    [Tooltip("移動速度"), SerializeField] float moveSpeed = 5f;

    [SerializeField] AudioSource audioSource;
    bool isOpenOneFlameBefore;    

    Vector2 m_normalPos;//初期位置

    void Start()
    {
        //オブジェクトをゲームマネージャーのリストに登録する
        GameManager.Instance.RegisterDoor(this.gameObject); 
        m_normalPos = transform.position;

        isOpenOneFlameBefore = false;
    }

    void Update()
    {
        Movement();
    }

    //状態による移動設定
    void Movement()
    {
        if (_IsOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos + moveTo, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_normalPos + moveTo) < 1e-3)
            {
                gear.SetBool("Open", false);
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 5 * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_normalPos) < 1e-3)
            {
                gear.SetBool("Open", false);
            }
        }

        PlaySE();
    }

    //SE再生
    void PlaySE()
    {
        if(isOpenOneFlameBefore != _IsOpen)
        {
            isOpenOneFlameBefore = _IsOpen;
            audioSource.Play();
        }
    }


    //-------------------------------------------------------
    //起動する
    public override void Open()
    {
        _IsOpen = !_IsOpen;
        gear.SetBool("Open", true);
    }
    //終了する
    public override void Close()
    {
        _IsOpen = !_IsOpen;
        gear.SetBool("Open", true);
    }
}
