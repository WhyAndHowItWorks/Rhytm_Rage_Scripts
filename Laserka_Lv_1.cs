using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laserka_Lv_1 : WeaponLevel
{
    public Laserka ls;
    public override void Start()
    {
        base.Start();
        ls = GetComponent<Laserka>();

    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (Pressed && nfg.noteInfo.Color == 2 && ls.rt.nt.IsGgFhase)
        {
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                    ls.LaserDoDamage(ls.Damage);
                ls.WeaponImageDotCheck();
            }
            if (nfg.noteInfo.type == NoteType.Note)
            {
                ls.WeaponImageNoteCheck();               
                ls.LaserDoDamage(ls.Damage);
                ls.CastLaser();
            }
        }
    }
}
