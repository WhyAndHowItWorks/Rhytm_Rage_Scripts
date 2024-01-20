using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteGraffic : Actor
{
    public GameObject[] Effects = new GameObject[4];
    public Transform[] PlacesToEffect = new Transform[4];
    
    public Router rt;
    public float DestroyTime;
   
    public GameObject[] SliderEffect = new GameObject[4];
    public GameObject[] CurrentEffect = new GameObject[4];

    

   

    void Start()
    {
        rt.nt.NoteActedEvent += NoteAction;
    }

    // Update is called once per frame
    void Update()
    {
     
       
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetKeyDown(rt.oh.so.Controls[i]) && rt.nt.IsSlider[i])
            {
                StartSliderEffect(i);
                rt.ht.PlayHitSound();
            }
            if (Input.GetKeyUp(rt.oh.so.Controls[i]))
            {
                StopSliderEffect(i);
            }
        }

    }
    public void StartSliderEffect(int Line)
    {
        if (CurrentEffect[Line] != null)
        {
            Destroy(CurrentEffect[Line]);
            CurrentEffect[Line] = null;
        }
        CurrentEffect[Line] = Instantiate(SliderEffect[rt.nt.sliderInfo[Line].noteInfo.Color], PlacesToEffect[Line].position, PlacesToEffect[Line].rotation);
    }
    public void StopSliderEffect(int Line)
    {
        if (CurrentEffect[Line] != null)
        {
            Destroy(CurrentEffect[Line]);
            CurrentEffect[Line] = null;
        }
    }
    public void SliderTapCheck(int Line)
    {
        if (Input.GetKey(rt.oh.so.Controls[Line]))
        {
            if (rt.nt.IsSlider[Line])
            {
                StartSliderEffect(Line);
                
            }
            else 
            {
                StopSliderEffect(Line);
            }
            
        }
    }
    public override void NoteAction(bool Pressed, NoteForGame nfg)
    {
        if (Pressed && nfg.noteInfo.type == NoteType.Note)
        {
            GameObject g = Instantiate(Effects[nfg.noteInfo.Color], PlacesToEffect[nfg.noteInfo.Line].position, PlacesToEffect[nfg.noteInfo.Line].rotation);
            Destroy(g, DestroyTime);
        }
    }

}
