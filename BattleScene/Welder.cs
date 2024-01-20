using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Welder : Enemy
{
    [Header("Параметры Сварщика")]
    public bool GasBaloonTaken;
    public bool Pressed;
    public NoteForGame nfg;
    public GameObject GasBaloon;
    public GameObject GasBaloonHandler;
    public GameObject tempGasBaloon;
   




    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {

        if (!rt.nt.IsGgFhase && nfg.noteInfo.Color == 3)
        {
            this.Pressed = Pressed;
            this.nfg = nfg;
            if (nfg.noteInfo.type == NoteType.Note)
            {
                Attack();
            }
            else if (nfg.noteInfo.type == NoteType.SliderDot)
            {

                DotsCheck();
            }
        }
       
    }
    public override void TakeDamage()
    {
        base.TakeDamage();
        PlayAnim("Taking Damage", "Take Damage");
    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");

    }

    public void Attack()
    {
        if (!GasBaloonTaken)
        {
            TakeGasBaloon();
        }
        else { ThrowGasBaloon(); }
    }
    public void TakeGasBaloon()
    {

        an.Play("Gas_Baloon_Take");
        GasBaloonTaken = true;
        tempGasBaloon = Instantiate(GasBaloon, GasBaloonHandler.transform.position,GasBaloonHandler.transform.rotation);
        tempGasBaloon.transform.SetParent(GasBaloonHandler.transform);
        Invoke("FireBaloon", 0.85f);

    }
    public void FireBaloon()
    {
        tempGasBaloon.GetComponent<Gas_Baloon>().IsActivated = true;
    }
    public void ThrowGasBaloon()
    {
        an.Play("Gas_Baloon_Throw");
        GasBaloonTaken = false;
        GasBaloonHandler.transform.DetachChildren();
        tempGasBaloon.AddComponent<ProjectileMover>();
        tempGasBaloon.GetComponent<ProjectileMover>().SetDirectionAndTime(GasBaloonHandler.transform.position, rt.pa.TargetPoint.position,0.3f,GasBaloonHandler);
        Destroy(tempGasBaloon, 0.3f);
        if (!Pressed)
        {
            DoDamageToPlayer(Damage,0.3f);
        }
    }
   
    public override void LoadOptions()
    {
        base.LoadOptions();

    }
    

    public override void DotsAction()
    {
        Attack();
    }
}
