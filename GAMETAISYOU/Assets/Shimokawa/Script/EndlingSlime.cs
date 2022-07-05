using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class EndlingSlime : MonoBehaviour
{
    #region SerializeField
    [SerializeField,Tooltip("スライムの移動速度")]float MoveSpeed;
    [SerializeField,Tooltip("エンディングBGM関数の取得")] PlayBGM PlayBGM;
    #endregion

    private bool IsMove = true;
    private bool StopOnce;

    void Start()
    {
        this.UpdateAsObservable()
           .Subscribe(_ =>
           {
               if (IsMove)
               {
                   transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
               }
               if (!StopOnce&&transform.position.x <= -1)
               {
                   StopOnce = true;
                   IsMove = false;
               }
           });
    }
    #region public function
    public void Move() //アニメーションイベントで参照される関数
    {
        IsMove = true;
        MoveSpeed = -1.2f;
    }

    public void ToTitle()//アニメーションイベントで参照される関数
    {
        FadeManager.Instance.LoadScene("Title", 4f);
        PlayBGM._FadeOutStart();
    }
    #endregion
}
