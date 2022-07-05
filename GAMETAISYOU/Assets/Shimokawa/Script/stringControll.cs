using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stringControll : MonoBehaviour
{
    //壁とアイテムの位置関係
    [SerializeField, Tooltip("Itemオブジェクト")]
    private GameObject Item;
    [SerializeField, Tooltip("Wallオブジェクト")]
    private GameObject Wall;
    [SerializeField, Tooltip("Playerオブジェクト")]
    private GameObject Player;

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
        OnMouseDrag();
        ItemPosition = Item.transform.position;
        WallPosition = Wall.transform.position;
        Distance = Vector3.Distance(ItemPosition, WallPosition);
        if (Distance >= 8.0f)
        {
            GameObject.Destroy(Item.GetComponent<SpringJoint2D>());
            Item.GetComponent<Renderer>().material.color = Color.red;
           
        }
    }
    private void OnMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 objectPoint
                = Camera.main.WorldToScreenPoint(transform.position);


            Vector3 pointScreen
                = new Vector3(Input.mousePosition.x,
                              Input.mousePosition.y,
                              objectPoint.z);


            Vector3 pointWorld = Camera.main.ScreenToWorldPoint(pointScreen);
            pointWorld.z = transform.position.z;


            transform.position = pointWorld;
        }
    }
    public void OnCollisionStay2D(Collision2D collision)
    {

    }
}
