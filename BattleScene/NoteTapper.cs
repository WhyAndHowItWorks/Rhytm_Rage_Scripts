using System.Collections;
using System.Collections.Generic;
using System.IO;

using System.Xml;
using UnityEditor;

using UnityEngine;

public class NoteTapper : MonoBehaviour
{
    public bool IsActive;

    public List<GameObject>[] NoteLines = new List<GameObject>[4];

    public List<GameObject>[] NoteLines_sl = new List<GameObject>[4];

    public Router rt;

    public delegate void NoteActed(bool Pressed,NoteForGame note);
    public delegate void SliderAction(bool StartOrEnd, NoteForGame slider);
    public delegate void PhaseChanged(bool IsGGPhase);
    public event NoteActed NoteActedEvent;
    public event SliderAction SliderActionEvent;
    public event PhaseChanged PhaseChangedEvent;

    public SliderForGame[] sliderInfo = new SliderForGame[4];
    public bool[] IsSlider = new bool[4];

    public bool IsGgFhase;
    public bool Testing;

    [Header("Эффект для смены фазы")]
    public GameObject GGPhase;
    public GameObject EnemyPhase;
    public Transform PhaseSpawnPoint;

    [Header("Адаптация под скорость")]
    public Transform[] TapZones = new Transform[4];
    public Transform[] DeleteZones = new Transform[4];
    public float TapZoneSize
    {
        get { return tapZoneSize; }
        set 
        {
            tapZoneSize = value;
            for (int i = 0; i < TapZones.Length; i++)
            {
                TapZones[i].localScale = new Vector3(TapZones[i].localScale.x, tapZoneSize, 1);
                
                DeleteZones[i].localPosition = new Vector3(DeleteZones[i].localPosition.x,Mathf.Lerp(-9.5f,-11.2f, (value - MinSize) / (MaxSize - MinSize)), DeleteZones[i].localPosition.z);
            }
        }
    }
    public float tapZoneSize;

    public float MinSize;
    public float MinSpeed;
    public float MaxSize;
    public float MaxSpeed;
    public float CurrentSpeed 
    {
        get { return currentSpeed; }
        set 
        {
            currentSpeed = value;
            if (currentSpeed <= MinSpeed)
            {
                TapZoneSize = MinSize;
            }
            else if (currentSpeed >= MaxSpeed)
            {
                TapZoneSize = MaxSize;
            }
            else 
            {
                TapZoneSize = Mathf.Lerp(MinSize, MaxSize, (currentSpeed - MinSpeed) / (MaxSpeed - MinSpeed));
            }
        }
    }
    public float currentSpeed;



