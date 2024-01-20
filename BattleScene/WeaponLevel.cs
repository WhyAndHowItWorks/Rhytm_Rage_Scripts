using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponLevel : BattleParticipants
{
    public Weapon mw;
    public Sprite WeaponForm;
    public bool IsActiveLevel;
    


    public override void DotsAction()
    {
        
    }
    public virtual void Start()
    {
        mw = GetComponent<Weapon>();
    }
    public virtual void ActivateLevel()
    { 
        IsActiveLevel = true;
    }
    public virtual void DeactivateLevel()
    {
        IsActiveLevel = false;
    }

}
