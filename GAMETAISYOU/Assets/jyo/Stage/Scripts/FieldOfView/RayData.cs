using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayData : MonoBehaviour
{
    public Vector3 m_start; //開始点
    public float m_distance; //長さ
    public float m_angle; //視野角
    public Vector3 m_direction; //射線方向
    public Vector3 m_end; //終了点
    public Collider m_hitCollider; //当たった物体
    public bool m_hit;

    public RayData(Vector3 start_, float angle_, float distance_)
    {
        m_start = start_;
        m_distance = distance_;

    }

    /// <summary>
    /// 射線方向更新
    /// </summary>
    /// <param name="angle_"></param>
    public void UpdateDirection(float angle_)
    {
        m_angle += angle_;
        m_direction = DirectionFromAngle(m_angle);
        m_end = m_start + m_direction * m_distance;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="angle_"></param>
    /// <returns></returns>
    Vector3 DirectionFromAngle(float angle_)
    {
        float pi = Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angle_ * pi), 0, Mathf.Cos(angle_ * pi));
    }

    public static bool IsHittingSameObject(RayData data1_, RayData data2_)
    {
        return data1_.m_hitCollider == data2_.m_hitCollider;
    }
}
