using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReboundBall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(-7.0f, 6.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    
}
