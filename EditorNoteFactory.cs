using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNoteFactory : MonoBehaviour
{
    [Header("����������")]
    public NoteTrack nt;

    [Header("�����")]
    public Color[] colors = new Color[4];
    public float NoteOffset//����������� ����� ����� ������
    {
        get
        {
            noteOffset = 0.0625f * nt.NoteSpeed / nt.BPM * nt.standardBPM;
            return noteOffset;
        }
        set { noteOffset = value; }
    }
    public float noteOffset;

    [Header("������� ����")]
    public GameObject Note;
    public Sprite NoteSprite;

    [Header("�������")]
    public GameObject TriggerNote;
    public Sprite TriggerSprite;
    public Color TriggerColor;
    public List<string> TriggerTitles = new List<string>();
    public List<string> TriggerEnemyTitles = new List<string>();

    [Header("�������")]
    public GameObject Slider;
    public Sprite SliderCap;
    public Sprite SliderMiddle;
    public GameObject SliderPoint;
    
   
   
    



    public void Start()
    {

        NoteOffset = 0.0625f * nt.NoteSpeed * nt.BPM / nt.standardBPM;
    }



}
