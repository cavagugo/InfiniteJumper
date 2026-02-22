using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterToSkip : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private SceneLoader sceneLoader;
    private bool pressedOnce = false;
    void Update()
    {
        if (!pressedOnce)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                sceneLoader.LoadSceneByName(sceneName);
                pressedOnce = true;
            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                sceneLoader.LoadSceneByName(sceneName);
                pressedOnce = true;
            }
        }        
    }
}
