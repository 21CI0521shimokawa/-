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
    private float StartPosition;
    public bool _IsFloor;//����g������

    [SerializeField, Tooltip("�|�X�g�G�t�F�N�g�̎擾")]
    private PostProcessVolume Volume;
    // [SerializeField, Tooltip("�|�X�g�G�t�F�N�gVignette�̎擾")] //�G���x�[�^�[��������ɈÂ��Ȃ�
    // private Vignette Vignette;
    [SerializeField, Tooltip("scene�̖��O")]
    Scene SceneName;
    [SerializeField, Tooltip("SE")]
    AudioClip SE;
    [SerializeField, Tooltip("�I�[�f�B�Isource")]
    AudioSource AudioSource;
    [SerializeField]
    Animator AutoDoorAnimator;
    [SerializeField]
    GameObject CameraActive;
    //�����蔻��̂��蔲���h�~
    public Rigidbody2D ElevatorRigidbody;
    void Start()
    {
        CameraActive.SetActive(true);
        StartPosition = this.transform.position.y;
        ElevatorRigidbody = GetComponent<Rigidbody2D>();
        _IsFloor = false;
        DestinationNum = Mathf.Abs(Destination.position.y);
        //�|�X�g�G�t�F�N�g
        //  Volume.profile.TryGetSettings<Vignette>(out Vignette);
    }
    public void ElevatorStart()
    {
        AutoDoorAnimator.SetTrigger("Close");
        CameraActive.SetActive(false);
        //  AutoDoorControll.PlaySE(SE);
        // Vignette.enabled.Override(true);
        StartCoroutine("ElevatorUp");
    }
    public void SceneChange()  //�������@�ύX�\��
    {
        //���݂�scene���擾
        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "S0-1")
        {
            FadeManager.Instance.LoadScene("S0-2", FadeTime);
            return;
        }
        else if (SceneName == "S0-2")
        {
            FadeManager.Instance.LoadScene("S0-3", FadeTime);
            return;
        }
        else if (SceneName == "S0-3")
        {
            FadeManager.Instance.LoadScene("S1-1", FadeTime);
        }
        else if (SceneName == "S1-1")
        {
            FadeManager.Instance.LoadScene("S1-2", FadeTime);
        }
        else if (SceneName == "S1-2")
        {
            FadeManager.Instance.LoadScene("S1-3", FadeTime);
        }
        else if (SceneName == "S1-3")
        {
            FadeManager.Instance.LoadScene("S2-1", FadeTime);
        }
        else if (SceneName == "S2-1")
        {
            FadeManager.Instance.LoadScene("S2-2", FadeTime);
        }
        else if (SceneName == "S2-2")
        {
            FadeManager.Instance.LoadScene("S2-3", FadeTime);
        }
        else if (SceneName == "S2-3")
        {
            FadeManager.Instance.LoadScene("S2-4", FadeTime);
        }
        else if (SceneName == "S2-4")
        {
            FadeManager.Instance.LoadScene("S3-1", FadeTime);
        }
        else if (SceneName == "S3-1")
        {
            FadeManager.Instance.LoadScene("S3-2", FadeTime);
        }
        else if (SceneName == "S3-2")
        {
            FadeManager.Instance.LoadScene("S3-3", FadeTime);
        }
        else if (SceneName == "S3-3")
        {
            FadeManager.Instance.LoadScene("4-1", FadeTime);
        }
    }
    public void PlaySE(AudioClip audio)
    {
        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("�I�[�f�B�I�\�[�X���ݒ肳��ĂȂ�");
        }
    }
    #region �R���[�`��
    public IEnumerator ElevatorUp()
    {
        if (this.AudioSource.isPlaying == false)
        {
            PlaySE(SE);
        }
        while (this.AudioSource.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }
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
        yield return new WaitForSeconds(1.5f);//�ꎞ��~���Ă���~���J�n
        yield break;
    }
    #endregion
}
