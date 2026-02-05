using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<PlatformsSO> platforms = new List<PlatformsSO>();
    //[SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private int _numPlatformsToSpawn = 5;
    [SerializeField] private GameObject _firstPlatform;
    [SerializeField] private float _maxDistanceFromCameraBeforeSpawn = 5f;
    [SerializeField] private float _minDistanceFromPreviousPlatform = 1.5f;
    [SerializeField] private float _maxDistanceFromPreviousPlatform = 3f;
    [SerializeField] private float _removeDistanceBelowCamera = 10f;
    [SerializeField] private int _initialPoolSize = 20;
    

    private GameObject _lastSpawnedPlatform;
    private Queue<GameObject> _platformPool = new Queue<GameObject>();
    private List<GameObject> _activePlatforms = new List<GameObject>();



    // Start is called before the first frame update
    void Start()
    {
        platforms = Resources.LoadAll<PlatformsSO>("Platforms").ToList();
        _lastSpawnedPlatform = _firstPlatform;
        _activePlatforms.Add(_firstPlatform);

        // Inicializar object pooler
        for (int i = 0; i < _initialPoolSize; i++)
        {           
            GameObject obj = Instantiate(RandomizePlatforms().PlatformPrefab);
            obj.SetActive(false);
            _platformPool.Enqueue(obj);
        }

        SpawnPlatforms();
    }

    // Update is called once per frame
    void Update()
    {
        // Spawnear nuevas plataformas si la cámara sube lo suficiente
        if ((Camera.main.transform.position.y + _maxDistanceFromCameraBeforeSpawn) > _lastSpawnedPlatform.transform.position.y)
        {
            SpawnPlatforms();
        }

        // Quitar plataformas si están muy por debajo de la cámara
        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            if (_activePlatforms[i].transform.position.y < Camera.main.transform.position.y - _removeDistanceBelowCamera)
            {
                _activePlatforms[i].SetActive(false);
                _platformPool.Enqueue(_activePlatforms[i]);
                _activePlatforms.RemoveAt(i);
            }
        }
    }

    private void SpawnPlatforms()
    {
        for (int i = 0; i < _numPlatformsToSpawn; i++)
        {
            if (_platformPool.Count == 0)
            {                
                Debug.LogWarning("Platform pooler está vacío.");
                return;
            }

            GameObject platform = _platformPool.Dequeue();
            platform.SetActive(true);

            float distanceToNextPlatform = UnityEngine.Random.Range(_minDistanceFromPreviousPlatform, _maxDistanceFromPreviousPlatform);
            float xPosition = UnityEngine.Random.Range(-GlobalVariables.xLimit, GlobalVariables.xLimit);

            Vector2 spawnPosition = new Vector2(xPosition, _lastSpawnedPlatform.transform.position.y + distanceToNextPlatform);
            platform.transform.position = spawnPosition;

            _lastSpawnedPlatform = platform;
            _activePlatforms.Add(platform);
        }
    }

    //Devuelve un tipo de plataforma basado en la rareza de la misma
    public PlatformsSO RandomizePlatforms()
    {
        float totalWeight = 0;

        foreach (var platform in platforms)
        {
            totalWeight += 1f / platform.PlatformLevel;
        }

        float randomWeight = UnityEngine.Random.Range(0, totalWeight);

        PlatformsSO selectedPlatform = null;
        float cumulativeWeight = 0;

        foreach (var platform in platforms)
        {
            cumulativeWeight += 1f / platform.PlatformLevel;
            if (randomWeight <= cumulativeWeight)
            {
                selectedPlatform = platform;
                break;
            }
        }

        return selectedPlatform;
    }
}