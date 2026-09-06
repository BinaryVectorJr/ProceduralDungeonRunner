using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainChunkGenerator : MonoBehaviour
{
    public static MainChunkGenerator mainChunkInstance {get; private set; }

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
        if (mainChunkInstance != null && mainChunkInstance != this)
        {
            Destroy(this);   
        }
        else
        {
            mainChunkInstance = this;
        }
    }

    private void Start()
    {
        tileManagers.Add(Instantiate(tileManagerMainScript).gameObject);
        tileManagers[globalChunkCount].GetComponent<TileManager>().GenerateBase(startPos, globalChunkCount);
        SpawnPlayer();
    }

    private void Update()
    {
        globalChunkParents.RemoveAll(item => item == null);
    }

    public void SpawnPlayer()
    {
        StartCoroutine(SpawnDelay());
        if (playerSpawned == false)
        {
            InstantiatePlayer();
            playerSpawned = true;
        }
        else if (playerSpawned == true)
        {
            StopAllCoroutines();
        }
    }

    IEnumerator SpawnDelay()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public void InstantiatePlayer()
    {
        var playerGO = Instantiate(mainPlayerPrefab, playerStartPos.position, Quaternion.identity);
        var cam = Instantiate(mainPlayerCam, new Vector3(playerStartPos.position.x, playerStartPos.position.y, -5.0f), Quaternion.identity);
        cam.transform.parent = playerGO.transform;
    }

    //Create Listener events here, so that we can subscribe to two events - chunk created and dungeons created. These will drive the main "sidescroller" part of the code

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

    public void SpawnNextChunk()
    {
        Debug.Log("Spawning next chunk");
        nextSpawnPosition = nextSpawnPosition + new Vector2(5.0f, -5.0f);
        //TODO: Update Tile manager to this pos
        //TODO: Call Tile Managers generate base

        NextChunkGenerate(globalChunkCount);
    }

    public void NextChunkGenerate(int _nextChunkCount)
    {
        tileManagers.Add(Instantiate(tileManagerMainScript).gameObject);
        tileManagers[globalChunkCount].GetComponent<TileManager>().transform.position = nextSpawnPosition;
        tileManagers[globalChunkCount].GetComponent<TileManager>().GenerateBase(nextSpawnPosition, _nextChunkCount);

        //TODO: set updated postition and Tile Manager for dungeon manager
        //}
        //else
        //{
        //tilesDoneEvent.RemoveAllListeners();
        //}
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
