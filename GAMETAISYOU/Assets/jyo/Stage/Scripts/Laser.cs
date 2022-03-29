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

    GameObject m_hitObject; //���������I�u�W�F�N�g
    LineRenderer m_lineRenderer; //���C�̕`��
    Vector2 m_hitPos; //���������|�C���g
    float lineWidth = 0.15f; //���̕�

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

        //�폜�\��
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
                m_hitPos = hitInfo.point; //���������ʒu
                m_hitObject = hitInfo.collider.gameObject; //���������I�u�W�F�N�g
                m_lineRenderer.SetPosition(1, m_hitPos); //�`��I�_
            }
        }
        else
        {
            Vector2 endPoint = transform.up * rayLength;
            m_lineRenderer.SetPosition(1, pos + endPoint); //�`��I�_
        }
        Debug.DrawRay(ray2D.origin, ray2D.direction * rayLength, Color.red);
    }

    /// <summary>
    /// �A�C�e���I�u�W�F�N�g��j�󂷂�
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
    /// �v���C���[��j�󂵂āA�X�e�[�W�����Z�b�g����
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
        //�j��ڕW���ɂ���ĐF�ύX
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
