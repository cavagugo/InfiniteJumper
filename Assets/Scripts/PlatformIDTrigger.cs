using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformIDTrigger : MonoBehaviour
{
    [SerializeField] private LevelGenerator levelGenerator;

    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Incrementamos en 1 el ID para que se puedan generar nuevos tipos de plataforma.
            levelGenerator.MaxPlatformID++;
            Debug.Log($"MaxPlatformID: {levelGenerator.MaxPlatformID}");
            //Lo desactivamos para que no lo vuelva a tocar por error.
            gameObject.SetActive(false);
        }
    }
}
