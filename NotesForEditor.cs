using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class NotesForEditor : MonoBehaviour
{
    public static int UniqId
    {
        get 
        {
            uniqId++;
            return uniqId;
        }
        set
        {
            uniqId = value;
        }
    }
    public static int uniqId;
    public int NoteUniqId;
    public bool Uniq;
    public int Line
    {
        get { return line; }
        set { line = value; }
    } // Линия, на которой находится нота
    public int line;
    public float TimeOfTap 
    {
        get { return timeOfTap; }
        set { 
            timeOfTap = value;
           
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, nt.NoteTimeToPosition(timeOfTap), gameObject.transform.position.z);
        }
    } // Время нажатия ноты в СЕКУНДАХ 

    public float timeOfTap;
    public int Number;// Цвет ноты

    public NoteEditorType Type;
    public GameObject Text;
    public GameObject DotOnMinimap;

    public NoteTrack nt;

    public Vector3 Position 
    {
        get { return gameObject.transform.position; }
        set 
        {
            gameObject.transform.position = value;
            
            timeOfTap = nt.NotePositionToTime(gameObject);
            if (Type != NoteEditorType.Trigger)
            {
                line = nt.NearestAxis(gameObject);
            }
          
            ChangeDotMinimapPosition();
            nt.CheckMaxYNoteInCreatingAndMoving(gameObject);
            
        }
    }
    // если слайдер
    public float Length; // Продолжительность прожатия слайдера в секундах

    

    public GameObject ChosedHalo;
    public GameObject ChosedAreaHalo;
    /// <summary>
    /// Является выбранной инструментом массового выбора. К ней будут применятся все инструменты, которые будут применены хотя бы к одной выбранной ноте.
    /// </summary>
    public bool IsChosen 
    {
        get { return ischosen; }
        set 
        {
            if (ischosen != value)
            {
                SelectingMode sm = (SelectingMode)nt.Instruments[(int)ToolMode.Selecting];
                ischosen = value;
                if (ischosen)
                {
                    sm.ChosenObjects.Add(gameObject);
                    if (ChosedHalo == null)
                    {
                        ChosedHalo = Instantiate(nt.ChosedHalo, gameObject.transform);
                        ChosedHalo.transform.localPosition = new Vector3(0, 0, -1);
                    }

                }
                else
                {
                    sm.ChosenObjects.Remove(gameObject);
                    if (ChosedHalo != null)
                    {
                        Destroy(ChosedHalo);
                    }
                }
            }
        }
    }
    /// <summary>
    /// Является выбранной при помощи области выделения. Нужна для понимания, что попадает в область
    /// </summary>
    public bool IsChosenByArea 
    {
        get { return ischosenByArea; }
        set 
        {
            ischosenByArea = value;
            if (ischosenByArea)
            {
                
                    ChosedAreaHalo = Instantiate(nt.ChosedAreaHalo, gameObject.transform);
                    ChosedAreaHalo.transform.localPosition = new Vector3(0, 0, -1);
                
           
            }
            else
            {
                if (ChosedAreaHalo != null)
                {
                    Destroy(ChosedAreaHalo);
                    
                }
            }

        }
    }
    public bool IsMovingByCursor
    {
        get { return isMovingByCursor; }
        set
        {
            if (IsMovingByCursor != value)
            {

            
            isMovingByCursor = value;
            MovingMode mm = (MovingMode)nt.Instruments[(int)ToolMode.Moving];
            if (isMovingByCursor)
            {
                mm.MovingObjects.Add(gameObject);
                gameObject.transform.SetParent(nt.Cursor.transform);
            }
            else
            {
                mm.MovingObjects.Remove(gameObject);
                gameObject.transform.SetParent(null);
            }
        }
        }
    }
    /// <summary>
    /// Будет ли переменная записыватся в финальный файл
    /// </summary>
    public bool IsFileCoder { get; set; }
    public bool isFileCoder;

    
    bool ischosen;
    bool ischosenByArea;
    bool isMovingByCursor;
    public void Start()
    {       
        nt = GameObject.Find("Нотный трек").GetComponent<NoteTrack>();       
        BuildMinimapDot(); 
        BuildNote();
        Position = gameObject.transform.position;
        
    }
    public void OnInstantiate()
    {
        if (Type == NoteEditorType.Trigger)
        {
            nt.euh.AttachTextToNote(gameObject, nt.euh.DoNameForText(Number, Line));
            
        }
       
        nt.ParameterChangedEvent += NoteTrackParameterChanged;
        nt.EditorNoteEvent += NoteEditorEvent;
        NoteUniqId = UniqId;
    }
    void BuildMinimapDot()
    {
        switch (Type)
        {
            case (NoteEditorType.Note):
                DotOnMinimap = Instantiate(nt.euh.DotOnMinimapNote, nt.euh.SliderDotsObject.transform);
                DotOnMinimap.GetComponent<Image>().color = nt.enf.colors[Number];
                break;
            case (NoteEditorType.Trigger):
                DotOnMinimap = Instantiate(nt.euh.DotOnMinimapTrigger, nt.euh.SliderDotsObject.transform);
                break;
            case (NoteEditorType.Slider):
                DotOnMinimap = Instantiate(nt.euh.DotOnMinimapSlider, nt.euh.SliderDotsObject.transform);
                DotOnMinimap.GetComponent<SliderMinimapDot>().Middle.GetComponent<Image>().color = nt.enf.colors[Number];
                DotOnMinimap.GetComponent<SliderMinimapDot>().Size = Length * nt.NoteSpeed / (nt.MaxY - nt.StartPointY) * nt.euh.HighDot.GetComponent<RectTransform>().anchoredPosition.y;
                break;

        }
        
    }

    public void ChangeDotMinimapPosition()
    {

        if (DotOnMinimap != null)
        {
            switch (Type)
            {
                case (NoteEditorType.Note):
                    DotOnMinimap.GetComponent<RectTransform>().anchoredPosition = new Vector2(nt.euh.AxisDots[Line].anchoredPosition.x, (transform.position.y - nt.StartPointY) / (nt.MaxY - nt.StartPointY) * nt.euh.HighDot.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
                case (NoteEditorType.Trigger):
                    if (Text != null)
                    {
                        Text.transform.position = transform.position;
                    }
                   
                    DotOnMinimap.GetComponent<RectTransform>().anchoredPosition = new Vector2(nt.euh.AxisForTrigger.anchoredPosition.x, (transform.position.y - nt.StartPointY) / (nt.MaxY - nt.StartPointY) * nt.euh.HighDot.GetComponent<RectTransform>().anchoredPosition.y);
                    break;
                case (NoteEditorType.Slider):
                    DotOnMinimap.GetComponent<RectTransform>().anchoredPosition = new Vector2(nt.euh.AxisDots[Line].anchoredPosition.x, (transform.position.y - nt.StartPointY) / (nt.MaxY - nt.StartPointY) * nt.euh.HighDot.GetComponent<RectTransform>().anchoredPosition.y);
                    if (DotOnMinimap.GetComponent<SliderMinimapDot>())
                    {
                        DotOnMinimap.GetComponent<SliderMinimapDot>().ChangeSize(Length * nt.NoteSpeed / (nt.MaxY - nt.StartPointY) * nt.euh.HighDot.GetComponent<RectTransform>().anchoredPosition.y);

                    }
                    break;

            }
        }
    }

    public void BuildNote() // Установка начальных графических настроек для ноты.
    {       
        switch (Type)
        {
            case (NoteEditorType.Note):
                gameObject.GetComponent<SpriteRenderer>().sprite = nt.enf.NoteSprite;
                gameObject.GetComponent<SpriteRenderer>().color = nt.enf.colors[Number];
                break;
            case (NoteEditorType.Trigger):
                gameObject.GetComponent<SpriteRenderer>().sprite = nt.enf.TriggerSprite;
                gameObject.GetComponent<SpriteRenderer>().color = nt.enf.TriggerColor;
              
                break;
            case (NoteEditorType.Slider):
                float sl_size = Length * nt.noteSpeed / 2;
                gameObject.GetComponent<SliderNote>().SizeChanger = sl_size;
                

                gameObject.GetComponent<SliderNote>().Up.GetComponent<SpriteRenderer>().sprite = nt.enf.SliderCap;
                gameObject.GetComponent<SliderNote>().Up.GetComponent<SpriteRenderer>().color = nt.enf.colors[Number];

                gameObject.GetComponent<SliderNote>().Middle.GetComponent<SpriteRenderer>().sprite = nt.enf.SliderMiddle;
                gameObject.GetComponent<SliderNote>().Middle.GetComponent<SpriteRenderer>().color = nt.enf.colors[Number];

                gameObject.GetComponent<SliderNote>().Down.GetComponent<SpriteRenderer>().sprite = nt.enf.SliderCap;
                gameObject.GetComponent<SliderNote>().Down.GetComponent<SpriteRenderer>().color = nt.enf.colors[Number];
                break;
        }
    }
    public void Update()
    {
        if (IsMovingByCursor)
        {
            Position = gameObject.transform.position;
        }
        if (Text != null)
        {
            Text.transform.position = gameObject.transform.position;
        }
    }
    public void OnDestroy()
    {
        if (Text != null) // Стереть текст, если он есть
        {
            Destroy(Text);         
        }
        if (DotOnMinimap != null) // Стереть точку на карте, если она есть
        {
            Destroy(DotOnMinimap);
        }
        nt.ParameterChangedEvent -= NoteTrackParameterChanged;
        nt.EditorNoteEvent -= NoteEditorEvent;
        nt.Notes.Remove(gameObject);
        nt.CheckMaxYNoteInDeleting(gameObject);
        if (IsChosen)
        {
            IsChosen = false;
        }

    }
    /// <summary>
    /// Изменениие Параметра par c oldval на newval
    /// </summary>
    /// <param name="par"></param>
    /// <param name="oldval"></param>
    /// <param name="newval"></param>
    public void NoteTrackParameterChanged(TrackParameter par, float oldval, float newval) 
    {
        switch (par)
        {
            case (TrackParameter.NoteSpeed):
                Position = new Vector3(Position.x,nt.NoteTimeToPosition(TimeOfTap),Position.z);
                if (Type == NoteEditorType.Slider)
                {
                    float sl_size = Length * nt.noteSpeed / 2;
                    gameObject.GetComponent<SliderNote>().SizeChanger = sl_size;
                }   
                ChangeDotMinimapPosition();
                break;
            case (TrackParameter.MaxY):
                ChangeDotMinimapPosition();
            break;

        }
    }

    public void NoteEditorEvent(EditorNoteAction ena, GameObject g)
    {
        if (g != gameObject)
        {
            switch (ena)
            {
                case (EditorNoteAction.Created):

                    CheckUniq(g);
                    if (Type == NoteEditorType.Trigger)
                    {
                        if (Number == 2)
                        {
                            EndLevelCheck(g);
                        }
                    }
                    break;
                case (EditorNoteAction.Moved):
                    if (Number == 2 && Type == NoteEditorType.Trigger)
                    {
                        EndLevelCheck(g);
                    }
                    break;
            }
        }
        else 
        {
           
            if (Number == 0 && Type == NoteEditorType.Trigger)
            {
                SpawningMode sp = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
                switch (ena)
                {
                    case (EditorNoteAction.Created):

                        sp.ChangeFhaseTriggers.Add(gameObject);
                        break;
                    case (EditorNoteAction.Deleted):
                        
                        sp.ChangeFhaseTriggers.Remove(gameObject);
                        break;
                }
                
                PhaseCheck();
            }
                
            
        }
           
    }
    public void CheckUniq(GameObject g)
    {
        NotesForEditor note = g.GetComponent<NotesForEditor>();     
        if (note.Uniq && Uniq && Number == note.Number)
        {
            Destroy(gameObject);
        }
    }
    public void EndLevelCheck(GameObject g)
    {
        if (g != gameObject && transform.parent != GameObject.Find("Префабы и прочее").transform)
        {
            if (g.transform.position.y + g.GetComponent<NotesForEditor>().Length * nt.NoteSpeed > Position.y)
            {
                Position = new Vector3(Position.x, g.transform.position.y + g.GetComponent<NotesForEditor>().Length * nt.NoteSpeed, Position.z);

            }
        }
        
    }
    public void PhaseCheck()
    {
        SpawningMode sp = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
        List<GameObject> delta = sp.ChangeFhaseTriggers;
        delta.Sort(new ComparerNote());
        for (int i = 0; i < delta.Count; i++)
        {
            if (i % 2 == 0)
            {
                delta[i].GetComponent<NotesForEditor>().Text.GetComponent<Text>().text = "Change Phase: Enemy Phase";
            }
            else 
            {
                delta[i].GetComponent<NotesForEditor>().Text.GetComponent<Text>().text = "Смена фазы: GG Phase";
            }
        }
    }
    
    
}
