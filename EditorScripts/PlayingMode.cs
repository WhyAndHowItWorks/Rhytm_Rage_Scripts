using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayingMode : InstrumentMode
{
   
    public AudioSource Music;
    public Sprite PlayS;
    public Sprite PauseS;

    public bool IsPlaying
    {
        get { return isPlaying; }
        set 
        {
            isPlaying = value;
            if (isPlaying)
            {
                nt.euh.PlayButton.GetComponent<Image>().sprite = PauseS;
            }
            else 
            {
                nt.euh.PlayButton.GetComponent<Image>().sprite = PlayS;
            }
        }
    }
    public bool isPlaying;    
    public void Start()
    {
        SpawningMode sm = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
        Music = nt.Music;
    }
    public void Update()
    {
        // Режим проигрывания
        if (IsActive)
        {
            if (IsPlaying)
            {
                nt.CameraYpos =  (nt.Music.time - nt.oh.so.Delay) * nt.NoteSpeed + nt.DeltaCamVal;
            }
            
        }
    }

    public override void StartWork()
    {
        
    }
    public override void EndWork()
    {
        Pause();
    }

    public void PlayFromStart()
    {
        Music.time = 0;
        Music.Play(); 
        IsPlaying = true;
    }
    public void Pause()
    {
        IsPlaying = false;
        Music.Stop();
    }
    public void Play()
    {
        if (IsPlaying)
        {
            IsPlaying = false;
            Music.Stop();
        }
        else 
        {
            IsPlaying = true;
            float d = (nt.CameraYpos - nt.deltaCamVal) / nt.noteSpeed + nt.oh.so.Delay;
            if (d >= 0)
            {
                Music.time = d;
                Music.Play();
            }
            else
            {
                Invoke("DelayedPlay", -d);
            }
        }
       
        

    }
    public void DelayedPlay()
    {
        Music.time = 0;
        Music.Play();
    }
   

}
