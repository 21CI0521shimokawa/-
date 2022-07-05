using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class StartElevator : MonoBehaviour
{
    #region SerializeField
    [SerializeField, Tooltip("目標地点の設定")] Vector3 DestinationPos;
    [SerializeField,Tooltip("現在地")] Vector3 NowPositon;
    [SerializeField, Tooltip("エレベーターのスピード")] float StartElevatorSpeed;
    [SerializeField, Tooltip("開閉アニメーション")] Animator StartElevatorAnimator;
    [SerializeField, Tooltip("スライムの操作可否取得")] SlimeController SlimeController;
    [SerializeField, Tooltip("オーディオソース取得")] AudioSource StartElevatorAudioSource;
    [SerializeField, Tooltip("エレベーターの開くSE取得")] AudioClip OpenSE;
    #endregion

    void Start()
    {
        SlimeController._ifOperation = false;
        NowPositon = this.transform.position;
        this.UpdateAsObservable()
            .TakeWhile(_ => NowPositon.y < DestinationPos.y)
            .Subscribe(_ =>
            {
                NowPositon = transform.position;
                transform.Translate(0, StartElevatorSpeed * Time.deltaTime, 0);
            },
            () =>
            {
                SlimeController._ifOperation = true;
                PlaySE(OpenSE);
                StartElevatorAnimator.SetTrigger("Open");
            });

    }
    #region public function
    public void PlaySE(AudioClip audio) //SEを一回再生する
    {
        if (StartElevatorAudioSource != null)
        {
            StartElevatorAudioSource.PlayOneShot(audio);
        }
    }
    #endregion
}