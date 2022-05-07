using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateControll : MonoBehaviour
{
    Vector2 Startposition;
    // Start is called before the first frame update
    private void Awake()
    {
        Startposition = this.transform.position;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Startposition;
    }
}
