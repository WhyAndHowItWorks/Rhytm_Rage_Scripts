using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class NoteTrack : MonoBehaviour
{

    //����������� ��� = 120
    //�� ������ ������� ����� ��� ��� ������ 16 ���� ��� ������ ��� �� ������ �����
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


    // ��� ��� ���������
    public float maxY; // ���������� ����� ������� ����

    public float MaxY  // ������������ ������ �����
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



    public float minSizeOfTrack; // ����������� ������ ����� ��� ��������� ������� ����������
    public GameObject MaxYNote;
    //����� ���� ��� �����������
    public GameObject Cursor;
    public GameObject ChosenObject;

    public bool MovingAllNotes;

    public GameObject ChosedHalo;
    public GameObject ChosedAreaHalo;
    //�������� ����� ���
    public GameObject FirstDot;
    public GameObject SecondDot;
    public GameObject SelectingArea;
    public GameObject TapLine;


    public GameObject[] Lines = new GameObject[5];

    public float[] Axis = new float[4];

    public List<GameObject> Notes = new List<GameObject>();



    [Header("����������")]
    public TrackFileCoder tfc;
    public TrackLoder tl;
    public TrackPrinter tp;

    public EditorUIHandler euh;
    public EditorNoteFactory enf;
    public Cancel cnc;

    [Header("�����������")]
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
            if (mode != value) // ���� ���������� ������ ����������
            {
                Instruments[(int)mode].IsActive = false;
                Instruments[(int)value].IsActive = true;
                euh.ChosenInstrument.text = Instruments[(int)value].InstrumentName;
                mode = value;
            }
        }
    }
    public List<InstrumentMode> Instruments = new List<InstrumentMode>();


    [Header("������ ��������")]
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
       
        oh = GameObject.Find("��������� ��������").GetComponent<OptionsHandler>();
        lm = GameObject.Find("��������� ��������").GetComponent<LevelManager>();
        StartPointY = Lines[4].transform.position.y;
    }
    void Start() 
    {
        for (int i = 0; i < 4; i++) // ����������� ���������� ����� �� ����
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
       
        FindCursorCoordinates(cam, out euh.ClickX, out euh.ClickY); // ��������� ���������� �������
        Cursor.transform.position = new Vector3(euh.ClickX, euh.ClickY, 0); // ���������� �� ��� ���������� ������ �������
        if (euh.CursorOnTrack && euh.CursorIsActive) // ���� ������ ��������� �� �����
        {
            float mw = Input.GetAxis("Mouse ScrollWheel");
            if (mw != 0)// ��������� ������������ ���� ��������� ����
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

    public GameObject? FindObjectUnderMouse(Camera cam) // ���������� ������ ��� �������� (������ ���� ���� ������� ��� ��� ���� ������ ��� ����������)
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
    public void FindCursorCoordinates(Camera cam, out float ClickX, out float ClickY)// ������� ���������� �������
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ClickX = mousePos.x;
        ClickY = mousePos.y;
    }
    public int NearestAxis(GameObject g)// ������� ��������� ����� ������������ �������
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
    public int NearestAxis(float x) // ������� ��������� ��� �� ������ ����������
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
        if (g == MaxYNote && Notes.Count > 0)// � ������, ���� ��� ���� ����� ������� �� ������ ����, ������������� ��������� � ������ ������� ���������
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
   
    
    
    private void OnTriggerExit2D(Collider2D collision) // �������� �� ������������ �������
    {
        if (collision.gameObject.name == "������ �������")
        {
            euh.CursorOnTrack = false;
        } 
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.name == "������ �������")
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
        for (int i = 0; i < Notes.Count; i++) // ������� ���� ���
        {
            Destroy(Notes[i]);
            SpawningMode sp = (SpawningMode)Instruments[(int)ToolMode.PlaseNote];
           sp.ChangeFhaseTriggers.Clear();
        }
        MaxYNote = null;
        MaxY = 0;
        for (int i = 0; i < tl.Types.Count; i++) // ���������� ��� �� �����
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

    #region ����������
    public float NotePositionToTactTime(float value) // �������� ������� � �������� �������
    {
        return (value - StartPointY) / NoteSpeed * BPM / standardBPM;
    }
    public float NoteTactTimeToPosition(float time) // ������� �������� ������ � �������
    {
        return StartPointY + time * NoteSpeed / BPM * standardBPM;
    }

    public float NoteTactTimeToTime(float value) // ��������� �������� ����� � ������� �����
    {
        return value / BPM * standardBPM;
    }

    public float NoteTimeToTactTime(float value) // ��������� ������� ����� � �������� �����
    {
        return value * BPM / standardBPM;
    }
    public float NotePositionToTime(GameObject g) // ����������� ������������ ���� �� ������� �� ������ �����
    {
        return (g.transform.position.y - StartPointY) / NoteSpeed;
    }
    public float NotePositionToTime(float value) // ����������� ������������ �� ��� y �� ������� �� ������ �����
    {
        return (value - StartPointY) / NoteSpeed;
    }
    public float NoteTimeToPosition(float time) // ��������� ����� � �������
    {
        return StartPointY + time * NoteSpeed;
    }
    public NoteEditorType ConvertStringToNoteEditorType(string str)
    {

        switch (str)
        {
            case ("�������"):
                return NoteEditorType.Note;
                break;
            case ("�������"):
                return NoteEditorType.Trigger;
                break;
            case ("�������"):
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
                return "�������";
                break;
            case (NoteEditorType.Trigger):
                return "�������";
                break;
            case (NoteEditorType.Slider):
                return "�������";
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
                Debug.Log("���� " + g + " ���� �������");
                break;
            case (EditorNoteAction.Moved):
                Debug.Log("���� " + g + " ���� ����������");
                break;
            case (EditorNoteAction.Deleted):
                Debug.Log("���� " + g + " ���� �������");
                break;
        }
    }
    public float CenterX(float value) // ��������� ���� �� �������� ��������, ��������� ������������
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
    public float CenterTimeX(float value)// ��������� ������� ����, ��������� �������� �������
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
    /// ��������, �������� �� ����� � ��� ����������� �� ��������� �����.
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
