using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Router rt;
    public TextMeshProUGUI Phase; // Отображает фазу
    public Image[] WeaponImage = new Image[4];
    public Text[] WeaponText = new Text[4];

    public Image Shotgun_lv2_counter_image;
    public Text Shotgun_lv2_counter_text;

    public GameObject[] LvlUpEffect = new GameObject[4];
    public GameObject LvlDownEffect;
    public Transform[] EffectSpawnPoint = new Transform[4];

    public GameObject[] WeaponShootImages = new GameObject[4];

    public GameObject BlockPanel;

    public GameObject ChangePhaseEffectG;
    public GameObject ChangePhaseGGSparks;
    public GameObject ChangePhaseEnemySparks;
    public Transform ChangeFhaseSparksSpawnDot;

    public void ChangeFhaseEffect(bool IsGGPhase)
    {
        GameObject g = null;
        if (IsGGPhase)
        {
            ChangePhaseEffectG.GetComponent<Animator>().Play("GGPhase");
            g = Instantiate(ChangePhaseGGSparks, ChangeFhaseSparksSpawnDot);
        }
        else 
        {
            ChangePhaseEffectG.GetComponent<Animator>().Play("EnemyPhase");
            g = Instantiate(ChangePhaseEnemySparks, ChangeFhaseSparksSpawnDot);
        }

        
        Destroy(g, 2f);
    }

    public void CastLvlEffect(int Color, bool Up)
    {
        if (Up)
        {
            GameObject g = Instantiate(LvlUpEffect[Color], EffectSpawnPoint[Color].transform);
            Destroy(g, 1f);
        }
        else 
        {
            GameObject g = Instantiate(LvlDownEffect, EffectSpawnPoint[Color].transform);
            Destroy(g, 1f);
        }
    }
    public void CastShootEffect(Weapon w)
    {
        int i = Random.Range(0, w.WeaponShootImage.Count);
        WeaponShootImages[w.WeaponSlot].GetComponent<Image>().sprite = w.WeaponShootImage[i];

        Animator an = WeaponShootImages[w.WeaponSlot].GetComponent<Animator>();
        if (an.GetCurrentAnimatorStateInfo(0).IsName("Blink"))
        {
            an.SetTrigger("Blink");
        }
        else { an.Play("Blink"); }
    }

    public void Start()
    {
        rt.st.BattleStart += OnStartBattle;
        rt.st.BattleEnd += OnEndBattle;
    }
    public void OnDestroy()
    {
        rt.st.BattleStart -= OnStartBattle;
        rt.st.BattleEnd -= OnEndBattle;
    }
    public void OnStartBattle()
    {
        Phase.text = "Let's Go!";
    }
    public void OnEndBattle(bool IsWin)
    {
        if (IsWin)
        {
            Phase.text = "Level Complete!";
        }
        else 
        {
            Phase.text = "Game over bro!";
        }
    }
}
