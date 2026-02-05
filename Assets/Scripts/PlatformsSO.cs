using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlatformsSO : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int platformRarity; //La rareza de la plataforma (Mientras más alto, más raro)
    [SerializeField] private int id;//ID del tipo de plataforma (no rareza)

    public int ID { get { return id; } }
    public int PlatformRarity {  get { return platformRarity; } }
    public GameObject PlatformPrefab { get { return prefab; } set { prefab = value; } }
}
