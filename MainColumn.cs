using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainColumn : Column
{
    public Transform DotUpEffect;
    public Transform DotDownEffect;

    public GameObject UpEffect;
    public GameObject DownEffect;

    public GameObject Line;

    // public ParticleSystem ps;
    // private ParticleSystem.EmissionModule e;
    //private ParticleSystem.ShapeModule s;

    public override float TempValue 
    {
        get
        {
            return base.TempValue;
        }
        set 
        {          
            if (tempValue >= MaxValue)
            {
                Debug.Log("¬верх");
                GameObject g = Instantiate(UpEffect, DotUpEffect.position, DotUpEffect.rotation);
                Destroy(g, 1f);
            }
            if (tempValue <= MinValue)
            {
                Debug.Log("¬низ");
                GameObject g = Instantiate(DownEffect, DotDownEffect.position, DotDownEffect.rotation);
                Destroy(g, 1f);
            }
            Line.transform.position = Vector3.Lerp(DotDownEffect.position, DotUpEffect.position, tempValue / MaxValue);


            // ps.emissionRate = 150*(tempValue / maxValue);

            //s = ps.shape;
            //s.position = new Vector3(0, 2.3744f, -3.32f + (3.32f * tempValue / maxValue));
            //s.scale = new Vector3(0.9f, 1.0625f, 6.8f * (tempValue / maxValue));
            base.TempValue = value;
        }
    }

}
