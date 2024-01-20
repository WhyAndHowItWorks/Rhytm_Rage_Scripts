using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IamUniq : MonoBehaviour
{
    public static IamUniq Instance;
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else 
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    
}
