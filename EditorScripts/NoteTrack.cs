using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class NoteTrack : MonoBehaviour
{

    //Стандартный БМП = 120
    //На каждую секунду трека при нем дается 16 мест для ставки нот на каждом треке
    public GameObject Camera;

    public float DeltaCamVal
    {
        get { return deltaCamVal; }
        set { deltaCamVal = value; }
    }
    public float deltaCamVal;
    public float CameraYpos
    {
        get { return Camera.transform.position.y; }
        set
        {
            if (value == float.NaN)
            {
                value = 0;
            }
            Camera.transform.position = new Vector3(Camera.transform.position.x, value, Camera.transform.position.z);

            if (Camera.transform.position.y <= maxY)
            {

                euh.scr.SetValueWithoutNotify(Camera.transform.position.y / maxY);
            }
            if (Camera.transform.position.y < 0)
            {
                Camera.transform.position = new Vector3(Camera.transform.position.x, 0, Camera.transform.position.z);
            }

        }
    }
    public Camera cam;
    public float CamSpeed;


    // Все для прокрутки
    public float maxY; // координаты самой дальней ноты

    public float MaxY  // Максимальная высота трека
    {
        get
        { return maxY;
        }
        set {
            float val = maxY;
            maxY = value;
            ParameterChangedEvent(TrackParameter.MaxY, val, maxY);

        }
    }



    public float minSizeOfTrack; // минимальный размер трека для пропорций размера скроллбара
    public GameObject MaxYNote;
    //Выбор ноты для перемещения
    public GameObject Cursor;
    public GameObject ChosenObject;

    public bool MovingAllNotes;

    public GameObject ChosedHalo;
    public GameObject ChosedAreaHalo;
    //Массовый выбор нот
    public GameObject FirstDot;
    public GameObject SecondDot;
    public GameObject SelectingArea;
    public GameObject TapLine;


    public GameObject[] Lines = new GameObject[5];

    public float[] Axis = new float[4];

    public List<GameObject> Notes = new List<GameObject>();



    [Header("Подсистемы")]
    public TrackFileCoder tfc;
    public TrackLoder tl;
    public TrackPrinter tp;

    public EditorUIHandler euh;
    public EditorNoteFactory enf;
    public Cancel cnc;

    [Header("Инструменты")]
    public SpawningMode spm;
    public SelectingMode sem;
    public DeletingMode dem;
    public PlayingMode pm;
    


    public ToolMode mode;
    public ToolMode Mode
    {
        get { return mode; }
        set
        {
            if (mode != value) // Если выбирается другой инструмент
            {
                Instruments[(int)mode].IsActive = false;
                Instruments[(int)value].IsActive = true;
                euh.ChosenInstrument.text = Instruments[(int)value].InstrumentName;
                mode = value;
            }
        }
    }
    public List<InstrumentMode> Instruments = new List<InstrumentMode>();


    [Header("Важные свойства")]
    public AudioSource Music;

    


    public float bpm;
    public float standardBPM = 120;
    public float BPM
    {
        get { return bpm; }
        set {
            float oldBpm = bpm;
            bpm = value;
            ParameterChangedEvent(TrackParameter.BPM, oldBpm, noteSpeed);
        }
    }
    public float NoteSpeed
    {
        get { return noteSpeed; }
        set {
            float nt = noteSpeed;
            noteSpeed = value;
            ParameterChangedEvent(TrackParameter.NoteSpeed, nt, noteSpeed);

        }
    }
    public float noteSpeed;
    public float SliderStep;

    public float StartPointY;
    public bool NeedCenterX;

    public OptionsHandler oh;
    public LevelManager lm;

    public delegate void ParameterChanged(TrackParameter par, float oldvalue, float newvalue);
    public event ParameterChanged ParameterChangedEvent;

    public delegate void EditorNote(EditorNoteAction ena, GameObject g);
    public event EditorNote EditorNoteEvent;

    public EventSystem es;
        
  

    private void Awake()
    {
       
        oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        lm = GameObject.Find("Хранитель настроек").GetComponent<LevelManager>();
        StartPointY = Lines[4].transform.position.y;
    }
    void Start() 
    {
        for (int i = 0; i < 4; i++) // Высчитывает координаты линий по иксу
        {
            Axis[i] = Lines[i].transform.position.x;
        }
        tfc = gameObject.GetComponent<TrackFileCoder>();
        tl = gameObject.GetComponent<TrackLoder>();
        tl.lm = lm;
        tp = gameObject.GetComponent<TrackPrinter>();
        Music = gameObject.GetComponent<AudioSource>();
        Music.volume = oh.Volume;
        Mode = ToolMode.No;
        EditorNoteEvent += TestNoteEvent;
        CamSpeed = oh.so.CamSpeed;
    }

    


    void Update()
    {
       
        FindCursorCoordinates(cam, out euh.ClickX, out euh.ClickY); // Обновляет координаты курсора
        Cursor.transform.position = new Vector3(euh.ClickX, euh.ClickY, 0); // Перемещает на эти координаты объект курсора
        if (euh.CursorOnTrack && euh.CursorIsActive) // Если курсор находится на треке
        {
            float mw = Input.GetAxis("Mouse ScrollWheel");
            if (mw != 0)// Позволяет прокрутывать трек колесиком мыши
            {
                if (mw < 0 && Camera.transform.position.y > 0)
                {
                    CameraYpos += Time.deltaTime * mw * CamSpeed;
                }
                if (mw > 0)
                {
                    CameraYpos += Time.deltaTime * mw * CamSpeed;
                }

            }
        }    
    }

    public GameObject? FindObjectUnderMouse(Camera cam) // Возвращает объект под курсором (вернет ноль если объекта нет или этот объект без коллайдера)
    {
        RaycastHit hit;
        Ray MyRay;
        MyRay = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(MyRay, out hit, 100);

        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else { return null; }    
    }
    public void FindCursorCoordinates(Camera cam, out float ClickX, out float ClickY)// Находит координаты курсора
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ClickX = mousePos.x;
        ClickY = mousePos.y;
    }
    public int NearestAxis(GameObject g)// Находит ближайшую линию относительно объекта
    {
        float[] dists = new float[4];
        float mindist = Mathf.Abs(g.transform.position.x - Axis[0]);
        int distindex = 0;
        for (int i = 1; i < 4; i++)
        {
            float dist = Mathf.Abs(g.transform.position.x - Axis[i]);
            if (dist < mindist)
            {
                mindist = dist;
                distindex = i;
            }
        }
        
        return distindex;
    }
    public int NearestAxis(float x) // Находит ближайшую ось от осевой координаты
    {
        float[] dists = new float[4];
        float mindist = Mathf.Abs(x - Axis[0]);
        int distindex = 0;
        for (int i = 1; i < 4; i++)
        {
            float dist = Mathf.Abs(x - Axis[i]);
            if (dist < mindist)
            {
                mindist = dist;
                distindex = i;
            }
        }

        return distindex;
    }
   
    public void CheckMaxYNoteInCreatingAndMoving(GameObject g)
    {
        float f = g.transform.position.y + g.GetComponent<NotesForEditor>().Length * NoteSpeed;
        if (f > MaxY || g == MaxYNote)
        {
            MaxY = f;
            MaxYNote = g;
        }

    }
    public void CheckMaxYNoteInDeleting(GameObject g)
    {
        if (g == MaxYNote && Notes.Count > 0)// В случае, если эта была самая дальняя от начала нота, пересчитывает положение и размер бегунка прокрутки
        {
            float newmaxY = 0;
            int newNote = 0;
            for (int i = 0; i < Notes.Count; i++)
            {
                if (newmaxY < Notes[i].transform.position.y + Notes[i].GetComponent<NotesForEditor>().Length*NoteSpeed)
                {
                    newmaxY = Notes[i].transform.position.y + Notes[i].GetComponent<NotesForEditor>().Length * NoteSpeed;
                    newNote = i;
                }
            }
            MaxY = newmaxY;
            MaxYNote = Notes[newNote];
        }
    }
   
    
    
    private void OnTriggerExit2D(Collider2D collision) // Проверка на расположение курсора
    {
        if (collision.gameObject.name == "Объект курсора")
        {
            euh.CursorOnTrack = false;
        } 
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "Объект курсора")
        {
            euh.CursorOnTrack = true;
        }
    }
  
    public void BuildTrackFromFile(int Level)
    {  
        tl.lm = lm;
        tl.LoadFile(lm.CustomLevel[Level]);
        tfc.NameOfTrack = tl.NameOfTrack;
        tfc.NoteSpeed = tl.NoteSpeed;
        euh.NoteSpeedUI.text = tl.NoteSpeed.ToString();
        tfc.Bpm = tl.Bpm;
        BPM = tl.Bpm;
        for (int i = 0; i < Notes.Count; i++) // Очистка всех нот
        {
            Destroy(Notes[i]);
            SpawningMode sp = (SpawningMode)Instruments[(int)ToolMode.PlaseNote];
           sp.ChangeFhaseTriggers.Clear();
        }
        MaxYNote = null;
        MaxY = 0;
        for (int i = 0; i < tl.Types.Count; i++) // Построение нот из файла
        {
            SpawningMode sp = (SpawningMode)Instruments[(int)ToolMode.PlaseNote];
                sp.SpawnNoteWithoutChecks(ConvertStringToNoteEditorType(tl.Types[i]), tl.Colors[i], tl.Lines[i], NoteTimeToPosition(tl.Time[i]), tl.Slider_Length[i]);          
            
        }
        if (Notes.Count == 0)
        {
            SpawningMode sp = (SpawningMode)Instruments[(int)ToolMode.PlaseNote];
            sp.SpawnNoteWithoutChecks(NoteEditorType.Trigger, 2, 0, StartPointY, 0);
        }
        
           
              
    }

    #region Конверторы
    public float NotePositionToTactTime(float value) // Перевеод позиции в тактовые секунды
    {
        return (value - StartPointY) / NoteSpeed * BPM / standardBPM;
    }
    public float NoteTactTimeToPosition(float time) // Перевод тактовых секунд в позицию
    {
        return StartPointY + time * NoteSpeed / BPM * standardBPM;
    }

    public float NoteTactTimeToTime(float value) // Переводит тактовое время в обычное время
    {
        return value / BPM * standardBPM;
    }

    public float NoteTimeToTactTime(float value) // Переводит обычное время в тактовое время
    {
        return value * BPM / standardBPM;
    }
    public float NotePositionToTime(GameObject g) // Высчитывает расположение ноты по времени от начала трека
    {
        return (g.transform.position.y - StartPointY) / NoteSpeed;
    }
    public float NotePositionToTime(float value) // Высчитывает расположение по оси y по времени от начала трека
    {
        return (value - StartPointY) / NoteSpeed;
    }
    public float NoteTimeToPosition(float time) // Переводит Время в позицию
    {
        return StartPointY + time * NoteSpeed;
    }
    public NoteEditorType ConvertStringToNoteEditorType(string str)
    {

        switch (str)
        {
            case ("Нажатие"):
                return NoteEditorType.Note;
                break;
            case ("Триггер"):
                return NoteEditorType.Trigger;
                break;
            case ("Слайдер"):
                return NoteEditorType.Slider;
                break;
        }

        return NoteEditorType.Note;
    }
    public string ConvertNoteEditorTypeToString(NoteEditorType nt)
    {

        switch (nt)
        {
            case (NoteEditorType.Note):
                return "Нажатие";
                break;
            case (NoteEditorType.Trigger):
                return "Триггер";
                break;
            case (NoteEditorType.Slider):
                return "Слайдер";
                break;
        }

        return "sss";

    }
    #endregion

    public void DoNoteEvent(EditorNoteAction ena, GameObject g)
    {
        EditorNoteEvent(ena, g);
    }
    public void TestNoteEvent(EditorNoteAction ena, GameObject g)
    {
        switch (ena)
        {
            case (EditorNoteAction.Created):
                Debug.Log("Нота " + g + " была создана");
                break;
            case (EditorNoteAction.Moved):
                Debug.Log("Нота " + g + " была перемещена");
                break;
            case (EditorNoteAction.Deleted):
                Debug.Log("Нота " + g + " была удалена");
                break;
        }
    }
    public float CenterX(float value) // Центровка ноты по тактовым секундам, используя пространство
    {
        if (NeedCenterX)
        {
            float time = NotePositionToTactTime(value);
            float step = 1f / Mathf.Pow(2, tp.divideLevel);
            float delta = time % step;
            if (delta > (step / 2))
            {
                return NoteTactTimeToPosition(time - delta + step);
            }
            else { return NoteTactTimeToPosition(time - delta); }
            
        }
        else { return value; }
       
    }
    public float CenterTimeX(float value)// Центровка времени ноты, используя тактовые секунды
    {
        if (NeedCenterX)
        {
            float time = NoteTimeToTactTime(value);
            float step = 1f / Mathf.Pow(2, tp.divideLevel);
            float delta = time % step;
            if (delta > (step / 2))
            {
                return NoteTactTimeToTime(time - delta + step);
            }
            else { return NoteTactTimeToTime(time - delta); }

        }
        else { return value; }
    }
    /// <summary>
    /// Проверка, свободно ли место и его окрестности на указанной линии.
    /// </summary>
    /// <param name="Yposition"></param>
    /// <param name="Line"></param>
    /// <returns></returns>
    public bool PlaceIsFree(NoteEditorType net,float Yposition, int Line) 
    {
        bool res = true;
        if (net != NoteEditorType.Trigger)
        {
            for (int i = 0; i < Notes.Count; i++)
            {
                float right = Notes[i].transform.position.y + enf.NoteOffset;
                float left = Notes[i].transform.position.y - enf.NoteOffset;
                if ((Yposition < right && Yposition > left) && Notes[i].GetComponent<NotesForEditor>().Line == Line)
                {
                    res = false;
                }
            }
        }
        
        return res;
    }

    public NotesForEditor FindNoteById(int Id)
    {
        for (int i = 0; i < Notes.Count; i++)
        {
            NotesForEditor n = Notes[i].GetComponent<NotesForEditor>();
            if (n.NoteUniqId == Id)
            {
                return n;
            }
        }
        return null;
    }
}
