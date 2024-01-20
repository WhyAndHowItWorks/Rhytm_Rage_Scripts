using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BossProjectileMod : Mod
{
    
    public float Damage;
    public GameObject Wave;
    public BOSS Boss;

    public override void StartAction()
    {
        
    }
    public override void UpdateAction()
    {
        
    }
    public override void EndAction(bool Pressed)
    {
        GameObject.Destroy(Wave);
        if (!Pressed)
        {
            Boss.DoDamageToPlayer(Damage);
        }
    }
    /// <summary>
    /// 0 - Босс 1 - Волна 2 - Дамаг 
    /// </summary>
    /// <param name="values"></param>
    public override void InsertValues(object[] values)
    {
        Boss = (BOSS)values[0];
        Wave = (GameObject)values[1];
        Damage = (float)values[2];
    }
}
