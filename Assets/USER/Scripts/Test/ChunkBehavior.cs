using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkBehavior : MonoBehaviour
{
    public static ChunkBehavior chunkBehaviorInstance;

    public Vector2 chunkSize = new Vector2(100, 100);

    public enum ChunkGates
    {
        One, Two,  Three, Four
    }

    public List<GameObject> entryGates = new List<GameObject>();
    public List<GameObject> exitGates = new List<GameObject>();

    public List<Transform> startPositions = new List<Transform>();
    public List<Transform> endPositions = new List<Transform>();

    private void OnValidate()
    {
        if (this.transform.CompareTag("ChunkBG"))
        {
            transform.localScale = chunkSize;
        }

        foreach (Transform child in this.gameObject.transform)
        {
            if (child.transform.CompareTag("EntryGate") && !entryGates.Contains(child.transform.gameObject))
            {
                entryGates.Add(child.transform.gameObject);
            }
        }

        foreach (Transform child in this.gameObject.transform)
        {
            if (child.transform.CompareTag("ExitGate") && !exitGates.Contains(child.transform.gameObject))
            {
                exitGates.Add(child.transform.gameObject);
            }
        }

        GenerateEntryGatePositions(chunkSize, entryGates);
        GenerateExitGatePositions(chunkSize, exitGates);
    }

    private void Awake()
    {
        chunkBehaviorInstance= this;
        //DungeonGenerator.dungeonGeneratorInstance.currStartingPositon = startPositions[Random.Range(1,startPositions.Count)];
    }

    private void Start()
    {
    }

    public void GenerateEntryGatePositions(Vector2 _chunkSize, List<GameObject> _enGates)
    {
        float topBound = _chunkSize.y/2;
        float bottomBound = -(_chunkSize.y/2);
        float leftBound = -(_chunkSize.x / 2);

        _enGates[0].transform.localPosition = new Vector3(leftBound, bottomBound-(bottomBound/4), 1);
        _enGates[0].transform.localScale = new Vector2(_chunkSize.x/10, _chunkSize.y/10);
        startPositions[0].transform.localPosition = _enGates[0].transform.position;

        _enGates[1].transform.localPosition = new Vector3(leftBound, bottomBound/4, 1);
        _enGates[1].transform.localScale = new Vector2(_chunkSize.x/10, _chunkSize.y/10);
        startPositions[1].transform.localPosition = _enGates[1].transform.position;

        _enGates[2].transform.localPosition = new Vector3(leftBound, topBound/4, 1);
        _enGates[2].transform.localScale = new Vector2(_chunkSize.x/10, _chunkSize.y/10);
        startPositions[2].transform.localPosition = _enGates[2].transform.position;

        _enGates[3].transform.localPosition = new Vector3(leftBound, topBound-(topBound/4), 1);
        _enGates[3].transform.localScale = new Vector2(_chunkSize.x/10, _chunkSize.y/10);
        startPositions[3].transform.localPosition = _enGates[3].transform.position;


    }

    public void GenerateExitGatePositions(Vector2 _chunkSize,  List<GameObject> _exGates)
    {
        float topBound = _chunkSize.y / 2;
        float bottomBound = -(_chunkSize.y / 2);
        float rightBound = _chunkSize.x / 2;

        _exGates[0].transform.localPosition = new Vector3(rightBound, bottomBound - (bottomBound / 4), 0);
        _exGates[0].transform.localScale = new Vector2(_chunkSize.x / 10, _chunkSize.y / 10);
        endPositions[0].transform.localPosition = _exGates[0].transform.position;

        _exGates[1].transform.localPosition = new Vector3(rightBound, bottomBound / 4, 0);
        _exGates[1].transform.localScale = new Vector2(_chunkSize.x / 10, _chunkSize.y / 10);
        endPositions[1].transform.localPosition = _exGates[1].transform.position;

        _exGates[2].transform.localPosition = new Vector3(rightBound, topBound / 4, 0);
        _exGates[2].transform.localScale = new Vector2(_chunkSize.x / 10, _chunkSize.y / 10);
        endPositions[2].transform.localPosition = _exGates[2].transform.position;

        _exGates[3].transform.localPosition = new Vector3(rightBound, topBound - (topBound / 4), 0);
        _exGates[3].transform.localScale = new Vector2(_chunkSize.x / 10, _chunkSize.y / 10);
        endPositions[3].transform.localPosition = _exGates[3].transform.position;
    }
}
