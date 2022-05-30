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
        //現在のsceneを取得
        oneFlamebeforeSceneName = sceneName;
        sceneName = SceneManager.GetActiveScene().name;

        SlimeTimerControll();

        //タイムカウント
        if (isTimeCount)
        {
            timeCount += Time.deltaTime;
        }

        //リトライ
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TimeReset();

            FadeManager.Instance.LoadScene("Title", 1);
            return;
        }

        if(oneFlamebeforeSceneName != sceneName)
        {
            //スタート
            if (sceneName == "S0-1" && GetIsTimeReset())
            {
                TimeStart();
            }

            //ゴール
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
        //存在確認
        if (!slimeController)
        {
            //スライム取得
            GameObject[] slimes = GameObject.FindGameObjectsWithTag("Slime");

            //スライムが不在
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

            //コアを探す
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

            //みつからない
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

        //分
        while (second >= 60.0)
        {
            ++minute;
            second -= 60.0;
        }

        // 先に整数の部分を先に書式指定する
        string secondText = ((int)second).ToString("00"/*2桁ゼロ埋め*/);
        // 次に小数部分のみを計算して書式指定を行う
        secondText += (second - ((int)second)).ToString("F3"/*小数点以下2桁*/).TrimStart('0'/*先頭のゼロ削除*/);

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
