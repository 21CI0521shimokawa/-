using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevator : MonoBehaviour
{
    private Vector3 pos;
    [SerializeField, Tooltip("Y�̈ʒu")]
    private float DestinationPosY;
    public Vector3 DestinationPos;
    public bool is2ndFloor;

    void Start()
    {
        DestinationPos = new Vector3(0, DestinationPosY, 0);
        pos = this.transform.position;
        is2ndFloor = false;
        MoveUp();
    }
    public void MoveUp()
    {
        StartCoroutine("MoveUpStart");
    }

    IEnumerator MoveUpStart()
    {
        while (pos.y < DestinationPos.y)
        {
            pos = transform.position;
            transform.Translate(0, 0.02f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        is2ndFloor = true;
    }
    public void MoveDown()
    {
        StartCoroutine("MoveDownStart");

    }

    IEnumerator MoveDownStart()
    {
        while (pos.y > 0.0f)
        {
            pos = transform.position;
            transform.Translate(0, -0.02f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        is2ndFloor = false;
    }
}