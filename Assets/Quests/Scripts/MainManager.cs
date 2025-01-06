using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager mainManager;
    public List<string> questNames = new();
    public List<string> discoveryNames = new();

    private void Awake()
    {
        if (mainManager != null) 
        {
            Destroy(gameObject);
        }

        mainManager = this;
        DontDestroyOnLoad(gameObject);
    }
}
