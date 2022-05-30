using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeAttack : MonoBehaviour
{
    bool isTimeCount;
    bool isTimeReset;
    double timeCount;

    string sceneName;
    string oneFlamebeforeSceneName;

    SlimeController slimeController;

    [SerializeField] Text text;

    // Start is called before the first frame update
    void Start()
    {
         TimeReset();
    }

    // Update is called once per frame
    void Update()
    {
        //���݂�scene���擾
        oneFlamebeforeSceneName = sceneName;
        sceneName = SceneManager.GetActiveScene().name;

        SlimeTimerControll();

        //�^�C���J�E���g
        if (isTimeCount)
        {
            timeCount += Time.deltaTime;
        }

        //���g���C
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TimeReset();

            FadeManager.Instance.LoadScene("Title", 1);
            return;
        }

        if(oneFlamebeforeSceneName != sceneName)
        {
            //�X�^�[�g
            if (sceneName == "S0-1" && GetIsTimeReset())
            {
                TimeStart();
            }

            //�S�[��
            if (sceneName == "GameClear" && GetIsTimeCount())
            {
                TimeStop();
            }
        }

        TextUpdate();
    }

    bool GetIsTimeCount()
    {
        return isTimeCount;
    }

    bool GetIsTimeReset()
    {
        return isTimeReset;
    }

    void SlimeTimerControll()
    {
        //���݊m�F
        if (!slimeController)
        {
            //�X���C���擾
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");

            //�X���C�����s��
            bool isExist = false;
            foreach (GameObject it in slimes)
            {
                if (it)
                {
                    isExist = true;
                    break;
                }
            }
            if(!isExist)
            {
                return;
            }

            //�R�A��T��
            foreach (GameObject it in slimes)
            {
                SlimeController buf = it.GetComponent<SlimeController>();
                if(buf)
                {
                    if (buf.core)
                    {
                        slimeController = buf;
                    }
                }
            }

            //�݂���Ȃ�
            if (!slimeController)
            {
                return;
            }
        }
        
        if(slimeController._ifOperation && !GetIsTimeCount())
        {
            TimeRestart();
        }
        else if(!slimeController._ifOperation && GetIsTimeCount())
        {
            TimeStop();
        }
    }

    void TextUpdate()
    {
        int minute = 0;
        double second = timeCount;

        //��
        while (second >= 60.0)
        {
            ++minute;
            second -= 60.0;
        }

        // ��ɐ����̕������ɏ����w�肷��
        string secondText = ((int)second).ToString("00"/*2���[������*/);
        // ���ɏ��������݂̂��v�Z���ď����w����s��
        secondText += (second - ((int)second)).ToString("F3"/*�����_�ȉ�2��*/).TrimStart('0'/*�擪�̃[���폜*/);

        text.text = minute.ToString("00") + ":" + secondText;
    }

    void TimeStart()
    {
        TimeReset();
        isTimeCount = true;
        isTimeReset = false;
    }

    void TimeRestart()
    {
        if (!GetIsTimeReset())
        {
            isTimeCount = true;
            isTimeReset = false;
        }
    }

    void TimeStop()
    {
        isTimeCount = false;
    }

    void TimeReset()
    {
        TimeStop();
        timeCount = 0.0;
        isTimeReset = true;
    }
}
