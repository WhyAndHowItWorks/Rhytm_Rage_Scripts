using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownStick : MonoBehaviour
{
    public NoteTrack nt;
    public PlayingMode pm;
    public void Start()
    {
        pm = (PlayingMode)nt.Instruments[(int)ToolMode.Playing];
    }
    
    public void OnTriggerEnter(Collider other)
    {
        NotesForEditor nte = other.gameObject.GetComponent<NotesForEditor>();
        if (nte.Type == NoteEditorType.Trigger)
        {
            Debug.Log("Триггер");
        }
    }

}
