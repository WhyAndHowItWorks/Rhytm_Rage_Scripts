using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectingMode : InstrumentMode
{
    public List<GameObject> ChosenObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsActive)
        {
            if (nt.euh.CursorIsActive && nt.euh.CursorOnTrack)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    nt.FirstDot.transform.position = new Vector3(nt.euh.ClickX, nt.euh.ClickY, -1);
                    if (nt.FindObjectUnderMouse(nt.cam) == null) // Если клик по пустому месту, то отменяет выделение для всех нот
                    {
                        List<GameObject> delta = new List<GameObject>();
                        foreach (GameObject g in ChosenObjects)
                        {
                            
                            delta.Add(g);                          
                        }
                        foreach (GameObject g in delta)
                        {
                            g.GetComponent<NotesForEditor>().IsChosen = false;
                        }
                      
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    nt.SecondDot.transform.position = new Vector3(nt.euh.ClickX, nt.euh.ClickY, -1);
                    Vector2 _1pos = nt.FirstDot.transform.position;
                    Vector2 _2pos = nt.SecondDot.transform.position;
                    Vector2 pos = new Vector2((_1pos.x + _2pos.x) / 2, (_1pos.y + _2pos.y) / 2);
                    Vector2 size = new Vector2(Mathf.Abs(_1pos.x - _2pos.x), Mathf.Abs(_1pos.y - _2pos.y));

                    nt.SelectingArea.GetComponent<BoxCollider>().size = new Vector3(size.x, size.y, 100);
                    
                    nt.SelectingArea.transform.position = pos;


                }
                if (Input.GetMouseButtonUp(0))
                {
                    foreach (GameObject g in nt.SelectingArea.GetComponent<SelectArea>().SelectedObjects)
                    {
                        NotesForEditor nfe = g.GetComponent<NotesForEditor>();
                        if (!g.GetComponent<NotesForEditor>().IsChosen && !(nfe.Type == NoteEditorType.Trigger && nfe.Number == 2))
                        {
                            g.GetComponent<NotesForEditor>().IsChosen = true;
                            ChosenObjects.Add(g);
                        }
                        
                        g.GetComponent<NotesForEditor>().IsChosenByArea = false;
                    }
                    nt.SelectingArea.transform.position = new Vector3(-100, -100);
                    nt.FirstDot.transform.position = new Vector3(-100, -100);
                    nt.SecondDot.transform.position = new Vector3(-100, -100);
                }
                if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift)) // Выбрать множество нот для массового перемещения
                {
                   
                    GameObject c = nt.FindObjectUnderMouse(nt.cam);
                    NotesForEditor nfe = c.GetComponent<NotesForEditor>();
                    //Выбрать объект
                    if (!c.GetComponent<NotesForEditor>().IsChosen && !(nfe.Type == NoteEditorType.Trigger && nfe.Number == 2))
                    {
                        ChosenObjects.Add(c);
                        c.GetComponent<NotesForEditor>().IsChosen = true;
                    }
                    else
                    {
                        ChosenObjects.Remove(c);
                        c.GetComponent<NotesForEditor>().IsChosen = false;
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
            }
        }
    }
    public override void StartWork()
    {
        
    }
    public override void EndWork()
    {
       
    }

    public void SelectAll()
    {

       
    }

}
