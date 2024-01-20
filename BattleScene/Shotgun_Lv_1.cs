using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun_Lv_1 : WeaponLevel
{
    
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
    public void Shoot()
    {
        mw.rt.ui.CastShootEffect(mw);
        mw.ShootGunDoDamage(mw.Damage);
        mw.CastShootEffect();
        mw.SoundEffect();
        mw.rt.pa.PlayAnim(PlayerAbilities.Shoot_twohand_short);
    }
    public override void DotsAction()
    {
        Shoot();
    }
}
