using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputDemo : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("joystick button 0"))
        {
            Debug.Log("button0");
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            Debug.Log("button1");
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            Debug.Log("button2");
        }
        if (Input.GetKeyDown("joystick button 3"))
        {
            Debug.Log("button3");
        }
        if (Input.GetKeyDown("joystick button 4"))
        {
            Debug.Log("button4");
        }
        if (Input.GetKeyDown("joystick button 5"))
        {
            Debug.Log("button5");
        }
        if (Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("button6");
        }
        if (Input.GetKeyDown("joystick button 7"))
        {
            Debug.Log("button7");
        }
        if (Input.GetKeyDown("joystick button 8"))
        {
            Debug.Log("button8");
        }
        if (Input.GetKeyDown("joystick button 9"))
        {
            Debug.Log("button9");
        }

        //左スティック
        {
            float hori = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            if ((hori != 0) || (vert != 0))
            {
                Debug.Log("Lstick:" + hori + "," + vert);
            }
        }

        //右スティック
        {
            float hori = Input.GetAxis("R_Stick_H");
            float vert = Input.GetAxis("R_Stick_V");
            if ((hori != 0) || (vert != 0))
            {
                Debug.Log("Rstick:" + hori + "," + vert);
            }
        }

        //左トリガー
        {
            float trigger = Input.GetAxis("L_Trigger");

            if(trigger != 0)
            {
                Debug.Log("Ltrigger:" + trigger);
            }
        }

        //右トリガー
        {
            float trigger = Input.GetAxis("R_Trigger");

            if (trigger != 0)
            {
                Debug.Log("Rtrigger:" + trigger);
            }
        }
    }
}
