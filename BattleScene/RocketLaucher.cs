using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLaucher : Weapon
{
    
    public GameObject Rocket;
    public GameObject RocketSpawn;
    public GameObject PointToExplode;
   
    public NoteForGame nfg;
    public int AddsDamage;

    public void Start()
    {
        AddsDamage = rt.oh.so.RocketLauncerAddDamageToNote;
        Damage = rt.oh.so.WeaponDamage[3];
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        Levels[CurrentLevel].NoteAction(Pressed, nfg);
    }
    public override void DotsAction()
    {
    }
}
