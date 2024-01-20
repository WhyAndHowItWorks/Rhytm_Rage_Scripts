using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LoaderBox : MonoBehaviour
{
    public ParabolicProjectileMover mover;
    public float DestroyEffectLifeTime;
    public GameObject DestroyEffect;

    public GameObject AirEffect;

    public void ProjectileDestroy()
    {
        GameObject g = Instantiate(DestroyEffect, transform.position, transform.rotation);
        Destroy(g, DestroyEffectLifeTime);
    }
    public void DestroyToBuildBarrier()
    {
        
    }
}
