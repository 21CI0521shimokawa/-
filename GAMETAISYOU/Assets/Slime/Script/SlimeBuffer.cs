using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBuffer : MonoBehaviour
{
    GameObject slimeCore;
    public bool _ifTearOff;  //�����������ǂ���
    public float _slimeMass; //�X���C���̏d��
    public float _doNotDeadAreaRadius;  //�X���C���������Ȃ��͈́i�R�A����̔��a�j

    [SerializeField] private LineRenderer m_lineRenderer = null; // �~��`�悷�邽�߂� LineRenderer
    [SerializeField] private float m_radius = 0;    // �~�̔��a
    [SerializeField] private float m_lineWidth = 0;    // �~�̐��̑���
    [SerializeField] private float center_x = 0;    // �~�̒��S���W
    [SerializeField] private float center_y = 0;    // �~�̒��S���W
    private void Reset()
    {
        m_lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(slimeCore)
        {
            center_x = slimeCore.transform.position.x;
            center_y = slimeCore.transform.position.y;
            m_radius = _doNotDeadAreaRadius;

            RangeRender();
        }
        else
        {
            //�{�̂�T��
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            //�z��̗v�f���ɑ΂��ď������s��
            foreach (GameObject i in slimes)
            {
                if (i.GetComponent<SlimeController>().core)  //�{�̂�������
                {
                    slimeCore = i;
                    break;
                }
            }
        }
    }

    void RangeRender()
    {
        int segments = 380;
        m_lineRenderer.startWidth = m_lineWidth;
        m_lineRenderer.endWidth = m_lineWidth;
        m_lineRenderer.positionCount = segments;
        var points = new Vector3[segments];
        for (int i = 0; i < segments; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 380f / segments);
            var x = center_x + Mathf.Sin(rad) * m_radius;
            var y = center_y + Mathf.Cos(rad) * m_radius;
            points[i] = new Vector3(x, y, 0);
        }
        m_lineRenderer.SetPositions(points);
    }
}
