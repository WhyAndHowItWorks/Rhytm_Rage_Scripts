using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Novise_Pistol : Weapon
{
    
    public void Start()
    {
        Damage = rt.oh.so.WeaponDamage[0];
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        Levels[CurrentLevel].NoteAction(Pressed, nfg);   
    }
    public override void DotsAction()
    {
        
    }


}
