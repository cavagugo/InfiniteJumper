 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    
    //La que quieres/se va a editar
    public static float xLimit = 5f;
    public static float yLimit = 5f;
    public static bool gameOver = false;
    public static int coins;
    [Header("Límites horizontales del mapa")]
    public float inspectorXLimit = 5f;
    [Header("Límite vertical del mapa")]
    public float inspectorYLimit = 5000f;
    [Header("GameOver")]
    public bool inspectorGameOver = false;

    void Awake()
    {
        // Initialize the static variable from the Inspector value when the object awakes
        xLimit = inspectorXLimit;
        yLimit = inspectorYLimit;
        gameOver = inspectorGameOver;
    }

    void OnValidate()
    {
        //Se actualiza si cambia el valor
        xLimit = inspectorXLimit;
        yLimit = inspectorYLimit;
        gameOver = inspectorGameOver;
    }
}
