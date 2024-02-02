using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMark : Enemy
{

    public GameObject LaserEffect;
   
    public bool IsShooting
    {
        get { return isShooting; }
        set 
        {
            if (isShooting != value)
            {
                isShooting = value;
                if (isShooting)
                {
                    PlayAnim("StartShoot");
                    LaserEffect.SetActive(true);
                        LookAt2D(LaserEffect, rt.pa.TargetPoint.position);
                    
                   
                }
                else {
                   
                    an.SetTrigger("EndShoot");
                    LaserEffect.SetActive(false);
                }
            }
        }
    }

    public bool isShooting;
    public SliderDot nfg;
    public bool Pressed;
    public GameObject Laser;

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase)
        {
            if (nfg.noteInfo.Color == 2)
            {
                if (nfg.noteInfo.type == NoteType.SliderDot)
                {
                    
                    this.nfg = (SliderDot)nfg;
                    this.Pressed = Pressed;
                    DotsCheck();
                }
                else if (nfg.noteInfo.type == NoteType.Note)
                {
                    Shoot();
                    if (!Pressed)
                    {
                        DoDamageToPlayer(FinalDamage);
                    }
                }
            }
        }
        else { IsShooting = false; }  
    }
    public new void Start()
    {
        
        base.Start();
        if (rt != null)
        {
            rt.nt.SliderActionEvent += SliderCheck;
            rt.nt.PhaseChangedEvent += PhaseChanged;
        }
    }
    public void PhaseChanged(bool Phase)
    {
        if (Phase)
        {
            IsShooting = false;
        }
    }
    
    public override void LoadOptions()
    {
        base.LoadOptions();
    }

    public void SliderCheck(bool StartOrEnd, NoteForGame nfg)
    {
        if (!rt.nt.IsGgFhase)
        {
            if (!StartOrEnd)
            {

                for (int i = 0; i < 4; i++)
                {
                    if (rt.nt.IsSlider[i])
                    {

                        if (rt.nt.sliderInfo[i].noteInfo.Color == 2)
                        {
                            return;
                        }
                    }
                }
                
                IsShooting = false;
            }
            else
            {
                if (nfg.noteInfo.type == NoteType.Slider && nfg.noteInfo.Color == 2)
                {
                    IsShooting = true;
                }
            }
        }
    }
    
    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking Damage", "TakeDamage");
    }
    public override void OnDeath()
    {
        base.OnDeath();

        PlayAnim("Death");
        rt.nt.SliderActionEvent -= SliderCheck;
        rt.nt.PhaseChangedEvent -= PhaseChanged;
    }
    public void Shoot() // Выстрел на ноту
    {
        PlayAnim("Shooting", "Shoot");
        GameObject g = Instantiate(LaserEffect, LaserEffect.transform.position, LaserEffect.transform.rotation);
        g.SetActive(true);
        LookAt2D(g, rt.pa.TargetPoint.position);
        Destroy(g, 0.2f);
    }
    public override void DotsAction()
    {
        if (!Pressed)
        {
            DoDamageToPlayer(FinalDamage);
        }
        if (!IsShooting && !rt.nt.IsGgFhase && nfg.index != SliderDotType.End)
        {
            IsShooting = true;
        }
               
    }
}
