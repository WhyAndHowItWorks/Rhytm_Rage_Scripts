using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteInfo 
{
    public int UniqId;
    public int IdInTrack;
    public int Color;
    public float TimeToTap;
    public NoteType type;
    public float SliderLength;
    public int Line;


    
    public NoteInfo(int UniqId,int IdInTrack,NoteType type, int Color,  float TimeToTap, int line, float sliderLength)
    {
        this.UniqId = UniqId;
        this.IdInTrack = IdInTrack;
        this.Color = Color;
        this.TimeToTap = TimeToTap;
        this.type = type;
        SliderLength = sliderLength;
        Line = line;
    }
    public NoteInfo()
    {

    }
}
