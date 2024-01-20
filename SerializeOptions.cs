using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class SerializeOptions 
{
    // Кнопки управления
    public KeyCode[] Controls = new KeyCode[4];
  
    //Общее для Врагов
    public int[] EnemyDamage = new int[12]; // 0 Massive

    public int[] EnemyHp = new int[12];// 1 Massive

    // Особые вещи для противников // 2 Massive
    public int BarrierHp; // 0
    public int LaserCutterPlusHP;//1
    public int PolisherPlusDamage;//2
    public int RepairerPlusHP;//3

    // Все для босса //3 Massive
    public int BossHP; //0
    public int PistolDamage;//1
    public int ShotgunDamage;//2
    public int LaserDamage;//3
    public int BladeDamage;//4
    public int BossPlusShield;//5
    public int BossPlusDamage;//6

    //Общее для оружий
    public int[] WeaponDamage = new int[4];//4 Massive

    //Особые вещи для оружий // 5 Massive
    public int HeroHP;//0
    public int ShotgunMaxLoad;//1
    public int ShotgunTimeToLoad;//2
    public int LaserShieldPerNote;//3
    public int RocketLauncerAddDamageToNote;//4


    //Настройки пользователя
    public float Delay;
    public float Volume;
    public float HitSoundVolume;

    public bool VerSins;
    public bool FullScreen;
    public bool DoorsAnim;


    //Сохранения прогресса
    public int StoryLevel; // На каком уровне остановился игрок
    public int[] WeaponStartLevels = new int[4];
    public int Level_Unlocked; // Какой последний уровень открыт
    public bool EndTitles_Unlocked;

    //Настройки редактора
    public int CamSpeed;

}



