using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class ElevatorControll : MonoBehaviour
{
    #region SerializeField
    [SerializeField, Tooltip("�h�A�̊J�X�s�[�h")] float Speed;
    [SerializeField, Tooltip("�ړ���"),] Vector3 MoveTo = new Vector3(0, 4);
    [SerializeField, Tooltip("�h�A�̈ړ�����")] float ElevatorMoveTime;
    [SerializeField, Tooltip("�I�[�f�B�I�\�[�X")] AudioSource AudioSource = null;
    [SerializeField, Tooltip("�h�A�J��SE")] AudioClip DoorSE;
    [SerializeField, Tooltip("�h�A�̊J�A�j���[�V����")] Animator ElevatorAnimator;
    [SerializeField, Tooltip("�ړ��C�[�W���O�w��")] Ease EaseType;
    [SerializeField, Tooltip("�t�F�[�h�̎���")] float FadeTime;
    [SerializeField, Tooltip("���݂̃V�[���̖��O")] Scene SceneName;
    [SerializeField, Tooltip("�X���C���I�u�W�F�N�g�̎擾")] SlimeController ClonSlime;
    [SerializeField, Tooltip("���̎擾")] Rigidbody2D ElevatorRigidbody;
    [SerializeField,Tooltip("�J�����̒ǐՊǗ�")] GameObject CameraActive;
    #endregion

    #region �X�e�[�g�Ǘ�
    private enum ElevatorState { MOVE, WAIT };
    ElevatorState NowState;
    #endregion

    void Awake()
    {
        CameraActive.SetActive(true);
    }
    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Select(collison => collison.tag)
            .Where(tag => tag == "Slime")
            .Take(1)
            .Subscribe(collision =>
            {
                if (NowState == ElevatorState.MOVE)
                {
                    PlaySE(DoorSE);
                    this.transform.DOMoveY(MoveTo.y, ElevatorMoveTime).SetDelay(1f)
                        .OnStart(() =>
                        {//���s�J�n���̃R�[���o�b�N
                            ElevatorAnimator.SetBool("Start", true);
                        })
                        .OnComplete(() =>
                        {//���s�������̃R�[���o�b�N
                            ElevatorAnimator.SetBool("Start", false);
                            ElevatorRigidbody.constraints = RigidbodyConstraints2D.FreezePosition;
                            NowState = ElevatorState.WAIT;
                            SceneChange();
                            return;
                        })
                    .SetEase(EaseType);
                }
            });
    }

    #region public function
    public void ElevatorStart() //Elevator�N�����ɃQ�[���p�b�h�̓��͂������󂯂Ȃ�
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Slime");
        foreach (GameObject obj in objects)
        {
            obj.GetComponent<SlimeController>().liveTime = 20;
        }
        CameraActive.SetActive(false);
        ElevatorAnimator.SetTrigger("Close");
    }
    public void SceneChange()  //scene�ڍs
    {
        //���݂�scene���擾
        string SceneName = SceneManager.GetActiveScene().name;
        if (SceneName == "Title")
        {
            FadeManager.Instance.LoadScene("S0-1", FadeTime);
            return;
        }
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
            FadeManager.Instance.LoadScene("S3-4", FadeTime);
        }
        else if (SceneName == "S3-4")
        {
            FadeManager.Instance.LoadScene("S4-1", FadeTime);
        }
        else if(SceneName== "TGS-1")
        {
            FadeManager.Instance.LoadScene("TGS-2", FadeTime);
        }
    }
    public void PlaySE(AudioClip audio) //SE�����̂ݍĐ�
    {
        if (AudioSource != null)
        {
            AudioSource.PlayOneShot(audio);
        }
    }
    #endregion
}