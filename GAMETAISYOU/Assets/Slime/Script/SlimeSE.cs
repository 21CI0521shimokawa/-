using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSE : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip soundJump;
    [SerializeField] AudioClip soundTearoff;
    [SerializeField] AudioClip soundMove;
    [SerializeField] AudioClip soundLanding;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void _PlayJumpSE()
    {
        if(soundJump)
        {
            audioSource.PlayOneShot(soundJump);
        }
        Debug.Log("”ò‚Ô‰¹");
    }

    public void _PlayTearoffSE()
    {
        if (soundTearoff)
        {
            audioSource.PlayOneShot(soundTearoff);
        }
        Debug.Log("‚¿‚¬‚ê‚é‰¹");
    }

    public void _PlayMoveSE()
    {
        if (soundMove)
        {
            audioSource.PlayOneShot(soundMove);
        }
        Debug.Log("•à‚­‰¹");
    }

    public void _PlayLandingSE()
    {
        if (soundLanding)
        {
            audioSource.PlayOneShot(soundLanding);
        }
        Debug.Log("’…’n‰¹");
    }
}
