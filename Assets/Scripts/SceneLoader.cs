using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneLoader : MonoBehaviour
{
    // Method to load a scene by its name
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // Optional: Method to load a scene by its build index
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ExitGame()
    {
        // Quits the application. This only works in a built game (e.g., .exe).
        // It is ignored in the Unity Editor's Play Mode.
        Application.Quit();

        // Optional: Add a debug log to confirm the function is called in the editor
        Debug.Log("Game is exiting");

        // Optional: Code to stop play mode in the Editor (for testing)
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
