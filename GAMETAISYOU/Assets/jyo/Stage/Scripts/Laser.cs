using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : StageGimmick
{
    [Header("Laser Script")]
    [Tooltip("始点の偏移量"), SerializeField] Vector2 rayStartPointOffset = Vector2.zero;
    [Tooltip("レイの長さ"), SerializeField, Range(1f, 100f)] public float rayLength = 10f;
    [Tooltip("アイテム破壊"), SerializeField] bool destoryItem = true;
    [Tooltip("プレイヤー破壊"), SerializeField] bool destoryPlayer = false;
    [Tooltip("破壊目標切り替え"), SerializeField] bool pressToSwitchTarget = false;
    [Tooltip("検知範囲"), SerializeField] LayerMask searchLayer = ~0;

    GameObject m_hitObject; //当たったオブジェクト
    LineRenderer m_lineRenderer; //レイの描画
    Vector2 m_hitPos; //当たったポイント
    float lineWidth = 0.15f; //線の幅

    void Start()
    {
        GameManager.Instance.RegisterLaser(this.gameObject);
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.startWidth = m_lineRenderer.endWidth = lineWidth;

        if (_Number == -1) _IsOpen = true;
    }

    void Update()
    {
        if (_IsOpen)
        {
            RayCheck();
        }

        //削除予定
        debugfunction();
    }

    void RayCheck()
    {
        Vector2 pos = transform.position;

        RaycastHit2D hitInfo;
        Ray2D ray2D = new Ray2D(pos + rayStartPointOffset, transform.up);
        m_lineRenderer.SetPosition(0, pos);

        hitInfo = Physics2D.Raycast(ray2D.origin, ray2D.direction, rayLength, searchLayer);
        if (hitInfo)
        {
            if(!ItemDestory(hitInfo.collider.gameObject) && !PlayerDestory(hitInfo.collider.gameObject))
            {
                m_hitPos = hitInfo.point; //当たった位置
                m_hitObject = hitInfo.collider.gameObject; //当たったオブジェクト
                m_lineRenderer.SetPosition(1, m_hitPos); //描画終点
            }
        }
        else
        {
            Vector2 endPoint = transform.up * rayLength;
            m_lineRenderer.SetPosition(1, pos + endPoint); //描画終点
        }
        Debug.DrawRay(ray2D.origin, ray2D.direction * rayLength, Color.red);
    }

    /// <summary>
    /// アイテムオブジェクトを破壊する
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ItemDestory(GameObject item)
    {
        if(!destoryItem)
        {
            return false;
        }

        if(item.tag == "Item")
        {
            Destroy(item);
            return true;
        }
        return false;
    }

    /// <summary>
    /// プレイヤーを破壊して、ステージをリセットする
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool PlayerDestory(GameObject item)
    {
        if(!destoryPlayer)
        {
            return false;
        }

        if(item.tag == "Player" || item.tag == "Slime")
        {
            Destroy(item);
            GameManager.Instance.RestartStage();
            return true;
        }
        return false;
    }
    //------------------------------------------------
    public override void Open()
    {
        if(pressToSwitchTarget)
        {
            if(destoryItem)
            {
                destoryItem = false;
                destoryPlayer = true;
            }
            else
            {
                destoryItem = true;
                destoryPlayer = false;
            }
        }

        _IsOpen = !_IsOpen;
        m_lineRenderer.enabled = _IsOpen;
    }
    public override void Close()
    {
        _IsOpen = !_IsOpen;
        m_lineRenderer.enabled = _IsOpen;
    }

    void debugfunction()
    {
        //破壊目標物によって色変更
        if (destoryItem && destoryPlayer)
        {
            m_lineRenderer.material.SetColor("_Color", Color.red);
        }
        else if (destoryItem)
        {
            m_lineRenderer.material.SetColor("_Color", Color.green);
        }
        else if (destoryPlayer)
        {
            m_lineRenderer.material.SetColor("_Color", Color.cyan);
        }
    }
}
