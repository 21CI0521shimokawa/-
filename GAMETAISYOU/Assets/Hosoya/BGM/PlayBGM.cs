using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBGM : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] float volume;

    [SerializeField] float fadeInTime;
    [SerializeField] float fadeOutTime;

    float fadeInTimeNow;
    float fadeOutTimeNow;

    bool isFadeIn;
    bool isFadeOut;


    [SerializeField] bool debugFadeInStart;
    [SerializeField] bool debugFadeOutStart;


    // Start is called before the first frame update
    void Start()
    {
        isFadeIn = false;
        isFadeOut = false;

        fadeInTimeNow = 0.0f;
        fadeOutTimeNow = 0.0f;

        FadeInStart();
    }

    // Update is called once per frame
    void Update()
    {
        if(isFadeIn)
        {
            fadeInTimeNow += Time.deltaTime;

            if(fadeInTimeNow > fadeInTime)
            {
                FadeInEnd();
            }
            else
            {
                audioSource.volume = fadeInTimeNow / fadeInTime * volume;
            }
        }
        else if(isFadeOut)
        {
            fadeOutTimeNow += Time.deltaTime;

            if (fadeOutTimeNow > fadeOutTime)
            {
                FadeOutEnd();
            }
            else
            {
                audioSource.volume = volume - (fadeOutTimeNow / fadeOutTime * volume);
            }
        }
        else
        {
            audioSource.volume = volume;
        }

        Debug();
    }


    void Play()
    {
        audioSource.Play();
    }

    void Stop()
    {
        audioSource.Stop();
    }

    void FadeInStart()
    {
        isFadeIn = true;

        if(isFadeOut)
        {
            FadeOutEnd();
        }

        Play();
    }

    public void _FadeOutStart()
    {
        isFadeOut = true;

        if (isFadeIn)
        {
            FadeInEnd();
        }
    }

    void FadeInEnd()
    {
        audioSource.volume = volume;
        fadeInTimeNow = 0.0f;
        isFadeIn = false;
    }

    void FadeOutEnd()
    {
        Stop();
        audioSource.volume = 0.0f;
        fadeOutTimeNow = 0.0f;
        isFadeOut = false;
    }


    void Debug()
    {
        if(debugFadeInStart)
        {
            FadeInStart();
            debugFadeInStart = false;
        }

        if(debugFadeOutStart)
        {
            _FadeOutStart();
            debugFadeOutStart = false;
        }
    }
}
