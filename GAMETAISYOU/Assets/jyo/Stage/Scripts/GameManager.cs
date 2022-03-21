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

    //�}�b�v�A�C�e���o�^�i�h�A�[�j
    public void RegisterDoor(GameObject door_)
    {
        if (!m_doorGroup.Contains(door_))
            m_doorGroup.Add(door_);
    }
    //�}�b�v�A�C�e���o�^�i���U�[�j
    public void RegisterLaser(GameObject laser_)
    {
        if (!m_laserGroup.Contains(laser_))
            m_laserGroup.Add(laser_);
    }

    //�Ή��}�b�v�A�C�e���擾�i�h�A�[�j
    public bool SearchDoor(int num_, out List<GameObject> doors)
    {
        doors = new List<GameObject>();
        foreach(var item in m_doorGroup)
        {
            if(item.GetComponent<Door>()._Number == num_)
                doors.Add(item);
        }

        //����Ȃ��ꍇ�Aflase��Ԃ�
        if (doors.Count > 0)
            return true;
        else
            return false;
    }
    //�Ή��}�b�v�A�C�e���擾�i���U�[�j
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
