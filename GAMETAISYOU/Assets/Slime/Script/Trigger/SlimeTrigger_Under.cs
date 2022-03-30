using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrigger_Under : MonoBehaviour
{
    [SerializeField] GameObject slime;
    [SerializeField] SlimeController slimeController;

    public bool _onGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        slimeController._SlimeAnimator.SetBool("OnGround", _onGround);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _onGround = true;

        if (_onGround && slimeController._SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Fall"))
        {
            slimeController._SlimeAnimator.SetTrigger("Landing");
        }

        if (slime.GetComponent<SlimeController>().s_state == State.AIR)
        {
            slime.GetComponent<SlimeController>().s_state = State.MOVE;
            //rigid2D.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SlimeController slimeController = slime.GetComponent<SlimeController>();
        //if (slimeController._SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slime_Fall"))
        //{
        //    slimeController._SlimeAnimator.SetTrigger("Landing");
        //}

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _onGround = false;
    }
}
