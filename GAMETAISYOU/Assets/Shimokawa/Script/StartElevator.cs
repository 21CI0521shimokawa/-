using UnityEngine;
using DG.Tweening;

public class StartElevator : ClimbingMachine
{
    [SerializeField, Tooltip("スライムの操作可否取得")] SlimeController slimeController;
    [SerializeField, Tooltip("エレベーターの開くSE取得")] AudioClip openSE;

    /// <summary>
    /// ゲームが始まる時に一度だけ呼ばれる関数
    /// </summary>
    void Start()
    {
        Moving();
    }
    protected override void Moving()
    {
        //エレベーターが開くまでSlimeの移動を停止
        slimeController._ifOperation = false;
        transform.DOMoveY(UpMoveValue.y, MovingTime)
            　　 .OnComplete(() =>
               {
                   // 移動完了したらSlimeの移動を再開
                   slimeController._ifOperation = true;
                   //移動完了したらOpenSEを再生
                   PlayAudio.PlaySE(openSE);
                   //移動完了したらOpenアニメーション再生
                   ClimbingMachineAnimator.SetTrigger("Open"); 
               });
    }
}