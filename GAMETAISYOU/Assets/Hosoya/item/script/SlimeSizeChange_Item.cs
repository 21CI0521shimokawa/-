using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSizeChange_Item : MonoBehaviour
{
    [SerializeField] float chengeSize;  //大きさの変化量

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //スライムだったら
        if(GameObject.FindWithTag("Slime"))
        {
            collision.gameObject.GetComponent<SlimeController>().scale += chengeSize;
            Destroy(this.gameObject);
        }
    }
}
