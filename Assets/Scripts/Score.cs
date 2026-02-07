using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private GameObject scoreTracker;
    [SerializeField] private float scoreMultiplier = 1f;
    private float score;
    public TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        score = Mathf.RoundToInt(scoreTracker.transform.position.y) * scoreMultiplier;
        scoreText.text = "Score: " + score;
    }
}
