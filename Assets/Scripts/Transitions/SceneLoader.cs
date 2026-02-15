using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
    // Method to load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    // Optional: Method to load a scene by its build index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }


    public void ExitGame()
    {
        //Sale de la aplicación (solo build del juego)
        // Es ignorado en el Play Mode del editor de Unity
        Application.Quit();

        Debug.Log("Game is exiting");

        // Detiene el editor (para testeo)
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator LoadAsync(string scene)
    {
        TransitionLayer layer = TransitionManager.Instance.Layer;
        AsyncOperation aop = SceneManager.LoadSceneAsync(scene);
        aop.allowSceneActivation = false;

        // Transition: FADE-IN
        layer.Show(0.5f, 0f);
        //layer.Show(fadeInTime, fadeInDelay);

        while (aop.progress < 0.9f || !layer.IsDone)
            yield return null;

        aop.allowSceneActivation = true;
        // Transition: FADE-OUT
        layer.Hide(0.5f, 0.25f);
    }
}
