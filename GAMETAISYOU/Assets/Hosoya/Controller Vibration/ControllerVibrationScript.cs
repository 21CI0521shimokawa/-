using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibrationScript : MonoBehaviour
{
    private struct ControllerVibration
    {
        public string name;        //���O
        public float leftPower;    //��
        public float rightPower;   //�E

        public float priority;     //�D��x
    }

    List<ControllerVibration> vibrationList = new List<ControllerVibration>();    //���X�g

    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        GetGamepad();
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���p�b�h���ڑ�����Ă��Ȃ��Ȃ珈�����Ȃ�
        if (gamepad == null)
        {
            if(!GetGamepad())
            {
                Debug.Log("�R���g���[���[���ڑ�����Ă��܂���");
                return;
            }
        }

        VibrationUpdate(gamepad);
    }

    //���̃I�u�W�F�N�g��������Ƃ�
    void OnDestroy()
    {
        //�Q�[���p�b�h���ڑ�����Ă��Ȃ��Ȃ珈�����Ȃ�
        if (gamepad == null)
        {
            return;
        }

        End(gamepad);
    }

    //�A�v���P�[�V�������I������Ƃ�
    void OnApplicationQuit()
    {
        //�Q�[���p�b�h���ڑ�����Ă��Ȃ��Ȃ珈�����Ȃ�
        if (gamepad == null)
        {
            return;
        }

        End(gamepad);
    }

    //�Q�[���p�b�h�擾
    bool GetGamepad()
    {
        gamepad = Gamepad.current;
        return !(gamepad == null);
    }

    //�U��
    void VibrationUpdate(Gamepad gamepad_)
    {
        float maxPowerLeft = 0;
        float maxPowerRight = 0;

        float maxpriority = 0;


        //��ԍ����D��x�𒲂ׂ�
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            maxpriority = Mathf.Max(maxpriority, vibrationList[i].priority);
        }

        List<ControllerVibration> processList = new List<ControllerVibration>();

        //�D��x����ԍ������̂𒊏o
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if(vibrationList[i].priority == maxpriority)
            {
                processList.Add(vibrationList[i]);
            }
        }

        //�D��x���������̂̒��ŐU������ԋ������̂��g��
        for (int i = 0; i < processList.Count; ++i)
        {
            if (maxPowerLeft < vibrationList[0].leftPower)
            {
                maxPowerLeft = vibrationList[0].leftPower;
            }

            if (maxPowerRight < vibrationList[0].rightPower)
            {
                maxPowerRight = vibrationList[0].rightPower;
            }
        }

        //�U��
        gamepad_.SetMotorSpeeds(maxPowerLeft, maxPowerRight);
    }

    //���X�g�ǉ��E�ύX
    public void Vibration(string name_, float leftPower_, float rightPower_, float priority_)
    {
        ControllerVibration buf = new ControllerVibration();

        buf.name = name_;
        buf.leftPower = leftPower_;
        buf.rightPower = rightPower_;
        buf.priority = priority_;

        //�������O�̗v�f����������ύX
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if(buf.name == vibrationList[i].name)
            {
                vibrationList[i] = buf;
                return;
            }
        }

        //�Ȃ�������ǉ�
        vibrationList.Add(buf); //���X�g�ǉ�
    }

    //���X�g�ǉ��E�ύX �D��x�ȗ�
    public void Vibration(string name_, float leftPower_, float rightPower_)
    {
        ControllerVibration buf = new ControllerVibration();

        buf.name = name_;
        buf.leftPower = leftPower_;
        buf.rightPower = rightPower_;
        buf.priority = 0;

        //�������O�̗v�f����������ύX
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if (buf.name == vibrationList[i].name)
            {
                vibrationList[i] = buf;
                return;
            }
        }

        //�Ȃ�������ǉ�
        vibrationList.Add(buf); //���X�g�ǉ�
    }

    //���X�g���� ������true ���s��false
    public bool Destroy(string name_)
    {
        //�������O�̗v�f�������������
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if (name_ == vibrationList[i].name)
            {
                vibrationList.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    //�I������
    void End(Gamepad gamepad_)
    {
        gamepad_.SetMotorSpeeds(0, 0);
    }
}
