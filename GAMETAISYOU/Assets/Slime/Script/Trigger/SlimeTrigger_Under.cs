using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrigger_Under : MonoBehaviour
{
    [SerializeField] GameObject slime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (slime.GetComponent<SlimeController>().s_state == State.AIR)
        {
            slime.GetComponent<SlimeController>().s_state = State.MOVE;
            //rigid2D.velocity = Vector3.zero;
        }
    }
}
