using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSizeChange_Item : MonoBehaviour
{
    [SerializeField] float chengeSize;  //‘å‚«‚³‚Ì•Ï‰»—Ê

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
        //ƒXƒ‰ƒCƒ€‚¾‚Á‚½‚ç
        if (collision.gameObject.tag == "Slime")
        {
            collision.gameObject.GetComponent<SlimeController>()._scaleMax += chengeSize;
            Destroy(this.gameObject);
        }
    }
        
}
