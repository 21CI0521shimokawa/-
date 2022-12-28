using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    [SerializeField] PlayBGM PlayBGM;
    [SerializeField] float FadeTime;

    /// <summary>
    /// "Slime"のタグがついているオブジェクトに衝突した時
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
            FadeManager.Instance.LoadScene("GameClear", FadeTime); //GameClearシーンに移行
            PlayBGM._FadeOutStart(); //BGMをフェードアウトさせる
            collision.gameObject.GetComponent<SlimeController>()._ifOperation = false; //ゴール地点に到達したらスライムの行動を止める
        }
    }
}
