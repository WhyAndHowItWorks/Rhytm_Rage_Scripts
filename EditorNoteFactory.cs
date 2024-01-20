using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNoteFactory : MonoBehaviour
{
    [Header("Компоненты")]
    public NoteTrack nt;

    [Header("Общее")]
    public Color[] colors = new Color[4];
    public float NoteOffset//Необходимое место между нотами
    {
        get
        {
            noteOffset = 0.0625f * nt.NoteSpeed / nt.BPM * nt.standardBPM;
            return noteOffset;
        }
        set { noteOffset = value; }
    }
    public float noteOffset;

    [Header("Обычная нота")]
    public GameObject Note;
    public Sprite NoteSprite;

    [Header("Триггер")]
    public GameObject TriggerNote;
    public Sprite TriggerSprite;
    public Color TriggerColor;
    public List<string> TriggerTitles = new List<string>();
    public List<string> TriggerEnemyTitles = new List<string>();

    [Header("Слайдер")]
    public GameObject Slider;
    public Sprite SliderCap;
    public Sprite SliderMiddle;
    public GameObject SliderPoint;
    
   
   
    



    public void Start()
    {

        NoteOffset = 0.0625f * nt.NoteSpeed * nt.BPM / nt.standardBPM;
    }



}
