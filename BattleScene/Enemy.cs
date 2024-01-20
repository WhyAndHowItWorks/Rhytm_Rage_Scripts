using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Basic Class For All Enemies
/// </summary>

public abstract class Enemy : BattleParticipants
{
    [Header("MetaLevel")]
    public int Index;
    public int NoteColor;
    public bool UseNotes;
    [Header("Admins")]
    public Router rt;
    public Animator an;

    [Header ("Parameters for all enemies")]
    // Death values
    public GameObject DeathEffect;
    public Transform DeathEffectSpawnPoint;
    public float DeathEffectLifeTime;

    // Take Damage values
    public GameObject TakeDamageEffect;
    public Transform TakeDamageEffectSpawnPoint;
    public float TakeDamageEffectLifeTime;

    // Throwing TrashParts value
    public Sprite[] TrashParts = new Sprite[3];
    public GameObject TrashPart;

    // Invinsible values
    public bool IsInvisible = true; // if true Enemy isnt used for damage calculations
    public float StartInvisibleTime;
    public float EndDeathTime;

    // Damage
    public float FinalDamage 
    {
        get { return damage * DamageMultiplayer; }
        set {
            float olddamage = damage;
            damage = value;
            if (damage > olddamage)
            {
                CastModEffectWithPeriod(EffectType.DamageUp, EnemyPlace);
            }
            if (damage < olddamage)
            {
                CastModEffectWithPeriod(EffectType.DamageDown, EnemyPlace);
            }
        } 
    }
    public int DamageMultiplayer;
    public float damage;
    public float StartDamage;
    // Health
    public float Health
    {
        get { return health; }
        set
        {
            float delta = value - health;
            if (delta < 0)
            {
                OnTakingDamage();
                if (Shield > 0)
                {
                    OnShielded();
                    if (-delta <= Shield) // Shield consumes all damage
                    {
                        Shield += delta;
                        value = health;
                    }
                    else // Only part of Damage
                    {
                        value += Shield;
                        Shield = 0;
                    }
                }
                
            }
            else
            {
                OnHealing();
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
                rt.es.EnemiesOnPositions[EnemyPlace] = null;
                OnDeath();
            }
        }
    }
    public float health;
    public HealthBar HealthBar;
    public float MaxHealth { get; set; }
    public float StartHealth;
    //Shield
    public float Shield
    {
        get { return shield; }
        set
        {
            shield = value;
            HealthBar.Shield_health = shield;
        }
    }
    public float shield;
    // EnemyPlace
    public int EnemyPlace 
    {
        get { return enemyPlace; }
        set {
            enemyPlace = value;
            if (HealthBar != null)
            {
                HealthBar.EnemyPosition = enemyPlace;
            }
            
        }
    }
    public int enemyPlace;
   


   public void Start()
    {
        rt = GameObject.Find("Маршрутизатор").GetComponent<Router>();
        LoadOptions();   
        MaxHealth = health;
        OnBirth();
        rt.es.DoEnemyEvent(this, EnemyAction.Created);
    }

    /// <summary>
    /// Load Options To Enemy When he come to battlefield
    /// </summary>
    public virtual void LoadOptions()
    {
        StartDamage = rt.oh.so.EnemyDamage[Index];
        damage = StartDamage;
        StartHealth = rt.oh.so.EnemyHp[Index];
        health = StartHealth;
    }
    #region Case Methods
    public virtual void OnShielded() // Эффект щита
    {

    }
    public virtual void OnBirth() // что делает он при появлении
    {
        rt.uie.CreateEnemyHealthBar(this);
        HealthBar.gameObject.SetActive(false);
        Invoke("UnInvisible", StartInvisibleTime);
        Invoke("OnEnemyComesToBattle", StartInvisibleTime);

    }

