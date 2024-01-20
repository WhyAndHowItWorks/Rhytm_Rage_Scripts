using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Packer : Enemy
{
    [Header("Параметры упаковщика")]
    public bool deltapr;
    public GameObject Nail;
    public Transform NailSpawn;
    public GameObject NailStart;
    public Transform NailStartSpawn;
    
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 0)
        {
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                deltapr = Pressed;
                DotsCheck();
                
            }
            if (nfg.noteInfo.type == NoteType.Note)
            {
                //Анимация выстрела
                Shoot();
                if (!Pressed)// Нанести урон
                {
                    DoDamageToPlayer(FinalDamage);
                }
            }
           
        }
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
    }
    public override void DotsAction()
    {

        //Анимация выстрела
        Shoot();
        if (!deltapr)// Нанести урон
        {
            DoDamageToPlayer(FinalDamage,0.4f);
        }
    }
    public override void OnBirth()
    {
        base.OnBirth();
        
    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
    }
    
    public void Shoot()
    {
        CastEffect(NailStart, NailStartSpawn.position, 0.25f);
       GameObject g = ThrowProjectileToGG(Nail, 0.4f, NailSpawn);
        g.transform.rotation = Quaternion.Euler(0, 0, -90);
        PlayAnim("Shooting", "Shoot");
    }
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking FinalDamage", "TakingDamage");
    }

}
