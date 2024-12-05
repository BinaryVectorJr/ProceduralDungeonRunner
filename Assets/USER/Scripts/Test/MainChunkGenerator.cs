using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MainChunkGenerator : MonoBehaviour
{
    public static MainChunkGenerator mainChunkInstance;

    public List<GameObject> globalChunkParents;

    public TileManager tileManagerMainScript;
    public DungeonGenerator currDungeonAgent;

    public Vector2 startPos = new Vector2(0, 0);
    public Transform playerStartPos;
    public Vector2 nextSpawnPosition = new Vector2(0, 0);

    public List<GameObject> tileManagers = new List<GameObject>();
    public List<GameObject> dungeonManagers = new List<GameObject>();

    public int globalChunkCount = 0;
    [SerializeField] private int maxSessionChunksCount;

    //public UnityEvent tilesDoneEvent = new UnityEvent();

    public GameObject mainPlayerPrefab;
    public GameObject mainPlayerCam;

    public bool playerSpawned = false;

    private void Awake()
    {
        mainChunkInstance = this;
    }

    private void Start()
    {
        tileManagers.Add(Instantiate(tileManagerMainScript).gameObject);
        tileManagers[globalChunkCount].GetComponent<TileManager>().GenerateBase(startPos, globalChunkCount);
        //globalChunkCount++;
        //globalChunkCount = chunks[0].GetComponent<TileManager>().chunkCount;

        //if(dungeonManagers.Count < 3)
        //{
        //    dungeonManagers[globalChunkCount].GetComponent<DungeonGenerator>().InitiateDungeon(globalChunkCount);
        //}

        //tilesDoneEvent.AddListener(NextChunkGenerate);




    }

    private void Update()
    {
        globalChunkParents.RemoveAll(item => item == null);

        //if(Input.GetKeyDown(KeyCode.T)) 
        //Replace 2 with max session counts
        if (currDungeonAgent.stopGeneration == true && globalChunkCount < maxSessionChunksCount)
        {
            //SpawnNextChunk();

            if(globalChunkCount == 1)
            {
                StartCoroutine(SpawnDelay());
                if (playerSpawned == false)
                {
                    SpawnPlayer();
                    playerSpawned = true;
                }
                else if (playerSpawned == true)
                {
                    StopAllCoroutines();
                }
            }
        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void SpawnNextChunk()
    {
        //if(globalChunkCount < 3)
        //{
        //    dungeonManagers[globalChunkCount].GetComponent<DungeonGenerator>().stopGeneration = false;
        //}    

        Debug.Log("Spawning next chunk");
        nextSpawnPosition = nextSpawnPosition + new Vector2(5.0f, -5.0f);
        //TODO: Update Tile manager to this pos
        //TODO: Call Tile Managers generate base

        //if(PlayerController collides with something)
        //tilesDoneEvent.Invoke();

        NextChunkGenerate(globalChunkCount);
    }

    public void SpawnPlayer()
    {
        //var playerGO = Instantiate(mainPlayerPrefab, startPos - new Vector2(4.0f, 0.0f), Quaternion.identity);
        var playerGO = Instantiate(mainPlayerPrefab, playerStartPos.position, Quaternion.identity);
        var cam = Instantiate(mainPlayerCam, new Vector3(playerStartPos.position.x, playerStartPos.position.y, -5.0f), Quaternion.identity);
        cam.transform.parent = playerGO.transform;

        //tilesDoneEvent.RemoveAllListeners();

    }

    //Create Listener events here, so that we can subscribe to two events - chunk created and dungeons created. These will drive the main "sidescroller" part of the code

    public void NextChunkGenerate(int _nextChunkCount)
    {
        //Replace 2 with max chunk count you want in one session
        //if (globalChunkCount < maxSessionChunksCount)
        //{
            //chunks.Add(Instantiate(tileManagerMainScript).gameObject);
            //tileManagers[0].GetComponent<TileManager>().transform.position = nextSpawnPosition;
            //tileManagers[0].GetComponent<TileManager>().GenerateBase(nextSpawnPosition, _nextChunkCount);
            //globalChunkCount++;

            tileManagers.Add(Instantiate(tileManagerMainScript).gameObject);
            tileManagers[globalChunkCount].GetComponent<TileManager>().transform.position = nextSpawnPosition;
            tileManagers[globalChunkCount].GetComponent<TileManager>().GenerateBase(nextSpawnPosition, _nextChunkCount);

        //globalChunkCount++;

        //TODO: set updated postition and Tile Manager for dungeon manager
        //}
        //else
        //{
        //tilesDoneEvent.RemoveAllListeners();
        //}
    }


    public void TriggerNextChunk()
    {
        if (globalChunkCount ==  maxSessionChunksCount)
        {
            Debug.Log("Destroy the first chunk and reduce global chunk count by 1");
            DestroyOldChunk(maxSessionChunksCount - globalChunkCount);
            globalChunkCount -= 1;
        }

        Debug.Log("Generating next chunk");
        SpawnNextChunk();

    }

    public void DestroyOldChunk(int indexOfObject)
    {
        for (var i = globalChunkParents[indexOfObject].transform.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(globalChunkParents[indexOfObject].transform.GetChild(i).gameObject);
            Object.Destroy(globalChunkParents[indexOfObject].transform.gameObject);
        }
    }
}
