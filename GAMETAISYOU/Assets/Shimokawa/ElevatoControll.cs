using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    void Start()
    {
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
    }

     public void ElevatorStart()
    {
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
