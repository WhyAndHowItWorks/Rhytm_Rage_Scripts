using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Базовый класс для оружия
/// </summary>
public abstract class Weapon : BattleParticipants
{
    // Содержит в себе все данные об оружии

    public int Damage;
    
    public AudioClip ShootSound;
    public GameObject DamageParticle;
    public Transform BurstSpawnPoint;
    public GameObject BurstP;
    public Router rt;

    public Column WeaponColumn;

    public int CurrentLevel 
    {
        get 
        {
            return currentLevel;
        }

        set {
            int oldlevel = currentLevel;
            if (Levels[currentLevel].IsActiveLevel)
            {
                Levels[currentLevel].DeactivateLevel();
            }
            currentLevel = value;
            Levels[CurrentLevel].ActivateLevel();
            if (currentLevel != LevelBegins.Count - 1)
            {
                WeaponColumn.MaxValue = LevelBegins[currentLevel + 1];
            }
            else { WeaponColumn.MaxValue = 999; }
            WeaponColumn.MinValue = LevelBegins[currentLevel];
            if (CurrentEnergy < LevelBegins[currentLevel])
            {
                CurrentEnergy = LevelBegins[currentLevel];
            }

            gameObject.GetComponent<SpriteRenderer>().sprite = Levels[currentLevel].WeaponForm;
            rt.ui.WeaponImage[WeaponSlot].sprite = Levels[currentLevel].WeaponForm;
            rt.ui.WeaponText[WeaponSlot].text = "LV " + (currentLevel+1).ToString();
            rt.rp.WeaponStartLevels[WeaponSlot] = currentLevel;
            if (oldlevel < currentLevel)
            {
                rt.ui.CastLvlEffect(WeaponSlot, true);
            }
            else if(oldlevel > currentLevel)
            {
                rt.ui.CastLvlEffect(WeaponSlot, false);
            }
        }
    }
    public int currentLevel;
    public List<int> LevelBegins = new List<int>();
    public List<WeaponLevel> Levels = new List<WeaponLevel>();

    public List<Sprite> WeaponShootImage = new List<Sprite>();
    public float CurrentEnergy 
    {
        get { return currentEnergy; }
        set 
        {
            currentEnergy = value;
            WeaponColumn.TempValue = value;
            if (currentEnergy > rt.cm.ColorColumns[WeaponSlot].MaxValue && currentLevel < Levels.Count - 1) 
            {
                CurrentLevel++;
            }
            else if (currentEnergy < rt.cm.ColorColumns[WeaponSlot].MinValue && CurrentLevel != 0)
            {
                CurrentLevel--;
            }
        }
    }
    public float currentEnergy;
    public int WeaponSlot;
    

    [Header("Метаданные")]
    public int MaxLevel;
    
    public virtual void ChangePlatform(int Platform)
    { 

    }
    public Enemy FindTargetEnemy()
    {
        if (rt.ws.ChosedPlatform == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if (rt.es.EnemiesOnPositions[i] != null)
                {
                    if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                    {
                        return rt.es.EnemiesOnPositions[i].GetComponent<Enemy>();

                    }

                }
            }

        }
        else
        {
            for (int i = 0; i < 8; i++)
            {
                if (rt.es.EnemiesOnPositions[i] != null)
                {
                    if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                    {
                        return rt.es.EnemiesOnPositions[i].GetComponent<Enemy>();

                    }

                }
            }
        }
        return null;
    }

    public void DoDamageToEnemy(Enemy en, float Damage)
    {
        en.GetComponent<Enemy>().Health -= Damage;
    }
    public void PistolDamage(float Damage) // Нанести повреждение противнику на прицеле
    {
        if (rt.ws.ChosedPlatform == 0)
        {
            
            for (int i = 0; i < 4; i++)
             {
                if (rt.es.EnemiesOnPositions[i] != null)
                {
                     if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                     {
                        DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                        break;
                     }

                }
             }
            
        }
        else
        {         
                for (int i = 4; i < 8; i++)
                {
                    if (rt.es.EnemiesOnPositions[i] != null)
                    {
                        if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                        {
                        DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                        break;
                        }
                    }
                }         
        }
        

    }
    /// <summary>
    /// Находит позицию цели, в которую прицелился игрок
    /// </summary>
    /// <returns></returns>
    
    
    public void ShootGunDoDamage(float Damage)
    {
        for (int i = 0; i < 4; i++)
        {
            if (rt.es.EnemiesOnPositions[i] != null)
            {
                if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                {
                    DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                    break;
                }
            }
        }
        for (int i = 4; i < 8; i++)
        {
            if (rt.es.EnemiesOnPositions[i] != null)
            {
                if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                {
                    DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                    break;
                }
            }
        }

    }
    public void LaserDoDamage(float Damage)
    {
        if (rt.ws.ChosedPlatform == 0) 
        {
            for (int i = 0; i < 4; i++)
            {
                
                if (rt.es.EnemiesOnPositions[i] != null)
                {
                    if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                    {
                        DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                    }
                }
                
            }
        }
        else
        {
            for (int i = 4; i < 8; i++)
            {
                if (rt.es.EnemiesOnPositions[i] != null)
                {
                    if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                    {
                        DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                    }
                }
                
            }
        }
    }
    public void RocketDoDamage(float Damage)
    {
        for (int i = 0; i < 8; i++)
        {

            if (rt.es.EnemiesOnPositions[i] != null)
            {
                if (!rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().IsInvisible)
                {
                    DoDamageToEnemy(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>(), Damage);
                }
            }

        }
    }

    public void CastShootEffect(GameObject Effect, Transform EffectSpawnPoint)
    {
        GameObject g = Instantiate(Effect, EffectSpawnPoint.position, EffectSpawnPoint.rotation);
        g.SetActive(true);
        Destroy(g, 1f);
    }
    public void CastShootEffect()
    {
        LookAt2D(BurstSpawnPoint.gameObject, rt.ws.WeapnonTargets[rt.ws.ChosedPlatform].position);
        GameObject g = Instantiate(BurstP, BurstSpawnPoint.position, BurstSpawnPoint.rotation);
        g.SetActive(true);
        g.GetComponent<ParticleSystem>().Play();
        Destroy(g, 1f);
    }
    public void SoundEffect()
    {
       
         rt.au.PlayOneShot(ShootSound); 
    }
    public void SoundEffect(AudioClip clip)
    {
        rt.au.PlayOneShot(clip);
    }

    public void SoundEffectForLongShoot(AudioClip clip)
    {
        if (rt.au.isPlaying == clip)
        {
            rt.au.clip = clip;
            rt.au.Play();
        }
        
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



