using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance;
    public static TransitionManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<TransitionManager>();
            return instance;
        }
    }
    public TransitionLayer Layer { get; private set; }

    void Awake()
    {
        Layer = GetComponentInChildren<TransitionLayer>();
    }
    void Start()
    {
        if (!Layer.IsDone)
        {
            Layer.HideImmediately();
        }
    }
}