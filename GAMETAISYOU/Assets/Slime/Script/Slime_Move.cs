using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Move : MonoBehaviour
{
    [SerializeField] SlimeController slimeController;
    [SerializeField] Rigidbody2D rigidBody;

    [SerializeField] float speed; //à⁄ìÆë¨ìx

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
            float horizontal = slimeController.modeLR == SlimeController.LRMode.Left ? Input.GetAxis("L_Stick_Horizontal") : Input.GetAxis("R_Stick_Horizontal");

            float moveForceX = horizontal * speed * Time.deltaTime;

            if (moveForceX != 0)
            {
                //Xé≤Ç…ëŒÇµÇƒÇ©Ç©Ç¡ÇƒÇ¢ÇÈóÕÇè¡Ç∑
                float velocityY = rigidBody.velocity.y;
                rigidBody.velocity = new Vector3(0.0f, velocityY, 0.0f);
                rigidBody.angularVelocity = 0;

                Vector3 force = new Vector3(moveForceX, 0.0f, 0.0f);    // óÕÇê›íË
                rigidBody.AddForce(force);
            }
        }
    }
}
