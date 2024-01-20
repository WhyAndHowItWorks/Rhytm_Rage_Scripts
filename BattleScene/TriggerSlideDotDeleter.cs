using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteTapper;

public class TriggerSlideDotDeleter : MonoBehaviour
{
    public int WhatLine;
    public NoteTapper nt;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Slider Dot")
        {
            nt.NoteLines_sl[WhatLine].Remove(collision.gameObject);
            nt.DoNoteEvent(false, collision.gameObject.GetComponent<SliderDot>());
            if (collision.gameObject.GetComponent<SliderDot>().index == SliderDotType.Begin)
            {
                collision.gameObject.GetComponent<SliderDot>().slider.DoEndActions(false);
            }
            Destroy(collision.gameObject);
        }
        
    }   
}
