using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMover : MonoBehaviour
{
    public GameObject parent;
    void Update()
    {
        if (parent != null)
        {
            transform.position = parent.transform.position;
        }
       
    }
}
