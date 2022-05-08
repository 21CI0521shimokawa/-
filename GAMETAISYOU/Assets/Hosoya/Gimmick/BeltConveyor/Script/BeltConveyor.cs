using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltConveyor : MonoBehaviour
{
    [SerializeField] GameObject beltconveyor;
    [SerializeField] WeighingBoard weighingBoardScript;

    [SerializeField] SpriteRenderer renderer;

    [SerializeField] Sprite[] images;

    public bool _isOn;  //ƒ‚ƒm‚ð“®‚©‚·‚©‚Ç‚¤‚©
    public float _speed; //‘¬“x

    [SerializeField] float moveImage;    //‰æ‘œ•Ï‚¦‚é

    // Start is called before the first frame update
    void Start()
    {
        moveImage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isOn)
        {
            Vector2 move = new Vector2(_speed * Time.deltaTime, 0);
            move = Quaternion.Euler(0, 0, beltconveyor.transform.rotation.z) * move;

            Animation();
         
            foreach (GameObject i in weighingBoardScript.onObjectsOll)
            {
                if(i)
                {
                    i.transform.Translate(move, Space.World);
                }
            }
        }
    }

    void Animation()
    {
        moveImage += _speed * Time.deltaTime * 3;

        while (moveImage < 0)
        {
            moveImage += 16.0f;
        }
        while (moveImage > 16)
        {
            moveImage -= 16.0f;
        }

        renderer.sprite = images[(int)moveImage];
    }
}
