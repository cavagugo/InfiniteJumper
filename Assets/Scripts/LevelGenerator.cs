using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<PlatformsSO> platformTypes = new List<PlatformsSO>();
    [SerializeField] private int numPlatformsToSpawn = 5;
    [SerializeField] private PlatformsSO firstPlatform;
    [SerializeField] private float maxDistanceFromCameraBeforeSpawn = 5f;
    [SerializeField] private float minDistanceFromPreviousPlatform = 1.5f;
    [SerializeField] private float maxDistanceFromPreviousPlatform = 3f;
    //5f funciona bien para quitar cualquier plataforma que la cámara deje de mostrar
    [SerializeField] private float removeDistanceBelowCamera = 5f;
    [SerializeField] private int maxPlatformID = 0;

    private PlatformsSO lastSpawnedPlatform;
    private Queue<PlatformsSO> platformPool = new Queue<PlatformsSO>();
    private List<PlatformsSO> activePlatforms = new List<PlatformsSO>();
    [SerializeField] private bool automaticPlatforms = true;

    // Start is called before the first frame update
    void Start()
    {
        // Crear instancia de la primera plataforma y su SO asociado
        GameObject firstPlatGO = Instantiate(firstPlatform.PlatformPrefab, new Vector3(0, -3.3f, 0), Quaternion.identity);
        PlatformsSO firstPlatSO = Instantiate(firstPlatform); // Crear copia del SO
        firstPlatSO.PlatformPrefab = firstPlatGO; // Asignar la instancia al SO
        lastSpawnedPlatform = firstPlatSO;
        activePlatforms.Add(firstPlatSO);

        if (automaticPlatforms)
        {
            platformTypes = Resources.LoadAll<PlatformsSO>("Platforms").ToList();
        }

        // Inicializar object pool
        InitializePool();

        SpawnPlatforms();
    }

    private void InitializePool()
    {
        for (int i = 0; i < platformTypes.Count; i++)
        {
            // Agrego 10 de plataformas básicas
            if (i == 0)
            {
                for (int j = 0; j < 10; j++)
                {
                    PlatformsSO plat = Instantiate(platformTypes[i]);
                    GameObject platPrefab = Instantiate(plat.PlatformPrefab);
                    plat.PlatformPrefab = platPrefab; // ASIGNAR LA INSTANCIA AL SO
                    platPrefab.SetActive(false);
                    platformPool.Enqueue(plat);
                }
            }
            // Agrego 5 de cada plataforma especial
            else
            {
                for (int j = 0; j < 5; j++)
                {
                    PlatformsSO plat = Instantiate(platformTypes[i]);
                    GameObject platPrefab = Instantiate(plat.PlatformPrefab);
                    plat.PlatformPrefab = platPrefab; // ASIGNAR LA INSTANCIA AL SO
                    platPrefab.SetActive(false);
                    platformPool.Enqueue(plat);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Spawnear nuevas plataformas si la cámara sube lo suficiente
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
                platformPool.Enqueue(activePlatforms[i]);
                activePlatforms.RemoveAt(i);
            }
        }
    }

    private void SpawnPlatforms()
    {
        int spawnedCount = 0;
        int maxAttemptsPerSpawn = 10; // Límite de intentos por selección para evitar loops infinitos

        for (int i = 0; i < numPlatformsToSpawn; i++)
        {
            PlatformsSO selectedPlatform = null;
            int attempts = 0;

            // Intentar seleccionar una plataforma basada en rareza y disponibilidad
            while (selectedPlatform == null && attempts < maxAttemptsPerSpawn)
            {
                // Primero, elegir un ID basado en rareza global
                int selectedID = RandomizePlatform();

                // Buscar en el pool una plataforma con ese ID y que cumpla ID <= maxPlatformID
                foreach (var platform in platformPool)
                {
                    if (platform.ID == selectedID && platform.ID <= maxPlatformID)
                    {
                        selectedPlatform = platform;
                        break; // Encontramos una coincidencia, salir del foreach
                    }
                }

                attempts++;
            }

            // Si no se encontró después de intentos, saltar esta selección
            if (selectedPlatform == null)
            {
                Debug.LogWarning($"No se pudo encontrar una plataforma con ID válido después de {maxAttemptsPerSpawn} intentos. Saltando selección.");
                continue;
            }

            // Remover del pool
            platformPool = new Queue<PlatformsSO>(platformPool.Where(p => p != selectedPlatform));

            // Activar y posicionar
            selectedPlatform.PlatformPrefab.SetActive(true);


            // Buscamos si la plataforma tiene el componente de moneda
            // Solo activar lógica de monedas si es la plataforma básica (ID 0) ---
            if (selectedPlatform.ID == 0)
            {
                PlatformCoin coinScript = selectedPlatform.PlatformPrefab.GetComponentInChildren<PlatformCoin>();
                if (coinScript != null)
                {
                    coinScript.TryReactivateCoin();
                }
            }

            float distanceToNextPlatform = UnityEngine.Random.Range(minDistanceFromPreviousPlatform, maxDistanceFromPreviousPlatform);
            float xPosition = UnityEngine.Random.Range(-GlobalVariables.xLimit, GlobalVariables.xLimit);
            Vector2 spawnPosition = new Vector2(xPosition, lastSpawnedPlatform.PlatformPrefab.transform.position.y + distanceToNextPlatform);
            selectedPlatform.PlatformPrefab.transform.position = spawnPosition;

            lastSpawnedPlatform = selectedPlatform;
            activePlatforms.Add(selectedPlatform);
            spawnedCount++;
        }

        // Log para verificar cuántas se spawnearon
        if (spawnedCount < numPlatformsToSpawn)
        {
            Debug.Log($"Se spawnearon {spawnedCount} plataformas en lugar de {numPlatformsToSpawn}.");
        }
    }

    // Devuelve un ID de plataforma basado en rareza global
    public int RandomizePlatform()
    {
        float totalWeight = 0;

        foreach (var platform in platformTypes)
        {
            totalWeight += 1f / platform.PlatformRarity;
        }

        float randomWeight = UnityEngine.Random.Range(0, totalWeight);

        PlatformsSO selectedPlatform = null;
        float cumulativeWeight = 0;

        foreach (var platform in platformTypes)
        {
            cumulativeWeight += 1f / platform.PlatformRarity;
            if (randomWeight <= cumulativeWeight)
            {
                selectedPlatform = platform;
                break;
            }
        }

        return selectedPlatform.ID;
    }
}