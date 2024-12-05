using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTiles : MonoBehaviour
{
    public List<GameObject> objects;
    //private Transform tileParent;

    private void Start()
    {
        int randObj = Random.Range(0, objects.Count);
        GameObject instance = (GameObject) Instantiate(objects[randObj], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
        instance.transform.localScale = new Vector2(2,2);
    }
}
