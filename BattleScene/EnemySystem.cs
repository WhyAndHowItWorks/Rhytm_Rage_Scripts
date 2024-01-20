
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// —истема противников дл€ их совместной работы
/// </summary>
public class EnemySystem : MonoBehaviour
{
    public GameObject[] EnemyPositions = new GameObject[8];
    public GameObject[] EnemiesOnPositions = new GameObject[8];
    public GameObject[] EnemiesPrefab = new GameObject[2];

    public Transform[] DotsForShootParabollic;
    public Transform[] DotsToBuildBarrierParabollic;

    public Router rt;
   
    // √оворит какие противники есть и как они атакуют (добавить свойства, при ображении к которым происходит сбор информации)
    public int KolvoEnemies;

    // —обыти€ этого класса
    public delegate void EnemyEvent(Enemy Enemy, EnemyAction e);
    public event EnemyEvent OnEnemyEvent;

    public void DoEnemyEvent(Enemy Enemy, EnemyAction e)
    {
        OnEnemyEvent(Enemy,e);
    }

    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            EnemyPositions[i].GetComponent<EnemyPosition>().MyNumber = i;
            EnemiesOnPositions[i] = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ”станавливает противника данного типа в данную позицию
    /// </summary>
    /// <param name="Plase"></param>
    /// <param name="Type"></param>
    /// <returns></returns>
    public GameObject SpawnEnemy(int Plase , int Type)
    {
        GameObject g = Instantiate(EnemiesPrefab[Type], EnemyPositions[Plase].transform.position, EnemyPositions[Plase].transform.rotation);
        EnemiesOnPositions[Plase] = g;
        g.GetComponent<Enemy>().rt = rt;
        g.GetComponent<Enemy>().MyNumber = Plase;
        return g;
    }
    public GameObject SpawnEnemy(GameObject Type, int Plase)
    {
        GameObject g = Instantiate(Type, EnemyPositions[Plase].transform.position, EnemyPositions[Plase].transform.rotation);
        EnemiesOnPositions[Plase] = g;
        g.GetComponent<Enemy>().rt = rt;
        g.GetComponent<Enemy>().MyNumber = Plase;
        return g;
    }
    /// <summary>
    /// ”станавливает противника данного типа на случайное пустое место
    /// </summary>
    /// <param name="Type"></param>
    /// <returns></returns>
    public GameObject SpawnEmemy (int Type)
    {
        int i =0;
        GatherEnemyInformation();
        if (KolvoEnemies < 6)
        {
            if (Type == 12)
            {
                i = 1;
                if (EnemiesOnPositions[1] != null)
                {
                    EnemiesOnPositions[1].GetComponent<Enemy>().Health = -100;
                }
            }
            else
            {
                List<int> values = new List<int>();
                for (int temp = 1; temp < 8; temp++)
                {
                    if (temp != 4)
                    {
                        if (EnemiesOnPositions[temp] == null)
                        {
                            values.Add(temp);
                        }
                    }
                }
                i = values[Random.Range(0, values.Count)];
            }
            
            

            GameObject g = Instantiate(EnemiesPrefab[Type], EnemyPositions[i].transform.position, EnemyPositions[i].transform.rotation);
            EnemiesOnPositions[i] = g;

            g.GetComponent<Enemy>().rt = rt;
            g.GetComponent<Enemy>().MyNumber = i;
            g.GetComponent<Enemy>().Index = Type;
            if (Type == 12 && !rt.rp.IsCustomLevel)
            {
                if (rt.rp.IsCustomLevel)
                {
                    g.transform.position = new Vector3(12, 2f, 3);
                }
                else { g.transform.position = new Vector3(12, 3.5f, 3); }
               
            }
           
            
            rt.nt.NoteActedEvent += g.GetComponent<Enemy>().NoteAction;
            return g;

        }
        else 
        {
            for (int h = 1; h < 8; h++)
            {
                
                if (EnemiesOnPositions[h] != null && EnemiesOnPositions[h].GetComponent<Enemy>().Index == Type)
                {
                    Enemy g = EnemiesOnPositions[h].GetComponent<Enemy>();
                    g.Health += g.StartHealth;
                    g.DamageMultiplayer++;
                    
                    g.HealthBar.StackCount = g.DamageMultiplayer;
                    g.CastModEffectWithPeriod(EffectType.DamageUp, g.MyNumber);
                    break;
                }
            }
            return null; }
        
    }

 

    public void GatherEnemyInformation()
    {
        KolvoEnemies = 0;
        // ѕровер€ем количество противников
        for (int i = 1; i < 8; i++)
        {
            if (i != 4)
            {
                if (EnemiesOnPositions[i] != null)
                {
                    KolvoEnemies++;
                }
            }
            
        }
    }

}
