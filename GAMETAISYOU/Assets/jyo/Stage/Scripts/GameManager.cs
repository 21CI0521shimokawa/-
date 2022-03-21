using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]List<GameObject> m_doorGroup;
    [SerializeField]List<GameObject> m_laserGroup;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        m_doorGroup = new List<GameObject>();
        m_laserGroup = new List<GameObject>();
        DontDestroyOnLoad(this);
    }

    //マップアイテム登録（ドアー）
    public void RegisterDoor(GameObject door_)
    {
        if (!m_doorGroup.Contains(door_))
            m_doorGroup.Add(door_);
    }
    //マップアイテム登録（レザー）
    public void RegisterLaser(GameObject laser_)
    {
        if (!m_laserGroup.Contains(laser_))
            m_laserGroup.Add(laser_);
    }

    //対応マップアイテム取得（ドアー）
    public bool SearchDoor(int num_, out List<GameObject> doors)
    {
        doors = new List<GameObject>();
        foreach(var item in m_doorGroup)
        {
            if(item.GetComponent<Door>()._Number == num_)
                doors.Add(item);
        }

        //一つもない場合、flaseを返す
        if (doors.Count > 0)
            return true;
        else
            return false;
    }
    //対応マップアイテム取得（レザー）
    public bool SearchLaser(int num_, out List<GameObject> lasers)
    {
        lasers = new List<GameObject>();
        foreach(var item in m_laserGroup)
        {
            if (item.GetComponent<Laser>()._Number == num_)
                lasers.Add(item);
        }

        if (lasers.Count > 0)
            return true;
        else
            return false;
    }

    public void ResetStageData()
    {
        m_doorGroup.Clear();
        m_laserGroup.Clear();
    } 
}
