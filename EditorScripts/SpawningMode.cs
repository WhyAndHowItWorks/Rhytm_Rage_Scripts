using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class SpawningMode : InstrumentMode
{
    public EditorUIHandler euh;
    public EditorNoteFactory enf;

    public bool SliderBuilt;
    public int Slider_Line;
    public GameObject Slider_FirstPoint;
    public GameObject Slider_SecondPoint;

    // Нота для установки, которая изменяется при помощи кнопок, её копия и устанавливается на трек
    public NoteEditorType Deltanote_type
    {
        get { return delta_nt.Type; }
        set 
        {
            if (delta_nt.Type != value)
            {
                if (delta_nt.Type == NoteEditorType.Slider)
                {
                    SliderBuiltCancel();
                }
                delta_nt.Type = value;
            }            
           
            
        }
    }

    public NotesForEditor delta_nt;
    public GameObject delta_nt_g;

    public List<bool> UniqTrigger = new List<bool>();

    public List<GameObject> ChangeFhaseTriggers = new List<GameObject>();

   

    void Start()
    {
        euh = nt.euh;
        enf = nt.enf;
    }

    
    void Update()
    {
        if (IsActive)
        {
            if (euh.CursorOnTrack && euh.CursorIsActive)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    GameObject c = nt.FindObjectUnderMouse(nt.cam);
                    if (c != null)
                    {
                        //Если не нота не выбрана, удаляет только её
                        if (!c.GetComponent<NotesForEditor>().IsChosen)
                        {
                            DeletingMode dm = (DeletingMode)nt.Instruments[(int)ToolMode.Deleting];
                            dm.DeleteNote(c);
                        }
                        else // Если нота выбрана, то удаляет все выбранные
                        {
                            SelectingMode sm = (SelectingMode)nt.Instruments[(int)ToolMode.Selecting];
                            foreach (GameObject g in sm.ChosenObjects)
                            {
                                DeletingMode dm = (DeletingMode)nt.Instruments[(int)ToolMode.Deleting];
                                dm.DeleteNote(g);

                            }
                            sm.ChosenObjects.Clear();

                        }
                    }

                }
                else 
                {
                    switch (Deltanote_type)
                    {
                        case (NoteEditorType.Note):
                            if (Input.GetMouseButtonDown(0))
                            {
                                SpawnNote(delta_nt.Type, delta_nt.Number, nt.NearestAxis(nt.Cursor), euh.ClickY, 0);
                            }
                            break;
                        case (NoteEditorType.Trigger):
                            if (Input.GetMouseButtonDown(0))
                            {
                                SpawnNote(delta_nt.Type, delta_nt.Number, delta_nt.Line, euh.ClickY, 0);
                            }
                            break;
                        case (NoteEditorType.Slider):
                            if (Input.GetMouseButtonDown(0))
                            {
                                if (!SliderBuilt)
                                {

                                    Slider_Line = nt.NearestAxis(nt.Cursor);
                                    Slider_FirstPoint = Instantiate(enf.SliderPoint, new Vector3(nt.Axis[Slider_Line], nt.CenterX(euh.ClickY), -4), nt.Cursor.transform.rotation);
                                    SliderBuilt = !SliderBuilt;
                                }
                                else
                                {
                                    if (nt.NearestAxis(nt.Cursor) == Slider_Line)
                                    {
                                        Slider_SecondPoint = Instantiate(enf.SliderPoint, new Vector3(nt.Axis[Slider_Line], nt.CenterX(euh.ClickY), -4), nt.Cursor.transform.rotation);
                                        SliderBuilt = !SliderBuilt;
                                        if (Slider_FirstPoint.transform.position.y < Slider_SecondPoint.transform.position.y) // учет положения точек друг от друга
                                        {
                                            SpawnSliderInEditor(Slider_FirstPoint, Slider_SecondPoint);
                                        }
                                        else { SpawnSliderInEditor(Slider_SecondPoint, Slider_FirstPoint); }

                                    }
                                }
                            }
                            break;


                    }
                }
               
                
            }
            
        }
    }

    public override void StartWork()
    {
        
    }
    public override void EndWork()
    {
        SliderBuiltCancel();
    }
    //////Вспомогательные функции//////////
    public void SliderBuiltCancel()
    {
        if (Slider_FirstPoint != null)
        {
            Destroy(Slider_FirstPoint);
            SliderBuilt = false;
        }
    }
    public GameObject SpawnNote(NoteEditorType Type, int Number, int Line, float Yposition, float Length) // Спавнит ноту и привязывает её к массиву нот, также дает ей текстуру
    {
        Yposition = nt.CenterX(Yposition);
        Length = nt.CenterTimeX(Length);
        if (CheckNoteProperties(Type,Number,Line,Yposition,Length))
        {

            GameObject g = null;
            switch (Type)
            {
                case (NoteEditorType.Note):
                    g = Instantiate(enf.Note, new Vector3(nt.Axis[Line], Yposition, -5), Quaternion.Euler(0, 0, 0));
                    break;
                case (NoteEditorType.Trigger):
                    g = Instantiate(enf.TriggerNote, new Vector3(-0.4f, Yposition, -3), Quaternion.Euler(0, 0, 0));
                    g.GetComponent<NotesForEditor>().Uniq = UniqTrigger[Number];
                    break;
                case (NoteEditorType.Slider):
                    g = Instantiate(enf.Slider, new Vector3(nt.Axis[Line], Yposition, -4), Quaternion.Euler(0, 0, 0));
                    break;
            }

            nt.CheckMaxYNoteInCreatingAndMoving(g);
            NotesForEditor n = g.GetComponent<NotesForEditor>();
            n.nt = nt;
            n.Line = Line;
            n.TimeOfTap = nt.NotePositionToTime(g);
            n.Number = Number;
            n.Type = Type;
            n.nt = nt;
            n.Length = Length;
            n.OnInstantiate();
            nt.DoNoteEvent(EditorNoteAction.Created, g);
            nt.cnc.AddCreateComand(n.NoteUniqId);
            
            nt.Notes.Add(g);
            return g;
        }
        return null;
    }

    public GameObject SpawnNoteWithoutChecks(NoteEditorType Type, int Number, int Line, float Yposition, float Length)
    {
        
        
            GameObject g = null;
            switch (Type)
            {
                case (NoteEditorType.Note):
                    g = Instantiate(enf.Note, new Vector3(nt.Axis[Line], Yposition, -5), Quaternion.Euler(0, 0, 0));
                    break;
                case (NoteEditorType.Trigger):
                    g = Instantiate(enf.TriggerNote, new Vector3(-0.4f, Yposition, -3), Quaternion.Euler(0, 0, 0));
                    g.GetComponent<NotesForEditor>().Uniq = UniqTrigger[Number];
                
                    break;
                case (NoteEditorType.Slider):
                    g = Instantiate(enf.Slider, new Vector3(nt.Axis[Line], Yposition, -4), Quaternion.Euler(0, 0, 0));
                    break;
            }

            nt.CheckMaxYNoteInCreatingAndMoving(g);
        NotesForEditor n = g.GetComponent<NotesForEditor>();
        n.nt = nt;
        n.Line = Line;
        n.TimeOfTap = nt.NotePositionToTime(g);
        n.Number = Number;
        n.Type = Type;
        n.nt = nt;
        n.Length = Length;
        n.OnInstantiate();
        nt.DoNoteEvent(EditorNoteAction.Created, g);
            nt.Notes.Add(g);
            return g;
    }
    public GameObject SpawnNote(GameObject Note)// Спавнит полную копию ноты
    {

        int Line = Note.GetComponent<NotesForEditor>().Line;
        int Number = Note.GetComponent<NotesForEditor>().Number;
        float Length = Note.GetComponent<NotesForEditor>().Length;
        NoteEditorType Type = Note.GetComponent<NotesForEditor>().Type;
        GameObject g = SpawnNote(Type, Number, Line, nt.NotePositionToTime(Note), Length);
        return g;
    }

    public GameObject SpawnSliderInEditor(GameObject First, GameObject Second) // Создает слайдер и добавляет его в массив(во время работы в редакторе)
    {
        GameObject g = null;
        if ((Second.transform.position.y - First.transform.position.y) / nt.NoteSpeed >= 0.1f)
        {
            g = SpawnNote(NoteEditorType.Slider, delta_nt.Number, nt.NearestAxis(nt.Cursor), First.transform.position.y, (Second.transform.position.y - First.transform.position.y) / nt.NoteSpeed);

        }
        else 
        {
            nt.euh.WriteToConsole("Слишком короткий слайдер");
        }
        Destroy(First);
        Destroy(Second);
        return g;
    }

    public bool CheckNoteProperties(NoteEditorType Type, int Number, int Line, float Yposition, float Length)
    {
        if (nt.PlaceIsFree(Type,Yposition, Line))
        {
            if (Yposition >= nt.StartPointY)
            {
                switch (Type)
                {
                    case (NoteEditorType.Trigger):
                        if (Number == 2)
                        {
                            for (int i = 0; i < nt.Notes.Count; i++)
                            {
                                NotesForEditor nfe = nt.Notes[i].GetComponent<NotesForEditor>();
                                if (nfe.Number != 2 && nt.Notes[i].transform.position.y + nfe.Length * nt.NoteSpeed > Yposition)
                                {
                                    nt.euh.WriteToConsole("End of the level must be higher then every note !");
                                    return false;
                                }
                            }
                        }
                        break;

                }
            }
            else { return false; }
            
        }
        else 
        {
            nt.euh.WriteToConsole("Too little space!");
            return false;
        }
        nt.euh.WriteToConsole("");
        return true;
    }
}
