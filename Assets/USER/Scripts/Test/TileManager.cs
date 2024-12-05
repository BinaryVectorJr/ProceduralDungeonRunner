using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    //public static TileManager tileManagerInstance;

    public CoreTile coreBlock;

    //Object Settings (1 by blocks are not being made)
    public int maxChunkLength = 51;
    public int maxChunkWidth = 51;

    public int currentSeatLength;
    public int currentSeatWidth;

    [SerializeField] public Vector2 positionOffset;

    //Store Generated Blocks
    public Dictionary<Vector2, CoreTile> gridTiles;

    //Store list of edges
    public List<Transform> edgePosition = new List<Transform>();

    //Specifically right edge position to use in ExitMarker.cs
    public List<Transform> leftEdgePosition = new List<Transform>();

    //Specifically right edge position to use in ExitMarker.cs
    public List<Transform> rightEdgePosition = new List<Transform>();

    //Store list of corner blocks
    public List<Transform> cornerPosition = new List<Transform>();

    //Store list of center blocks
    public List<Transform> centerPosition = new List<Transform>();

    public GameObject parent = null;

    public GameObject dungeonAgent;

    public bool readyForDungeon = false;
    public bool readyForGates = false;

    public GameObject activeDungeonManager;

    public int chunkCount = 0;

    public List<float> leftBorderDistances = new List<float>();
    public List<float> rightBorderDistances = new List<float>();


    // Start is called before the first frame update
    void Awake()
    {
        //tileManagerInstance = this;
    }

    private void Start()
    {
        //GenerateBase();
    }

    private void Update()
    {
        edgePosition.RemoveAll(item => item == null);
        leftEdgePosition.RemoveAll(item => item == null);
        rightEdgePosition.RemoveAll(item => item == null);
        cornerPosition.RemoveAll(item => item == null);
        centerPosition.RemoveAll(item => item == null);
    }

    public void GenerateBase(Vector2 _pos, int _chunkCount)
    {
        chunkCount = _chunkCount;
        parent = new GameObject($"Chunk {_chunkCount}");
        MainChunkGenerator.mainChunkInstance.globalChunkParents.Add(parent);

        //Need a minimum of 2 to have a seat
        currentSeatLength = maxChunkLength;
        currentSeatWidth =  maxChunkWidth;

        GenerateGrid(currentSeatLength, currentSeatWidth, _pos);
        ClearNormalTiles(gridTiles);

        if(readyForDungeon == true)
        {
            activeDungeonManager = Instantiate(dungeonAgent, centerPosition[0].position, Quaternion.identity);
            MainChunkGenerator.mainChunkInstance.currDungeonAgent = activeDungeonManager.GetComponent<DungeonGenerator>();
            MainChunkGenerator.mainChunkInstance.dungeonManagers.Add(activeDungeonManager);

            activeDungeonManager.GetComponent<DungeonGenerator>().InitiateDungeon(_chunkCount);

            readyForDungeon = false;

            //if(readyForGates == true)
            //{
                //EnableSideRoomsAndGates(gridTiles);
            //}

            //foreach(Transform t in centerPosition)
            //{
            //    t.gameObject.AddComponent<SpawnSiderooms>();
            //    t.gameObject.GetComponent<SpawnSiderooms>().currentChunkDungeonGenerator = dm.GetComponent<DungeonGenerator>();
            //    t.gameObject.GetComponent<SpawnSiderooms>().canBeSideRoom = dm.GetComponent<DungeonGenerator>().roomsLayer;
            //}
            //readyForDungeon = false;
        }
        
    }

    public void ClearPreviousGenerated()
    {
        var children = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            children.Add(child.gameObject);
            children.ForEach(child => Destroy(child));
        }

        edgePosition.Clear();
        leftEdgePosition.Clear();
        rightEdgePosition.Clear();
        cornerPosition.Clear();
        centerPosition.Clear();
    }

    public void GenerateGrid(int _length, int _width, Vector2 _positionOffset)
    {
        //Clear the parent for each
        ClearPreviousGenerated();

        gridTiles = new Dictionary<Vector2, CoreTile>();    //Instantiate new dictionary to collect all the generated blocks

        for (int x = 0; x < _length; x++)
        {
            for (int y = 0; y < _width; y++) 
            {
                var spawnedTile = Instantiate(coreBlock, _positionOffset + new Vector2(x, y), Quaternion.identity);
                spawnedTile.name = $"{x} {y}";
                spawnedTile.block_x_loc = x;
                spawnedTile.block_y_loc = y;

                spawnedTile.transform.parent = parent.transform;


                //Setting offset color for seat
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                //Checking the edges
                var isEdge = (x == 0 && y == 0) || (x % _length != 0 && y % (_width - 1) == 0) || (x % (_length - 1) == 0 && y % _width != 0);
                spawnedTile.CheckEdge(isEdge);

                gridTiles[new Vector2(x, y)] = spawnedTile;
            }
        }
    }

    public CoreTile GetEdgeBlock(Dictionary<Vector2, CoreTile> _activeBlockDictionary)
    {
        return _activeBlockDictionary.Where(t => t.Value.currBlockType.Equals("edge")).First().Value;
    }

    public CoreTile GetCornerBlock(Dictionary<Vector2, CoreTile> _activeBlockDictionary)
    {
        return _activeBlockDictionary.Where(t => t.Value.currBlockType.Equals("corner")).First().Value;
    }

    public CoreTile GetCenterBlock(Dictionary<Vector2, CoreTile> _activeBlockDictionary)
    {
        return _activeBlockDictionary.Where(t => t.Value.isCenter.Equals(true)).First().Value;
    }

    public void SetMaxLengthSlider(System.Single _maxLength)
    {
        maxChunkLength = (int)_maxLength;
    }

    public void SetMaxWidthSlider(System.Single _maxWidth)
    {
        maxChunkWidth = (int)_maxWidth;
    }

    public void ClearNormalTiles(Dictionary<Vector2, CoreTile> allTiles)
    {
        foreach(var tile in allTiles)
        {
            if(tile.Value.currBlockType == CoreTile.TileType.normal && tile.Value.isCenter != true )
            {
                Destroy(tile.Value.gameObject);
            }
        }

        AddSideRoomSpawner(allTiles);

        readyForDungeon = true;

    }

    public void AddSideRoomSpawner(Dictionary<Vector2, CoreTile> allTiles)
    {
        foreach (var tile in allTiles)
        {
            if (tile.Value.isCenter == true)
            {
                tile.Value.transform.gameObject.AddComponent<SpawnSiderooms>();
                //tile.Value.GetComponent<SpawnSiderooms>().currentChunkDungeonGenerator = activeDungeonManager.GetComponent<DungeonGenerator>();
            }
        }
    }

    public void EnableSideRoomsAndGates(Dictionary<Vector2, CoreTile> allTiles)
    {
        foreach (var tile in allTiles)
        {
            if (tile.Value.isCenter == true)
            {
                //tile.Value.transform.gameObject.AddComponent<SpawnSiderooms>();
                tile.Value.GetComponent<SpawnSiderooms>().UpdateWithSiderooms(activeDungeonManager.GetComponent<DungeonGenerator>());
                tile.Value.transform.gameObject.SetActive(false);

                if(tile.Value.isStart == true)
                {
                    tile.Value.transform.gameObject.SetActive(true);
                    CalculateClosestLeftBorderTile(tile.Value.transform);
                }

                if(tile.Value.isFinish == true)
                {
                    tile.Value.transform.gameObject.SetActive(false);
                    MainChunkGenerator.mainChunkInstance.nextSpawnPosition = tile.Value.transform.position;
                    CalculateClosestRightBorderTile(tile.Value.transform);
                }
            }
        }

        MainChunkGenerator.mainChunkInstance.globalChunkCount += 1;

    }

    public void CalculateClosestLeftBorderTile(Transform _firstRoom)
    {
        Debug.Log("CREATE ENTRY GATES");

        foreach (Transform t in leftEdgePosition)
        {
            float distance = Vector2.Distance(_firstRoom.position, t.position);
            leftBorderDistances.Add(distance);
            leftBorderDistances.Sort();

            //This value came from the float list above
            if (distance < 5.4f)
            {
                Destroy(t.gameObject);
            }

            //Debug.DrawLine(this.transform.position, t.position);
            readyForGates = false;
        }
    }

    public void CalculateClosestRightBorderTile(Transform _lastRoom)
    {
        Debug.Log("CREATE EXIT GATES");

        foreach (Transform t in rightEdgePosition)
        {
            float distance = Vector2.Distance(_lastRoom.position, t.position);
            rightBorderDistances.Add(distance);
            rightBorderDistances.Sort();

            //This value came from the float list above
            if (distance < 5.4f)
            {
                Destroy(t.gameObject);
            }

            //Debug.DrawLine(this.transform.position, t.position);
            readyForGates = false;
        }
    }
}
