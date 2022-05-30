using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class StartGame : MonoBehaviour
{
    [SerializeField] GameObject timeLine;

    [SerializeField, Tooltip("ボタンを押してもゲームが始まらない時間（second）")] float notStartTime;

    float timeCount;

    // Start is called before the first frame update
    void Start()
    {
        timeCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeLine.activeSelf)
        {
            if (timeCount >= notStartTime)
            {
                if (ControllerOperation())
                {
                    timeLine.SetActive(true);
                }
            }
            else
            {
                timeCount += Time.deltaTime;
            }
        }
    }

    bool ControllerOperation()
    {
        Gamepad gamepad = Gamepad.current;

        if (gamepad == null) { return false; }

        return gamepad.aButton.isPressed || gamepad.bButton.isPressed || gamepad.buttonEast.isPressed || gamepad.buttonNorth.isPressed || gamepad.buttonSouth.isPressed || gamepad.buttonWest.isPressed || gamepad.circleButton.isPressed || gamepad.crossButton.isPressed || gamepad.leftStick.left.isPressed || gamepad.leftStick.right.isPressed || gamepad.leftStick.up.isPressed || gamepad.leftStick.down.isPressed || gamepad.leftStickButton.isPressed || gamepad.leftTrigger.isPressed || gamepad.rightStick.left.isPressed || gamepad.rightStick.right.isPressed || gamepad.rightStick.up.isPressed || gamepad.rightStick.down.isPressed || gamepad.rightStickButton.isPressed || gamepad.rightTrigger.isPressed || gamepad.selectButton.isPressed || gamepad.squareButton.isPressed || gamepad.startButton.isPressed || gamepad.triangleButton.isPressed || gamepad.xButton.isPressed || gamepad.yButton.isPressed;
    }

}
