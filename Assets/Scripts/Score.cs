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
    public TextMeshProUGUI coinsText;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.coins = 0;
        score = 0;
        scoreText.text = "Score: " + score;
    }

    // Actualizar score/altura basado en la altura de la cámara (para evitar que el score disminuya si el jugador cae)
    void Update()
    {
        score = Mathf.RoundToInt(scoreTracker.transform.position.y) * scoreMultiplier;
        scoreText.text = "Score: " + score;
        coinsText.text = GlobalVariables.coins.ToString();
    }
}
