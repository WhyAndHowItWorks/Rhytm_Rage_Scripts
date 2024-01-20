using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol_Lv_1 : WeaponLevel
{
   
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {

        if (mw.rt.nt.IsGgFhase && nfg.noteInfo.Color == 0 && Pressed)
        {
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                DotsCheck();
            }
            else
            {
                Shoot();
            }

        }
    }
    public void Shoot()
    {
        mw.rt.ui.CastShootEffect(mw);
        mw.PistolDamage(mw.Damage*(mw.CurrentLevel+1));
        mw.CastShootEffect();
        mw.SoundEffect();
        mw.rt.pa.PlayAnim(PlayerAbilities.Shoot_onehand_short);
    }
    public override void DotsAction()
    {
        Shoot();
    }
}
