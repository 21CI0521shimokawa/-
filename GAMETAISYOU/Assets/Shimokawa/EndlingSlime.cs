using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndlingSlime : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    bool isMove = true;
    bool stopOnce;

    void Update()
    {
        if(isMove) { transform.Translate(moveSpeed * Time.deltaTime, 0, 0); }
        if(!stopOnce && transform.position.x <= -1){ isMove = false; stopOnce = true; }
    }

    public void Move()
    {
        isMove = true;
        moveSpeed = -1.2f;
    }

    public void ToTitle()
    {
        FadeManager.Instance.LoadScene("Title", 4f);
    }
}
