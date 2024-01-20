using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mod 
{
    public NoteForGame Note;

    public abstract void StartAction();

    public abstract void UpdateAction();

    public abstract void EndAction(bool Pressed);

    public abstract void InsertValues(object[] values);
    
        
    
}
