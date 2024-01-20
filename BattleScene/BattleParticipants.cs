using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleParticipants : Actor
{
    [Header("Работа со слайдерами")]
    public int DeltaDots;
    public int DotsToShoot;
    public void DotsCheck()
    {
       
            DeltaDots++;
            if (DeltaDots >= DotsToShoot)
            {
                DotsAction();
                DeltaDots = 0;
            }
        
    }
    public abstract void DotsAction();
    

}
