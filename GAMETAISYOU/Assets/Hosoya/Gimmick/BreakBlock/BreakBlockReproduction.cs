using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBlockReproduction : MonoBehaviour
{
    [SerializeField] GameObject breakBlockPrefab;

    private struct BreakBlockInfo
    {
        public GameObject breakblock;
        public Vector2 pos;
    }

    List<BreakBlockInfo> breakBlocks = new List<BreakBlockInfo>();

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] findBlocks = GameObject.FindGameObjectsWithTag("BreakBlock");

        foreach(GameObject i in findBlocks)
        {
            BreakBlockInfo buf = new BreakBlockInfo();
            buf.breakblock = i.gameObject;
            buf.pos = i.gameObject.transform.position;
            breakBlocks.Add(buf);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SlimeTrigger")
        {
            Reproduction();
        }
    }

    void Reproduction()
    {
        List<int> reproductionNumber = new List<int>();

        //ê∂ê¨
        AutoDoorControll door = GameObject.Find("Automatic door").GetComponent<AutoDoorControll>();

        for(int i = 0; i < breakBlocks.Count; ++i)
        {
            if(!breakBlocks[i].breakblock)
            {
                //BreakBlockInfo block = new BreakBlockInfo();
                //block.breakblock = GameObject.Instantiate(breakBlockPrefab);
                //block.breakblock.transform.position = breakBlocks[i].pos;
                //block.pos = breakBlocks[i].pos;

                ////ê›íË
                //BreakWall buf = block.breakblock.GetComponent<BreakWall>();
                //buf.SetPlaySEDoor(door);
                //buf.SetBreakWeight(1.5f);
                //buf.SetBrokenWallPieces(2);

                //breakBlocks.Add(block);

                //reproductionNumber.Add(i);
            }
        }

        //ÉäÉXÉgêÆóù
        foreach(int i in reproductionNumber)
        {
            breakBlocks.Remove(breakBlocks[i]);
        }


        for (int i = 0; i < breakBlocks.Count; ++i)
        {
            if (!breakBlocks[i].breakblock)
            {
                BreakBlockInfo bufBlockInfo = breakBlocks[i];
                bufBlockInfo.breakblock = Instantiate(breakBlockPrefab);
                bufBlockInfo.breakblock.transform.position = breakBlocks[i].pos;

                //ê›íË
                BreakWall buf = bufBlockInfo.breakblock.GetComponent<BreakWall>();
                buf.SetPlaySEDoor(door);
                buf.SetBreakWeight(1.5f);
                buf.SetBrokenWallPieces(2);

                breakBlocks[i] = bufBlockInfo;
            }
        }

    }
}
