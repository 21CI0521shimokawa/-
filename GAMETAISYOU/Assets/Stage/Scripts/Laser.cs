using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("対応番号")] public int _Number;

    [Header("レイ関係")]
    [Tooltip("レイの長さ")] public float _RayLength;
    [Tooltip("始点の偏移量")] public float _RayStartPointOffset;
    [Tooltip("線の幅")] public float _LineWidth;

    GameObject m_hitObject; //当たったオブジェクト
    LineRenderer m_lineRenderer; //レイの描画
    Vector2 m_hitPos; //当たったポイント
    bool isOpen;

    void Start()
    {
        GameManager.Instance.RegisterLaser(this.gameObject);
        m_lineRenderer = GetComponent<LineRenderer>();
        m_lineRenderer.startWidth = _LineWidth;

        if (_Number == -1) isOpen = true;
    }

    void Update()
    {
        if(isOpen)
            RayCheck();
    }

    void RayCheck()
    {
        Vector2 rayStartPoint = transform.up * _RayStartPointOffset;
        Vector2 pos = transform.position;

        RaycastHit2D hitInfo;
        Ray2D ray2D = new Ray2D(pos + rayStartPoint, transform.up);
        m_lineRenderer.SetPosition(0, transform.position);

        hitInfo = Physics2D.Raycast(ray2D.origin, ray2D.direction, _RayLength);
        if (hitInfo)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                GameManager.Instance.ResetStageData();
                SwitchScene.ReloadScene();
            }
            else
            {
                m_hitPos = hitInfo.point; //当たった位置
                m_hitObject = hitInfo.collider.gameObject; //当たったオブジェクト
                m_lineRenderer.SetPosition(1, m_hitPos); //描画終点
            }
        }
        else
        {
            Vector2 endPoint = transform.up * _RayLength;
            m_lineRenderer.SetPosition(1, pos + endPoint); //描画終点
        }
        Debug.DrawRay(ray2D.origin, ray2D.direction * _RayLength, Color.red);
    }

    //------------------------------------------------
    public void Open()
    {
        isOpen = true;
        m_lineRenderer.enabled = true;
    }
    public void Close()
    {
        isOpen = false;
        m_lineRenderer.enabled = false;
    }
}
