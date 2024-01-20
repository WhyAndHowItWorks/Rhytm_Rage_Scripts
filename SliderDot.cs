using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderDot : NoteForGame
{
    public SliderDotType index;
    public SliderForGame slider;
}
public enum SliderDotType : int
{
    Middle,
    Begin,
    End
}
