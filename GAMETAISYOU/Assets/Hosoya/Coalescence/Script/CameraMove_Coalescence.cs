using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove_Coalescence : MonoBehaviour
{
    float x, y, z;

    Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        cameraTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //âE
        if(Input.GetKey(KeyCode.RightArrow))
        {
            x += 0.1f;
        }

        //ç∂
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x -= 0.1f;
        }

        //è„
        if (Input.GetKey(KeyCode.UpArrow))
        {
            y -= 0.1f;
        }

        //â∫
        if (Input.GetKey(KeyCode.DownArrow))
        {
            y += 0.1f;
        }

        cameraTransform.position = new Vector3(x, y, z);
    }
}
