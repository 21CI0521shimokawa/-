using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject StartLogo;
    [SerializeField] GameObject TimeLine;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip StartSE;
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
            PlaySE(StartSE);
            StartLogo.SetActive(false);
        }
    }
    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(8f);//�X���C�������������܂ő҂�
        StartLogo.SetActive(true);
    }
    public void PlaySE(AudioClip audio)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ肳��ĂȂ�");
        }
    }
}
