using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedScript : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    
    private float destroyTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        destroyTimer += Time.deltaTime;

        if(destroyTimer > 5 ) 
        {
            Destroy(gameObject);
        }

        rb.velocity = Vector2.left * (speed + GameManagerA.gmInstance.speedMultiplier);
    }
}
