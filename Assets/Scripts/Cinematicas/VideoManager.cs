using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Configuración de Escena")]
    [Tooltip("Escribe el nombre exacto de la escena a la que quieres ir")]
    [SerializeField] private string nextSceneName;
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += LoadScene;
    }
    void LoadScene(VideoPlayer vp)
    {
        sceneLoader.LoadSceneByName(nextSceneName);
    }
}
