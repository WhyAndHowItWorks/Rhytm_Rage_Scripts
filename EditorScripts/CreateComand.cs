using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateComand : CancelComand
{
    public int NoteId;
    public void SetInformation(int NoteId)
    {
        this.NoteId = NoteId;
    }
    public override void Undo()
    {
        NotesForEditor n = c.nt.FindNoteById(NoteId);
        if (n.Number == 0 && n.Type == NoteEditorType.Trigger)
        {
            c.nt.spm.ChangeFhaseTriggers.Remove(n.gameObject);
            n.PhaseCheck();
        }
        Destroy(n.gameObject);
        
    }
}
