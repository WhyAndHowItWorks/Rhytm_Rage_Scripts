using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Laserka : Weapon
{
    public Transform[] LaserPoints = new Transform[2];
    public GameObject LaserEffect;
    public GameObject LaserSpawnWhenIdle;
    public GameObject LaserSpawnWhenNotIdle;
    public GameObject LaserDeltaEffect;

    public bool[] SliderThere = new bool[4];

    public bool IsShoot // Игрок прожимает нужную клавишу
    {
        get { return isShoot; }
        set 
        {
            isShoot = value;
            if (isShoot)
            {
                Debug.Log("Запуск лазерки");
                rt.ui.WeaponShootImages[2].SetActive(true);
                PlayerStartShoot();
                
                if (rt.pa.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    LaserDeltaEffect = Instantiate(LaserEffect, LaserSpawnWhenIdle.transform.position,LaserSpawnWhenIdle.transform.rotation);
                    
                }
                else
                {
                    LaserDeltaEffect = Instantiate(LaserEffect, LaserSpawnWhenNotIdle.transform.position, LaserSpawnWhenNotIdle.transform.rotation);
                    
                }
            }
            else 
            {
                Debug.Log("Конец лазерки");
                rt.ui.WeaponShootImages[2].SetActive(false);
                PlayerStopShoot();
                Destroy(LaserDeltaEffect);
            }
        }
    }
    public bool isShoot;

    

    
    
    
    public int deltaImage;

    public void Start()
    {
        Damage = rt.oh.so.WeaponDamage[2];
        rt.nt.SliderActionEvent += SliderAction;
        rt.nt.PhaseChangedEvent += PhasedChangedAction;
        rt.st.BattleEnd += BattleWasEnded;
    }
    public void BattleWasEnded(bool IsWon)
    {
        IsShoot = false;
        enabled = false;
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        Levels[CurrentLevel].NoteAction(Pressed, nfg);
        if (nfg.noteInfo.Color == 2 && nfg.noteInfo.type == NoteType.Note)
        {
            LookAt2D(gameObject, LaserPoints[rt.ws.ChosedPlatform].position);
        }
    }
    // Динамическое отображение графики
    public void Update()
    {
        if (rt.nt.IsGgFhase)
        {
        
        if (IsShoot) // Лазерка смотрит на нужную платформу и пускает раз в 0.2f частицы
        {
                if (rt.pa.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    LaserDeltaEffect.transform.position = LaserSpawnWhenIdle.transform.position;
                }
                else 
                {
                    LaserDeltaEffect.transform.position = LaserSpawnWhenNotIdle.transform.position;
                }
            LookAt2D(gameObject, LaserPoints[rt.ws.ChosedPlatform].position);
            LookAt2D(LaserDeltaEffect, LaserPoints[rt.ws.ChosedPlatform].position);
        }

        bool Noone = true;
        for (int i = 0; i < 4; i++) // Смотрим, нужно ли стрелять
        {
            if (Input.GetKey(rt.oh.so.Controls[i]) && rt.nt.IsSlider[i] && rt.nt.sliderInfo[i].noteInfo.Color == 2)
            {
                Noone = false;
                if (!IsShoot)
                {
                    IsShoot = true;
                }
                return;
            }
        }
        if (Noone)
        {
            if (IsShoot)
            {
                IsShoot = false;
            }
        }
    }

    }
    public void PhasedChangedAction(bool IsGGPhase)
    {
        if (!IsGGPhase)
        {
            if (IsShoot)
            {
                IsShoot = false;
            }
        }
    }
    public void PlayerStartShoot()
    { 
        rt.pa.PlayAnim("Shooting_long");   
    }
    public void PlayerStopShoot()
    {
        rt.pa.animator.SetTrigger("Stop_Shoot");
    }
    
    public void SliderAction(bool StartOrEnd, NoteForGame nfg)
    {
        Debug.Log("Событие");
        if (StartOrEnd && nfg.noteInfo.Color == 2) // Если слайдер подходит
        {
            SliderThere[nfg.noteInfo.Line] = true;
        }
        if (!StartOrEnd && nfg.noteInfo.Color == 2)
        {
            SliderThere[nfg.noteInfo.Line] = false;
        }
    }
    public void CastLaser()
    {
        if (rt.nt.IsGgFhase)
        { 
        LookAt2D(LaserSpawnWhenIdle, LaserPoints[rt.ws.ChosedPlatform].position);
        GameObject g = Instantiate(LaserEffect, LaserSpawnWhenIdle.transform.position, LaserSpawnWhenIdle.transform.rotation);
        Destroy(g, 0.2f);
        }   
    }
  
    public override void DotsAction()
    {
    }

    public void WeaponImageDotCheck()
    {
        deltaImage++;
        if (deltaImage >= WeaponShootImage.Count)
        {
            deltaImage = 0;
        }
        rt.ui.WeaponShootImages[2].GetComponent<Image>().sprite = WeaponShootImage[deltaImage];
    }
    public void WeaponImageNoteCheck()
    {
        deltaImage++;
        if (deltaImage >= WeaponShootImage.Count)
        {
            deltaImage = 0;
        }
        rt.ui.WeaponShootImages[2].GetComponent<Image>().sprite = WeaponShootImage[deltaImage];
        if (!IsShoot)
        {
            rt.ui.WeaponShootImages[2].SetActive(true);
            Invoke("WeaponImageCheck2", 0.2f);
        }
    }
    public void WeaponImageCheck2()
    {
        if (!IsShoot)
        {
            rt.ui.WeaponShootImages[2].SetActive(false);
        }
    }
}
