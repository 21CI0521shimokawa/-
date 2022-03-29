using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenWall : MonoBehaviour
{
    float destroyTimeCnt;
    float destroyTimeMax;

    // Start is called before the first frame update
    void Start()
    {
        destroyTimeCnt = 0.0f;
        destroyTimeMax = Random.Range(0.5f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimeCnt += Time.deltaTime;
        if(destroyTimeCnt >= destroyTimeMax)
        {
            Destroy(gameObject);
        }
    }
}
