using System.Runtime.CompilerServices;
using System;
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

    [SerializeField] GameObject startVFX;
    [SerializeField] GameObject endVFX;
    [SerializeField] GameObject startBeam;
    [SerializeField] GameObject endBeam;

    [SerializeField] GameObject hitObject; //当たったオブジェクト
    LineRenderer lineRenderer; //レイの描画
    Material material;
    Vector2 hitPos; //当たったポイント

    void Start()
    {
        GameManager.Instance.RegisterLaser(this.gameObject);
        lineRenderer = GetComponentInChildren<LineRenderer>();
        material = GetComponent<Renderer>().material;
        //m_lineRenderer.startWidth = m_lineRenderer.endWidth = lineWidth;

        if (_Number == -1)
        {
            SetEffect();
            _IsOpen = true;
        }
    }

    void Update()
    {
        if (_IsOpen)
        {
            RayCheck();
        }
    }

    void SetEffect()
    {
        startVFX.SetActive(!startVFX.activeSelf);
        startBeam.SetActive(!startBeam.activeSelf);
        endVFX.SetActive(!endVFX.activeSelf);
        endBeam.SetActive(!endBeam.activeSelf);
    }

    void RayCheck()
    {
        Vector2 pos = transform.position;

        RaycastHit2D hitInfo;
        Ray2D ray2D = new Ray2D(pos + rayStartPointOffset, transform.up);
        lineRenderer.SetPosition(0, pos);

        hitInfo = Physics2D.Raycast(ray2D.origin, ray2D.direction, rayLength, searchLayer);

        if (hitInfo)
        {
            if (hitInfo.collider.gameObject != hitObject)
            {
                hitObject = hitInfo.collider.gameObject; //当たったオブジェクト
                ItemDestory(hitObject);
            }

            hitPos = hitInfo.point; //当たった位置
            lineRenderer.SetPosition(1, hitPos); //描画終点
            endVFX.transform.position = hitPos;
            endBeam.transform.position = hitPos;
        }
        else
        {
            Vector2 endPoint = transform.up * rayLength;
            lineRenderer.SetPosition(1, pos + endPoint); //描画終点
            hitObject = null;

            endVFX.transform.position = pos + endPoint;
            endBeam.transform.position = pos + endPoint;
        }

        //if (hitInfo)
        //{
        //    if (!ItemDestory(hitInfo.collider.gameObject))
        //    {
        //        m_hitPos = hitInfo.point; //当たった位置
        //        _hitObject = hitInfo.collider.gameObject; //当たったオブジェクト
        //        _lineRenderer.SetPosition(1, m_hitPos); //描画終点

        //        endVFX.transform.position = m_hitPos;
        //        endBeam.transform.position = m_hitPos;
        //    }
        //}
        //else
        //{
        //    Vector2 endPoint = transform.up * rayLength;
        //    _lineRenderer.SetPosition(1, pos + endPoint); //描画終点

        //    endVFX.transform.position = pos + endPoint;
        //    endBeam.transform.position = pos + endPoint;
        //}
        Debug.DrawRay(ray2D.origin, ray2D.direction * rayLength, Color.red);
    }

    /// <summary>
    /// アイテムオブジェクトを破壊する
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ItemDestory(GameObject item)
    {
        if (!destoryItem)
        {
            return false;
        }

        if (item.tag == "Item")
        {
            item.GetComponent<FallGarekiController>().DestoryThisItem();
            material.SetFloat("_LaserEdgeSmoothness", 20);
            lineRenderer.endWidth = 0.22f;
            endBeam.transform.localScale = new Vector3(2f, 2f, 2f);
            endVFX.transform.localScale = new Vector3(2f, 2f, 2f);
            Invoke("ResetLaserView", 0.5f);
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
        if (!destoryPlayer)
        {
            return false;
        }

        if (item.tag == "Player" || item.tag == "Slime")
        {
            Destroy(item);
            GameManager.Instance.RestartStage();
            return true;
        }
        return false;
    }

    void ResetLaserView()
    {
        material.SetFloat("_LaserEdgeSmoothness", 8);
        lineRenderer.endWidth = 0.2f;
        endBeam.transform.localScale = new Vector3(1f, 1f, 1f);
        endVFX.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    //------------------------------------------------
    public override void Open()
    {
        if (pressToSwitchTarget)
        {
            if (destoryItem)
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
        lineRenderer.enabled = _IsOpen;

        SetEffect();
    }
    public override void Close()
    {
        _IsOpen = !_IsOpen;
        lineRenderer.enabled = _IsOpen;

        SetEffect();
    }
}
