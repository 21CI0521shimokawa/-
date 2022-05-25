using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSizeChange_Item : MonoBehaviour
{
    [SerializeField] float chengeSize;  //大きさの変化量
    [SerializeField] AudioSource audioSource;

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
        //スライムだったら
        if (collision.gameObject.tag == "Slime")
        {
            //コアだったら
            SlimeController slimeController = collision.gameObject.GetComponent<SlimeController>();
            if (slimeController.core)
            {
                slimeController._scaleMax += chengeSize;
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
                Destroy(this.gameObject);
            }
        }
    }
        
}
