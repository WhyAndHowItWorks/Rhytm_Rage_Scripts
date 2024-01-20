using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyorist : Enemy
{

    public bool deltapr;
    public GameObject Projectile;
    public Transform LaunchPoint;

    public List<Sprite> ProjSkins = new List<Sprite>();

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 1)
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
        IsInvisible = true;
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
    }
    public void Shoot()
    {
        GameObject g = ThrowParabollicProjectileToGG(Projectile, 0.2f, LaunchPoint);
        g.GetComponent<SpriteRenderer>().sprite = ProjSkins[Random.Range(0, ProjSkins.Count)];
        PlayAnim("Conveyor", "Shoot");
    }
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking damage", "OnTakingDamage");
    }
}
