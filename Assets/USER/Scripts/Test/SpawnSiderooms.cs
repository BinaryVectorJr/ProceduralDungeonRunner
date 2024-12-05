using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSiderooms : MonoBehaviour
{
    [SerializeField] public LayerMask canBeSideRoom;

    private void Start()
    {
        //canBeSideRoom = LayerMask.GetMask("Room");
        canBeSideRoom = LayerMask.GetMask("RoomCenter");
        //currentChunkDungeonGenerator = DungeonGenerator.dungeonGeneratorInstance;
    }

    public void UpdateWithSiderooms(DungeonGenerator _currentChunkDungeonGenerator)
    {
        Collider2D roomDetector = Physics2D.OverlapCircle(transform.position, 2, canBeSideRoom);

            if (roomDetector == null && _currentChunkDungeonGenerator.stopGeneration == true)
            {
                //Spawn Random sideroom
                int randSideRoom = Random.Range(0, _currentChunkDungeonGenerator.rooms.Count);
                GameObject tempSideRoom = (GameObject)Instantiate(_currentChunkDungeonGenerator.rooms[randSideRoom], transform.position, Quaternion.identity);
                tempSideRoom.transform.parent = MainChunkGenerator.mainChunkInstance.globalChunkParents[MainChunkGenerator.mainChunkInstance.globalChunkCount].transform;

                //Destroy(gameObject);
        }
    }
}
