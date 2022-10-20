using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using DG.Tweening;
using UniRx.Triggers;

public class StartElevator : MonoBehaviour
{
    [SerializeField, Tooltip("目標地点の設定")] Vector3 DestinationPositon;
    [SerializeField, Tooltip("エレベーターのスピード")] float StartElevatorSpeed;
    [SerializeField, Tooltip("開閉アニメーション")] Animator StartElevatorAnimator;
    [SerializeField, Tooltip("スライムの操作可否取得")] SlimeController SlimeController;
    [SerializeField, Tooltip("エレベーターの開くSE取得")] AudioClip OpenSE;

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        SlimeController._ifOperation = false; //エレベーターが開くまでSlimeの移動を停止
        this.transform.DOMoveY(DestinationPositon.y, StartElevatorSpeed)
        .OnComplete(() =>
        {
            SlimeController._ifOperation = true; //移動完了したらSlimeの移動を再開
            PlayAudio.PlaySE(OpenSE); //移動完了したらOpenSEを再生
            StartElevatorAnimator.SetTrigger("Open"); //移動完了したらOpenアニメーション再生
        });
    }
}