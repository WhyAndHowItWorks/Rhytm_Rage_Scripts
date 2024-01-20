using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletingMode : InstrumentMode
{
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
                    GameObject c = nt.FindObjectUnderMouse(nt.cam);
                    if (c != null)
                    {
                    //Если не нота не выбрана, удаляет только её
                    if (!c.GetComponent<NotesForEditor>().IsChosen)
                    {
                        DeleteNote(c);
                    }
                    else // Если нота выбрана, то удаляет все выбранные
                    {
                        SelectingMode sm = (SelectingMode)nt.Instruments[(int)ToolMode.Selecting];     
                        foreach (GameObject g in sm.ChosenObjects)
                        {
                                DeleteNote(g);
                                
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
    public void DeleteNote(GameObject g)
    {
        nt.cnc.AddDeleteComand(g.GetComponent<NotesForEditor>());
        nt.DoNoteEvent(EditorNoteAction.Deleted, g);
        Destroy(g);
    }
    
}
