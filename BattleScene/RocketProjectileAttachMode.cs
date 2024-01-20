using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectileAttachMode : Mod
{
    public Rocket rc;
    public override void StartAction()
    {
        
    }
    public override void EndAction(bool Pressed)
    {
        rc.Explode();
        GameObject.Destroy(rc.gameObject);
    }
    public override void UpdateAction()
    {
        
    }
    public override void InsertValues(object[] values)
    {
      rc = (Rocket)values[0];
    }
}
