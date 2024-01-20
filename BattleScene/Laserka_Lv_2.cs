using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laserka_Lv_2 : WeaponLevel
{
    public int ShieldPerNote;
    public Laserka ls;
    public override void Start()
    {
        base.Start();
        ls = GetComponent<Laserka>();
        ShieldPerNote = mw.rt.oh.so.LaserShieldPerNote;
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (Pressed && nfg.noteInfo.Color == 2 && ls.rt.nt.IsGgFhase)
        {
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                DoDamage();
                ls.WeaponImageDotCheck();
            }
            if (nfg.noteInfo.type == NoteType.Note)
            {
                ls.WeaponImageNoteCheck();
                DoDamage();
                ls.CastLaser();
            }
        }
    }

    public void DoDamage()
    {
        ls.LaserDoDamage(ls.Damage);
        ls.rt.pa.Shield_Health += ShieldPerNote;
    }
}
