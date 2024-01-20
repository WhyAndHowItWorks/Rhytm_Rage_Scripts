using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicProjectileMover : MonoBehaviour
{
    public Vector2 StartP;
    public Vector2 EndP;
    public Vector2 CasatP;

    public float StartTime;
    public float TimeToFly;
    bool IsMoving;
    float x;
    float y;
    float t;

    public GameObject DestroyEffect;
    public float DestroyEffectLifeTime;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving)
        {
            t = (Time.time - StartTime) / TimeToFly;
            if (t > 1)
            {
                IsMoving = false;
            }
            else 
            {
               
            x = (1-t)* (1 - t)*StartP.x + 2*(1-t)*t*CasatP.x+ t*t*EndP.x;
            y = (1 - t) * (1 - t) * StartP.y + 2 * (1 - t) * t * CasatP.y + t * t * EndP.y;
            transform.position = new Vector2(x, y);           
            }
           
        }
    }
    public void MoveStart(Vector2 Start, Vector2 End, Vector2 Casat, float TimeToFly)
    {
        IsMoving = true;
        StartP = Start;
        EndP = End;
        CasatP = Casat;
        StartTime = Time.time;
        this.TimeToFly = TimeToFly;
        Destroy(gameObject,TimeToFly+0.01f);
    }

    public void OnDestroy()
    {
        if (DestroyEffect != null)
        {
            GameObject g = Instantiate(DestroyEffect, transform.position, transform.rotation);
            Destroy(g, DestroyEffectLifeTime);
        }
    }

}
