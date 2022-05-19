using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighingBoard_ChangeCounter : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] counterAnimation;
    
    [SerializeField] WeighingBoard weighingBoard;

    public float _goalWeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(spriteRenderer)
        {
            if(_goalWeight == 0)
            {
                spriteRenderer.sprite = counterAnimation[(int)weighingBoard.weight % 10];
            }
            else
            {
                int buf = (int)_goalWeight - (int)weighingBoard.weight;
                buf = Mathf.Max(0, buf);
                spriteRenderer.sprite = counterAnimation[buf % 10];
            }
        }
    }
}
