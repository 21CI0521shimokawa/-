using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeDeadTimeChange_Item : MonoBehaviour
{
    [SerializeField] float chengeTime;  //時間の変化量

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
            //全てのスライムを取得
            GameObject[] Slimes = GameObject.FindGameObjectsWithTag("Slime");

            foreach(GameObject slime in Slimes)
            {
                SlimeController buf = slime.GetComponent<SlimeController>();

                //コアなら
                if (buf.core)
                {
                    slime.GetComponent<Slime_Tearoff>().deadEndTime += chengeTime;
                }
                else
                {
                    if(buf.deadTime != 0.0f)
                    {
                        buf.deadTime += chengeTime;
                    }
                }
            }

            Destroy(this.gameObject);
        }
    }
}
