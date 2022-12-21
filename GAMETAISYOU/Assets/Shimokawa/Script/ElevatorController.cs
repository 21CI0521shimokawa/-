using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using SceneDefine;

public class ElevatorController : ClimbingMachine
{
    [SerializeField, Tooltip("カメラの追跡管理")] GameObject cameraActive;
    [SerializeField, Tooltip("シーン移行時の時間")] float fadeTime;
    [SerializeField, Tooltip("エレベーターの開閉SE")] AudioClip openSE;
    private const int cloneSlimeDieTime = 20; //分裂されたスライムが親スライムに戻る時間を指定
    private const float delayTime = 1f; //アニメーションを再生してから動きだすためのDelayTimeを指定

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        Moving();
    }

    /// <summary>
    /// エレベーターの動きの心臓部
    /// </summary>
    protected override void Moving()
    {
        TriggerEnterDirector
                         //衝突したオブジェクトのタグが
                         .Select(collison => collison.tag)
                         //"Slime"だったら
                         .Where(tag => tag == SubjectObjectTag)
                         //現在のステートがMOVEだったら実行
                         .Where(_State => NowState == ClimbingState.MOVE)
                         //一度だけ実行
                         .Take(DoTime)
                         .Subscribe(async collision =>
                         {
                             //DoorSEを再生
                             PlayAudio.PlaySE(openSE);
                             //エレベーターが閉じるアニメーションが終わるまで待機
                             var WaitAnimEnd = await IsEndNowAnimation();
                             //アニメーションが終了してからUpMoveValue.yの地点までdelayTimeの速さで移動しイージングタイプを指定
                             ClimbingTween = transform.DOMoveY(UpMoveValue.y, MovingTime).SetDelay(delayTime).SetEase(EaseType);

                             ClimbingTween
                                  .OnStart(() =>
                                  {
                                      // Elevator起動時に分裂している子供スライムを全員集合
                                      ElevatorStart();
                                      //実行する時にアニメーション再生
                                      ClimbingMachineAnimator.SetBool("Start", true);
                                  })
                                  .OnComplete(() =>
                                  {
                                      //移動完了したらステートをWAITに変更
                                      NowState = ClimbingState.WAIT;
                                      //シーン移行
                                      SceneManagement.LoadNextScene(fadeTime);
                                  });
                         });
    }

    /// <summary>
    /// Elevator起動時に分裂している子供スライムを全員集合
    /// </summary>
    public void ElevatorStart()
    {
        //シーンに存在している分裂している子供スライムを全て取得
        GameObject[] slimeObjects = GameObject.FindGameObjectsWithTag(SubjectObjectTag);
        //シーン上に存在しているスライムオブジェクトを取得
        foreach (GameObject objcet in slimeObjects)
        {
            //子供スライムが親スライムに戻る時間を指定する事で集合させる
            objcet.GetComponent<SlimeController>().liveTime = cloneSlimeDieTime;
        }
        //カメラの追跡を停止させる
        cameraActive.SetActive(false);
        //エレベーターのCloseアニメーション再生
        ClimbingMachineAnimator.SetTrigger("Close");
    }

}