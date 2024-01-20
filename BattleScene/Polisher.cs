using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Security.Cryptography;
using Ookii.Dialogs;

public class Polisher : Enemy
{
    public bool IsPolishing
    {
        get 
        {
            return isPolishing;
        }
        set 
        {
            bool pol = isPolishing;
            isPolishing = value;
            if (pol != isPolishing)
            {
                
                if (isPolishing)
                {
                    deltaPolishEffect = Instantiate(PolishingEffect, PolishingEffectSpawn.position, PolishingEffectSpawn.rotation);
                    PlayAnim("Start Polish");
                }
                else
                {
                    an.SetTrigger("EndPolish");
                    if (deltaPolishEffect != null)
                    {
                        Destroy(deltaPolishEffect);
                    }
                }
            }
            
        }
    }
    public bool isPolishing;
    
    
    public int Charges
    {
        get { return charges; }
        set {
            charges = value;
            Damage = StartDamage + charges * DamagePerCharge;
        }
    }
    public int charges;
    public int DamagePerCharge;
    public NoteForGame nfg;
    public bool Pressed;

    public GameObject Projectile;
    public Transform ProjSpawnDot;
    public float ProjTimeToFly;


    [Header("Эффект полировки")]
    public GameObject PolishingEffect;
    public GameObject deltaPolishEffect;
    public Transform PolishingEffectSpawn;

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (!IsInvisible)
        {
            if (!rt.nt.IsGgFhase) // Фаза противников
            {
                if (nfg.noteInfo.Color == 3)
                {
                    this.Pressed = Pressed;
                    if (nfg.noteInfo.type == NoteType.SliderDot)
                    {
                        this.nfg = (SliderDot)nfg;
                        
                        DotsCheck();
                    }
                    else if (nfg.noteInfo.type == NoteType.Note)
                    {
                        Shoot();
                    }
                }
            }
            else // Фаза ГГ
            {
                if (!Pressed) // Нота не была нажата
                {
                    if (IsPolishing) // Уже полирует
                    {
                        Charges++;
                              
                    }
                }

            }
        }
       
    }
    public new void Start()
    {
        
        rt = GameObject.Find("Маршрутизатор").GetComponent<Router>();
        LoadOptions();
        MaxHealth = health;
        rt.uie.CreateEnemyHealthBar(this);
        HealthBar.gameObject.SetActive(false);
        
        Invoke("un", 0.8f);
        Invoke("EnemyComesToBattle", StartInvisibleTime);
        rt.es.DoEnemyEvent(this, EnemyAction.Created);
        rt.nt.PhaseChangedEvent += PhaseChanged;
        if (rt.nt.IsGgFhase)
        {
            Invoke("StartPolishing", StartInvisibleTime + 0.1f);
        }
    }
    public void un()
    {
        IsInvisible = false;
    }
    public void Update()
    {
        if (rt.nt.IsGgFhase && !IsPolishing)
        {
            StartPolishing();
        }
        if (!rt.nt.IsGgFhase && IsPolishing)
        {
            StopPolishing();
        }
    }
    public void PhaseChanged(bool IsGGPhase)
    {
        if (IsGGPhase) // Наступила фаза ГГ
        {
            StartPolishing();
            Charges = 0;
        }
        else // Наступила фаза противников
        {
            if (!IsInvisible)
            {
                IsPolishing = false;
            }
            
        }
    }
    public override void OnDeath()
    {
        base.OnDeath();     
        an.Play("Death");
        if (deltaPolishEffect != null)
        {
            Destroy(deltaPolishEffect);
        }
    }
    public void StartPolishing()
    {
        if (!IsInvisible)
        {
            IsPolishing = true;        
        }
        
    }
    public void StopPolishing()
    {
        if (!IsInvisible)
        {
            IsPolishing = false;
        }
        
    }
    public override void LoadOptions() // Загрузка значений из OptionHandler
    {
        base.LoadOptions();
        DamagePerCharge = rt.oh.so.PolisherPlusDamage;
    }
    public override void DotsAction() // Действия при достижении нужного колво точек
    {
        Shoot();
    }
    public void Shoot() // Графическая и числовая состоявляющие атаки
    {
        // Проигрывание или повторение анимации атаки
        PlayAnim("Throw", "Shoot");
        Invoke("ShootPart", Pressed,0.3f);
       
    }
    public void ShootPart(bool Pressed)
    {
        // Запуск детали в ГГ
        ThrowParabollicProjectileToGG(Projectile, ProjTimeToFly, ProjSpawnDot);
        // Нанесение дамага если не прожал
        if (!Pressed)
        {
            DoDamageToPlayer(Damage, ProjTimeToFly);
        }
        
    }
    public override void TakeDamage()
    {
      base.TakeDamage();
      if (!IsPolishing)
      {
      PlayAnim("Taking damage", "Take Damage");
      }
        

    }
   
}
