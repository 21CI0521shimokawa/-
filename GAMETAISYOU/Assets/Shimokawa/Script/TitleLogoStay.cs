using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] TitleAudioManager TitleLogo;
    [SerializeField] GameObject StartLogo;
    [SerializeField] GameObject TimeLine;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip StartSE;
    [SerializeField] float StayTime;
    
    private void Awake()
    {
        StartLogo.SetActive(false);
    }

    void Start()
    {
        StartCoroutine(Stay());
    }

    private void Update()
    {
        if (TimeLine.activeSelf&&StartLogo.activeSelf)
        {
            PlayAudio.PlaySE(StartSE);
            ObjectDorw(StartLogo, false);
        }
    }

    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(StayTime);//ƒXƒ‰ƒCƒ€‚ª—Ž‚¿’…‚­‚Ü‚Å‘Ò‚Â
        ObjectDorw(StartLogo, true);
    }

    private void ObjectDorw(GameObject SubjectObject, bool IsDrow)
    {
        switch (IsDrow)
        {
            case true:
                SubjectObject.SetActive(true);
                break;
            case false:
                SubjectObject.SetActive(false);
                break;
        }
    }
}
