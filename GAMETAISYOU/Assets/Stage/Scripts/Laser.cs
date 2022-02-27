using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [Header("�Ή��ԍ�")] public int _Number;

    [Header("���C�֌W")]
    [Tooltip("���C�̒���")] public float _RayLength;
    [Tooltip("�n�_�̕Έڗ�")] public float _RayStartPointOffset;
    [Tooltip("���̕�")] public float _LineWidth;

    GameObject m_hitObject; //���������I�u�W�F�N�g
    LineRenderer m_lineRenderer; //���C�̕`��
    Vector2 m_hitPos; //���������|�C���g
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
                m_hitPos = hitInfo.point; //���������ʒu
                m_hitObject = hitInfo.collider.gameObject; //���������I�u�W�F�N�g
                m_lineRenderer.SetPosition(1, m_hitPos); //�`��I�_
            }
        }
        else
        {
            Vector2 endPoint = transform.up * _RayLength;
            m_lineRenderer.SetPosition(1, pos + endPoint); //�`��I�_
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
