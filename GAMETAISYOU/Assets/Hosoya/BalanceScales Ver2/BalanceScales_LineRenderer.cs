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
        new Vector3(left.transform.position.x, left.transform.position.y + 0.38f, -5),
        new Vector3(leftPulley.transform.position.x, leftPulley.transform.position.y, -5),
        new Vector3(rightPulley.transform.position.x, rightPulley.transform.position.y, -5),
        new Vector3(right.transform.position.x, right.transform.position.y + 0.38f, -5)
        };

        // “_‚Ì”‚ğw’è‚·‚é
        lineRenderer.positionCount = positions.Length;

        // ü‚ğˆø‚­êŠ‚ğw’è‚·‚é
        lineRenderer.SetPositions(positions);
    }
}
