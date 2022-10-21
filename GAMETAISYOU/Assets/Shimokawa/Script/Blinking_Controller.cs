using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blinking_Controller : MonoBehaviour
{
    [SerializeField] float Speed = 1.0f; //�_�ł���X�s�[�h
    private Text Text; //�_�ł�����e�L�X�g
    private Image Image; //�_�ł�����摜
    private float NowTime; //�_�ł����邽�߂ɕK�v�Ȏ��Ԃ��������邽�߂̓����I�ȕϐ�

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