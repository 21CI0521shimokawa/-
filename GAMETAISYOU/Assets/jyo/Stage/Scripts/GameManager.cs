using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] List<GameObject> m_doorGroup;
    [SerializeField] List<GameObject> m_laserGroup;
    [SerializeField] List<GameObject> m_buttonGroup;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        m_doorGroup = new List<GameObject>();
        m_laserGroup = new List<GameObject>();
        //DontDestroyOnLoad(this);
    }

    //�}�b�v�A�C�e���o�^�i�h�A�[�j
    public void RegisterDoor(GameObject door)
    {
        if (!m_doorGroup.Contains(door))
        {
            m_doorGroup.Add(door);
        }
    }
    //�}�b�v�A�C�e���o�^�i���U�[�j
    public void RegisterLaser(GameObject laser)
    {
        if (!m_laserGroup.Contains(laser))
        {
            m_laserGroup.Add(laser);
        }
    }
    //�}�b�v�A�C�e���o�^�i�{�^���j
    public void RegisterButton(GameObject button)
    {
        if (!m_buttonGroup.Contains(button))
        {
            m_buttonGroup.Add(button);
        }
    }

    //�Ή��ԍ��̃}�b�v�A�C�e�����擾����
    public bool SearchItem(int num, out List<GameObject> items)
    {
        items = new List<GameObject>();
        foreach(var item in m_doorGroup)
        {
            if(item.GetComponent<Door>()._Number == num)
            {
                items.Add(item);
            }
        }

        foreach(var item in m_laserGroup)
        {
            if(item.GetComponent<Laser>()._Number == num)
            {
                items.Add(item);
            }
        }

        if(items.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// �����ԍ��̃{�^�����擾����
    /// </summary>
    /// <param name="num"></param>
    /// <param name="items"></param>
    /// <returns></returns>
    public bool GetSameNumberButton(int num, out List<GameObject> items)
    {
        items = new List<GameObject>();
        foreach(var button in m_buttonGroup)
        {
            if(button.GetComponent<StageGimmick>()._Number == num)
            {
                items.Add(button);
            }
        }

        if(items.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetStageData()
    {
        m_doorGroup.Clear();
        m_laserGroup.Clear();
    } 

    public void RestartStage()
    {
        ResetStageData();
        SwitchScene.ReloadScene();
    }
}
