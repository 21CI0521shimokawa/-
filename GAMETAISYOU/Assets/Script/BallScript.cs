using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public GameObject BallPrefab;   //ボールのプレファブ
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Space))
        if (Input.GetMouseButtonDown(1))
        {
            Instantiate(BallPrefab);
        }
    }
}
