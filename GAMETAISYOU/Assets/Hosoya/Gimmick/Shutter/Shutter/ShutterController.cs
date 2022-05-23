using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutterController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;

    [SerializeField] LightBrightnessChange lightBrightnessChange;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void _Open()
    {
        animator.SetTrigger("ShutterOpen");
        audioSource.Play();

        if(lightBrightnessChange)
        {
            lightBrightnessChange.BrightnessChangeStart();
        }
    }
}
