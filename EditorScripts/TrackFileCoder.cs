using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
using System.Globalization;
using System;

public class TrackFileCoder : MonoBehaviour
{

    public string path;

    public NotesInformation[] Notes;

    public string NameOfTrack;
    public string PathToMusic;
    public float Bpm;
    public float NoteSpeed;
    public float SliderStep;

    public NoteTrack nt;
    
    public struct NotesInformation
    {
        public string Type;
        public int Number;
        public int Line;
        public float Time;
        public float Length;
    }

    public void Start()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");
    }

    public void ChangeTrackFile()
    {
        if (nt.euh.ChooseTrack.value != 10)
        { 
        path = nt.lm.CustomLevel[nt.euh.ChooseTrack.value].TrackPath;
        StreamWriter sw = new StreamWriter(path, false);
        // Заталкиваем информацию о нотах
        Notes = new NotesInformation[nt.Notes.Count];
        for (int i = 0; i < nt.Notes.Count; i++)
        {
            Notes[i].Line = nt.Notes[i].GetComponent<NotesForEditor>().Line;
            Notes[i].Time = nt.Notes[i].GetComponent<NotesForEditor>().TimeOfTap;
            Notes[i].Type = nt.ConvertNoteEditorTypeToString(nt.Notes[i].GetComponent<NotesForEditor>().Type);
            Notes[i].Number = nt.Notes[i].GetComponent<NotesForEditor>().Number;
            Notes[i].Length = nt.Notes[i].GetComponent<NotesForEditor>().Length;
        }
        // Отсортировать ноты по возрастанию

        NotesInformation note;
        for (int i = 0; i < Notes.Length; i++)
        {
            for (int j = i + 1; j < Notes.Length; j++)
            {
                if (Notes[i].Time > Notes[j].Time)
                {
                    note = Notes[i];
                    Notes[i] = Notes[j];
                    Notes[j] = note;
                }
            }
        }
        sw.WriteLine(NameOfTrack);
        sw.WriteLine(PathToMusic);
        sw.WriteLine(nt.BPM);
        sw.WriteLine(FloatToString(nt.NoteSpeed));
        sw.WriteLine(FloatToString(nt.SliderStep));

        sw.WriteLine(Notes.Length);
        for (int i = 0; i < Notes.Length; i++)
        {
            sw.WriteLine(Notes[i].Type);
            sw.WriteLine(Notes[i].Number);
            sw.WriteLine(FloatToString(Notes[i].Length));
            sw.WriteLine(Notes[i].Line);
            sw.WriteLine(FloatToString(Notes[i].Time));
        }
        sw.WriteLine("Конец файла");


        sw.Close();
    }
    }
    public string FloatToString(float s)
    {
        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        return s.ToString(formatter);
    }
}
