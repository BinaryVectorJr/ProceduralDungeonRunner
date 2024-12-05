using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreTile : MonoBehaviour
{
    public int block_x_loc;
    public int block_y_loc;

    [SerializeField] private Color baseColor, offsetColor, edgeColor, cornerColor, middleColor;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public bool isCenter = false;
    public bool isCorner = false;

    public bool isStart = false;
    public bool isFinish = false;

    public LayerMask gridCenterLayer;

    public TileManager currentChunksTileManager;

    public enum TileType
    {
        normal = 0,
        edge = 1,
        corner = 2,
        center = 3
    }

    public TileType currBlockType;

    public void Init(bool isOffset)
    {
        spriteRenderer.material.color = isOffset ? offsetColor : baseColor;

        CheckCenter();
    }

    public void CheckCenter()
    {
        currentChunksTileManager = MainChunkGenerator.mainChunkInstance.tileManagers[MainChunkGenerator.mainChunkInstance.globalChunkCount].GetComponent<TileManager>();

        var checkCenter = (block_x_loc % 5 == 0 && block_y_loc % 5 == 0) && (block_x_loc % 10 != 0 && block_y_loc % 10 != 0) && (block_x_loc != 0 && block_y_loc != 0);
        if(checkCenter == true)
        {
            isCenter = true;
            currentChunksTileManager.centerPosition.Add(this.gameObject.transform);
            spriteRenderer.material.color = middleColor;

            //transform.gameObject.AddComponent<SpawnSiderooms>();
            //transform.gameObject.GetComponent<SpawnSiderooms>().currentChunkDungeonGenerator = GameObject.FindGameObjectWithTag("DungeonManager").GetComponent<DungeonGenerator>();
            //gameObject.GetComponent<SpawnSiderooms>().canBeSideRoom = LayerMask.GetMask("Room");

            if (isCenter == true && checkCenter == true)
            {
                currBlockType = (TileType)3;
                this.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
                this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.0f, 1.0f);
                this.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.0f, -2.0f);
                //13 is the GridCenter layer
                this.gameObject.layer = 13;
                //this.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
        

        //if (block_x_loc % 5 == 0 && block_y_loc % 5 == 0)
        //{
        //    //Debug.Log("Center");
        //    this.currBlockType = CoreTile.TileType.center;
        //    spriteRenderer.material.color = middleColor;
        //}
    }

    public void CheckEdge(bool isEdge)
    {

        currBlockType = isEdge ? TileType.edge : TileType.normal;

        if(block_x_loc % (currentChunksTileManager.currentSeatLength - 1) == 0 && block_x_loc != 0)
        {
            currentChunksTileManager.rightEdgePosition.Add(this.gameObject.transform);
        }
        else if (block_x_loc == 0)
        {
            currentChunksTileManager.leftEdgePosition.Add(this.gameObject.transform);
        }

        if (currBlockType == TileType.edge)
        {
            spriteRenderer.material.color = edgeColor;
            currentChunksTileManager.edgePosition.Add(this.gameObject.transform);

            //Checking for corners
            var checkCorner = (block_x_loc == 0 && block_y_loc == 0) || (block_x_loc % (currentChunksTileManager.currentSeatLength - 1) == 0 && block_y_loc % (currentChunksTileManager.currentSeatWidth - 1) == 0);
            if (checkCorner)
            {
                isCorner = true;
                currBlockType = TileType.corner;
                spriteRenderer.material.color = cornerColor;
                currentChunksTileManager.cornerPosition.Add(this.gameObject.transform);
                currentChunksTileManager.edgePosition.Remove(this.gameObject.transform);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            StartCoroutine(TriggerDelay());
        }
        else
        {
            StopCoroutine(TriggerDelay());
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    { 

    }

    IEnumerator TriggerDelay()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        MainChunkGenerator.mainChunkInstance.TriggerNextChunk();
        this.gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
