using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Basic class for all classes that work with sliders
/// </summary>
public abstract class BattleParticipants : Actor
{
    [Header("Slider Actions")]
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
