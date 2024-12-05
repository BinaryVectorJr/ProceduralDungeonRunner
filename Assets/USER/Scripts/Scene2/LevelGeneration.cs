//Credits to MirrorFish (2020) for original idea

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    public LevelChunkData[] currLevelChunkData;
    public LevelChunkData firstChunkToBeSpawned;

    private LevelChunkData previousSpawnedChunk;

    public Transform chunkSpawnOrigin;

    private Vector3 chunkSpawnPosition;

    [SerializeField] public int chunksToSpawn = 3;

    //public List<LevelChunkData> allowedChunkList = new List<LevelChunkData>();

    ///Event Systems
    private void OnEnable()
    {
        TriggerExit.OnChunkExited += PickAndSpawnChunk;
    }

    private void OnDisable()
    {
        TriggerExit.OnChunkExited -= PickAndSpawnChunk;
    }

    ///DEBUG SPAWNING
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            PickAndSpawnChunk();
        }
    }

    private void Start()
    {
        previousSpawnedChunk = firstChunkToBeSpawned;

        for (int i = 0; i < chunksToSpawn; i++)
        {
            PickAndSpawnChunk();
        }
    }

    LevelChunkData PickNextChunk()
    {
        List<LevelChunkData> allowedChunkList = new List<LevelChunkData>();
        LevelChunkData nextChunk = null;

        LevelChunkData.Gates nextRequiredDirection = LevelChunkData.Gates.One;

        switch(previousSpawnedChunk.exitGate)
        {
            case LevelChunkData.Gates.One:
                nextRequiredDirection = LevelChunkData.Gates.One;
                chunkSpawnPosition = chunkSpawnPosition + new Vector3(previousSpawnedChunk.chunkSize.x, 0f, 0f);
                break;

            case LevelChunkData.Gates.Two:
                nextRequiredDirection = LevelChunkData.Gates.Two;
                chunkSpawnPosition = chunkSpawnPosition + new Vector3(previousSpawnedChunk.chunkSize.x, 0f, 0f);
                break;

            case LevelChunkData.Gates.Three:
                nextRequiredDirection = LevelChunkData.Gates.Three;
                chunkSpawnPosition = chunkSpawnPosition + new Vector3(previousSpawnedChunk.chunkSize.x, 0f, 0f);
                break;

            case LevelChunkData.Gates.Four:
                nextRequiredDirection = LevelChunkData.Gates.Four;
                chunkSpawnPosition = chunkSpawnPosition + new Vector3(previousSpawnedChunk.chunkSize.x, 0f, 0f);
                break;

            default:
                break;
        }

        for (int i = 0; i < currLevelChunkData.Length; i++)
        {
            if(currLevelChunkData[i].entryGate == nextRequiredDirection)
            {
                allowedChunkList.Add(currLevelChunkData[i]);
            }
        }

        nextChunk = allowedChunkList[Random.Range(0, allowedChunkList.Count)];

        return nextChunk;
    }

    void PickAndSpawnChunk()
    {
        LevelChunkData chunkToSpawn = PickNextChunk();

        GameObject objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
        previousSpawnedChunk = chunkToSpawn;
        Instantiate(objectFromChunk, chunkSpawnPosition + chunkSpawnOrigin.position, Quaternion.identity);
    }

    public void UpdateSpawnOrigin(Vector3 originDelta)
    {
        chunkSpawnOrigin.localPosition = chunkSpawnOrigin.position + originDelta;
    }
}
