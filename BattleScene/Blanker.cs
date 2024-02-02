using UnityEngine;
using System;

public class Blanker : Enemy
{
    // CurrentAction Info
    public bool Pressed;
    //All for LaunchProjectile
    public GameObject Projectile;
    public Transform LaunchPoint;

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        this.Pressed = Pressed;
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 1)
        {
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                DotsCheck();
            }
            if (nfg.noteInfo.type == NoteType.Note)
            {
                Shoot();
            }
        }
    }
    public override void DotsAction()
    {
        Shoot();
    }
    public void Shoot()
    {
        ThrowParabollicProjectileToGG(Projectile, 0.25f, LaunchPoint);
        PlayAnim("Attack", "Shoot");
        if (!Pressed)
        {
            DoDamageToPlayer(FinalDamage, 0.25f);
        }
    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
        IsInvisible = true;
    }  
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking damage", "TakeDamage");
    }
    
}