    public virtual void OnDeath()
    {
        Destroy(gameObject, EndDeathTime);
        SpawnDeathEffect(DeathEffectSpawnPoint);
        ThrowTrashParts();
        rt.es.DoEnemyEvent(this, EnemyAction.Died);
        IsInvisible = true;
    }
    public void OnEnemyComesToBattle()
    {
        HealthBar.gameObject.SetActive(true);
        OnEnemyArrived();
    }
    public virtual void OnEnemyArrived()
    {
    }

    public void OnDestroy()
    {
        if (HealthBar != null)
        {
            Destroy(HealthBar.gameObject);
        }
    }
    public virtual void OnTakingDamage()
    {
        CastEffect(TakeDamageEffect, TakeDamageEffectSpawnPoint.position, TakeDamageEffectLifeTime);
    }
    public virtual void OnHealing()
    {
        CastModEffectWithPeriod(EffectType.Heal, EnemyPlace);
    }
    #endregion
    #region Enemy Abilities
    public virtual void ThrowTrashParts()
    {
        for (int i = 0; i < TrashParts.Length; i++)
        {
            GameObject g = Instantiate(TrashPart, transform.position + new Vector3(0, 0, -i), transform.rotation);
            g.GetComponent<SpriteRenderer>().sprite = TrashParts[i];
            g.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-8, 8), 30));
            rt.es.EnemyPositions[EnemyPlace].GetComponent<EnemyPosition>().TrashParts.Add(g);
        }
    }
    public void MoveToNewPosition(float MoveTime, int NewPosition)
    {

        if (rt.es.EnemiesOnPositions[NewPosition] == null)
        {
            IsInvisible = true;
            Invoke("UnInvisible", MoveTime);
            gameObject.AddComponent<EnemyMover>();
            gameObject.GetComponent<EnemyMover>().SetDirectionAndTime(rt.es.EnemyPositions[EnemyPlace].transform.position, rt.es.EnemyPositions[NewPosition].transform.position, MoveTime);
            rt.es.EnemiesOnPositions[EnemyPlace] = null;
            rt.es.EnemiesOnPositions[NewPosition] = gameObject;
            EnemyPlace = NewPosition;
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
        if (EnemyPlace < 4)
        {
            arg = 0;
        }
        else { arg = 1; }
        g.GetComponent<ParabolicProjectileMover>().MoveStart(StartPoint.position, rt.pa.TargetPoint.position, rt.es.DotsForShootParabollic[arg].position, TimeToReach);
        return g;
    }
    public void SpawnDeathEffect(Transform AttachTo)
    {
        GameObject g = Instantiate(DeathEffect, transform.position, transform.rotation);
        Destroy(g, DeathEffectLifeTime);
        g.AddComponent<EffectMover>();
        g.GetComponent<EffectMover>().parent = AttachTo.gameObject;
    }

    public void UnInvisible()
    {
        IsInvisible = false;
    }
    #endregion
    #region Do Damage For Player
    public void DoDamageToPlayer(float Damage,float Time)
    {
        Invoke("DoDamageToPlayer", Damage, Time);
    }
    public void DoDamageToPlayer(float Damage)
    {
        rt.pa.Health -= Damage;
    }
    #endregion
    #region Cast Mod Effects
    public void CastModEffectWithPeriod(EffectType et, int Place)
    {
        rt.em.CastEffectWithPeriod(et, Place);
    }
    public void CastModEffect(EffectType et, int Place)
    {
        rt.em.CastEffect(et, Place);
    }
    # endregion
    #region Cast Visual Effects
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
    #region Animator methods
    public virtual void PlayAnim(string AnimName, string TriggerName)
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
    #endregion
    #region Usefull methods
    /// <summary>
    /// Using for Invoke methods with parametres after current time
    /// </summary>
    /// <param name="method"></param>
    /// <param name="options"></param>
    /// <param name="delay"></param>
    public void Invoke(string method, object options, float delay)
    {
        StartCoroutine(_invoke(method, delay, options));
    }
    private IEnumerator _invoke(string method, float delay, params object[] obj)
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

    #endregion

}
