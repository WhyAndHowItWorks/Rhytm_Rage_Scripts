using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public float SliderDotMultiple;
    public float sliderDotdeltaCombo;

    public float SliderDotDeltaCombo
    {
        get { return sliderDotdeltaCombo; }
        set 
        {
            if (value > 1 || value < -1)
            {

                Combo += Mathf.FloorToInt(value);
                sliderDotdeltaCombo = value - Mathf.FloorToInt(value);
            }
            else {

                sliderDotdeltaCombo = value; }
        }
    }
    public int combo;

    public int NoNoteButTapMultiple;
    public int NoNoteButTapLoseEnergy;
    public int Combo 
    {
        get { return combo; }
        set
        {
            if (value >= 0)
            {
                combo = value;
                ComboChanged(combo);
            }
           
            Combo_Text.text =  combo.ToString()+"x";
            if (combo >= 50 && deltaFireEffect == null)
            {
                deltaFireEffect = Instantiate(FireEffect, DotForFireEffect.position, DotForFireEffect.rotation);
                deltaFireEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
            }
            if (combo < 50 && deltaFireEffect != null)
            {
                Destroy(deltaFireEffect);
            }
        }
    }
    public int NeededCombo;
    public Router rt;

    ///////////// Столбцы
    //Главный столбец
    public Column MainColumn;
    public Column[] ColorColumns = new Column[4];

    
    public float MinValueToAdd;
    public float MaxValueToAdd;
    public float MinValueToTake;
    public float MaxValueToTake;
    public float MainColumnEnergy
    {
        get { return MainColumn.tempValue; }
        set 
        {
            float delta = value - MainColumn.tempValue;
            if (delta > 0)  
            {
                if (delta > MaxValueToAdd)
                {
                    MainColumn.TempValue += MaxValueToAdd;
                }
                else if (delta < MinValueToAdd)
                {
                    MainColumn.TempValue += MinValueToAdd;
                }
                else { 
                    MainColumn.TempValue = value;
                }
                if (value > MainColumn.MaxValue)
                {
                    ChangeColumnEnergy(value - MainColumn.MaxValue);
                }

            }
            else
            {
                delta = MathF.Abs(delta);
                if (delta > MaxValueToTake)
                {
                    MainColumn.TempValue -= MaxValueToTake;
                }
                else if (delta < MinValueToTake)
                {
                    MainColumn.TempValue -= MinValueToTake;
                }
                else {
                   
                    MainColumn.TempValue = value; 
                }
                if (value < MainColumn.MinValue)
                {
                    ChangeColumnEnergy(value - MainColumn.MinValue);
                }
            }
        }
    }


    public int MainColumnMax;
    // Интерфейс
    public TextMeshProUGUI Combo_Text;
    public Transform DotForFireEffect;
    public GameObject deltaFireEffect;
    public GameObject FireEffect;
    public NoteForGame nfg;

    //События класса
    public delegate void ComboEvent(int Combo);
    public event ComboEvent ComboChanged;

    /// <summary>
    /// Была нажата нота
    /// </summary>
    public void NoteTap(NoteForGame nt)
    {
        if (nt.noteInfo.type == NoteType.Note)
        {
            Combo++;
        }
        else if (nt.noteInfo.type == NoteType.SliderDot)
        {
            SliderDotDeltaCombo += SliderDotMultiple;
        }
        
        float temp = 1 + 0.05f * (combo - NeededCombo);
       
        MainColumnEnergy += temp;

    }
    public void ChangeColumnEnergy(float Energy)
    {
        
        if (nfg != null)
        {
            rt.ws.Active_weapon[nfg.noteInfo.Color].CurrentEnergy += Energy;
           
        }
    }
    /// <summary>
    /// Нота не была нажата и исчезла
    /// </summary>
    public void NoteDontTap(NoteForGame nt)
    {
        if (nt.noteInfo.type == NoteType.Note)
        {
            if (combo / 10 > 1)
            {
                Combo -= Combo / 10;
            }
            else { Combo--; }
        }
        else if(nt.noteInfo.type == NoteType.SliderDot)
        {
            SliderDotDeltaCombo -= SliderDotMultiple;
        }
        float temp =  1 - 0.05f * (combo - NeededCombo);
       
        MainColumnEnergy -= temp;
       
    }
    /// <summary>
    /// Было нажатие, но не было нажато не одной ноты
    /// </summary>
    public void NoNoteButTap()
    {
        Combo -= NoNoteButTapMultiple;
        
        MainColumn.TempValue -= NoNoteButTapLoseEnergy;
        rt.pa.Health -= 10;
        nfg = null;
    }

    public void NoteAction(bool Pressed, NoteForGame nt)
    {
        nfg = nt;
        if (Pressed)
        {
            NoteTap(nt);
        }
        else 
        {
            NoteDontTap(nt);
        }
    }

    
    public void Start()
    {
        rt.nt.NoteActedEvent += NoteAction;
        MainColumn.MaxValue = MainColumnMax;
        MainColumn.MinValue = 0;
        MainColumn.TempValue = 50;

    }
}
