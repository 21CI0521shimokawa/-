using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;
using System.Linq;

public class AutoDoorControll : MonoBehaviour
{
    [SerializeField, Tooltip("ドアの開閉スピード")] float Speed;
    [SerializeField, Tooltip("移動量"),] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("ドアの移動時間")] float AutoDoorMoveTime;
    [SerializeField, Tooltip("オーディオソース")] AudioSource AudioSource = null;
    [SerializeField, Tooltip("ドア開閉SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("ドアの開閉アニメーション")] Animator AutoDoorAnimator;
    [SerializeField, Tooltip("移動イージング指定")] Ease EaseType;

    #region ステート管理
    private enum AutoDoorState { MOVE, WAIT };
    AutoDoorState NowState;
    #endregion

    private void Awake()
    {
        NowState = AutoDoorState.MOVE;
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag)
            .Where(tag => tag == "Slime")
            .Take(1)
            .Subscribe(collision =>
            {
                if (NowState == AutoDoorState.MOVE)
                {
                    this.transform.DOMoveY(MoveTo.y, AutoDoorMoveTime)
                        .OnStart(() =>
                        {//実行開始時のコールバック
                            PlaySE(DoorSE);
                            AutoDoorAnimator.SetBool("Start", true);
                        })
                        .OnComplete(() =>
                        {//実行完了時のコールバック
                            AutoDoorAnimator.SetBool("Start", false);
                            NowState = AutoDoorState.WAIT;
                        })
                    .SetEase(EaseType);
                }
            });
    }
    public void PlaySE(AudioClip audio) //SEを一回再生
    {
        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(audio);
        }
    }

}