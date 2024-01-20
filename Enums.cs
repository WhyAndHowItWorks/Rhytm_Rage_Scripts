using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteEditorType : int
{
    Note,
    Slider,
    Trigger
}
public enum TrackParameter : int
{
    BPM,
    NoteSpeed,
    MaxY
}
public enum ToolMode : int
{
    No,
    PlaseNote,
    Deleting,
    Playing,
    Moving,
    Recording,
    Selecting
}
public enum EditorNoteAction : int
{
    Created,
    Moved,
    Deleted
}
public enum OptionVarName : int
{
    Volume,
    HitSoundVolume

}

public enum EffectType : int
{
    Heal,
    DamageUp,
    DamageDown,
    ChargeUp,
    ChargeDown,
}
public class ComparerNote : IComparer<GameObject>
{
    public int Compare(GameObject? p1, GameObject? p2)
    {
        if (p1.transform.position.y > p2.transform.position.y)
        {
            return 1;
        }
        else if (p1.transform.position.y < p2.transform.position.y)
        {
            return -1;
        }
        else { return 0; }
    }
}

public enum EnemyAction:int
{
    Created,
    Moved,
    Died

}


