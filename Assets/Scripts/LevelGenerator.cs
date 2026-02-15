using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Configuración de Fin de Nivel")]
    [SerializeField] private Transform shipTransform;
    [SerializeField] private float stopSpawningMargin = 2f;
    private bool goalReached = false;

    [Header("Configuración de Densidad (Multijugador)")]
    [SerializeField] private int minPlatformsPerLevel = 1;
    [SerializeField] private int maxPlatformsPerLevel = 3;
    //El ancho de cada plataforma para evitar que se sobrepongan
    [SerializeField] private float platformWidth = 1.5f;

    [Header("Configuración del generador")]
    [SerializeField] private List<PlatformsSO> platformTypes = new List<PlatformsSO>();
    [SerializeField] private int numPlatformsToSpawn = 5;
    [SerializeField] private PlatformsSO firstPlatform;
    [SerializeField] private float maxDistanceFromCameraBeforeSpawn = 5f;
    [SerializeField] private float minDistanceFromPreviousPlatform = 1.5f;
    [SerializeField] private float maxDistanceFromPreviousPlatform = 2f;
    [SerializeField] private float removeDistanceBelowCamera = 5f;
    [SerializeField] private int maxPlatformID = 0;

    private PlatformsSO lastSpawnedPlatform;
    private List<PlatformsSO> platformPool = new List<PlatformsSO>();
    private List<PlatformsSO> activePlatforms = new List<PlatformsSO>();

    public int MaxPlatformID { get { return maxPlatformID; } set { maxPlatformID = value; } }
    void Start()
    {
        GameObject firstPlatGO = Instantiate(firstPlatform.PlatformPrefab, new Vector3(0, -3.3f, 0), Quaternion.identity);
        PlatformsSO firstPlatSO = Instantiate(firstPlatform);
        firstPlatSO.PlatformPrefab = firstPlatGO;
        lastSpawnedPlatform = firstPlatSO;
        activePlatforms.Add(firstPlatSO);

        platformTypes = Resources.LoadAll<PlatformsSO>("Platforms").ToList();

        InitializePool();
        SpawnPlatforms();
    }

    private void InitializePool()
    {
        for (int i = 0; i < platformTypes.Count; i++)
        {
            int amount = (i == 0) ? 20 : 10;
            for (int j = 0; j < amount; j++)
            {
                PlatformsSO plat = Instantiate(platformTypes[i]);
                GameObject platPrefab = Instantiate(plat.PlatformPrefab);
                plat.PlatformPrefab = platPrefab;
                platPrefab.SetActive(false);
                platformPool.Add(plat);
            }
        }
    }

    void Update()
    {
        //No hace nada si alcanzó la nave
        if (goalReached) return;

        if ((Camera.main.transform.position.y + maxDistanceFromCameraBeforeSpawn) > lastSpawnedPlatform.PlatformPrefab.transform.position.y)
        {
            SpawnPlatforms();
        }
        // Quitar plataformas si están muy por debajo de la cámara
        for (int i = activePlatforms.Count - 1; i >= 0; i--)
        {
            if (activePlatforms[i].PlatformPrefab.transform.position.y < Camera.main.transform.position.y - removeDistanceBelowCamera)
            {
                activePlatforms[i].PlatformPrefab.SetActive(false);
                platformPool.Add(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    private void SpawnPlatforms()
    {
        //No hace nada si alcanzó la nave
        if (goalReached) return;

        for (int i = 0; i < numPlatformsToSpawn; i++)
        {
            float distanceToNextLevel = UnityEngine.Random.Range(minDistanceFromPreviousPlatform, maxDistanceFromPreviousPlatform);
            float baseHeight = lastSpawnedPlatform.PlatformPrefab.transform.position.y + distanceToNextLevel;

            if (shipTransform != null && baseHeight >= (shipTransform.position.y - stopSpawningMargin))
            {
                goalReached = true;
                break;
            }

            int platformsInThisLevel = UnityEngine.Random.Range(minPlatformsPerLevel, maxPlatformsPerLevel + 1);

            // Creamos una lista de "carriles" disponibles para este piso
            // Dividimos el espacio total en 3 zonas: Izquierda, Centro, Derecha.
            List<int> availableLanes = new List<int> { 0, 1, 2 };

            for (int j = 0; j < platformsInThisLevel; j++)
            {
                if (availableLanes.Count == 0) break; // No hay más carriles para este piso

                PlatformsSO selectedPlatform = GetRandomPlatformFromPool();
                if (selectedPlatform == null) continue;

                //Elegimos un carril al azar y lo quitamos para que la siguiente plataforma use otro
                int laneIndex = UnityEngine.Random.Range(0, availableLanes.Count);
                int chosenLane = availableLanes[laneIndex];
                availableLanes.RemoveAt(laneIndex);

                //Calculamos la X basada en el carril elegido
                float xPos = CalculateXInLane(chosenLane);

                //Variación de altura individual más pronunciada (decimales)
                float individualHeightOffset = UnityEngine.Random.Range(-0.5f, 0.5f);
                Vector2 spawnPosition = new Vector2(xPos, baseHeight + individualHeightOffset);

                selectedPlatform.PlatformPrefab.transform.position = spawnPosition;
                selectedPlatform.PlatformPrefab.SetActive(true);

                if (selectedPlatform.ID == 0)
                {
                    PlatformCoin coinScript = selectedPlatform.PlatformPrefab.GetComponentInChildren<PlatformCoin>();
                    if (coinScript != null) coinScript.TryReactivateCoin();
                }

                lastSpawnedPlatform = selectedPlatform;
                activePlatforms.Add(selectedPlatform);
            }
        }
    }

    //Divide la pantalla en 3 áreas y devuelve una X aleatoria dentro de la elegida
    private float CalculateXInLane(int lane)
    {
        float fullWidth = GlobalVariables.xLimit * 2;
        float laneWidth = fullWidth / 3;

        // Punto de partida a la izquierda
        float startX = -GlobalVariables.xLimit + (lane * laneWidth);
        // Margen interno para que no queden pegadas al borde del carril
        float margin = platformWidth / 2;

        return UnityEngine.Random.Range(startX + margin, startX + laneWidth - margin);
    }

    private PlatformsSO GetRandomPlatformFromPool()
    {
        int attempts = 0;
        while (attempts < 10)
        {
            int selectedID = RandomizePlatform();
            var plat = platformPool.FirstOrDefault(p => p.ID == selectedID && p.ID <= maxPlatformID);
            if (plat != null)
            {
                platformPool.Remove(plat);
                return plat;
            }
            attempts++;
        }
        return null;
    }

    public int RandomizePlatform()
    {
        float totalWeight = platformTypes.Sum(p => 1f / p.PlatformRarity);
        float randomWeight = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        foreach (var platform in platformTypes)
        {
            cumulativeWeight += 1f / platform.PlatformRarity;
            if (randomWeight <= cumulativeWeight) return platform.ID;
        }
        return 0;
    }
}