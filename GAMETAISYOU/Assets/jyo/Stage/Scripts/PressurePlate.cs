using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{ 
    [Header("対応番号")]
    public int _Number;

    List<GameObject> m_matchItem;   //対応番号のマップアイテム
    Vector3 m_normalPos;    //初期位置
    enum STATE { NORMAL, PRESSED };
    [SerializeField]STATE m_state;

    void Start()
    {
        m_matchItem = new List<GameObject>();
        m_normalPos = transform.position;
    }

    void Update()
    {
        SwitchState();
    }

    //状態更新
    void SwitchState()
    {
        switch (m_state)
        {
            case STATE.NORMAL:
                transform.position = Vector3.MoveTowards(transform.position, m_normalPos, Time.deltaTime);

                //状態変更
                if (IsPressed())
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow;
                    m_state = STATE.PRESSED;
                    OpenItem();
                }
                break;
            case STATE.PRESSED:
                transform.position = Vector3.MoveTowards(transform.position, m_normalPos + new Vector3(0, -0.3f, 0), Time.deltaTime);

                //状態変更
                if (!IsPressed())
                {
                    GetComponent<SpriteRenderer>().color = Color.green;
                    m_state = STATE.NORMAL;
                    CloseItem();
                }
                break;
        }
    }

    //ボタンの状態確認
    bool IsPressed()
    {
        Vector2 pos = transform.position;
        //判定範囲
        var colliders =  Physics2D.OverlapBoxAll(pos + new Vector2(0f, 0.4f), new Vector2(0.5f, 0.2f), 0f);
        foreach(var item in colliders)
        {
            if (item.CompareTag("Slime") || item.CompareTag("Player"))
                return true;
        }
        return false;
    }

    //対応番号のマップアイテムを起動する
    void OpenItem()
    {
        if (GameManager.Instance.SearchDoor(_Number, out m_matchItem))
        {
            foreach (var item in m_matchItem)
                item.GetComponent<Door>().Open();
        }

        if(GameManager.Instance.SearchLaser(_Number, out m_matchItem))
        {
            foreach (var item in m_matchItem)
                item.GetComponent<Laser>().Open();
        }
    }

    //対応番号のマップアイテムを終了する
    void CloseItem()
    {
        if (GameManager.Instance.SearchDoor(_Number, out m_matchItem))
        {
            foreach (var item in m_matchItem)
                item.GetComponent<Door>().Close();
        }

        if(GameManager.Instance.SearchLaser(_Number, out m_matchItem))
        {
            foreach (var item in m_matchItem)
                item.GetComponent<Laser>().Close();
        }
    }

    //判定範囲を可視化する
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.4f, 0), new Vector2(0.5f, 0.2f));
    }
}
