using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingMode : InstrumentMode
{
    public List<GameObject> MovingObjects = new List<GameObject>();
    public SpawningMode sp;
    public SelectingMode sm;

    public List<GameObject> CopiedObjects = new List<GameObject>();
    
    public bool Selecting;
    public bool Moving;
    // Start is called before the first frame update
    void Start()
    {
        sp = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
        sm = (SelectingMode)nt.Instruments[(int)ToolMode.Selecting];
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            if (nt.euh.CursorOnTrack && nt.euh.CursorIsActive)
            {
                if (Input.GetMouseButtonDown(0)) // Выбрать ноту на нажатие левой кнопки мыши(удерживать)
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        GameObject c = nt.FindObjectUnderMouse(nt.cam);
                        //Выбрать объект
                        if (!c.GetComponent<NotesForEditor>().IsChosen)
                        {
                            sm.ChosenObjects.Add(c);
                            c.GetComponent<NotesForEditor>().IsChosen = true;
                        }
                        else
                        {
                            sm.ChosenObjects.Remove(c);
                            c.GetComponent<NotesForEditor>().IsChosen = false;
                        }
                    }
                    else 
                    {
                        if (nt.FindObjectUnderMouse(nt.cam) != null)
                        {
                           
                            // Если нота не выбрана, то перемещает только её
                            GameObject c = nt.FindObjectUnderMouse(nt.cam);
                            if (!c.GetComponent<NotesForEditor>().IsChosen)
                            {
                                NotesForEditor n = c.GetComponent<NotesForEditor>();
                                n.IsMovingByCursor = true;
                                nt.cnc.AddMovingComand(n.NoteUniqId,n.Position);
                            }
                            else // Если нота выбрана, то переместит все
                            {
                                int oldCount = nt.cnc.History.Count;
                                SelectingMode sm = (SelectingMode)nt.Instruments[(int)ToolMode.Selecting];
                                foreach (GameObject g in sm.ChosenObjects)
                                {
                                    NotesForEditor n = g.GetComponent<NotesForEditor>();
                                    n.IsMovingByCursor = true;
                                    nt.cnc.AddMovingComand(n.NoteUniqId, n.Position);
                                }
                                nt.cnc.AddUniteComand(nt.cnc.History.Count - oldCount);
                            }
                        }
                        else
                        {
                            Selecting = true;

                            nt.FirstDot.transform.position = new Vector3(nt.euh.ClickX, nt.euh.ClickY, -1);
                            if (nt.FindObjectUnderMouse(nt.cam) == null) // Если клик по пустому месту, то отменяет выделение для всех нот
                            {
                                List<GameObject> delta = new List<GameObject>();
                                foreach (GameObject g in sm.ChosenObjects)
                                {
                                    delta.Add(g);
                                }
                                foreach (GameObject g in delta)
                                {
                                    g.GetComponent<NotesForEditor>().IsChosen = false;
                                }

                            }
                        }
                    }
                    
                }
                if (Input.GetMouseButton(0))
                {
                    if (Selecting)
                    {
                        nt.SecondDot.transform.position = new Vector3(nt.euh.ClickX, nt.euh.ClickY, -1);
                        Vector2 _1pos = nt.FirstDot.transform.position;
                        Vector2 _2pos = nt.SecondDot.transform.position;
                        Vector2 pos = new Vector2((_1pos.x + _2pos.x) / 2, (_1pos.y + _2pos.y) / 2);
                        Vector2 size = new Vector2(Mathf.Abs(_1pos.x - _2pos.x), Mathf.Abs(_1pos.y - _2pos.y));

                        nt.SelectingArea.GetComponent<BoxCollider>().size = new Vector3(size.x, size.y, 100);
                        nt.SelectingArea.GetComponent<SpriteRenderer>().size = new Vector2(size.x, size.y);
                        nt.SelectingArea.transform.position = pos;
                    }
                }
                if (Input.GetMouseButtonUp(0)) // Поставить ноту на отжатие левой кнопки мыши
                {
                    if (Selecting)
                    {
                        Selecting = false;
                        foreach (GameObject g in nt.SelectingArea.GetComponent<SelectArea>().SelectedObjects)
                        {
                            g.GetComponent<NotesForEditor>().IsChosen = true;
                            g.GetComponent<NotesForEditor>().IsChosenByArea = false;
                        }
                        nt.SelectingArea.transform.position = new Vector3(-100, -100);
                        nt.FirstDot.transform.position = new Vector3(-100, -100);
                        nt.SecondDot.transform.position = new Vector3(-100, -100);
                    }
                    else 
                    {
                        List<GameObject> delta = new List<GameObject>();
                        foreach (GameObject g in MovingObjects)
                        {
                            PlaseNoteAfterMoving(g);
                            delta.Add(g);
                        }
                        foreach (GameObject g in delta)
                        {
                            g.GetComponent<NotesForEditor>().IsMovingByCursor = false;
                        }
                    }
                                  

                }
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C))
                {
                    nt.euh.WriteToConsole("Copy!");
                    if (sm.ChosenObjects.Count > 0)
                    {
                        int oldCount = nt.cnc.History.Count;
                        List<GameObject> delta = new List<GameObject>();
                        foreach (GameObject g in sm.ChosenObjects)
                        {
                            if (g != null)
                            {
                                delta.Add(g);
                            }
                            
                        }
                        foreach (GameObject g in delta)
                        {
                            NotesForEditor f = g.GetComponent<NotesForEditor>();
                            GameObject d = sp.SpawnNoteWithoutChecks(f.Type, f.Number, f.Line, f.Position.y, f.Length);
                            d.GetComponent<NotesForEditor>().TimeOfTap += nt.NoteTimeToTactTime(0.0625f);
                            f.IsChosen = false;
                            d.GetComponent<NotesForEditor>().IsChosen = true;
                            nt.cnc.AddCreateComand(d.GetComponent<NotesForEditor>().NoteUniqId);
                            
                        }
                        nt.cnc.AddUniteComand(nt.cnc.History.Count - oldCount);
                       
                    }
                }
               
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
                            int oldCount = nt.cnc.History.Count;
                            SelectingMode sm = (SelectingMode)nt.Instruments[(int)ToolMode.Selecting];
                            foreach (GameObject g in sm.ChosenObjects)
                            {
                                DeletingMode dm = (DeletingMode)nt.Instruments[(int)ToolMode.Deleting];
                                dm.DeleteNote(g);

                            }
                            sm.ChosenObjects.Clear();
                            nt.cnc.AddUniteComand(nt.cnc.History.Count - oldCount);
                        }
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
        
    }
    public void PlaseNoteAfterMoving(GameObject g)
    {
        if (nt.NeedCenterX)
        {
            g.transform.position = new Vector3(g.transform.position.x, nt.CenterX(g.transform.position.y), -5);
        }
        if (g.GetComponent<NotesForEditor>().Type == NoteEditorType.Note || g.GetComponent<NotesForEditor>().Type == NoteEditorType.Slider) // Если обычная нота, то установить на ближайшую линию от курсора
        {
            g.transform.position = new Vector3(nt.Axis[nt.NearestAxis(g)], g.transform.position.y, -5);
            g.GetComponent<NotesForEditor>().Line = nt.NearestAxis(g);
        }
        else { g.transform.position = new Vector3(-0.4f, g.transform.position.y, -5); }// Если триггер, то установить по центру

        nt.DoNoteEvent(EditorNoteAction.Moved, g);
        
    }
}
