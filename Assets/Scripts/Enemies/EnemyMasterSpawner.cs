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
                // Variación vertical aleatoria entre -4 y 4 para que no siempre sea al centro
                spawnPos = new Vector3(lado * offsetLados, currentCameraY + Random.Range(-4f, 4f), 0);
                break;
        }

        currentEnemy = Instantiate(elegido.prefab, spawnPos, Quaternion.identity);
    }

    // --- MAGIA VISUAL ---
    private void OnDrawGizmosSelected()
    {
        // Solo dibujamos si existe una cámara principal para usar de referencia
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 camPos = cam.transform.position;

        // DIBUJAR LÍNEA DE SPAWN ARRIBA (VERDE)
        Gizmos.color = Color.green;
        Vector3 topLeft = new Vector3(-limiteHorizontal, camPos.y + offsetArriba, 0);
        Vector3 topRight = new Vector3(limiteHorizontal, camPos.y + offsetArriba, 0);
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawSphere(new Vector3(0, camPos.y + offsetArriba, 0), 0.5f); // Punto central

        // DIBUJAR LÍNEA DE SPAWN ABAJO (ROJA)
        Gizmos.color = Color.red;
        Vector3 botLeft = new Vector3(-limiteHorizontal, camPos.y - offsetAbajo, 0);
        Vector3 botRight = new Vector3(limiteHorizontal, camPos.y - offsetAbajo, 0);
        Gizmos.DrawLine(botLeft, botRight);

        // DIBUJAR PUNTOS LATERALES (AZULES)
        Gizmos.color = Color.cyan;
        // Izquierda
        Gizmos.DrawWireSphere(new Vector3(-offsetLados, camPos.y, 0), 1f);
        // Derecha
        Gizmos.DrawWireSphere(new Vector3(offsetLados, camPos.y, 0), 1f);

        // Conectar los puntos laterales con el centro para ver la distancia
        Gizmos.DrawLine(new Vector3(-offsetLados, camPos.y, 0), camPos);
        Gizmos.DrawLine(new Vector3(offsetLados, camPos.y, 0), camPos);
    }
}