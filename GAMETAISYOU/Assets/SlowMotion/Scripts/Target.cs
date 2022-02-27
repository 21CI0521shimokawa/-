using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    //public float _RecoveryTime = 10f;
    public int _HP = 2;

    SpriteRenderer m_spriteRenderer;    
    //float m_timer;
    bool m_hit;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = Color.green;
        //m_timer = _RecoveryTime;
    }

    // Update is called once per frame
    void Update()
    {
        //if (m_hit)
        //{
        //    m_timer -= Time.deltaTime;
        //    if(m_timer <= 0)
        //    {
        //        m_spriteRenderer.color = Color.green;
        //        m_hit = false;
        //        m_timer = _RecoveryTime;
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (m_hit) return;

        if (other.gameObject.tag == "DivideBody")
        {
            m_hit = true;
            m_spriteRenderer.color = Color.gray;
            Destroy(other.gameObject);

            _HP--;
            if (_HP <= 0) Destroy(gameObject);
        }
    }
}
