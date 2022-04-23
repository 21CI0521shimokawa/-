using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    [SerializeField] GameObject beltconveyor;
    [SerializeField] WeighingBoard weighingBoardScript;

    public bool _isOn;  //ƒ‚ƒm‚ð“®‚©‚·‚©‚Ç‚¤‚©
    public float _speed; //‘¬“x

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_isOn)
        {
            Vector2 move = new Vector2(_speed * Time.deltaTime, 0);
            move = Quaternion.Euler(0, 0, beltconveyor.transform.rotation.z) * move;

            foreach (GameObject i in weighingBoardScript.onObjectsOll)
            {
                if(i)
                {
                    i.transform.Translate(move, Space.World);
                }
            }
        }
    }
}
