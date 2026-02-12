using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Reproduce un sonido, desactiva la moneda y aumenta en 1 el contador si colisionó con un player   
        if (collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            gameObject.SetActive(false);
            GlobalVariables.coins++;
        }
    }
}
