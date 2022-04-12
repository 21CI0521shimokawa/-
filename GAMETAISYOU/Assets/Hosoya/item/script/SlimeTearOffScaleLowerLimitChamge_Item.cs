using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTearOffScaleLowerLimitChamge_Item : MonoBehaviour
{
    [SerializeField] float chengeLimitSize;  //�傫���̕ω���

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
        //�X���C����������
        if (collision.gameObject.tag == "Slime")
        {
            //�S�ẴX���C�����擾
            GameObject[] Slimes = GameObject.FindGameObjectsWithTag("Slime");

            foreach (GameObject slime in Slimes)
            {
                SlimeController buf = slime.GetComponent<SlimeController>();

                //�R�A�Ȃ�
                if (buf.core)
                {
                    slime.GetComponent<Slime_Tearoff>()._scaleLowerLimit += chengeLimitSize;
                    break;
                }
            }

            Destroy(this.gameObject);
        }
    }
}
