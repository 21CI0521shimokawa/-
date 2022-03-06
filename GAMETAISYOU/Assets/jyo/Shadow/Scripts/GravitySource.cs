using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySource : MonoBehaviour
{
    public Vector2 GetGravity (Vector2 position){
        return Physics2D.gravity;
    }


}
