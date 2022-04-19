using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatoControll : MonoBehaviour
{
    [SerializeField, Tooltip("���݂̈ʒu")]
    private Vector3 NowPosition;

    [SerializeField, Tooltip("�ڕW�ʒu")]
    private Transform Destination;

    [SerializeField, Tooltip("Elevator�̃X�s�[�h")]
    private float Speed;
    [SerializeField, Tooltip("�t�F�[�h�̎���")]
    private float FadeTime;
    private float DestinationNum;
    public bool _IsFloor;//����g������
    void Start()
    {
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
    }

     public void ElevatorStart()
    {
        StartCoroutine("ElevatorUp");
    }
    #region �R���[�`��
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
