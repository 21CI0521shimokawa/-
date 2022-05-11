using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallGarekiController : MonoBehaviour
{
    [SerializeField] int brokenWallPieces;
    public GameObject brokenWallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBrokenWall()
    {
        for (int i = 0; i < brokenWallPieces; ++i)
        {
            GameObject brokenWall = Instantiate(brokenWallPrefab) as GameObject;
            float offsetX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
            float offsetY = Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2);
            Vector3 offset = new Vector3(offsetX, offsetY, 0);
            brokenWall.transform.position = transform.position + offset;
            float moveSpeedX = Random.Range(0.1f, 3.0f);
            float moveSpeedY = Random.Range(0.1f, 3.0f);
            Vector2 moveSpeed = new Vector2(moveSpeedX, moveSpeedY);
            brokenWall.GetComponent<Rigidbody2D>().velocity = moveSpeed;
        }
    }
}
