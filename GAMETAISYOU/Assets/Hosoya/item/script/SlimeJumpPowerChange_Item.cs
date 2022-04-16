using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeJumpPowerChange_Item : MonoBehaviour
{
    [SerializeField] float chengePower;  //強さの変化量

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

            foreach (GameObject slime in Slimes)
            {
                slime.GetComponent<Slime_Haziku>()._power += chengePower;
            }

            Destroy(this.gameObject);
        }
    }
}
