using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanic : Enemy
{
    public bool deltapr;
    public GameObject Projectile;
    public List<Sprite> ProjSprites = new List<Sprite>();
    public Transform ProjectileSpawnPoint;

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 0)
        {
            deltapr = Pressed;
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                
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
            DoDamageToPlayer(FinalDamage);
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
        GameObject g = ThrowParabollicProjectileToGG(Projectile, 0.2f, ProjectileSpawnPoint);
        g.GetComponent<SpriteRenderer>().sprite = ProjSprites[Random.Range(0, ProjSprites.Count)];
        PlayAnim("Shooting", "Shoot");
    }
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();

        PlayAnim("Taking damage", "TakingDamage");
    }
}
