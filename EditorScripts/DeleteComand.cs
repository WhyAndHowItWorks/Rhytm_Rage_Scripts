using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteComand : CancelComand
{
    NoteEditorType Type;
    int Number;
    int Line;
    float Yposition;
    float Length;
    bool IsChosen;
    int UniqId;
    public void SetInformaion(NoteEditorType Type, int Number, int Line, float Yposition, float Length, bool IsChosen, int UniqId)
    {
        this.Type = Type;
        this.Number = Number;
        this.Line = Line;
        this.Yposition = Yposition;
        this.Length = Length;
        this.IsChosen = IsChosen;
        this.UniqId = UniqId;
    }
    public override void Undo()
    {

        GameObject g = null;
        switch (Type)
        {
            case (NoteEditorType.Note):
                g = Instantiate(c.enf.Note, new Vector3(c.nt.Axis[Line], Yposition, -5), Quaternion.Euler(0, 0, 0));
                break;
            case (NoteEditorType.Trigger):
                g = Instantiate(c.enf.TriggerNote, new Vector3(0, Yposition, -3), Quaternion.Euler(0, 0, 0));
                g.GetComponent<NotesForEditor>().Uniq = c.nt.spm.UniqTrigger[Number];

                break;
            case (NoteEditorType.Slider):
                g = Instantiate(c.enf.Slider, new Vector3(c.nt.Axis[Line], Yposition, -4), Quaternion.Euler(0, 0, 0));
                break;
        }

        c.nt.CheckMaxYNoteInCreatingAndMoving(g);
        NotesForEditor n = g.GetComponent<NotesForEditor>();
        n.nt = c.nt;
        n.Line = Line;
        n.TimeOfTap = c.nt.NotePositionToTime(g);
        n.Number = Number;
        n.Type = Type;
        
        n.Length = Length;
        n.IsChosen = IsChosen;
        n.OnInstantiate();
        if (n.Number == 0 && n.Type == NoteEditorType.Trigger)
        {
            c.nt.spm.ChangeFhaseTriggers.Add(g);
            n.PhaseCheck();
        }
        n.NoteUniqId = UniqId;
        c.nt.Notes.Add(g);
        IsCancelled = true;
    }
}
