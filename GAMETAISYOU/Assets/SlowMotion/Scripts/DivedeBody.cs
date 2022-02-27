using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivedeBody : MonoBehaviour
{
    Collider2D m_collider;

    // Start is called before the first frame update
    void Start()
    {
        m_collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            m_collider.isTrigger = true;
        else
        {
            m_collider.isTrigger = false;
            Destroy(this.gameObject, 2f);
        }
    }
}
