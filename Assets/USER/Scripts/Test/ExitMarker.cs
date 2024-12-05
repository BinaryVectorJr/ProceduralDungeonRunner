using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExitMarker : MonoBehaviour
{
    //OBSOLETE SCRIPT. NO NEED TO USE IT.

    public List<float> leftBorderDistances = new List<float>();
    public List<float> rightBorderDistances = new List<float>();

    public TileManager currentChunksTileManager;

    private void Start()
    {
        currentChunksTileManager = MainChunkGenerator.mainChunkInstance.tileManagers[MainChunkGenerator.mainChunkInstance.globalChunkCount].GetComponent<TileManager>();
    }

    public void CalculateClosestLeftBorderTile(GameObject _firstRoom)
    {
        Debug.Log("CREATE ENTRIES");

        foreach (Transform t in currentChunksTileManager.leftEdgePosition)
        {
            float distance = Vector2.Distance(_firstRoom.transform.position, t.position);
            leftBorderDistances.Add(distance);
            leftBorderDistances.Sort();

            //This value came from the float list above
            if (distance < 5.4f)
            {
                Destroy(t.gameObject);
            }

            //Debug.DrawLine(this.transform.position, t.position);
        }
    }

    public void CalculateClosestRightBorderTile(GameObject _lastRoom)
    {
        Debug.Log("CREATE EXITS");

        foreach(Transform t in currentChunksTileManager.rightEdgePosition)
        {
            float distance = Vector2.Distance(_lastRoom.transform.position, t.position);
            rightBorderDistances.Add(distance);
            rightBorderDistances.Sort();

            //This value came from the float list above
            if(distance < 5.4f)
            {
                Destroy(t.gameObject);
            }

            //Debug.DrawLine(this.transform.position, t.position);
        }
    }
}
