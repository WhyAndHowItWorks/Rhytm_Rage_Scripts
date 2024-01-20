using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PartShaper : Enemy
{
    
    public bool deltapr;
    public GameObject Projectile;
    public Transform ProjectileSpawn;

    public List<Sprite> ProjSprites = new List<Sprite>();

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        deltapr = Pressed;
        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 0)
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
    public override void LoadOptions()
    {
        base.LoadOptions();
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
    }
    public void Shoot()
    {
        if (!deltapr)// Нанести урон
        {
            DoDamageToPlayer(FinalDamage, 0.4f);
        }
        GameObject g = ThrowProjectileToGG(Projectile, 0.4f, ProjectileSpawn);
        g.GetComponent<SpriteRenderer>().sprite = ProjSprites[Random.Range(0, ProjSprites.Count)];
        PlayAnim("Throws detail", "Shoot");
    }
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking damage", "Taking FinalDamage");
    }
}
