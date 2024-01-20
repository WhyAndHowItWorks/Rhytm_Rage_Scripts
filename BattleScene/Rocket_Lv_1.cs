using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Rocket_Lv_1 : WeaponLevel
{
    public RocketLaucher rl;

    public override void Start()
    {
        base.Start();
        rl = GetComponent<RocketLaucher>();
    }

    // Update is called once per frame
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (nfg.noteInfo.Color == 3 && Pressed && rl.rt.nt.IsGgFhase)
        {
            rl.nfg = nfg;

            if (nfg.noteInfo.type == NoteType.Note)
            {
                Shoot();
            }
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                DotsCheck();
            }

        }
    }
    public void Shoot()
    {
        mw.rt.ui.CastShootEffect(mw);
        rl.rt.pa.PlayAnim(PlayerAbilities.Shoot_twohand_short);

        GameObject g = Instantiate(rl.Rocket, rl.RocketSpawn.transform.position, rl.RocketSpawn.transform.rotation);
        g.GetComponent<Rocket>().rl = rl;

        Vector2 v1 = new Vector2(rl.RocketSpawn.transform.position.x, rl.RocketSpawn.transform.position.y);
        Vector2 v2 = new Vector2(rl.PointToExplode.transform.position.x, rl.PointToExplode.transform.position.y);

        rl.LookAt2D(g, rl.PointToExplode.transform.position);
        int FoundId = rl.rt.nam.FoundNoteWithTime(rl.nfg.noteInfo, 0.4f);
        
         

        if (FoundId != 99999)
        {
            rl.rt.nam.AttachModToNote(FoundId, new RocketProjectileAttachMode(), new object[] { g.GetComponent<Rocket>() });
            g.GetComponent<ProjectileMover>().StepPerSecond = (v2 - v1) / rl.rt.nam.TimeBetweenNotes(rl.nfg.noteInfo, FoundId);
        }
        else { Destroy(g); }



    }
    public override void DotsAction()
    {
        Shoot();
    }
   
}
