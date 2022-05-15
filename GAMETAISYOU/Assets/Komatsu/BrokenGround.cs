using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenGround : MonoBehaviour
{
    WeighingBoard weighingBoard;
    [SerializeField] float breakWeight;
    [SerializeField] int brokenWallPieces;
    public GameObject brokenWallPrefab;

    [SerializeField] AutoDoorControll PlaySE;
    [SerializeField] AudioClip SE;
    [SerializeField] AudioSource BreakWallAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        weighingBoard = this.GetComponent<WeighingBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckBroken())
        {
            StartCoroutine(Break());
        }
    }

    private void SpawnBrokenWall()
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

    private bool CheckBroken()
    {
        return this.breakWeight <= weighingBoard.weight;
    }
    private IEnumerator Break()
    {
        yield return new WaitForSeconds(0.5f);
        PlaySE.PlaySE(SE);
        Destroy(gameObject);
        SpawnBrokenWall();
        yield break;
    }
}
