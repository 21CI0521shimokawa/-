using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : StageGimmick
{
    enum STATE { NORMAL, PRESSED };
    [Header("PressurePlate Script")]
    [SerializeField] STATE m_state;
    [Tooltip("押されているときの移動"), SerializeField] Vector2 sinkTo = new Vector2(0f, -0.1f);
    [Tooltip("同じ番号のボタンの確認が必要"), SerializeField] bool needAllSameButtonCheck = true;
    [Tooltip("検知アイテム"), SerializeField] LayerMask searchLayer = ~0;

    List<GameObject> m_matchItem; //同じ番号を持っているギミック
    List<GameObject> m_matchButton; //同じ番号のボタン
    Vector2 m_normalPos; //初期位置

    [SerializeField, Tooltip("SE")]
    AudioClip SE;
    [SerializeField, Tooltip("オーディオsource")]
    AudioSource AudioSource;
    [SerializeField]

    void Start()
    {
        GameManager.Instance.RegisterButton(this.gameObject);
        m_normalPos = transform.position;
    }

    void Update()
    {
        SwitchState();
    }

    void SwitchState()
    {
        switch (m_state)
        {
            case STATE.NORMAL:
                transform.position = Vector3.MoveTowards(transform.position, m_normalPos, 1);

                if (CheckAboveItem())
                {
                    _IsOpen = true;
                    m_state = STATE.PRESSED;

                    //同じ番号のボタンの状況確認
                    if (CheckSameNumberButton())
                    {
                        SECheck();
                        OpenItem();
                    }
                }
                break;
            case STATE.PRESSED:
                transform.position = Vector3.MoveTowards(transform.position, m_normalPos + sinkTo, 1);
                //OpenItem();
                if (!CheckAboveItem())
                {
                    _IsOpen = false;
                    m_state = STATE.NORMAL;
                    CloseItem();
                }
                break;
        }
    }

    void SECheck()
    {
        if(AudioSource.isPlaying==false)
        {
            PlaySE(SE);
        }
        if(AudioSource.isPlaying)
        {
            return;
        }
    }

    /// <summary>
    /// ボタン上のアイテム確認
    /// </summary>
    /// <returns></returns>
    bool CheckAboveItem()
    {
        if (!_IsOpen)
        {
            return false;
        }

        Vector2 pos = transform.position;

        var colliders = Physics2D.OverlapBoxAll(
            pos + new Vector2(0f, transform.localScale.y / 6.0f),
            new Vector2(transform.localScale.x / 2f, 1f), 0f,
            searchLayer
        );
        
        if (colliders.Length > 0)
        {
            foreach (var item in colliders)
            {
                if (item.gameObject.tag == "Slime")
                {
                    Slime_Outage(colliders);
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 同じ番号を持っているボタンの状態確認
    /// </summary>
    /// <returns></returns>
    bool CheckSameNumberButton()
    {
        //確認いらない場合抜ける
        if(!needAllSameButtonCheck)
        {
            return true;
        }

        if(GameManager.Instance.GetSameNumberButton(_Number, out m_matchButton))
        {
            foreach(var button in m_matchButton)
            {
                if(!button.GetComponent<StageGimmick>()._IsOpen)
                {
                    return false;
                }
            }
        }
        return true;
    }  

    void OpenItem()
    {
        if(GameManager.Instance.SearchItem(_Number, out m_matchItem))
        {
            foreach(var item in m_matchItem)
            {
                item.GetComponent<StageGimmick>().Open();  
            }
        }
    }

    void CloseItem()
    {
        if(GameManager.Instance.SearchItem(_Number, out m_matchItem))
        {
            foreach(var item in m_matchItem)
            {
                item.GetComponent<StageGimmick>().Close();
            }
        }
    }
    public void PlaySE(AudioClip audio)
    {
        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("オーディオソースが設定されてない");
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject)
        {
            _IsOpen = true;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(transform.position + new Vector3(0f, transform.localScale.y / 6.0f),
    //         new Vector2(transform.localScale.x / 2f, 1f));
    // }


    //スライムの操作停止 消えるまでの時間延長
    private void Slime_Outage(Collider2D[] _colliders)
    {
        foreach (var item in _colliders)
        {
            if (item.gameObject.tag == "Slime")
            {
                SlimeController slimeController = item.gameObject.GetComponent<SlimeController>();
                if (!slimeController.core && slimeController._ifOperation)
                {
                    slimeController._ifOperation = false;
                    slimeController.deadTime += 10.0f;
                }
            }
        }
    }
}
