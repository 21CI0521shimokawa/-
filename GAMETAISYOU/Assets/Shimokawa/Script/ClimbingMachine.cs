using System;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// 上昇系オブジェクトの基底クラス
/// </summary>
public abstract class ClimbingMachine : MonoBehaviour
{
    [SerializeField, Tooltip("目的地までのスピード")] protected float MovingTime;
    [SerializeField, Tooltip("ドアの上昇移動量")] protected Vector3 UpMoveValue;
    [SerializeField, Tooltip("移動イージング指定")] protected Ease EaseType;
    [SerializeField, Tooltip("対象のゲームオブジェクトタグ")] protected string SubjectObjectTag;
    [SerializeField, Tooltip("アニメ―ション")] protected Animator ClimbingMachineAnimator;
    protected const int DoTime = 1; //衝突した後の処理を一度だけ実行するための変数
    protected ClimbingState NowState = ClimbingState.MOVE; //継承先で使用する読み込み専用のenum
    protected IObservable<Collider2D> TriggerEnterDirector {  get; set; } //当たり判定をIObservableに変換
    protected Tween ClimbingTween; //ラムダ式で書く際に処理が長くなってしまい可読性が低くなるので処理を区切るための変数

    /// <summary>
    /// ClimbingMachineのステート
    /// </summary>
    protected enum ClimbingState
    {
        WAIT,
        MOVE
    }

    protected abstract void Moving(); //継承先で必ず実装する

    /// <summary>
    /// 現在のアニメーションを取得し終わりをコールする関数
    /// </summary>
    /// <returns></returns>
    protected virtual async UniTask<bool> IsEndNowAnimation()
    {
        //現在再生しているアニメーションを取得
        var NowAnimatorState = ClimbingMachineAnimator.GetCurrentAnimatorStateInfo(0);
        //取得したアニメーションの長さ分待機
        await UniTask.Delay(TimeSpan.FromSeconds(NowAnimatorState.length));
        return true;
    }
}
