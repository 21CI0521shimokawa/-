using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterColliderOff : MonoBehaviour
{
    [SerializeField] Collider2D collider;
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Opened()
    {
        collider.enabled = false;
        spriteRenderer.enabled = false;
    }
}
