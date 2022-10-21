using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking_Controller : MonoBehaviour
{
    [SerializeField] float Speed = 1.0f; //点滅するスピード
    private Text Text; //点滅させるテキスト
    private Image Image; //点滅させる画像
    private float NowTime; //点滅させるために必要な時間を所持するための内部的な変数

    private enum ObjType
    {
        TEXT,
        IMAGE
    };
    private ObjType thisObjType = ObjType.TEXT;

    void Start()
    {
        
        if (this.gameObject.GetComponent<Image>())
        {
            thisObjType = ObjType.IMAGE;
            Image = this.gameObject.GetComponent<Image>();
        }
        else if (this.gameObject.GetComponent<Text>())
        {
            thisObjType = ObjType.TEXT;
            Text = this.gameObject.GetComponent<Text>();
        }
    }

    void Update()
    {
       
        if (thisObjType == ObjType.IMAGE)
        {
            Image.color = GetAlphaColor(Image.color);
        }
        else if (thisObjType == ObjType.TEXT)
        {
            Text.color = GetAlphaColor(Text.color);
        }
    }

   
    Color GetAlphaColor(Color Color)
    {
        NowTime += Time.deltaTime * 5.0f * Speed;
        Color.a = Mathf.Sin(NowTime) * 0.5f + 0.5f; 

        return Color;
    }
}