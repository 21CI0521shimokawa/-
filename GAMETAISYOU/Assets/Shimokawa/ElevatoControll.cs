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
    [SerializeField, Tooltip("sceneの名前")]
    Scene SceneName;
    [SerializeField, Tooltip("SE関数取得")]
    AutoDoorControll AutoDoorControll;
    [SerializeField, Tooltip("SE")]
    AudioClip SE;
    [SerializeField, Tooltip("オーディオsource")]
    AudioSource AudioSource;
    //当たり判定のすり抜け防止
    public Rigidbody2D ElevatorRigidbody;
    void Start()
    {
        ElevatorRigidbody = GetComponent<Rigidbody2D>();
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
        //ポストエフェクト
        Volume.profile.TryGetSettings<Vignette>(out Vignette);
    }

    public void ElevatorStart()
    {
      //  AutoDoorControll.PlaySE(SE);
        Vignette.enabled.Override(true);
        StartCoroutine("ElevatorUp");
    }
    #region コルーチン
    public IEnumerator ElevatorUp()
    {
        while (NowPosition.y < DestinationNum)
        {
            NowPosition = transform.position;
            ElevatorRigidbody.velocity = new Vector2(0, Speed);
            yield return new WaitForSeconds(0.01f);
        }
        ElevatorRigidbody.bodyType = RigidbodyType2D.Static;
        SceneChange();
        yield return new WaitForSeconds(0.01f);
        yield break;
    }
    #endregion
    public void SceneChange()  //実装方法変更予定
    {
        //現在のsceneを取得
        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Stage0")
        {
            FadeManager.Instance.LoadScene("Stage1", FadeTime);
            return;
        }
        else if (SceneName == "Stage1")
        {
            FadeManager.Instance.LoadScene("Stage2", FadeTime);
            return;
        }
    }
}
