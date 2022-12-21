using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System.Linq;
using Cysharp.Threading.Tasks;
using System;

public class AutoDoorController : ClimbingMachine
{
    [SerializeField, Tooltip("オートドアの開閉SE")] AudioClip autoDoorSE;

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        Moving();
    }

    /// <summary>
    /// オートドアの動きの心臓部
    /// </summary>
    protected override void Moving()
    {
        TriggerEnterDirector
                       //衝突したオブジェクトのタグが
                       .Select(collison => collison.tag)
                       //"Slime"だったら
                       .Where(tag => tag == SubjectObjectTag)
                       //現在のステートがMOVEだったら実行
                       .Where(state => NowState == ClimbingState.MOVE)
                       //一度だけ実行
                       .Take(DoTime)
                       .Subscribe(Do =>
                       {
                           //UpMoveValue.yの位置までMovingTimeの速さで移動
                           ClimbingTween = transform.DOMoveY(UpMoveValue.y, MovingTime).SetEase(EaseType);

                           ClimbingTween
                           .OnStart(() =>
                           {
                               //移動開始するときにAutoDoorSEを再生
                               PlayAudio.PlaySE(autoDoorSE);
                               //移動開始するときにStartアニメーションを再生
                               ClimbingMachineAnimator.SetBool("Start", true);
                           })
                           .OnComplete(() =>
                           {
                               //移動が完了したらStartアニメーションを止める
                               ClimbingMachineAnimator.SetBool("Start", false);
                               //移動が完了したらステートをWAITに変更
                               NowState = ClimbingState.WAIT;
                           });
                       });
    }
}