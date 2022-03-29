using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StageGimmick : MonoBehaviour
{
    [Header("StageGimmick Script")]
    [Tooltip("対応番号")] public int _Number;
    [Tooltip("起動確認")]public bool _IsOpen;
    public virtual void Open() { }
    public virtual void Close() { }
}
