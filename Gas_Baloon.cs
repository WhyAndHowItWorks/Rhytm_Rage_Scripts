using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas_Baloon : MonoBehaviour
{
    public GameObject DestroyEffect;
    public GameObject FireEffect;
    public GameObject FireEffectSpawnPoint;
    public bool IsActivated

    {
        get { return  isActivated;  }
        set 
        {
            isActivated = value;
            if (isActivated)
            {
                Debug.Log("Подожжен");
                GameObject g = Instantiate(FireEffect, FireEffectSpawnPoint.transform);
                //Спавн потока искр из баллона
            }
        }
}
    bool isActivated;

    private void OnDestroy()
    {
        Debug.Log("БУМ");
        GameObject g = Instantiate(DestroyEffect, transform.position, transform.rotation);
        Destroy(g, 1.2f);
    }
}
