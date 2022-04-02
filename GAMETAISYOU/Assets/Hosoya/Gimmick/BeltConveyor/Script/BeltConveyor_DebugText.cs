using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor_DebugText : MonoBehaviour
{
    [SerializeField] GameObject beltConveyor;
    [SerializeField] TextMesh textMesh;

    BeltConveyor beltConveyorScript;

    // Start is called before the first frame update
    void Start()
    {
        if (!Debug.isDebugBuild) { Destroy(this.gameObject); return; }    //リリースだったら表示しない

        //大きさ・位置調整
        {
            float boardScaleX = beltConveyor.transform.localScale.x;
            float boardScaleY = beltConveyor.transform.localScale.y;
            float textScaleX = 1.0f / boardScaleX * 2.0f;
            float textScaleY = 1.0f / boardScaleY * 2.0f;

            transform.localScale = new Vector2(textScaleX, textScaleY);

            transform.Translate(new Vector3(0, boardScaleY / 2.0f + 0.5f));
        }

        beltConveyorScript = beltConveyor.GetComponent<BeltConveyor>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = beltConveyorScript._speed.ToString();
    }
}
