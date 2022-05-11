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
    private float StartPosition;
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
    [SerializeField]
    Animator AutoDoorAnimator;
    [SerializeField]
    GameObject CameraActive;
    //当たり判定のすり抜け防止
    public Rigidbody2D ElevatorRigidbody;
    void Start()
    {
        CameraActive.SetActive(true);
        StartPosition = this.transform.position.y;
        ElevatorRigidbody = GetComponent<Rigidbody2D>();
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
        //ポストエフェクト
        Volume.profile.TryGetSettings<Vignette>(out Vignette);
    }
    public void ElevatorStart()
    {
        AutoDoorAnimator.SetTrigger("Close");
        CameraActive.SetActive(false);
        //  AutoDoorControll.PlaySE(SE);
       // Vignette.enabled.Override(true);
        StartCoroutine("ElevatorUp");
    }
    public void SceneChange()  //実装方法変更予定
    {
        //現在のsceneを取得
        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "0-1")
        {
            FadeManager.Instance.LoadScene("0-2", FadeTime);
            return;
        }
        else if (SceneName == "0-2")
        {
            FadeManager.Instance.LoadScene("0-3", FadeTime);
            return;
        }
        else if(SceneName== "0-3")
        {
            FadeManager.Instance.LoadScene("1-1", FadeTime);
        }
        else if(SceneName== "1-1")
        {
            FadeManager.Instance.LoadScene("1-2", FadeTime);
        }
        else if (SceneName == "1-2")
        {
            FadeManager.Instance.LoadScene("2-1", FadeTime);
        }
        else if (SceneName == "2-1")
        {
            FadeManager.Instance.LoadScene("2-2", FadeTime);
        }
        else if (SceneName == "2-2")
        {
            FadeManager.Instance.LoadScene("3-1", FadeTime);
        }
        else if (SceneName == "3-1")
        {
            FadeManager.Instance.LoadScene("3-2", FadeTime);
        }
        else if (SceneName == "3-2")
        {
            FadeManager.Instance.LoadScene("4-1", FadeTime);
        }
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
        SceneChange();
        yield return new WaitForSeconds(0.01f);
        yield break;
    }
    public IEnumerator ElevatorStop()
    {
        StopCoroutine("ElevatorUp");
        ElevatorRigidbody.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(1.5f);//一時停止してから降下開始
        yield break;
    }
    #endregion
}
