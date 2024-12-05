using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerA : MonoBehaviour
{
    public static GameManagerA gmInstance;

    public GameObject spawnObject;
    public GameObject[] spawnPoints;

    public float timer;
    public float timeBetweenSpawns;
    public float score;
    public float speedMultiplier;
    public float scoreMultiplier = 0.8f;

    public TMP_Text scoreText;

    private void Awake()
    {
        gmInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerMovement.playerMovementInstance.isDead)
        {
            SceneManager.LoadScene(0);
        }

        score += Time.deltaTime * scoreMultiplier;
        scoreText.text = "Distance: " + score.ToString("F2");

        speedMultiplier += Time.deltaTime * 0.1f;
        timer += Time.deltaTime;

        if(timer > timeBetweenSpawns)
        {
            timer = 0;

            int randNo = Random.Range(0, spawnPoints.Length);

            Instantiate(spawnObject, spawnPoints[randNo].transform.position, Quaternion.identity);
        }
    }
}
