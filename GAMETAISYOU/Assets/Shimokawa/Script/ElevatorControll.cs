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
    [SerializeField, Tooltip("ドアの開閉スピード")] float Speed;
    [SerializeField, Tooltip("ドアの移動時間")] float ElevatorMoveTime;
    [SerializeField, Tooltip("移動量")] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("ドア開閉SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("ドアの開閉アニメーション")] Animator ElevatorAnimator;
    [SerializeField, Tooltip("移動イージング指定")] Ease EaseType;
    [SerializeField, Tooltip("現在のシーンの名前")] Scene SceneName;
    [SerializeField, Tooltip("スライムオブジェクトの取得")] SlimeController ClonSlime;
    [SerializeField, Tooltip("剛体取得")] Rigidbody2D ElevatorRigidbody;
    [SerializeField, Tooltip("カメラの追跡管理")] GameObject CameraActive;
    private const int DoTime = 1; //衝突した後の処理を一度だけ実行するための変数
    private const float DelayTime = 1f; //アニメーションを再生してから動きだすためのDelayTimeを指定

    #region ステート管理
    private enum ElevatorState { MOVE, WAIT };
    ElevatorState NowState;
    #endregion

    /// <summary>
    /// ゲームが始まる時に一度だけStart関数より先呼ばれる関数
    /// </summary>
    void Awake()
    {
        NowState = AutoDoorState.MOVE; //ステートをMOVEにしておく
    }

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag) //衝突したオブジェクトのタグが
            .Where(tag => tag == "Slime") //"Slime"だったら
            .Take(DoTime) //一度だけ実行
            .Subscribe(collision =>
            {
                if (NowState == ElevatorState.MOVE) //現在のステートがMOVEだったら実行
                {
                    this.transform.DOMoveY(MoveTo.y, ElevatorMoveTime).SetDelay(DelayTime) //DelayTimeの時間待ってからMoveto.yの地点までElevatorMoveTimeの速さで移動
                        .OnStart(() =>
                        {
                            PlayAudio.PlaySE(DoorSE); //DoorSEを再生
                            ElevatorAnimator.SetBool("Start", true); //実行する時にアニメーション再生
                        })
                        .OnComplete(() =>
                        {
                            ElevatorAnimator.SetBool("Start", false); //移動完了したら移動終了
                            ElevatorRigidbody.constraints = RigidbodyConstraints2D.FreezePosition; //移動完了したらそこに留まる
                            NowState = ElevatorState.WAIT; //移動完了したらステートをWAITに変更
                            SceneChange(); //一度だけシーン移行処理を呼び出す
                        })
                    .SetEase(EaseType);
                }
            });
    }

    /// <summary>
    /// Elevator起動時に分裂している子供スライムを全員集合
    /// </summary>
    public void ElevatorStart()
    {
        GameObject[] SlimeObjects = GameObject.FindGameObjectsWithTag("Slime"); //シーンに存在している分裂している子供スライムを全て取得
        foreach (GameObject Obj in SlimeObjects) //シーン上に存在しているスライムオブジェクトを取得
        {
            Obj.GetComponent<SlimeController>().liveTime = 20; //子供スライムが親スライムに戻る時間を指定する事で集合させる
        }
        CameraActive.SetActive(false); //カメラの追跡を停止させる
        ElevatorAnimator.SetTrigger("Close"); //エレベーターのCloseアニメーション再生
    }

    /// <summary>
    /// 現在のsceneを取得してシーン移行
    /// </summary>
    public void SceneChange()
    {
        string SceneName = SceneManager.GetActiveScene().name; //現在のシーンの名前を取得
        if (SceneName == "Title")
        {
            FadeManager.Instance.LoadScene("S0-1", FadeTime);
        }
        if (SceneName == "S0-1")
        {
            FadeManager.Instance.LoadScene("S0-2", FadeTime);
        }
        else if (SceneName == "S0-2")
        {
            FadeManager.Instance.LoadScene("S0-3", FadeTime);
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
        else if (SceneName == "TGS-1")
        {
            FadeManager.Instance.LoadScene("TGS-2", FadeTime);
        }
    }
}