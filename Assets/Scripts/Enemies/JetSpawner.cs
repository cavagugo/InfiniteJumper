using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetSpawner : MonoBehaviour
{
    [Header("Dificultad / Progresión")]
    [Tooltip("A qué altura (Y de la cámara) empiezan a salir los Jets")]
    [SerializeField] private float minHeightToStart = 10f;

    [Tooltip("Altura a la que Laika reemplaza a los Jets")]
    [SerializeField] private float heightToSwitchToLaika = 100f;

    [Header("Recursos")]
    [SerializeField] private GameObject warningPrefab;
    [SerializeField] private GameObject jetPrefab;
    [SerializeField] private GameObject laikaPrefab;

    [Header("Tiempos")]
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float spawnInterval = 5f;

    [Header("Posición Relativa a la CÁMARA")]
    [SerializeField] private float spawnXOffset = 10f;
    [SerializeField] private float minHeightOffset = -2f;
    [SerializeField] private float maxHeightOffset = 7f;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        StartCoroutine(AttackLoop());
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // 1. ESPERAR ALTURA INICIAL
            if (Camera.main.transform.position.y < minHeightToStart)
            {
                yield return new WaitForSeconds(1f);
                continue;
            }

            // 2. ESPERAR INTERVALO
            yield return new WaitForSeconds(spawnInterval);

            // 3. LANZAR ATAQUE
            if (playerTransform != null)
            {
                StartCoroutine(LaunchSequence());
            }
        }
    }

    IEnumerator LaunchSequence()
    {
        // Guardamos la altura de la cámara
        float currentCamY = Camera.main.transform.position.y;

        // Calculamos la altura del ataque
        float attackY = Random.Range(currentCamY + minHeightOffset, currentCamY + maxHeightOffset);

        bool comingFromRight = Random.value > 0.5f;
        int direction = comingFromRight ? -1 : 1;

        // Posiciones
        float warningX = comingFromRight ? (spawnXOffset - 2f) : (-spawnXOffset + 2f);
        Vector3 warningPos = new Vector3(warningX, attackY, 0);

        float jetX = comingFromRight ? spawnXOffset : -spawnXOffset;
        Vector3 jetSpawnPos = new Vector3(jetX, attackY, 0);

        // --- WARNING ---
        GameObject warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);

        yield return new WaitForSeconds(warningDuration);

        if (warning != null) Destroy(warning);

        // --- ELECCIÓN DE PREFAB ---
        // Por defecto usamos el jet
        GameObject prefabToUse = jetPrefab;

        // Si superamos la altura y asignaste a Laika en el Inspector, la cambiamos
        if (currentCamY >= heightToSwitchToLaika && laikaPrefab != null)
        {
            prefabToUse = laikaPrefab;
        }

        // --- SPAWN DEL ENEMIGO ---
        GameObject enemyToSpawn = Instantiate(prefabToUse, jetSpawnPos, Quaternion.identity);
        JetEnemy jetScript = enemyToSpawn.GetComponent<JetEnemy>();
        if (jetScript != null) jetScript.Setup(direction);
    }
}