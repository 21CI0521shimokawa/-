using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class ElevatorControll : MonoBehaviour
{
    #region SerializeField
    [SerializeField, Tooltip("ドアの開閉スピード")] float Speed;
    [SerializeField, Tooltip("移動量"),] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("ドアの移動時間")] float ElevatorMoveTime;
    [SerializeField, Tooltip("オーディオソース")] AudioSource AudioSource = null;
    [SerializeField, Tooltip("ドア開閉SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("ドアの開閉アニメーション")] Animator ElevatorAnimator;
    [SerializeField, Tooltip("移動イージング指定")] Ease EaseType;
    [SerializeField, Tooltip("フェードの時間")] float FadeTime;
    [SerializeField, Tooltip("現在のシーンの名前")] Scene SceneName;
    [SerializeField, Tooltip("スライムオブジェクトの取得")] SlimeController ClonSlime;
    [SerializeField, Tooltip("剛体取得")] Rigidbody2D ElevatorRigidbody;
    [SerializeField,Tooltip("カメラの追跡管理")] GameObject CameraActive;
    #endregion

    #region ステート管理
    private enum ElevatorState { MOVE, WAIT };
    ElevatorState NowState;
    #endregion

    void Awake()
    {
        CameraActive.SetActive(true);
    }
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag)
            .Where(tag => tag == "Slime")
            .Take(1)
            .Subscribe(collision =>
            {
                if (NowState == ElevatorState.MOVE)
                {
                    PlaySE(DoorSE);
                    this.transform.DOMoveY(MoveTo.y, ElevatorMoveTime).SetDelay(1f)
                        .OnStart(() =>
                        {//実行開始時のコールバック
                            ElevatorAnimator.SetBool("Start", true);
                        })
                        .OnComplete(() =>
                        {//実行完了時のコールバック
                            ElevatorAnimator.SetBool("Start", false);
                            ElevatorRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
                            NowState = ElevatorState.WAIT;
                            SceneChange();
                            return;
                        })
                    .SetEase(EaseType);
                }
            });
    }

    #region public function
    public void ElevatorStart() //Elevator起動時にゲームパッドの入力を引き受けない
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<SlimeController>().liveTime = 20;
        }
        CameraActive.SetActive(false);
        ElevatorAnimator.SetTrigger("Close");
    }
    public void SceneChange()  //scene移行
    {
        //現在のsceneを取得
        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Title")
        {
            FadeManager.Instance.LoadScene("S0-1", FadeTime);
            return;
        }
        if (SceneName == "S0-1")
        {
            FadeManager.Instance.LoadScene("S0-2", FadeTime);
            return;
        }
        else if (SceneName == "S0-2")
        {
            FadeManager.Instance.LoadScene("S0-3", FadeTime);
            return;
        }
        else if (SceneName == "S0-3")
        {
            FadeManager.Instance.LoadScene("S1-1", FadeTime);
        }
        else if (SceneName == "S1-1")
        {
            FadeManager.Instance.LoadScene("S1-2", FadeTime);
        }
        else if (SceneName == "S1-2")
        {
            FadeManager.Instance.LoadScene("S1-3", FadeTime);
        }
        else if (SceneName == "S1-3")
        {
            FadeManager.Instance.LoadScene("S2-1", FadeTime);
        }
        else if (SceneName == "S2-1")
        {
            FadeManager.Instance.LoadScene("S2-2", FadeTime);
        }
        else if (SceneName == "S2-2")
        {
            FadeManager.Instance.LoadScene("S2-3", FadeTime);
        }
        else if (SceneName == "S2-3")
        {
            FadeManager.Instance.LoadScene("S2-4", FadeTime);
        }
        else if (SceneName == "S2-4")
        {
            FadeManager.Instance.LoadScene("S3-1", FadeTime);
        }
        else if (SceneName == "S3-1")
        {
            FadeManager.Instance.LoadScene("S3-2", FadeTime);
        }
        else if (SceneName == "S3-2")
        {
            FadeManager.Instance.LoadScene("S3-3", FadeTime);
        }
        else if (SceneName == "S3-3")
        {
            FadeManager.Instance.LoadScene("S3-4", FadeTime);
        }
        else if (SceneName == "S3-4")
        {
            FadeManager.Instance.LoadScene("S4-1", FadeTime);
        }
        else if(SceneName== "TGS-1")
        {
            FadeManager.Instance.LoadScene("TGS-2", FadeTime);
        }
    }
    public void PlaySE(AudioClip audio) //SEを一回のみ再生
    {
        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(audio);
        }
    }
    #endregion
}