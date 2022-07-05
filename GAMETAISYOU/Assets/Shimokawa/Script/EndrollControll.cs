using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndrollControll : MonoBehaviour
{
    //　テキストのスクロールスピード
    [SerializeField]
    private float textScrollSpeed = 30;
    //　テキストの制限位置
    [SerializeField]
    private float limitPosition = 730f;
    //　エンドロールが終了したかどうか
    private bool isStopEndRoll;
    //　シーン移動用コルーチン
    private Coroutine endRollCoroutine;

    //最後に表示するテキスト
    [SerializeField]
    private GameObject LastText;
    // Update is called once per frame
    void Update()
    {

        //　エンドロール用テキストがリミットを越えるまで動かす
        if (transform.position.y <= limitPosition)
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
