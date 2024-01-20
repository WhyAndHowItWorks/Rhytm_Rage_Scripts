using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoteTrackGameSpawner : MonoBehaviour
{

    public bool IsActive { get; set; }

    [Header("Материалы для нот")]
    public GameObject TapNote;
    public GameObject TriggerNote;
    public Transform TriggerSpawnDot;
    public GameObject Slider;
    public Color[] Colours = new Color[4];
    public Sprite Note;
    public Sprite SliderCap;
    public Sprite SliderMiddle;
    

    [Header("Администраторы")]
    public Router rt;
    public AudioSource Music;

    [Header("Все для установки нот")]
    public int deltaToPlace; // Эта нота и выше еще не поставлены
    public int NoteUniqId
    {
        get {
            noteUniqId++;
            return noteUniqId;
        }
        set
        {
            noteUniqId = value;
        }
    }
    public int noteUniqId;// Нужен для поиска, чтобы каждая нота была уникальной
    public int NoteEnd;

    public float TimeEvoke;
    public float TimeSinceStart;

    public int RunNumber
    {
        get { return runNumber; }
        set {
            runNumber = value;
            AbsoluteNoteSpeed = rt.tl.NoteSpeed;
            RelativeNoteSpeed = rt.tl.NoteSpeed * (1 + runNumber * 0.5f);
            Music.pitch = 1 + runNumber * 0.5f;
        }
    }
    public int runNumber;
    public float AbsoluteNoteSpeed;
    public float RelativeNoteSpeed;
    
    public Level CurrentLevel;
    public GameObject HurryUpSign;


    [Header("Переменные для сюжета боевой сцены")]    
    public float EndTime;
    public bool BattleGo;

    [Header("Точки для ориентации нот и работы с ними")]
    public Transform TapPoint;
    public GameObject[] Axis = new GameObject[4];
    public GameObject GameSelectArea;

    public void OnStart()
    {
        RunNumber = 0;
        LoadNewLevel(rt.rp.CurrentLevel);
        rt.oh.OnVarChange += ChangeMusicVolume;
        Music.volume = rt.oh.so.Volume;
        rt.st.BattleStart += OnBattleStart;
        rt.st.BattleEnd += OnBattleEnd;
        rt.es.OnEnemyEvent += WhenEnemyDied;
        
    }
    public void WhenEnemyDied(Enemy e, EnemyAction a)
    {
        if (RunNumber > 0)
        {
            rt.es.GatherEnemyInformation();
            if (rt.es.KolvoEnemies == 0)
            {
                rt.st.EndBattle(true);
            }
        }
        
    }

    public void OnBattleStart()
    {
        StartBuildNote();
    }
    public void OnBattleEnd(bool IsWin)
    {
        IsActive = false;
    }
   
    private void OnDestroy()
    {
        rt.oh.OnVarChange -= ChangeMusicVolume;
    }
    public void ChangeMusicVolume(OptionVarName var, float volume)
    {
        if (var == OptionVarName.Volume )
        {
            Music.volume = volume;
        }
    }
    void LoadNewLevel(Level lv)
    {
        CurrentLevel = lv;
        rt.nam.FutureNotes.Clear();
        rt.tl.LoadFile(lv);
        for (int i = 0; i < rt.tl.Time.Count; i++)
        {
            rt.nam.FutureNotes.Add(new NoteInfo(999, i, ConvertStringToNoteType(rt.tl.Types[i]), rt.tl.Colors[i], rt.tl.Time[i], rt.tl.Lines[i], rt.tl.Slider_Length[i]));
        }
        AbsoluteNoteSpeed = rt.tl.NoteSpeed;
        RelativeNoteSpeed = rt.tl.NoteSpeed * (1 + runNumber * 0.5f);
        Music.pitch = 1 + runNumber * 0.5f;
        TimeEvoke = Mathf.Abs(TapPoint.position.y - Axis[0].transform.position.y) / RelativeNoteSpeed;
        rt.nt.CurrentSpeed = RelativeNoteSpeed;
        EndTime = rt.tl.Time[^1] + TimeEvoke - rt.oh.so.Delay;
        TimeSinceStart = 0;
        deltaToPlace = 0;
        NoteUniqId = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive) // Если активен, начинает строить ноты из файла
        {    
            TimeSinceStart += Time.deltaTime;
            if (TimeSinceStart*(1+0.5*RunNumber) >= EndTime)
            {
                rt.es.GatherEnemyInformation();
                if (rt.es.KolvoEnemies == 0)
                {
                    rt.st.EndBattle(true);
                }
                else 
                {
                    // добавить надпись
                    RunNumber++;
                    LoadNewLevel(CurrentLevel);
                    Music.pitch =  1 + 0.5f*(runNumber-1);
                    Music.clip = rt.rp.CurrentLevel.audioClip;
                    Invoke("DelayMusicPlay", TimeEvoke - rt.oh.so.Delay);
                    TimeSinceStart = 0;
                    rt.nt.IsGgFhase = true;
                    rt.ui.Phase.text = "F*ck it! Let's go FASTER!";
                }
               
            }
            else
            {
                // Если еще есть ноты в файле, то 
                PlaceNoteCheck(deltaToPlace);
            }      
    }
    }
    public void StartBuildNote() // Стартовые приготовления перед установкой нот перед битвой
    {
        Music.clip = rt.rp.CurrentLevel.audioClip;
        Invoke("DelayMusicPlay", TimeEvoke - rt.oh.so.Delay);
        TimeSinceStart = 0;
        IsActive = true;
    }
    public void DelayMusicPlay()
    {
        Debug.Log("F");
        Music.pitch = 1 + 0.5f * (runNumber);
        Music.time = 0;
        Music.Play();
    }
    // Спавнит ноту на треке
    public GameObject NoteSpawn(NoteType Type,int IdInTrack, int Color, float TimeToTap, int Line, float slider_Length)
    {
        GameObject g = null;
        switch (Type)
        {
            case (NoteType.Note):
                g = Instantiate(TapNote, Axis[Line].transform.position, Axis[Line].transform.rotation);
                g.GetComponent<SpriteRenderer>().sprite = Note;
                g.GetComponent<SpriteRenderer>().color = Colours[Color];
                break;
            case (NoteType.Trigger):
                g = Instantiate(TriggerNote, TriggerSpawnDot.position, transform.rotation);
                break;
            case (NoteType.Slider):
                g = Instantiate(Slider, Axis[Line].transform.position, Axis[Line].transform.rotation);
                SliderForGame sfl = g.GetComponent<SliderForGame>();
                sfl.Size = slider_Length * AbsoluteNoteSpeed / 2;

                sfl.Up.GetComponent<SpriteRenderer>().sprite = SliderCap;
                sfl.Up.GetComponent<SpriteRenderer>().color = Colours[Color];

                sfl.Down.GetComponent<SpriteRenderer>().sprite = SliderCap;
                sfl.Down.GetComponent<SpriteRenderer>().color = Colours[Color];

                sfl.Middle.GetComponent<SpriteRenderer>().sprite = SliderMiddle;
                sfl.Middle.GetComponent<SpriteRenderer>().color = Colours[Color];
                break;

        }
        NoteForGame nfg = g.GetComponent<NoteForGame>();

        rt.nam.NotesOnScreen.Add(nfg);

        nfg.rt = rt;
        nfg.noteInfo = ConvertDataToNoteInfo(NoteUniqId,IdInTrack, Type, Color, TimeToTap, Line, slider_Length);
        nfg.Speed = RelativeNoteSpeed;
        rt.nam.NotesHistory.Add(nfg.noteInfo);
        if (nfg.noteInfo.type == NoteType.Slider)
        {
            SliderForGame sfl = (SliderForGame)nfg;
            sfl.PrintDots(AbsoluteNoteSpeed, rt.tl.Bpm);
        }
        if(IdInTrack != 999)
        {
            rt.nam.CheckNoteMods(nfg);
        }
        return g;

    }

    
    public NoteType ConvertStringToNoteType(string value)
    {
        NoteType nt = NoteType.Note;
        switch (value)
        {
            case ("Нажатие"):
                nt = NoteType.Note;
                break;
            case ("Триггер"):
                nt = NoteType.Trigger;
                break;
            case ("Слайдер"):
                nt = NoteType.Slider;
                break;
        }
        return nt;
    }
    public NoteInfo ConvertDataToNoteInfo(int UniqId,int IdInTrack, NoteType Type, int Color, float TimeToTap, int Line, float slider_Length)
    {
        NoteInfo noteInfo = new NoteInfo(UniqId,IdInTrack, Type, Color, TimeToTap, Line, slider_Length);
        return noteInfo;
    }
    public void PlaceNoteCheck(int Count)
    {
        
        if (deltaToPlace < rt.tl.Time.Count)
        {
            if (TimeSinceStart * (1 + 0.5 * RunNumber) >= rt.tl.Time[Count])
            {
                NoteSpawn(ConvertStringToNoteType(rt.tl.Types[Count]), Count, rt.tl.Colors[Count], rt.tl.Time[Count], rt.tl.Lines[Count], rt.tl.Slider_Length[Count]);
                rt.nam.FutureNotes.RemoveAt(0);

                deltaToPlace++;

                PlaceNoteCheck(Count + 1);
            }
        }
    }
   
}
