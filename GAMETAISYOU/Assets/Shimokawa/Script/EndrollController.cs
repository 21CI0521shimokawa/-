using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndrollController : MonoBehaviour
{
    [SerializeField] private float textScrollSpeed = 30; //テキストのスクロールスピード
    [SerializeField] private float limitPosition = 730f; //テキストの制限位置
    [SerializeField] private GameObject LastText;//最後に表示するテキスト
    private bool isStopEndRoll; //エンドロールが終了したかどうか

    void Update()
    {
        if (transform.position.y <= limitPosition) //エンドロール用テキストがリミットを越えるまで動かす
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + textScrollSpeed * Time.deltaTime);
        }
        else
        {
            LastText.SetActive(true);
            isStopEndRoll = true;
        }
    }
}
