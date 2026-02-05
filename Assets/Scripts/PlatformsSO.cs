using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlatformsSO : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int platformLevel; //La rareza de la plataforma (Mientras más alto, más raro)

    public int PlatformLevel {  get { return platformLevel; } }
    public GameObject PlatformPrefab { get { return prefab; } }
}
