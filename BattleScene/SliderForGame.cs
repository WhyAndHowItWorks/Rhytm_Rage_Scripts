using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderForGame : NoteForGame
{
   
    public GameObject Dot; // Точка 
    public float Length; // Длина слайдера в секундах
    public BoxCollider2D bcl;
    public BoxCollider2D bclToDestroy;
    public bool HasAction;
   
    public float Size
    {
        get
        {
            return size;
        }
        set
        {
            size = value;
            Up.transform.position = gameObject.transform.position + new Vector3(0, size * 2, -1);
            Middle.transform.position = gameObject.transform.position + new Vector3(0, size, 0);
            Middle.transform.localScale = new Vector3(1, size*0.73f, 1);
            Middle.GetComponent<BoxCollider2D>().size = new Vector2(1, 1.37f);
        }
    }

    public float size;
    

    public GameObject Up;
    public GameObject Down;
    public GameObject Middle;
    

    private void Update()
    {
       
        transform.Translate(0, -Speed * Time.deltaTime, 0);
    }

    public void PrintDots(float NoteSpeed, float BPM)
    {
        
        float distance = noteInfo.SliderLength * NoteSpeed;
        float kolvotochek = Convert.ToInt16(noteInfo.SliderLength * 8 / 120 * BPM) -1;
        if (kolvotochek == 0)
        {
            kolvotochek = 1;
        }

        for (float i = 0; i <= kolvotochek; i++)
        {
            
            GameObject d = Instantiate(Dot, transform.position + new Vector3(0,distance*(i/ kolvotochek),-1), transform.rotation);
           
            d.transform.SetParent(gameObject.transform);
            SliderDot dot = d.GetComponent<SliderDot>();
            dot.slider = this;
            dot.noteInfo = new NoteInfo(rt.ntgs.NoteUniqId,999, NoteType.SliderDot, noteInfo.Color, noteInfo.TimeToTap + Length * (i / kolvotochek), noteInfo.Line, 0);
            dot.rt = rt;
            rt.nam.NotesOnScreen.Add(dot);

            if (i == 0) // Начальная точка 
            {
                d.GetComponent<SliderDot>().index = SliderDotType.Begin;
            }
           else if (i == kolvotochek ) // Конечная точка
            {
                d.GetComponent<SliderDot>().index = SliderDotType.End;
            }
            
        }
        
    }
}
