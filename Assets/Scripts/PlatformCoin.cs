using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCoin : MonoBehaviour
{
    [SerializeField] private GameObject coinChild;
    [SerializeField] private float spawnChance = 0.25f; 

    public void TryReactivateCoin()
    {
        // Random.value devuelve un número entre 0.0 y 1.0
        //La moneda tiene 25% de probabilidad de volver a generarse/activarse
        bool shouldShow = Random.value <= spawnChance;
        coinChild.SetActive(shouldShow);
    }
}
