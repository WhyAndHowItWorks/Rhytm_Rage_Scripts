using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Lv_2 : WeaponLevel
{
    public float OneShellReloadTime;
    
    public float deltaTime;
    public int MaxShells;
    public int deltaShells;
    public bool Acting;


    public new void Start()
    {
        base.Start();
        OneShellReloadTime = mw.rt.oh.so.ShotgunTimeToLoad/1000;
        MaxShells = mw.rt.oh.so.ShotgunMaxLoad;
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (mw.rt.nt.IsGgFhase && Pressed && nfg.noteInfo.Color == 1)
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
    public void Update()
    {
        if (IsActiveLevel)
        {
            if (deltaShells < MaxShells && !mw.rt.nt.IsGgFhase)
            {
                if (deltaTime > OneShellReloadTime)
                {
                    deltaTime = 0;
                    deltaShells++;
                    mw.rt.ui.Shotgun_lv2_counter_text.text = "x" + deltaShells.ToString();
                }
                else
                {
                    deltaTime += Time.deltaTime;
                    mw.rt.ui.Shotgun_lv2_counter_image.fillAmount = deltaTime / OneShellReloadTime;
                }
               
            }       
        }
        
    }
    public override void DeactivateLevel()
    {
        mw.rt.ui.Shotgun_lv2_counter_image.gameObject.SetActive(false);
        base.DeactivateLevel();       
    }
   
    public override void ActivateLevel()
    {
        mw.rt.ui.Shotgun_lv2_counter_image.gameObject.SetActive(true);
        base.ActivateLevel();
        deltaShells = 0;
        deltaTime = 0;   
    }
    public void ReloadShell()
    {
       
    }
    public void Shoot()
    {
        mw.rt.ui.CastShootEffect(mw);
        if (deltaShells >= 0)
        {
            for (int i = 0; i < deltaShells; i++)
            {
                mw.ShootGunDoDamage(mw.Damage);
                mw.CastShootEffect();
            }
            deltaShells = 0;
            mw.rt.ui.Shotgun_lv2_counter_text.text = "x" + deltaShells.ToString();
        }
        
        mw.ShootGunDoDamage(mw.Damage);
        mw.CastShootEffect();
        mw.SoundEffect();
        mw.rt.pa.animator.Play(PlayerAbilities.Shoot_twohand_short);
    }
    public override void DotsAction()
    {
        Shoot();
    }
}
