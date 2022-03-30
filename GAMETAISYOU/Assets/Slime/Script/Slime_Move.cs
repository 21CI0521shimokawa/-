using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Move : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;
    [SerializeField] Rigidbody2D rigidBody;

    [SerializeField] float speed; //ˆÚ“®‘¬“x

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(slimeController.moveUpdate)
        {
            float horizontal = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxisRaw("L_Stick_Horizontal") : Input.GetAxisRaw("R_Stick_Horizontal");
            if(horizontal > 0) { horizontal = 1; }
            if(horizontal < 0) { horizontal = -1; }

            float moveForceX = horizontal * speed * 0.02f;

            if (moveForceX != 0)
            {
                //XŽ²‚É‘Î‚µ‚Ä‚©‚©‚Á‚Ä‚¢‚é—Í‚ðÁ‚·
                //float velocityY = rigidBody.velocity.y;
                //rigidBody.velocity = new Vector3(0.0f, velocityY, 0.0f);
                //rigidBody.angularVelocity = 0;

                //Vector3 force = new Vector3(moveForceX, 0.0f, 0.0f);    // —Í‚ðÝ’è
                //rigidBody.AddForce(force);

                Vector3 force = new Vector3(moveForceX, rigidBody.velocity.y, 0.0f);    // —Í‚ðÝ’è
                //Debug.Log(moveForceX.ToString());
                rigidBody.velocity = force;
            }
            
            slimeController._SlimeAnimator.SetFloat("MoveSpeed", Mathf.Abs(rigidBody.velocity.x));
        }
    }
}
