using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : Enemy
{
    [Header ("Параметры Грузчика")]
    public int BarrierHP;
    public bool BoxTaken;
    public bool Pressed;
    public NoteForGame nfg;
    public GameObject Box;
    public GameObject BoxHandler;
    public GameObject tempBox;
    public GameObject Barrier;
    
    

   
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
        else
        {
            if (!Pressed && nfg.noteInfo.Color == 3)
            {
                if (!BoxTaken)
                {
                    TakeBox();
                }
                else { ThrowBoxToBuildBarrier(); }
               
            }
        }
    }
    public override void TakeDamage()
    {
        base.TakeDamage();
        PlayAnim("Taking damage", "TakeDamage");


    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
        
    }
   
    public void Attack()
    {
        if (!BoxTaken)
        {
            TakeBox();
        }
        else { ThrowBox(); }
    }
    public void TakeBox()
    {
        an.Play("Takes");
        BoxTaken = true;
       tempBox = Instantiate(Box, BoxHandler.transform.position,BoxHandler.transform.rotation);
        tempBox.transform.SetParent(BoxHandler.transform);
    }
    public void ThrowBox()
    {
        an.Play("Throws");
        BoxTaken = false;
        BoxHandler.transform.DetachChildren();
        ParabolicProjectileMover mover = tempBox.GetComponent<LoaderBox>().mover;
        tempBox.GetComponent<LoaderBox>().AirEffect.SetActive(true);
        mover.MoveStart(tempBox.transform.position, rt.pa.TargetPoint.position, rt.es.DotsForShootParabollic[0].position, 0.11f);
        tempBox.GetComponent<LoaderBox>().Invoke("ProjectileDestroy", 0.1f);
        if (!Pressed)
        {
            DoDamageToPlayer(Damage);
        }
    }
    public void ThrowBoxToBuildBarrier()
    {
        if (BoxTaken)
        {
            BoxTaken = false;
            if (rt.es.EnemiesOnPositions[0] == null)
            {             
                rt.es.EnemiesOnPositions[0] = tempBox;
                BoxHandler.transform.DetachChildren();

                tempBox.AddComponent<ProjectileMover>();
                tempBox.GetComponent<ProjectileMover>().SetDirectionAndTime(tempBox.transform.position, rt.es.EnemyPositions[0].transform.position, 0.1f);
               

               
                Invoke("BuildBarrier", 0,0.1f);

                tempBox = null;

            }
            else if (rt.es.EnemiesOnPositions[4] == null)
            {              
                rt.es.EnemiesOnPositions[4] = tempBox;
                BoxHandler.transform.DetachChildren();

                tempBox.AddComponent<ProjectileMover>();
                tempBox.GetComponent<ProjectileMover>().SetDirectionAndTime(tempBox.transform.position, rt.es.EnemyPositions[4].transform.position, 0.1f);
                Invoke("BuildBarrier",4, 0.1f);

                tempBox = null;
            }
        }
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
        BarrierHP = rt.oh.so.BarrierHp;
    }
    public void BuildBarrier(int arg)
    {
        rt.es.SpawnEnemy(Barrier, arg);
    }
    
    public override void DotsAction()
    {
        Attack();
    }
}
