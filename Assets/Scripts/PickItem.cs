using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            audioSource.Play();
            gameObject.SetActive(false);
                     
        }
    }
}
