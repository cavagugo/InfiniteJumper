using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorito : MonoBehaviour
{
    [Header("Referencia del Prefab")]
    public GameObject meteoritoPrefab;

    [Header("Configuración del Pool")]
    public int poolSize = 20;
    private List<GameObject> pool;

    [Header("Área de Spawn y Despawn")]
    public Vector2 tamañoArea = new Vector2(10, 10);

    [Header("Configuración de Tiempos")]
    public float intervaloMin = 0.5f;
    public float intervaloMax = 1.5f;

    [Header("Movimiento")]
    public float velocidadMin = 5f;
    public float velocidadMax = 10f;
    public Vector2 direccionDiagonal = new Vector2(-1, -1);

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(meteoritoPrefab);
            obj.SetActive(false);
            // IMPORTANTE: Los hacemos hijos para que su movimiento sea local al área
            obj.transform.SetParent(this.transform);
            pool.Add(obj);
        }

        Invoke("Spawn", Random.Range(intervaloMin, intervaloMax));
    }

    void Update()
    {
        float limiteIzquierdo = -tamañoArea.x / 2;
        float limiteInferior = -tamañoArea.y / 2;

        foreach (GameObject m in pool)
        {
            if (m.activeInHierarchy)
            {
                float vel = m.GetComponent<MeteoritoData>().velocidad;

                // Movimiento relativo al Spawner
                m.transform.localPosition += (Vector3)direccionDiagonal.normalized * vel * Time.deltaTime;

                // LÓGICA DE DESPAWN: Si cruza el borde izquierdo o el inferior
                if (m.transform.localPosition.x < limiteIzquierdo || m.transform.localPosition.y < limiteInferior)
                {
                    m.SetActive(false);
                }
            }
        }
    }

    void Spawn()
    {
        GameObject m = GetFromPool();
        if (m != null)
        {
            Vector3 puntoSpawnLocal = Vector3.zero;

            // Decidir borde: Superior (0) o Derecho (1)
            if (Random.value > 0.5f)
            {
                puntoSpawnLocal.x = Random.Range(-tamañoArea.x / 2, tamañoArea.x / 2);
                puntoSpawnLocal.y = tamañoArea.y / 2;
            }
            else
            {
                puntoSpawnLocal.x = tamañoArea.x / 2;
                puntoSpawnLocal.y = Random.Range(-tamañoArea.y / 2, tamañoArea.y / 2);
            }

            m.transform.localPosition = puntoSpawnLocal;

            MeteoritoData data = m.GetComponent<MeteoritoData>() ?? m.AddComponent<MeteoritoData>();
            data.velocidad = Random.Range(velocidadMin, velocidadMax);

            m.SetActive(true);
        }
        Invoke("Spawn", Random.Range(intervaloMin, intervaloMax));
    }

    GameObject GetFromPool()
    {
        return pool.Find(obj => !obj.activeInHierarchy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;

        // Bordes de DESPAWN (Azul claro: Izquierdo e Inferior)
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector3(-tamañoArea.x / 2, -tamañoArea.y / 2, 0), new Vector3(-tamañoArea.x / 2, tamañoArea.y / 2, 0));
        Gizmos.DrawLine(new Vector3(-tamañoArea.x / 2, -tamañoArea.y / 2, 0), new Vector3(tamañoArea.x / 2, -tamañoArea.y / 2, 0));

        // Bordes de SPAWN (Amarillo: Derecho y Superior)
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-tamañoArea.x / 2, tamañoArea.y / 2, 0), new Vector3(tamañoArea.x / 2, tamañoArea.y / 2, 0));
        Gizmos.DrawLine(new Vector3(tamañoArea.x / 2, -tamañoArea.y / 2, 0), new Vector3(tamañoArea.x / 2, tamañoArea.y / 2, 0));

        // Flecha de dirección
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Vector3.zero, (Vector3)direccionDiagonal.normalized * 2);
    }
}

public class MeteoritoData : MonoBehaviour
{
    public float velocidad;
}