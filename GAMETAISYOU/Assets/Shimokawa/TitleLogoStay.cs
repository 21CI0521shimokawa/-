using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleLogoStay : MonoBehaviour
{
    [SerializeField] GameObject StartLogo;
    [SerializeField] GameObject TimeLine;
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
        if(TimeLine)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator Stay()
    {
        yield return new WaitForSeconds(7f);//ƒXƒ‰ƒCƒ€‚ª—Ž‚¿’…‚­‚Ü‚Å‘Ò‚Â
        StartLogo.SetActive(true);
    }
}
