using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioSource BGM;

    public void GameOver()
    {
        GlobalVariables.gameOver = true;
        gameOverScreen.SetActive(true);
        BGM.Stop();
        Time.timeScale = 0f;
    }
}
