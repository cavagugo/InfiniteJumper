using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;

    public void GameOver()
    {
        GlobalVariables.gameOver = true;
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
    }
}
