using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class ElevatoControll : MonoBehaviour
{
    [SerializeField, Tooltip("現在の位置")]
    private Vector3 NowPosition;

    [SerializeField, Tooltip("目標位置")]
    private Transform Destination;

    [SerializeField, Tooltip("Elevatorのスピード")]
    private float Speed;
    [SerializeField, Tooltip("フェードの時間")]
    private float FadeTime;
    private float DestinationNum;
    public bool _IsFloor;//今後使うかも

    [SerializeField, Tooltip("ポストエフェクトの取得")]
    private PostProcessVolume Volume;
    [SerializeField, Tooltip("ポストエフェクトVignetteの取得")] //エレベーター乗った時に暗くなる
    private Vignette Vignette;
    [SerializeField, Tooltip("ポストエフェクトVinetteのアクティブ真偽判定")]
    private bool IsUseVInette;

    void Start()
    {
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
        //ポストエフェクト
        Volume.profile.TryGetSettings<Vignette>(out Vignette);
        IsUseVInette = false;
    }

     public void ElevatorStart()
    {
        IsUseVInette = true;
        Vignette.enabled.Override(true);
        StartCoroutine("ElevatorUp");
    }
    #region コルーチン
    public IEnumerator ElevatorUp()
    {
        while(NowPosition.y<DestinationNum)
        {
            NowPosition = transform.position;
            transform.Translate(0, Speed, 0);
            yield return new WaitForSeconds(0.01f);
        }
        FadeManager.Instance.LoadScene("SlimeTest",FadeTime);
    }
    #endregion
}
