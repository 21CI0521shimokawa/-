using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakWall : MonoBehaviour
{
    [SerializeField] float breakWeight;
    [SerializeField] int brokenWallPieces;
    public GameObject brokenWallPrefab;
    [SerializeField] AutoDoorControll PlaySE;
    [SerializeField] AudioClip SE;
    [SerializeField] AudioSource BreakWallAudioSource;
    ControllerVibrationScript controllerVibration;

    // Start is called before the first frame update
    void Start()
    {
        controllerVibration = GameObject.Find("ControllerVibration").GetComponent<ControllerVibrationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        GameObject @object = collision.gameObject;
        if (collision.gameObject.tag == "Slime")
        {
            Rigidbody2D rigid2D = @object.GetComponent<Rigidbody2D>();
            float weight = rigid2D.mass;
            float speed = rigid2D.velocity.magnitude;

            if (CheckWeight(weight))
            {
                StartCoroutine(Break());
            }
        }
    }

    private void SpawnBrokenWall()
    {
        for(int i = 0; i < brokenWallPieces; ++i)
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
    
    private bool CheckWeight(float weight)
    {
        return weight >= breakWeight;
    }

    void ObjectHide()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void ControllerVibration()
    {
        controllerVibration.Vibration("BreakWall", 1.0f, 1.0f);
        Invoke("ControllerVibrationDelayMethod", 1.0f);
    }

    private void ControllerVibrationDelayMethod()
    {
        controllerVibration.Destroy("BreakWall");
        Destroy(gameObject);
    }
    private IEnumerator Break()
    {
        yield return new WaitForSeconds(0.5f);
        PlaySE.PlaySE(SE);
        ObjectHide();
        SpawnBrokenWall();
        ControllerVibration();
        yield break;
    }

    //オートドア設定
    public void SetPlaySEDoor(AutoDoorControll door_)
    {
        PlaySE = door_;
    }
    public void PlaySEStart(AudioClip audio)
    {
        if (BreakWallAudioSource != null)
        {
            BreakWallAudioSource.PlayOneShot(audio);
        }
        else
        {
            Debug.Log("オーディオソースが設定されてない");
        }
    }

    public void SetBreakWeight(float weight_)
    {
        breakWeight = weight_;
    }

    public void SetBrokenWallPieces(int quantity_)
    {
        brokenWallPieces = quantity_;
    }
}
