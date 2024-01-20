using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
 
    public void Start()
    {
        Damage = rt.oh.so.WeaponDamage[1];
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {

        
        Levels[CurrentLevel].NoteAction(Pressed, nfg);

    }

    public override void DotsAction()
    {
       
    }
}
