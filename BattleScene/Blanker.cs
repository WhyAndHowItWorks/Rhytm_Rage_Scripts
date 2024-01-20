using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

public class Blanker : Enemy
{

    public bool deltapr;
    public GameObject Projectile;
    public Transform LaunchPoint;

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        deltapr = Pressed;
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 1)
        {
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                
                DotsCheck();

            }
            if (nfg.noteInfo.type == NoteType.Note)
            {
                //Анимация выстрела
                Shoot();
            }
        }
    }
    public override void DotsAction()
    {
        //Анимация выстрела
        Shoot();
        
    }
    public override void OnBirth()
    {
        base.OnBirth();

    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
        IsInvisible = true;
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
    }
    public void Shoot()
    {
        ThrowParabollicProjectileToGG(Projectile, 0.25f,LaunchPoint);
        PlayAnim("Attack", "Shoot");
        if (!deltapr)// Нанести урон
        {
            DoDamageToPlayer(FinalDamage, 0.25f);
        }
    }
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking damage", "OnTakingDamage");
    }
    
}
