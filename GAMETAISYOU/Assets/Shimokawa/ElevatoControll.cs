using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

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

    [SerializeField, Tooltip("�|�X�g�G�t�F�N�g�̎擾")]
    private PostProcessVolume Volume;
    [SerializeField, Tooltip("�|�X�g�G�t�F�N�gVignette�̎擾")] //�G���x�[�^�[��������ɈÂ��Ȃ�
    private Vignette Vignette;
    [SerializeField, Tooltip("�|�X�g�G�t�F�N�gVinette�̃A�N�e�B�u�^�U����")]
    private bool IsUseVInette;

    void Start()
    {
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
        //�|�X�g�G�t�F�N�g
        Volume.profile.TryGetSettings<Vignette>(out Vignette);
        IsUseVInette = false;
    }

     public void ElevatorStart()
    {
        IsUseVInette = true;
        Vignette.enabled.Override(true);
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
