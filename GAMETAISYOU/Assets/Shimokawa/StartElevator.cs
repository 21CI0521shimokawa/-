using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartElevator : MonoBehaviour
{

    [SerializeField, Tooltip("���݂̈ʒu")]
    private Vector3 NowPosition;

    [SerializeField, Tooltip("�ڕW�ʒu")]
    private GameObject Destination;

    [SerializeField, Tooltip("Elevator�̃X�s�[�h")]
    private float Speed;


    //�����蔻��̂��蔲���h�~
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
    #region �R���[�`��
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
