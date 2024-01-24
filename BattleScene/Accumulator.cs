using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accumulator : Enemy
{
    // CurrentNoteInfo
    public NoteForGame CurrentNote;
    public bool deltapr;

    // All For Charging/Discharging
    public int Charges
    {
        get { return charges; }
        set
        {
            int oldcharges = charges;

            charges = value;
            if (oldcharges != charges)
            {
                if (charges < oldcharges)
                {
                    CastModEffectWithPeriod(EffectType.ChargeDown, EnemyPlace);
                }
                else
                {
                    CastModEffectWithPeriod(EffectType.ChargeUp, EnemyPlace);
                }
                int chargesbars = Convert.ToInt32(Mathf.Round(charges / 5f));
                for (int i = 0; i < BatteryCharges.Count; i++)
                {
                    if (i < chargesbars)
                    {
                        BatteryCharges[i].SetActive(true);
                    }
                    else { BatteryCharges[i].SetActive(false); }
                }
            }

        }
    }
    public int charges;
    public float TimeBetweenNotes;
    public List<GameObject> BatteryCharges = new List<GameObject>();
    public bool IsDischarging;
    public float TimeBetweenDischarges;
    public float delta;

   

    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (rt.nt.IsGgFhase && !Pressed )
        {
            CurrentNote = nfg;
            if (nfg.noteInfo.type == NoteType.SliderDot)
            {
                DotsCheck();
            }
            else if (nfg.noteInfo.type == NoteType.Note)
            {
                Charge();
            }
        }
    }
    public override void DotsAction()
    {
        Charges += DamageMultiplayer;
    }
    public override void LoadOptions()
    {
        base.LoadOptions();
    }
    public new void Start()
    {
        IsDischarging = !rt.nt.IsGgFhase;
        base.Start();
        rt.nt.PhaseChangedEvent += OnSwitchFhase;
        TimeBetweenDischarges = rt.tl.Bpm / 240f;
    }
   
    public void Update()
    {
        if (IsDischarging && Charges > 0)
        {
            if (!an.GetCurrentAnimatorStateInfo(0).IsName("Discharging"))
            {
                an.Play("Discharging");
            }
            delta += Time.deltaTime;
            if (delta >= TimeBetweenDischarges)
            {
                Discharge();
                delta = 0;
            }
        }
        else
        {
            if (an.GetCurrentAnimatorStateInfo(0).IsName("Discharging"))
            {
                an.SetTrigger("End_Discharging");
            }
        }
    }

    #region Accumulator Abilities
    public void Discharge()
    {
        bool[] LineClear = new bool[4];
        for (int i = 0; i < LineClear.Length; i++)
        {
            LineClear[i] = true;
        }
        rt.ntgs.GameSelectArea.GetComponent<BoxCollider2D>().size = new Vector2(rt.ntgs.GameSelectArea.GetComponent<BoxCollider2D>().size.x, rt.tl.NoteSpeed / 8);
        foreach (GameObject g in rt.ntgs.GameSelectArea.GetComponent<GameSelectArea>().NoteFound)
        {
            LineClear[g.GetComponent<NoteForGame>().noteInfo.Line] = false;
        }
        List<int> LineSelection = new List<int>();
        List<int> ColorSelection = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (LineClear[i])
            {
                LineSelection.Add(i);
            }
        }
        if (LineSelection.Count > 0)
        {
            for (int i = 0; i < 8; i++)
            {
                if (rt.es.EnemiesOnPositions[i] != null && rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().UseNotes)
                {
                    ColorSelection.Add(rt.es.EnemiesOnPositions[i].GetComponent<Enemy>().NoteColor);
                }
            }
            if (ColorSelection.Count > 0)
            {
                int Line = LineSelection[UnityEngine.Random.Range(0, LineSelection.Count)];
                int Color = ColorSelection[UnityEngine.Random.Range(0, ColorSelection.Count)];
                NoteType type = NoteType.Note;
                switch (Color)
                {
                    case (0):
                        Charges--;
                        break;
                    case (1):
                        Charges -= 2;
                        break;
                    case (2):
                        Charges -= 2;
                        break;
                    case (3):
                        Charges -= 4;
                        break;
                }
                float time = rt.ntgs.TimeSinceStart + (rt.ntgs.GameSelectArea.transform.position.y + 12 - rt.ntgs.TapPoint.position.y) / rt.tl.NoteSpeed;
                GameObject h = rt.ntgs.NoteSpawn(type, 999, Color, time, Line, 0);

                h.transform.position = new Vector3(h.transform.position.x, rt.ntgs.GameSelectArea.transform.position.y + 12, h.transform.position.z);
            }

        }
    }
    public void Charge()
    {
        an.Play("Charging");
        switch (CurrentNote.noteInfo.Color)
        {
            case (0):
                Charges += DamageMultiplayer;
                break;
            case (1):
                Charges += 2 * DamageMultiplayer;
                break;
            case (2):
                Charges += 2 * DamageMultiplayer;
                break;
            case (3):
                Charges += 4 * DamageMultiplayer;
                break;

        }
    }
    #endregion

    #region Case Methods
    public void OnSwitchFhase(bool IsGGPhase)
    {
        IsDischarging = !IsGGPhase;
        delta = 0;
    }
    public override void OnBirth()
    {
        base.OnBirth();
    }
    public override void OnDeath()
    {
        base.OnDeath();
        an.Play("Death");
    }

    public override void OnTakingDamage()
    {
        base.OnTakingDamage();
        PlayAnim("Taking FinalDamage", "Take FinalDamage");
    }
    #endregion

}
