using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    //public static DungeonGenerator dungeonGeneratorInstance;

    public TileManager currentChunksTileManager;

    //Input edge detection algorithm here for start position generation
    public Transform currStartingPositon;

    public GameObject firstRoom;
    public Vector2 lastAgentRoomLoc;

    //Room index 0 = LR open, index 1 = LRB, index 2 = LRT, index 3 = LRBT 
    public List<GameObject> rooms = new List<GameObject>();

    public int roomDirection;
    public int randStartingPos;
    public float roomMoveAmount;

    private float timeBetweenRoom;
    [SerializeField] public float startTimeBetweenRoom = 0.0f;

    //TODO: Make these dependent on chumk X and Y
    public float minXLength;
    public float maxXLength;
    public float minYLength;
    public float maxYLength;
    public bool stopGeneration = false;

    public int upCounter;
    public int downCounter;
    public int leftCounter;

    public LayerMask roomsLayer;

    public bool dungeonDone = false;

    private void Awake()
    {
        //dungeonGeneratorInstance = this;
        //randStartingPos = Random.Range(0, ChunkBehavior.chunkBehaviorInstance.startPositions.Count);
        //randStartingPos = Random.Range(0, TileManager.tileManagerInstance.centerPosition.Count);
        //transform.position = ChunkBehavior.chunkBehaviorInstance.startPositions[randStartingPos].position;
        //transform.localPosition = TileManager.tileManagerInstance.centerPosition[0].position;

        //Problem, chunks can only be square in size; FIX IT!!!
        //roomMoveAmount = ChunkBehavior.chunkBehaviorInstance.chunkSize.x / 10;
    }

    private void Start()
    {
        //InitiateDungeon();
    }

    private void Update()
    {
        if(timeBetweenRoom <= 0 && stopGeneration == false)
        {
            RoomMove();
            timeBetweenRoom = startTimeBetweenRoom;
        }
        else
        {
            timeBetweenRoom -= Time.deltaTime;
        }

        //print(roomDirection.ToString());
    }

    public void InitiateDungeon(int _chunkNumber)
    {
        //roomsLayer = LayerMask.GetMask("Room");
        roomsLayer = LayerMask.GetMask("RoomCenter");

        currentChunksTileManager = MainChunkGenerator.mainChunkInstance.tileManagers[_chunkNumber].GetComponent<TileManager>();
        currStartingPositon = currentChunksTileManager.centerPosition[0];

        minXLength = currentChunksTileManager.centerPosition[0].position.x;
        maxXLength = currentChunksTileManager.centerPosition[currentChunksTileManager.centerPosition.Count - 1].position.x;
        minYLength = currentChunksTileManager.centerPosition[0].position.y;
        maxYLength = currentChunksTileManager.centerPosition[currentChunksTileManager.centerPosition.Count - 1].position.y;

        transform.localPosition = currStartingPositon.position;

        firstRoom = Instantiate(rooms[0], transform.localPosition, Quaternion.identity);

        if(firstRoom != null) 
        { 
            SetFirstRoom(firstRoom);
        }
        lastAgentRoomLoc = firstRoom.transform.position;
        currentChunksTileManager.centerPosition[0].transform.GetComponent<CoreTile>().isStart = true;
        //firstRoom.transform.localScale = new Vector2(ChunkBehavior.chunkBehaviorInstance.chunkSize.x / 100, ChunkBehavior.chunkBehaviorInstance.chunkSize.y / 100);
        firstRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
        roomDirection = Random.Range(1, 6);
    }

    private void RoomMove()
    {
        if (roomDirection == 1 || roomDirection == 2) //Move towards right
        {
            leftCounter = 0;
            
            //Do less than equal to, so we can have a CONNECTOR to next chunk
            if (transform.position.x < maxXLength)
            {
                Vector2 newPos = new Vector2(transform.position.x + roomMoveAmount, transform.position.y);
                transform.localPosition = newPos;

                int randomRoomIndex = Random.Range(0, rooms.Count);
                GameObject tempRoom = Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);
                tempRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                SetLastRoomAgentVisited(tempRoom);
                //tempRoom.transform.parent = this.transform;

                roomDirection = Random.Range(1, 6);

                if (roomDirection == 3)
                {
                    roomDirection = 2;
                }
                else if (roomDirection == 4)
                {
                    roomDirection = 5;
                }

            }
            else
            {
                //roomDirection = 5;
                //Stop level generation as we have reached the END
                stopGeneration = true;

                foreach (var t in currentChunksTileManager.centerPosition)
                {
                    //Debug.Log("center position stuff");

                    if (t.transform.position.Equals(lastAgentRoomLoc))
                    {
                        t.transform.GetComponent<CoreTile>().isFinish = true;
                        //return;
                    }
                }

                //update position of next chunk's start
                //Debug.Log("Call main chunk");

                //currentChunksTileManager.readyForGates = true;
                //{

                //}
                currentChunksTileManager.GetComponent<TileManager>().readyForGates = true;
                currentChunksTileManager.GetComponent<TileManager>().EnableSideRoomsAndGates(currentChunksTileManager.gridTiles);

                //MainChunkGenerator.mainChunkInstance.SpawnNextChunk();

            }

        }
        else if (roomDirection == 3 || roomDirection == 4) //Move towards left
        {
            leftCounter++;

            //You can adjust/remove the leftCounter value here and it still should work
            if (transform.position.x > minXLength && leftCounter <= 3)
            {
                upCounter = 0;

                Vector2 newPos = new Vector2(transform.position.x - roomMoveAmount, transform.position.y);
                transform.localPosition = newPos;

                int randomRoomIndex = Random.Range(0, rooms.Count);
                GameObject tempRoom = Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);
                tempRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                SetLastRoomAgentVisited(tempRoom);
                //tempRoom.transform.parent = this.transform;

                roomDirection = Random.Range(3, 6);
            }
            else
            {
                roomDirection = 5;
            }
        }
        else if (roomDirection == 5) //Move towards UP
        {
            upCounter++;

            if (transform.position.y < maxYLength)
            {
                leftCounter = 0;

                //Pre-process a collider to detect neighbors
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, roomsLayer);
                //print("here");
                //if(roomDetector.GetComponent<DungeonRoomType>().roomType != DungeonRoomType.RoomType.LRD && roomDetector.GetComponent<DungeonRoomType>().roomType != DungeonRoomType.RoomType.LRT)
                if (roomDetector.GetComponent<DungeonRoomType>().roomType != DungeonRoomType.RoomType.LRT)
                {
                    if (upCounter >= 2)
                    {
                        roomDetector.GetComponent<DungeonRoomType>().DestroyThisRoom();
                        GameObject upRoom = Instantiate(rooms[3], transform.position, Quaternion.identity);
                        upRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                        SetLastRoomAgentVisited(upRoom);
                        //downRoom.transform.parent = this.transform;
                    }
                    else
                    {
                        roomDetector.GetComponent<DungeonRoomType>().DestroyThisRoom();

                        int newRandTopRoom = Random.Range(1, 4);
                        if (newRandTopRoom == 2)
                        {
                            newRandTopRoom = 1;
                        }

                        GameObject tempRoom2 = Instantiate(rooms[newRandTopRoom], transform.position, Quaternion.identity);
                        tempRoom2.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                        SetLastRoomAgentVisited(tempRoom2);
                        //tempRoom2.transform.parent = this.transform;
                    }
                }


                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + roomMoveAmount);
                transform.localPosition = newPos;

                //Rooms at index 2 and 3 have top openings
                int randomRoomIndex = Random.Range(2, 3);
                GameObject tempRoom = Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);
                tempRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                SetLastRoomAgentVisited(tempRoom);
                //tempRoom.transform.parent = this.transform;

                roomDirection = Random.Range(1, 6);
            }
            else
            {
                //Stop level generation as we have reached the top limits
                //stopGeneration = true;
                roomDirection = 1;
            }

        }
        else if (roomDirection == 6) //Move DOWN
        {
            downCounter++;

            if (transform.position.y > minYLength)
            {
                downCounter = 0;

                //Pre-process a collider to detect neighbors
                Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 1, roomsLayer);
                //print("here");
                //if(roomDetector.GetComponent<DungeonRoomType>().roomType != DungeonRoomType.RoomType.LRD && roomDetector.GetComponent<DungeonRoomType>().roomType != DungeonRoomType.RoomType.LRT)
                if (roomDetector.GetComponent<DungeonRoomType>().roomType != DungeonRoomType.RoomType.LRT)
                {
                    if (upCounter >= 2)
                    {
                        roomDetector.GetComponent<DungeonRoomType>().DestroyThisRoom();
                        GameObject downRoom = Instantiate(rooms[3], transform.position, Quaternion.identity);
                        downRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                        SetLastRoomAgentVisited(downRoom);
                        //downRoom.transform.parent = this.transform;
                    }
                    else
                    {
                        roomDetector.GetComponent<DungeonRoomType>().DestroyThisRoom();

                        int newRandTopRoom = Random.Range(1, 4);
                        if (newRandTopRoom == 2)
                        {
                            newRandTopRoom = 1;
                        }

                        GameObject tempRoom2 = Instantiate(rooms[newRandTopRoom], transform.position, Quaternion.identity);
                        tempRoom2.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                        SetLastRoomAgentVisited(tempRoom2);
                        //tempRoom2.transform.parent = this.transform;
                    }
                }


                Vector2 newPos = new Vector2(transform.position.x, transform.position.y + roomMoveAmount);
                transform.localPosition = newPos;

                //Rooms at index 2 and 3 have top openings
                int randomRoomIndex = Random.Range(2, 3);
                GameObject tempRoom = Instantiate(rooms[randomRoomIndex], transform.position, Quaternion.identity);
                tempRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;
                SetLastRoomAgentVisited(tempRoom);
                //tempRoom.transform.parent = this.transform;

                roomDirection = Random.Range(1, 6);
            }

            else
            {
                //Stop level generation as we have reached the bottom limits
                //stopGeneration = true;
                roomDirection = 1;
            }
        }
    }

    public void SetFirstRoom(GameObject _firstRoom)
    {
        MainChunkGenerator.mainChunkInstance.startPos = _firstRoom.transform.position;
        //_firstRoom.AddComponent<ExitMarker>().CalculateClosestLeftBorderTile(_firstRoom);
    }

    public void SetLastRoomAgentVisited(GameObject _lastRoom)
    {
        MainChunkGenerator.mainChunkInstance.nextSpawnPosition = _lastRoom.transform.position;
        lastAgentRoomLoc = _lastRoom.transform.localPosition;
        //_lastRoom.AddComponent<ExitMarker>().CalculateClosestRightBorderTile(_lastRoom);
    }
}
