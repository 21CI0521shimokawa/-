using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceScales_LineRenderer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject left;
    [SerializeField] GameObject leftPulley;
    [SerializeField] GameObject rightPulley;
    [SerializeField] GameObject right;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var positions = new Vector3[]{
        left.transform.position,
        leftPulley.transform.position,
        rightPulley.transform.position,
        right.transform.position
        };

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }
}
