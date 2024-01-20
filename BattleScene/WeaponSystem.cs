using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
/// <summary>
/// Отвечает за работу с оружием
/// </summary>
public class WeaponSystem : Actor
{   // Объект текущего оружия (то что отображается в руках ГГ)
    public Weapon currentWeapon; // Текущее оружие
    public Weapon[] Active_weapon = new Weapon[4]; // Оружие в слотах

    public Transform[] WeapnonTargets = new Transform[2];
    // Взаимодействие с другими административными скриптами
    public Router rt;
    
    // Эффект камеры
    public float ShootZoom;

    // Прицеливание в противников
    public int TargetEnemy;
    public int ChosedPlatform;

    public Weapon CurrentWeapon // Свойства для обработки изменения текущего оружия
    {
        get { return currentWeapon; }
        set 
        {
            currentWeapon = value;       
        }
    }

    public bool IsActive { get; set; }
    public void OnStart()
    {
        for (int i = 0; i < Active_weapon.Length; i++)
        {
    
            Active_weapon[i].Damage = rt.oh.so.WeaponDamage[i];
            if (!rt.rp.IsCustomLevel)
            {
                for (int g = 0; g < Active_weapon.Length; g++)
                {
                    for (int j = 0; j < Active_weapon[g].Levels.Count; j++)
                    {
                        Active_weapon[g].Levels[j].Start();
                    }
                    
                }
                
                Active_weapon[i].CurrentLevel = rt.rp.WeaponStartLevels[i];
            }
            else { Active_weapon[i].CurrentLevel = 0; }

        }
        rt.nt.NoteActedEvent += NoteAction;
        rt.nt.PhaseChangedEvent += ChangePhaseAction;
        rt.st.BattleEnd += BattleWasEnd;
        IsActive = true;
    }
    public void Start()
    {
        
    }
    public void BattleWasEnd(bool IsWon)
    {
        IsActive = false;
        foreach (Weapon w in Active_weapon)
        {
            w.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg) // Маршрутизатор команд для оружия
    {
        if (IsActive)
        {
            if (rt.nt.IsGgFhase)
            {
                ChangeWeapon(Active_weapon[nfg.noteInfo.Color]);
            }
            Active_weapon[nfg.noteInfo.Color].NoteAction(Pressed, nfg);
        }
                
    }
    public void ChangeWeapon(Weapon w)// Отображение нужного оружия
    {
        if (currentWeapon != w)
        {
            if (currentWeapon != null)
            {
                currentWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            currentWeapon = w;
            currentWeapon.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
      
    }
    public void ChangePhaseAction(bool IsGgPhase)
    {
        if (!IsGgPhase)
        {
            foreach (Weapon w in Active_weapon)
            {
                w.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

   
   
  
    
    
    
   

}

