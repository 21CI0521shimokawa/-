using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor_Buffer : MonoBehaviour
{
    List<GameObject> beltConveyorMoveObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        beltConveyorMoveObjects.Clear();
    }

    public void _Add(GameObject object_)
    {
        beltConveyorMoveObjects.Add(object_);
    }

    public bool _SearchObject(GameObject object_)
    {
        return beltConveyorMoveObjects.Contains(object_);
    }
}
