using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeNoCoreMoveChangeArea : MonoBehaviour
{
    [SerializeField] Slime_Tearoff.TearoffSlimeType changeTearoffSlimeType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //スライムかどうか
        if (collision.gameObject.tag == "Slime")
        {
            SlimeController buf = collision.gameObject.GetComponent<SlimeController>();

            //コアだったら
            if(buf.core)
            {
                buf._tearoff._tearoffSlimeType = changeTearoffSlimeType;
            }
        }

    }
}
