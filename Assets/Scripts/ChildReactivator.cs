using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildReactivator : MonoBehaviour
{
    [SerializeField] private GameObject child;

    void OnEnable()
    {
        if (child != null)
        {
            // Forzamos al hijo a activarse cada vez que el padre despierta
            child.SetActive(true);
        }
    }
}
