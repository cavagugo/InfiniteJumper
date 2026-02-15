using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToSkip : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private SceneLoader sceneLoader;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sceneLoader.LoadSceneByName(sceneName);
        }
        else if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            sceneLoader.LoadSceneByName(sceneName);
        }
    }
}
