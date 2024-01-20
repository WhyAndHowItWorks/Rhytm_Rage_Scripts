using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Базовый класс для всех противников
/// </summary>

public abstract class Enemy : BattleParticipants
{
    [Header("Метауровень")]
    public int Index;
    public int NoteColor;
    public bool UseNotes;
    [Header("Админы")]
    public Router rt;
    public Animator an;

    [Header ("Общие для противников параметры")]

    public GameObject DeathEffect;
    public Transform DeathEffectSpawnPoint;
    public float DeathEffectLifeTime;

    public GameObject TakeDamageEffect;
    public Transform TakeDamageEffectSpawnPoint;
    public float TakeDamageEffectLifeTime;

    public float Damage 
    {
        get { return damage * DamageMultiplayer; }
        set {
            float olddamage = damage;
            damage = value;
            if (damage > olddamage)
            {
                CastModEffectWithPeriod(EffectType.DamageUp, MyNumber);
            }
            if (damage < olddamage)
            {
                CastModEffectWithPeriod(EffectType.DamageDown, MyNumber);
            }
        } 
    }
    public int DamageMultiplayer = 1;
    public float damage;

    public float StartDamage;
    public float StartHealth;

    public Sprite[] TrashParts = new Sprite[3];
    public GameObject TrashPart;

    public float Shield
    {
        get { return shield; }
        set { 
            shield = value;           
            HealthBar.Shield_health = shield;        
        }
    }
    public float shield;
    
    public float Health
    {
        get { return health; }
        set
        {
            float delta = value - health;
            if (delta < 0)
            {
                TakeDamage();
                if (Shield > 0)
                {
                    Shielded();
                    if (-delta <= Shield) // Щит полностью покрывает урон
                    {
                        Shield += delta;
                        value = health;
                    }
                    else // Лишь частично
                    {
                        value += Shield;
                        Shield = 0;
                    }
                }
                
            }
            else
            {
                Heal();
            }
            rt.uie.TakeDamage(health - value, gameObject);

           
            
                health = value;
            

            if (HealthBar != null)
            {
                HealthBar.Health = health;
            }
           
            
            if (health <= 0)
            {
                rt.nt.NoteActedEvent -= NoteAction;
                rt.es.EnemiesOnPositions[MyNumber] = null;
                OnDeath();
            }
        }
    }
    public float health;

    public bool IsInvisible = true; // если true то противник не используется в расчетах урона.
    public float StartInvisibleTime;
    public float EndDeathTime;

    public int MyNumber 
    {
        get { return myNumber; }
        set {
            myNumber = value;
            if (HealthBar != null)
            {
                HealthBar.EnemyPosition = myNumber;
            }
            
        }
    }
    public int myNumber;
    public HealthBar HealthBar;
    
    public float MaxHealth { get; set; }

    
   
   public void Start()
    {
        rt = GameObject.Find("Маршрутизатор").GetComponent<Router>();
        LoadOptions();   
        MaxHealth = health;
        OnBirth();
        rt.es.DoEnemyEvent(this, EnemyAction.Created);
    }

    public virtual void LoadOptions()
    {
        StartDamage = rt.oh.so.EnemyDamage[Index];
        damage = StartDamage;
        StartHealth = rt.oh.so.EnemyHp[Index];
        health = StartHealth;
    }
    public virtual void OnBirth() // что делает он при появлении
    {
        rt.uie.CreateEnemyHealthBar(this);
        HealthBar.gameObject.SetActive(false);
        Invoke("UnInvisible", StartInvisibleTime);
        Invoke("EnemyComesToBattle", StartInvisibleTime);
        
    }
  
    public virtual void OnDeath() 
    {
        Destroy(gameObject, EndDeathTime);
        SpawnDeathEffect(DeathEffectSpawnPoint);
        ThrowTrashParts();
        rt.es.DoEnemyEvent(this, EnemyAction.Died);
        IsInvisible = true;
    }
    public void SpawnDeathEffect(Transform AttachTo)
    {
        GameObject g = Instantiate(DeathEffect, transform.position, transform.rotation);
        Destroy(g, DeathEffectLifeTime);
        g.AddComponent<EffectMover>();
        g.GetComponent<EffectMover>().parent = AttachTo.gameObject;
    }
    
    public virtual void TakeDamage()
    {
        CastEffect(TakeDamageEffect, TakeDamageEffectSpawnPoint.position, TakeDamageEffectLifeTime);
    }
    

    public virtual void Heal()
    {
        CastModEffectWithPeriod(EffectType.Heal, MyNumber);
    }
    public void UnInvisible()
    {
        IsInvisible = false;
       
    }
    public void EnemyComesToBattle()
    {
        HealthBar.gameObject.SetActive(true);
        EnemyArrived();

    }
    public virtual void EnemyArrived()
    { 
    }

