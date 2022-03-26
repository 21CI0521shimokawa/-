using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard_DebugText : MonoBehaviour
{
    [SerializeField] GameObject weighingBoard;
    [SerializeField] TextMesh textMesh;

    WeighingBoard weighingBoardScript;

    // Start is called before the first frame update
    void Start()
    {
        //ëÂÇ´Ç≥ÅEà íuí≤êÆ
        {
            float boardScaleX = weighingBoard.transform.localScale.x;
            float boardScaleY = weighingBoard.transform.localScale.y;
            float textScaleX = 1.0f / boardScaleX * 2.0f;
            float textScaleY = 1.0f / boardScaleY * 2.0f;

            transform.localScale = new Vector2(textScaleX, textScaleY);

            transform.Translate(new Vector3(0, boardScaleY / 2.0f + 0.5f));
        }

        weighingBoardScript = weighingBoard.GetComponent<WeighingBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = weighingBoardScript.weight.ToString();
    }
}
