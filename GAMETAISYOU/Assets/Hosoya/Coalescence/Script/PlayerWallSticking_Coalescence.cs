using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSticking_Coalescence : MonoBehaviour
{
    public PlayerController_Coalescence playerController;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Vector2 velocityCopy = playerController.rigid2D.velocity;
            float vectorCos = Vector2.Dot(new Vector2(0.0f, 1.0f), velocityCopy);

            if (playerController.rigid2D.velocity.magnitude > 2.5f && vectorCos < -0.95f)
            {

                velocityCopy.Normalize();
                playerController.rigid2D.velocity = velocityCopy * 4.0f;
            }

            playerController.s_state = State.MOVE;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            playerController.s_state = State.MOVE;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            playerController.s_state = State.AIR;
        }
        if (collision.gameObject.tag == "Ground")
        {
            playerController.s_state = State.AIR;
        }
    }
}