    public void OnDestroy()
    {
        if (HealthBar != null)
        {
            Destroy(HealthBar.gameObject);
        }
        
    }
    public virtual void Shielded() // Эффект щита
    {
        
    }
    public void DoDamageToPlayer(float Damage,float Time)
    {
        Invoke("DoDamageToPlayer", Damage, Time);
    }
    public void DoDamageToPlayer(float Damage)
    {
        rt.pa.Health -= Damage;
    }
    public virtual void ThrowTrashParts()
    {
        for (int i = 0; i < TrashParts.Length; i++)
        {
            GameObject g = Instantiate(TrashPart, transform.position + new Vector3(0, 0, -i), transform.rotation);
            g.GetComponent<SpriteRenderer>().sprite = TrashParts[i];
            g.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-8, 8), 30));
            rt.es.EnemyPositions[MyNumber].GetComponent<EnemyPosition>().TrashParts.Add(g);
        }
    }
    public void MoveToNewPosition(float MoveTime, int NewPosition)
    {

        if (rt.es.EnemiesOnPositions[NewPosition] == null)
        {
            IsInvisible = true;
            Invoke("UnInvisible", MoveTime);
            gameObject.AddComponent<EnemyMover>();
            gameObject.GetComponent<EnemyMover>().SetDirectionAndTime(rt.es.EnemyPositions[MyNumber].transform.position, rt.es.EnemyPositions[NewPosition].transform.position, MoveTime);
            rt.es.EnemiesOnPositions[MyNumber] = null;
            rt.es.EnemiesOnPositions[NewPosition] = gameObject;
            MyNumber = NewPosition;
        }
    }

    public GameObject ThrowProjectileToGG(GameObject proj, float TimeToReach, Transform StartPoint)
    {
        GameObject g = Instantiate(proj, StartPoint.position, StartPoint.rotation);
        g.GetComponent<ProjectileMover>().SetDirectionAndTime(StartPoint.position, rt.pa.TargetPoint.position, TimeToReach);
        return g;
    }
    public GameObject ThrowParabollicProjectileToGG(GameObject proj, float TimeToReach, Transform StartPoint)
    {
        GameObject g = Instantiate(proj, StartPoint.position, StartPoint.rotation);
        int arg;
        if (MyNumber < 4)
        {
            arg = 0;
        }
        else { arg = 1; }
        g.GetComponent<ParabolicProjectileMover>().MoveStart(StartPoint.position, rt.pa.TargetPoint.position, rt.es.DotsForShootParabollic[arg].position, TimeToReach);
        return g;
    }
    

    public void CastModEffectWithPeriod(EffectType et, int Place)
    {
        rt.em.CastEffectWithPeriod(et, Place);
    }
    public void CastModEffect(EffectType et, int Place)
    {
        rt.em.CastEffect(et, Place);
    }
    #region Каст косметических эффектов
    public GameObject CastEffect(GameObject Effect, Vector3 pos, float TimeToLive)
    {
        GameObject g = Instantiate(Effect, pos, Quaternion.Euler(0, 0, 0));
        Destroy(g,TimeToLive);
        return g;
    }
    public GameObject CastEffect(GameObject Effect, Vector3 pos)
    {
        GameObject g = Instantiate(Effect, pos, Quaternion.Euler(0, 0, 0));
        return g;
    }
    public GameObject CastEffect(GameObject Effect, Vector3 pos,Vector3 rot, float TimeToLive)
    {
        GameObject g = Instantiate(Effect, pos, Quaternion.Euler(rot.x, rot.y, rot.z));
        Destroy(g, TimeToLive);
        return g;
    }
    public GameObject CastEffect(GameObject Effect, Vector3 pos, Vector3 rot)
    {
        GameObject g = Instantiate(Effect, pos, Quaternion.Euler(rot.x, rot.y, rot.z));
        return g;
    }
    #endregion
    public virtual void PlayAnim(string AnimName,string TriggerName)
    {
        if (an.GetCurrentAnimatorStateInfo(0).IsName(AnimName))
        {
            an.SetTrigger(TriggerName);
        }
        else { an.Play(AnimName); }
    }
    public virtual void PlayAnim(string AnimName)
    {
        an.Play(AnimName);
    }
    public void Invoke(string method, object options, float delay)
    {
        StartCoroutine(_invoke(method, delay, options));
    }
    private  IEnumerator _invoke( string method, float delay, params object[] obj)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        Type instance = GetType();

        MethodInfo[] mth = instance.GetMethods();
        List<MethodInfo> mthd = new List<MethodInfo>();

        for (int i = 0; i < mth.Length; i++)
        {
            if (mth[i].Name == method)
            {
                mthd.Add(mth[i]);
            }
        }

        if (mthd.Count == 1)
        {
            mthd[0].Invoke(this, obj);
        }
        else
        {
            for (int i = 0; i < mthd.Count; i++)
            {
                ParameterInfo[] par = mthd[i].GetParameters();
                if (par.Length == obj.Length)
                {
                    bool IsCorrect = true;
                    for (int g = 0; g < par.Length; g++)
                    {
                        if (par[g].ParameterType != obj[g].GetType())
                        {
                            IsCorrect = false;
                            break;
                        }
                    }
                    if (IsCorrect)
                    {
                        mthd[i].Invoke(this, obj);
                    }
                }
            }
        }



        yield return null;
    }
    public void LookAt2D(GameObject g, Vector3 lookTarget) // Позволяет направить объект в 2d сцене так, будто он смотрит на конкретную точку в мире
    {
        // the direction we want the X axis to face (from this object, towards the target)
        Vector3 xDirection = (lookTarget - transform.position).normalized;

        // Y axis is 90 degrees away from the X axis
        Vector3 yDirection = Quaternion.Euler(0, 0, 90) * xDirection;

        // Z should stay facing forward for 2D objects
        Vector3 zDirection = Vector3.forward;

        // apply the rotation to this object
        g.transform.rotation = Quaternion.LookRotation(zDirection, yDirection);
    }

}
