using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public Enemy Target;
    public bool Targeted;
    public RocketLaucher rl;

    public void Explode()
    {
        if (Targeted&&Target != null)
        {         
            rl.DoDamageToEnemy(Target, rl.AddsDamage);          
        }
            rl.RocketDoDamage(rl.Damage);
        
    }


}
