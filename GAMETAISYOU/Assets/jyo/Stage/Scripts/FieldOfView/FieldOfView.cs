using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FieldOfViewType
{
    Original,
    Normal,
    Approximation, //近似法
    Bisection, //二分法
}

public class FieldOfView : MonoBehaviour
{
    [SerializeField] FieldOfViewType m_fieldOfViewType;
    [SerializeField] protected float m_radius;
    [SerializeField] protected float m_angle = 45f;
    [SerializeField] protected int m_divide = 2; //視野角の分割数
    [SerializeField] protected float m_approximationPrecision = 0.01f;
    [SerializeField] protected int m_bisectionCount = 10;

    RaycastHit m_hit;

    public RayData[] GetRayDatas()
    {
        RayData[] datas = null;

        switch(m_fieldOfViewType)
        {
            case FieldOfViewType.Original:
                datas = GetOriginalDatas();
                break;
            case FieldOfViewType.Normal:
                datas = GetNormalDatas();
                break;
            case FieldOfViewType.Approximation:
                break;
            case FieldOfViewType.Bisection:
                break;
        }

        return datas;
    }

    RayData[] GetOriginalDatas()
    {
        RayData[] rayDatas = new RayData[m_divide + 1];

        Vector3 center = transform.position; //本体位置
        float startAngle = transform.eulerAngles.y - m_angle / 2; //左右等分
        float angle = m_angle / m_divide; //分割数に応じて
        RayData rayDataCache = null;

        for (int i = 0; i <= m_divide; i++)
        {
            rayDataCache = new RayData(center, startAngle + angle * i, m_radius);

            rayDatas[i] = rayDataCache;
        }

        return rayDatas;
    }

    private RayData[] GetNormalDatas()
    {
        RayData[] rayDatas = GetOriginalDatas();

        for (int i = 0; i < rayDatas.Length; i++)
        {
            UpdateRaycast(rayDatas[i]);
        }

        return rayDatas;
    }

    private void UpdateRaycast(RayData rayData)
    {
        rayData.m_hit = Physics.Raycast(transform.position, rayData.m_direction, out m_hit, m_radius);

        //物体を当たったら、その位置を記録する
        //当たらなかったら、限界距離の位置を記録する
        if (rayData.m_hit)
        {
            rayData.m_hitCollider = m_hit.collider;
            rayData.m_end = m_hit.point;
        }
        else
        {
            rayData.m_hitCollider = null;
            rayData.m_end = rayData.m_start + rayData.m_direction * m_radius;
        }
    }
}
