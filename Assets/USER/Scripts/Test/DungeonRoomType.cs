using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoomType : MonoBehaviour
{
    public enum RoomType
    {
        LR = 0,
        LRD = 1,
        LRT = 2,
        LRTD = 3
    }

    [SerializeField] public RoomType roomType;

    public void DestroyThisRoom()
    {
        Destroy(gameObject);
    }
}
