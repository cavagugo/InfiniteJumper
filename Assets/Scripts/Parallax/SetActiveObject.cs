using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SetActiveObject : MonoBehaviour
{
    [SerializeField] private GameObject BG;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            BG.SetActive(true);
        }
    }
}
