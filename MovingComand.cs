using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingComand : CancelComand
{

    public Vector3 Pos;
    public int NoteId;

    public void SetInformation(Vector3 OldPos, int NoteId)
    {
        Pos = OldPos;
        this.NoteId = NoteId;
    }

    public override void Undo()
    {
        c.nt.FindNoteById(NoteId).Position = Pos;
    }

}
