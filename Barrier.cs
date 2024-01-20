using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : Enemy
{
    public GameObject BoxDestroyEffect;
    public override void DotsAction()
    {
        
    }
    public override void LoadOptions()
    {
        health = rt.oh.so.BarrierHp;
    }
    public override void OnDeath()
    {
        base.OnDeath();
        GameObject g = Instantiate(BoxDestroyEffect, transform.position, transform.rotation);
        Destroy(g);
    }
}
