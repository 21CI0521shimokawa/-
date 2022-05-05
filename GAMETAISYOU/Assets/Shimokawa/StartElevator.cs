using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevator : MonoBehaviour
{

    [SerializeField, Tooltip("現在の位置")]
    private Vector3 NowPosition;

    [SerializeField, Tooltip("目標位置")]
    private GameObject Destination;

    [SerializeField, Tooltip("Elevatorのスピード")]
    private float Speed;


    //当たり判定のすり抜け防止
    public Rigidbody2D ElevatorRigidbody;
    void Start()
    {
        ElevatorRigidbody = GetComponent<Rigidbody2D>();

       // DestinationNum = Mathf.Abs(Destination.transform.position.y);
       // StartCoroutine("ElevatorUp");
       // Debug.Log(DestinationNum);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,Destination.transform.position, Speed*Time.deltaTime);
    }
    #region コルーチン
    /* public IEnumerator ElevatorUp()
     {
         while (NowPosition.y < DestinationNum)
         {
             NowPosition = transform.position;
             ElevatorRigidbody.velocity = new Vector2(0, Speed);
             yield return new WaitForSeconds(0.01f);
         }
         ElevatorRigidbody.bodyType = RigidbodyType2D.Static;
         yield return new WaitForSeconds(0.01f);
         yield break;
     }*/
    #endregion
}
