using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringControll : MonoBehaviour
{
    //�ǂƃA�C�e���̈ʒu�֌W
    [SerializeField, Tooltip("Item�I�u�W�F�N�g")]
    private GameObject Item;
    [SerializeField, Tooltip("Wall�I�u�W�F�N�g")]
    private GameObject Wall;

    private Vector2 ItemPosition;
    private Vector2 WallPosition;
    private float Distance;

    public Rigidbody2D rd;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ItemPosition = Item.transform.position;
        WallPosition = Wall.transform.position;
        Distance = Vector3.Distance(ItemPosition,WallPosition);
        if (Distance < 5.0f)
        {
            rd.mass = 1;
        }
    }
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Slime")
        {
           rd.bodyType = RigidbodyType2D.Kinematic;
        }
    }
}
