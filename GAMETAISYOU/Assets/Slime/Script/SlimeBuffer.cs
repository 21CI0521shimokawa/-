using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBuffer : MonoBehaviour
{
    GameObject slimeCore;
    public bool _ifTearOff;  //ちぎったかどうか
    public float _slimeMass; //スライムの重さ
    public float _doNotDeadAreaRadius;  //スライムが消えない範囲（コアからの半径）

    [SerializeField] private LineRenderer m_lineRenderer = null; // 円を描画するための LineRenderer
    [SerializeField] private float m_radius = 0;    // 円の半径
    [SerializeField] private float m_lineWidth = 0;    // 円の線の太さ
    [SerializeField] private float center_x = 0;    // 円の中心座標
    [SerializeField] private float center_y = 0;    // 円の中心座標
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
            //本体を探す
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");
            //配列の要素一つ一つに対して処理を行う
            foreach (GameObject i in slimes)
            {
                if (i.GetComponent<SlimeController>().core)  //本体だったら
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
