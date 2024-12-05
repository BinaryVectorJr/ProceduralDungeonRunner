//Credits to MirrorFish (2020) for original idea

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelChunkData")]
public class LevelChunkData : ScriptableObject
{
    public enum Gates
    {
        One, Two, Three, Four
    }

    //TODO make size resizeable if necessary
    public Vector2 chunkSize = new Vector2(10f, 10f);

    public GameObject[] levelChunks;
    public Gates entryGate;
    public Gates exitGate;
}
