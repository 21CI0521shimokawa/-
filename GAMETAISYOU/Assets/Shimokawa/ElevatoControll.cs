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
    [SerializeField, Tooltip("scene�̖��O")]
    Scene SceneName;
    [SerializeField, Tooltip("SE�֐��擾")]
    AutoDoorControll AutoDoorControll;
    [SerializeField, Tooltip("SE")]
    AudioClip SE;
    [SerializeField, Tooltip("�I�[�f�B�Isource")]
    AudioSource AudioSource;
    //�����蔻��̂��蔲���h�~
    public Rigidbody2D ElevatorRigidbody;
    void Start()
    {
        ElevatorRigidbody = GetComponent<Rigidbody2D>();
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
        //�|�X�g�G�t�F�N�g
        Volume.profile.TryGetSettings<Vignette>(out Vignette);
    }

    public void ElevatorStart()
    {
      //  AutoDoorControll.PlaySE(SE);
        Vignette.enabled.Override(true);
        StartCoroutine("ElevatorUp");
    }
    #region �R���[�`��
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
    public void SceneChange()  //�������@�ύX�\��
    {
        //���݂�scene���擾
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
