using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVibrationScript : MonoBehaviour
{
    private struct ControllerVibration
    {
        public string name;        //名前
        public float leftPower;    //左
        public float rightPower;   //右

        public float priority;     //優先度
    }

    List<ControllerVibration> vibrationList = new List<ControllerVibration>();    //リスト

    Gamepad gamepad;

    // Start is called before the first frame update
    void Start()
    {
        GetGamepad();
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームパッドが接続されていないなら処理しない
        if (gamepad == null)
        {
            if(!GetGamepad())
            {
                Debug.Log("コントローラーが接続されていません");
                return;
            }
        }

        VibrationUpdate(gamepad);
    }

    //このオブジェクトが消えるとき
    void OnDestroy()
    {
        //ゲームパッドが接続されていないなら処理しない
        if (gamepad == null)
        {
            return;
        }

        End(gamepad);
    }

    //アプリケーションが終了するとき
    void OnApplicationQuit()
    {
        //ゲームパッドが接続されていないなら処理しない
        if (gamepad == null)
        {
            return;
        }

        End(gamepad);
    }

    //ゲームパッド取得
    bool GetGamepad()
    {
        gamepad = Gamepad.current;
        return !(gamepad == null);
    }

    //振動
    void VibrationUpdate(Gamepad gamepad_)
    {
        float maxPowerLeft = 0;
        float maxPowerRight = 0;

        float maxpriority = 0;


        //一番高い優先度を調べる
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            maxpriority = Mathf.Max(maxpriority, vibrationList[i].priority);
        }

        List<ControllerVibration> processList = new List<ControllerVibration>();

        //優先度が一番高いものを抽出
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if(vibrationList[i].priority == maxpriority)
            {
                processList.Add(vibrationList[i]);
            }
        }

        //優先度が高いものの中で振動が一番強いものを使う
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

        //振動
        gamepad_.SetMotorSpeeds(maxPowerLeft, maxPowerRight);
    }

    //リスト追加・変更
    public void Vibration(string name_, float leftPower_, float rightPower_, float priority_)
    {
        ControllerVibration buf = new ControllerVibration();

        buf.name = name_;
        buf.leftPower = leftPower_;
        buf.rightPower = rightPower_;
        buf.priority = priority_;

        //同じ名前の要素があったら変更
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if(buf.name == vibrationList[i].name)
            {
                vibrationList[i] = buf;
                return;
            }
        }

        //なかったら追加
        vibrationList.Add(buf); //リスト追加
    }

    //リスト追加・変更 優先度省略
    public void Vibration(string name_, float leftPower_, float rightPower_)
    {
        ControllerVibration buf = new ControllerVibration();

        buf.name = name_;
        buf.leftPower = leftPower_;
        buf.rightPower = rightPower_;
        buf.priority = 0;

        //同じ名前の要素があったら変更
        for (int i = 0; i < vibrationList.Count; ++i)
        {
            if (buf.name == vibrationList[i].name)
            {
                vibrationList[i] = buf;
                return;
            }
        }

        //なかったら追加
        vibrationList.Add(buf); //リスト追加
    }

    //リスト消去 成功でtrue 失敗でfalse
    public bool Destroy(string name_)
    {
        //同じ名前の要素があったら消去
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

    //終了処理
    void End(Gamepad gamepad_)
    {
        gamepad_.SetMotorSpeeds(0, 0);
    }
}
