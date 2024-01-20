using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;


public class TrackLoder : MonoBehaviour
{
    public Level[] Level = new Level[2];
    public LevelManager lm;

    public string NameOfTrack;
    public string PathToMusic;
    public float Bpm;
    public float NoteSpeed;
    public float SliderStep;

    public List<string> Types = new List<string>();
    public List<int> Colors = new List<int>();
    public List<float> Slider_Length = new List<float>();
    public List<int> Lines = new List<int>();
    public List<float> Time = new List<float>();
    public string path;

    
    

    StreamReader sr;


    public void LoadFile(Level level)
    {
        path = level.TrackPath;
        
        sr = new StreamReader(path); // !!!Добавить окно диалога выбоа трека!!!
        
        // Считывание основных настроек трека
        NameOfTrack = sr.ReadLine();
        PathToMusic = sr.ReadLine();

        Bpm = FloatParse();
        NoteSpeed = FloatParse();
        SliderStep = FloatParse();

        Types.Clear();
        Colors.Clear();
        Slider_Length.Clear();
        Lines.Clear();
        Time.Clear();
        //Считать все данные и передать их треку
        int length = int.Parse(sr.ReadLine());
          
        for (int i = 0; i < length; i++)
        {    
            Types.Add(sr.ReadLine());
            Colors.Add(int.Parse(sr.ReadLine()));
            Slider_Length.Add(FloatParse());
            Lines.Add(int.Parse(sr.ReadLine()));
            Time.Add(FloatParse());
        }
       
        sr.Close();
    }
    public NoteType ConvertStringToNoteType(string value)
    {
        NoteType nt = NoteType.Note;
        switch (value)
        {
            case ("Нажатие"):
                nt = NoteType.Note;
                break;
            case ("Триггер"):
                nt = NoteType.Trigger;
                break;
            case ("Слайдер"):
                nt = NoteType.Slider;
                break;
        }
        return nt;
    }

    
    public float FloatParse()
    {
        
        float res;
        string s = sr.ReadLine();
       
        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        res = float.Parse(s, formatter);
        
        return res;
    }
}
