using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject playerGO;
    [SerializeField] public float yThreshold = -100.0f;

    public float score;
    [SerializeField] public float scoreMultiplier = 0.8f;

    public TMP_Text scoreText;

    // Update is called once per frame
    void Update()
    {
        if (playerGO != null)
        {
            if (playerGO.transform.position.y < yThreshold)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        CalculateScore();
    }

    public void CalculateScore()
    {
        score += Time.deltaTime * scoreMultiplier;
        scoreText.text = "Time: " + score.ToString("F2");
    }
}
