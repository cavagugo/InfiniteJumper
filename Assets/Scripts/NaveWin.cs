using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveWin : MonoBehaviour
{
    [SerializeField] private string winSceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneLoader sceneLoader = FindObjectOfType<SceneLoader>();
            Debug.Log("Ganaste!");
            sceneLoader.LoadSceneByName(winSceneName);
        }
    }
}
