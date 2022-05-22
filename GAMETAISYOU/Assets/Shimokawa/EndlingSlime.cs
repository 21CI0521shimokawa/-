using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndlingSlime : MonoBehaviour
{
    [SerializeField]
    private float Speed;
    void Update()
    {
        transform.Translate(Speed * Time.deltaTime, 0, 0);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ending")
        {
            FadeManager.Instance.LoadScene("Title", 4f);
        }
    }
}
