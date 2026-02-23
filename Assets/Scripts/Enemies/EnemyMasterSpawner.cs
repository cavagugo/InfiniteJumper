using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMasterSpawner : MonoBehaviour
{
    public enum SpawnType { DesdeArriba, DesdeAbajo, DesdeLosLados }

    [System.Serializable]
    public struct EnemyConfig
    {
        public string nombre;
        public GameObject prefab;
        public SpawnType tipoDeSpawn;
        public float alturaMinima;

        [Header("Evolución Espacial (Opcional)")]
        public GameObject prefabAvanzado;
        public float alturaParaAvanzado;
    }

    [Header("Lista de Enemigos")]
    [SerializeField] private List<EnemyConfig> listaDeEnemigos;

    [Header("Tiempos de Aparición")]
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 5f;

    [Header("Configuración de Distancias")]
    [Tooltip("Línea VERDE: Dónde aparecen los que caen")]
    [SerializeField] private float offsetArriba = 10f;

    [Tooltip("Línea ROJA: Dónde aparecen los que suben")]
    [SerializeField] private float offsetAbajo = 10f;

    [Tooltip("Esferas AZULES: Dónde aparecen los laterales")]
    [SerializeField] private float offsetLados = 12f;

    [Tooltip("Ancho de la línea de spawn (Arriba/Abajo)")]
    [SerializeField] private float limiteHorizontal = 7f;

    // Variables internas
    private GameObject currentEnemy;
    private float timer;
    private float nextSpawnTime;

    void Start()
    {
        SetNextSpawnTime();
    }

    void Update()
    {
        if (currentEnemy != null) return;

        timer += Time.deltaTime;

        if (timer >= nextSpawnTime)
        {
            TrySpawnEnemy();
            timer = 0;
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void TrySpawnEnemy()
    {
        float currentCameraY = Camera.main.transform.position.y;

        List<EnemyConfig> enemigosDisponibles = new List<EnemyConfig>();

        foreach (EnemyConfig enemigo in listaDeEnemigos)
        {
            if (currentCameraY >= enemigo.alturaMinima)
            {
                enemigosDisponibles.Add(enemigo);
            }
        }

        if (enemigosDisponibles.Count == 0) return;

        EnemyConfig elegido = enemigosDisponibles[Random.Range(0, enemigosDisponibles.Count)];
        Vector3 spawnPos = Vector3.zero;

        switch (elegido.tipoDeSpawn)
        {
            case SpawnType.DesdeArriba:
                spawnPos = new Vector3(Random.Range(-limiteHorizontal, limiteHorizontal), currentCameraY + offsetArriba, 0);
                break;

            case SpawnType.DesdeAbajo:
                spawnPos = new Vector3(Random.Range(-limiteHorizontal, limiteHorizontal), currentCameraY - offsetAbajo, 0);
                break;

            case SpawnType.DesdeLosLados:
                float lado = (Random.value > 0.5f) ? 1f : -1f;
                spawnPos = new Vector3(lado * offsetLados, currentCameraY + Random.Range(-4f, 4f), 0);
                break;
        }

        // --- SISTEMA DE EVOLUCIÓN ---
        // Por defecto usamos el prefab normal
        GameObject prefabFinal = elegido.prefab;

        // Si configuraste un prefab avanzado y ya superamos la altura necesaria, lo cambiamos
        if (elegido.prefabAvanzado != null && currentCameraY >= elegido.alturaParaAvanzado)
        {
            prefabFinal = elegido.prefabAvanzado;
        }

        currentEnemy = Instantiate(prefabFinal, spawnPos, Quaternion.identity);
    }

    // --- MAGIA VISUAL ---
    private void OnDrawGizmosSelected()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 camPos = cam.transform.position;

        Gizmos.color = Color.green;
        Vector3 topLeft = new Vector3(-limiteHorizontal, camPos.y + offsetArriba, 0);
        Vector3 topRight = new Vector3(limiteHorizontal, camPos.y + offsetArriba, 0);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawSphere(new Vector3(0, camPos.y + offsetArriba, 0), 0.5f);

        Gizmos.color = Color.red;
        Vector3 botLeft = new Vector3(-limiteHorizontal, camPos.y - offsetAbajo, 0);
        Vector3 botRight = new Vector3(limiteHorizontal, camPos.y - offsetAbajo, 0);
        Gizmos.DrawLine(botLeft, botRight);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(new Vector3(-offsetLados, camPos.y, 0), 1f);
        Gizmos.DrawWireSphere(new Vector3(offsetLados, camPos.y, 0), 1f);

        Gizmos.DrawLine(new Vector3(-offsetLados, camPos.y, 0), camPos);
        Gizmos.DrawLine(new Vector3(offsetLados, camPos.y, 0), camPos);
    }
}