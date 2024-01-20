using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalCutter : Enemy
{

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
                    PlayAnim("StartShooting");
                    LaserEffect.SetActive(true);
                    LookAt2D(LaserEffect, rt.pa.TargetPoint.position);


                }
                else
                {

                    an.SetTrigger("StopShooting");
                    LaserEffect.SetActive(false);
                }
            }
        }
    }
    public bool isShooting;
    public bool IsWending 
    {
        get { return isWending; }
        set {
            bool old = isWending;
            isWending = value;
            if (old != isWending)
            {
                if (isWending)
                {
                    deltaSparksEffect = Instantiate(SparksEffect, SparksEffectSpawnPoint);
                    an.Play("Welding");
                }
                else
                {
                    an.SetTrigger("StopRepairing");
                    if (deltaSparksEffect != null)
                    {
                        Destroy(deltaSparksEffect);
                    }
                }
            }
           
        }
    }
    public bool isWending;
    public float HealPerMiss;

    public SliderDot nfg;
    public bool Pressed;
    public GameObject Laser;

  

    public float MovingTime;
    

    [Header("Все для припаивания")]
    public GameObject SparksEffect;
    public Transform SparksEffectSpawnPoint;
    public GameObject deltaSparksEffect;

    [Header("Все для эффекта стрельбы")]

    public GameObject LaserEffect;



    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!IsInvisible)
        {
            if (!rt.nt.IsGgFhase) // Фаза противников
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
                            DoDamageToPlayer(Damage);
                        }
                    }
                }
            }
            else
            { // Фаза ГГ
                IsShooting = false;
                if (!Pressed)
                {
                    if (!IsWending)
                    {
                        if (rt.es.EnemyPositions[MyNumber].GetComponent<EnemyPosition>().TrashParts.Count == 0)
                        {
                            MoveToFreePosition();
                        }
                        else
                        {
                            StartWending();
                        }
                    }
                    else 
                    {
                        if (Health <= MaxHealth)
                        {
                            Health += HealPerMiss * DamageMultiplayer;
                        }
                        
                        
                    }
                   
                   

                }
            }
        }
        
    }
 
    public void MoveToFreePosition()
    {
        for (int i = 1; i < 8; i++)
        {
            if (i != 4)
            {

                if (rt.es.EnemiesOnPositions[i] == null && rt.es.EnemyPositions[i].GetComponent<EnemyPosition>().TrashParts.Count > 0)
                {
                    MoveToNewPosition(MovingTime, i);
                    Invoke("StartWending", MovingTime);
                    break;
                }

            }
        }

    }
    public void StartWending()
    {
        
        IsWending = true;
    }
    public void StopWending()
    {       
        IsWending=false;
    }

    public void PhaseChanged(bool IsGGPhase)
    {
        if (!IsGGPhase && IsWending)
        {       
            StopWending();
        }
        if (IsGGPhase)
        { IsShooting = false; }
    }
    public new void Start()
    {
        base.Start();
        
        rt.nt.SliderActionEvent += SliderCheck;
        rt.nt.PhaseChangedEvent += PhaseChanged;
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
        HealPerMiss = rt.oh.so.LaserCutterPlusHP;
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
   
    public override void TakeDamage()
    {
        base.TakeDamage();
        if (!IsWending)
        {
            PlayAnim("Taking damage", "TakeDamage");
        }
        
    }
    
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
        rt.nt.SliderActionEvent -= SliderCheck;
        rt.nt.PhaseChangedEvent -= PhaseChanged;
        if (deltaSparksEffect != null)
        {
            Destroy(deltaSparksEffect);
        }
    }
    public void Shoot()
    {
        PlayAnim("Shoot", "Shoot");
        GameObject g = Instantiate(LaserEffect, LaserEffect.transform.position, LaserEffect.transform.rotation);
        g.SetActive(true);
        LookAt2D(g, rt.pa.TargetPoint.position);
        Destroy(g, 0.2f);
    }
    public override void DotsAction()
    {
        if (!Pressed)
        {
            DoDamageToPlayer(Damage);
        }
        if (!IsShooting && !rt.nt.IsGgFhase && nfg.index != SliderDotType.End)
        {
            IsShooting = true;
        }
    }

}
