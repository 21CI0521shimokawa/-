using System.Runtime.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : StageGimmick
{
    [Header("Laser Script")]
    [Tooltip("�n�_�̕Έڗ�"), SerializeField] Vector2 rayStartPointOffset = Vector2.zero;
    [Tooltip("���C�̒���"), SerializeField, Range(1f, 100f)] public float rayLength = 10f;
    [Tooltip("�A�C�e���j��"), SerializeField] bool destoryItem = true;
    [Tooltip("�v���C���[�j��"), SerializeField] bool destoryPlayer = false;
    [Tooltip("�j��ڕW�؂�ւ�"), SerializeField] bool pressToSwitchTarget = false;
    [Tooltip("���m�͈�"), SerializeField] LayerMask searchLayer = ~0;

    [SerializeField] GameObject startVFX;
    [SerializeField] GameObject endVFX;
    [SerializeField] GameObject startBeam;
    [SerializeField] GameObject endBeam;

    [SerializeField] GameObject hitObject; //���������I�u�W�F�N�g
    LineRenderer lineRenderer; //���C�̕`��
    Material material;
    Vector2 hitPos; //���������|�C���g

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
                hitObject = hitInfo.collider.gameObject; //���������I�u�W�F�N�g
                ItemDestory(hitObject);
            }

            hitPos = hitInfo.point; //���������ʒu
            lineRenderer.SetPosition(1, hitPos); //�`��I�_
            endVFX.transform.position = hitPos;
            endBeam.transform.position = hitPos;
        }
        else
        {
            Vector2 endPoint = transform.up * rayLength;
            lineRenderer.SetPosition(1, pos + endPoint); //�`��I�_
            hitObject = null;

            endVFX.transform.position = pos + endPoint;
            endBeam.transform.position = pos + endPoint;
        }

        //if (hitInfo)
        //{
        //    if (!ItemDestory(hitInfo.collider.gameObject))
        //    {
        //        m_hitPos = hitInfo.point; //���������ʒu
        //        _hitObject = hitInfo.collider.gameObject; //���������I�u�W�F�N�g
        //        _lineRenderer.SetPosition(1, m_hitPos); //�`��I�_

        //        endVFX.transform.position = m_hitPos;
        //        endBeam.transform.position = m_hitPos;
        //    }
        //}
        //else
        //{
        //    Vector2 endPoint = transform.up * rayLength;
        //    _lineRenderer.SetPosition(1, pos + endPoint); //�`��I�_

        //    endVFX.transform.position = pos + endPoint;
        //    endBeam.transform.position = pos + endPoint;
        //}
        Debug.DrawRay(ray2D.origin, ray2D.direction * rayLength, Color.red);
    }

    /// <summary>
    /// �A�C�e���I�u�W�F�N�g��j�󂷂�
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
    /// �v���C���[��j�󂵂āA�X�e�[�W�����Z�b�g����
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
