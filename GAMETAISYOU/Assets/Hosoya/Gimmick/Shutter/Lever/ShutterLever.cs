using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterLever : MonoBehaviour
{
    [SerializeField] GameObject bar;
    [SerializeField] HingeJoint2D hingeJoint2D;
    [SerializeField] ShutterController shutter;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip audioClip;

    bool isStartOpen;

    // Start is called before the first frame update
    void Start()
    {
        isStartOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(bar.transform.rotation.z);

        //‰ñ“]’âŽ~
        if(bar.transform.rotation.z >= 0.35f && !isStartOpen)
        {
            isStartOpen = true;

            hingeJoint2D.useMotor = true;
            shutter._Open();
            audioSource.PlayOneShot(audioClip);
        }
    }


}