    public void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            NoteLines[i] = new List<GameObject>();
        }
        for (int i = 0; i < 4; i++)
        {
            NoteLines_sl[i] = new List<GameObject>();
        }
    }

    public void Start()
    {
        rt.st.BattleEnd += BattleEnd;
        rt.st.BattleStart += BattleStart;

        NoteActedEvent += Lol;
        SliderActionEvent = lol1;
        PhaseChangedEvent += lol2;
        
    }
    public void BattleStart()
    {
        IsActive = true;

    }
    
    public void BattleEnd(bool IsWin)
    {
        IsActive = false;
    }
    void Update()
    {
        if (IsActive && !rt.st.IsUnpausing) // Если ГГ не мертв и битва не окончена
        {
            #region Отработка нажатий обычных кнопок
            if (Input.GetKeyDown(rt.oh.so.Controls[0]))
            {

                if (NoteLines[0].Count != 0)
                {
                    GameObject g = NoteLines[0][0];
                    NoteLines[0].Remove(g);

                    DoNoteEvent(true, g.GetComponent<NoteForGame>());
                    Destroy(g);
                }
                else
                {
                    if (!IsSlider[0])
                    {
                        Debug.Log("Промах");
                        rt.cm.NoNoteButTap();
                    }
                }
            }
            if (Input.GetKeyDown(rt.oh.so.Controls[1]))
            {

                if (NoteLines[1].Count != 0)
                {
                    GameObject g = NoteLines[1][0];
                    NoteLines[1].Remove(g);

                    DoNoteEvent(true, g.GetComponent<NoteForGame>());
                    Destroy(g);
                }
                else {
                    if (!IsSlider[1])
                    {
                        Debug.Log("Промах");
                        rt.cm.NoNoteButTap();
                    }
                        }
            }
            if (Input.GetKeyDown(rt.oh.so.Controls[2]))
            {
                if (NoteLines[2].Count != 0)
                {
                    GameObject g = NoteLines[2][0];
                    NoteLines[2].Remove(g);

                    DoNoteEvent(true, g.GetComponent<NoteForGame>());
                    Destroy(g);
                }
                else
                {
                    if (!IsSlider[2])
                    {
                        Debug.Log("Промах");
                        rt.cm.NoNoteButTap();
                    }
                }
            }
            if (Input.GetKeyDown(rt.oh.so.Controls[3]))
            {
                if (NoteLines[3].Count != 0)
                {
                    GameObject g = NoteLines[3][0];
                    NoteLines[3].Remove(g);

                    DoNoteEvent(true, g.GetComponent<NoteForGame>());
                    Destroy(g);
                }
                else
                {

                    if (!IsSlider[3])
                    {
                        Debug.Log("Промах");
                        rt.cm.NoNoteButTap();
                    }
                }
            }
            #endregion

            #region Отработка точек слайдеров
            if (Input.GetKey(rt.oh.so.Controls[0]))
            {
                if (NoteLines_sl[0].Count != 0)
                {

                    GameObject g = NoteLines_sl[0][0];
                    NoteLines_sl[0].Remove(g);

                    DoNoteEvent(true, g.GetComponent<SliderDot>());
                    if (g.GetComponent<SliderDot>().index == SliderDotType.Begin)
                    {
                        g.GetComponent<SliderDot>().slider.DoEndActions(true);
                    }
                    Destroy(g);


                }
              
            }
            if (Input.GetKey(rt.oh.so.Controls[1]))
            {
                if (NoteLines_sl[1].Count != 0)
                {

                    GameObject g = NoteLines_sl[1][0];
                    NoteLines_sl[1].Remove(g);

                    DoNoteEvent(true, g.GetComponent<SliderDot>());
                    if (g.GetComponent<SliderDot>().index == SliderDotType.Begin)
                    {
                        g.GetComponent<SliderDot>().slider.DoEndActions(true);
                    }
                    Destroy(g);

                }
            }
            if (Input.GetKey(rt.oh.so.Controls[2]))
            {
                if (NoteLines_sl[2].Count != 0)
                {

                    GameObject g = NoteLines_sl[2][0];
                    NoteLines_sl[2].Remove(g);

                    DoNoteEvent(true, g.GetComponent<SliderDot>());
                    if (g.GetComponent<SliderDot>().index == SliderDotType.Begin)
                    {
                        g.GetComponent<SliderDot>().slider.DoEndActions(true);
                    }
                    Destroy(g);

                }
            }
            if (Input.GetKey(rt.oh.so.Controls[3]))
            {
                if (NoteLines_sl[3].Count != 0)
                {
                    
                        GameObject g = NoteLines_sl[3][0];
                        NoteLines_sl[3].Remove(g);

                    DoNoteEvent(true, g.GetComponent<SliderDot>());
                    if (g.GetComponent<SliderDot>().index == SliderDotType.Begin)
                    {
                        g.GetComponent<SliderDot>().slider.DoEndActions(true);
                    }
                    Destroy(g);
                    
                }
            }
            #endregion
        }

    }

    public void TriggerNote(int Type) // Действие для ноты триггера
    {
        if (IsActive)
        {
        switch (Type)
        {
            case 0:
                break;
            case 1:
                SwitchFhase();
                break;
        }
    }
    }
    /// <summary>
    /// Смена фазы в бою
    /// </summary>
    public void SwitchFhase() // 
    {
        IsGgFhase = !IsGgFhase;
        PhaseChangedEvent(IsGgFhase);
        rt.ui.ChangeFhaseEffect(IsGgFhase);
        if (IsGgFhase)
        {
            rt.ui.Phase.text = "GG Phase";
            
        }
        else
        {
            rt.ui.Phase.text = "Enemy Phase";
        }
    }
    /// <summary>
    /// Вызов события активации ноты
    /// </summary>
    /// <param name="Pressed"></param>
    /// <param name="nfg"></param>
    public void DoNoteEvent(bool Pressed, NoteForGame nfg)
    {
        if (IsActive)
        { 
        NoteActedEvent(Pressed, nfg);
            nfg.DoEndActions(Pressed);
    }
        
    }
    /// <summary>
    /// Вызов события начала или окончания слайдера
    /// </summary>
    /// <param name="StartOrEnd"></param>
    /// <param name="nfg"></param>
    public void DoSliderEvent(bool StartOrEnd, NoteForGame nfg)
    {
        if (IsActive)
        {
            SliderActionEvent(StartOrEnd, nfg);
        }
    }
    public void Lol(bool Pressed, NoteForGame nfg)
    {
    }
    public void lol1(bool StartOrEnd, NoteForGame nfg)
    {
        Debug.Log("a[a[a");
    }
    public void lol2(bool lol)
    {
        
    }
    
}
public enum NoteType : int
{
    Note,
    SliderDot,
    Trigger,
    Slider
}
