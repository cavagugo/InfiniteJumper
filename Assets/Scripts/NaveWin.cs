using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveWin : MonoBehaviour
{
    [SerializeField] private string winSceneName;
    [SerializeField] private AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();

            Debug.Log("Ganaste!");
            Time.timeScale = 0f;
            sceneLoader.LoadSceneByName(winSceneName);
            audioSource.Play();
        }
    }
}
