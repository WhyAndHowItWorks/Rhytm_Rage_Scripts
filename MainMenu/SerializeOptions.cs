using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class SerializeOptions 
{
    // ������ ����������
    public KeyCode[] Controls = new KeyCode[4];
  
    //����� ��� ������
    public int[] EnemyDamage = new int[12]; // 0 Massive

    public int[] EnemyHp = new int[12];// 1 Massive

    // ������ ���� ��� ����������� // 2 Massive
    public int BarrierHp; // 0
    public int LaserCutterPlusHP;//1
    public int PolisherPlusDamage;//2
    public int RepairerPlusHP;//3

    // ��� ��� ����� //3 Massive
    public int BossHP; //0
    public int PistolDamage;//1
    public int ShotgunDamage;//2
    public int LaserDamage;//3
    public int BladeDamage;//4
    public int BossPlusShield;//5
    public int BossPlusDamage;//6

    //����� ��� ������
    public int[] WeaponDamage = new int[4];//4 Massive

    //������ ���� ��� ������ // 5 Massive
    public int HeroHP;//0
    public int ShotgunMaxLoad;//1
    public int ShotgunTimeToLoad;//2
    public int LaserShieldPerNote;//3
    public int RocketLauncerAddDamageToNote;//4


    //��������� ������������
    public float Delay;
    public float Volume;
    public float HitSoundVolume;

    public bool VerSins;
    public bool FullScreen;
    public bool DoorsAnim;


    //���������� ���������
    public int StoryLevel; // �� ����� ������ ����������� �����
    public int[] WeaponStartLevels = new int[4];
    public int Level_Unlocked; // ����� ��������� ������� ������
    public bool EndTitles_Unlocked;

    //��������� ���������
    public int CamSpeed;

}



