using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrigger : MonoBehaviour
{
    List<GameObject> onObjects = new List<GameObject>();    //�G��Ă���I�u�W�F�N�g�ꗗ
    [SerializeField] List<GameObject> exclusionObjects = new List<GameObject>(); //���O����I�u�W�F�N�g�ꗗ

    public bool _onTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _onTrigger = false;
        onObjects.Clear();

        //���̃X���C�����g�����O
        exclusionObjects.Add(transform.root.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        _onTrigger = false;

        //�I�u�W�F�N�g���͈͂ɓ����Ă��邩���m
        foreach (GameObject i in onObjects)
        {
            //�ꕔ�̃I�u�W�F�N�g�͏��O
            if (exclusionObjects.Contains(i))
            {
                continue;
            }

            //�ꕔ�̃^�O�̂����I�u�W�F�N�g�͏��O
            switch (i.tag)
            {
                case "SlimeTrigger":            break;  //�X���C���g���K�[
                case "Tracking":                break;  //�J�����g���b�L���O
                case "AutomaticDoor":           break;  //�����h�A
                    //�����ɒǉ�

                default:
                    _onTrigger = true;
                    Debug.Log(i.gameObject.name);
                    break;
            }

            //�g���K�[��On�ɂȂ����珈���I��
            if (_onTrigger)
            {
                break;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�g���K�[�ł͂Ȃ�������
        if (!collision.isTrigger)
        {
            //���X�g�ɖ���������ǉ�
            if (!onObjects.Contains(collision.gameObject))
            {
                onObjects.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //���X�g�ɂ����������
        if (onObjects.Contains(collision.gameObject))
        {
            onObjects.Remove(collision.gameObject);
        }
    }
}
