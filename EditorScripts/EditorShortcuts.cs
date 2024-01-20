using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorShortcuts : MonoBehaviour
{
    public NoteTrack nt;

   
    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Alpha1) && (nt.spm.delta_nt.Type == NoteEditorType.Note || nt.spm.delta_nt.Type == NoteEditorType.Slider))
        {
            nt.spm.delta_nt.Number = 0;
            nt.euh.WriteToConsole("Note color is red");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && (nt.spm.delta_nt.Type == NoteEditorType.Note || nt.spm.delta_nt.Type == NoteEditorType.Slider))
        {
            nt.spm.delta_nt.Number = 1;
            nt.euh.WriteToConsole("Note color is blue");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && (nt.spm.delta_nt.Type == NoteEditorType.Note || nt.spm.delta_nt.Type == NoteEditorType.Slider))
        {
            nt.spm.delta_nt.Number = 2;
            nt.euh.WriteToConsole("Note color is yellow");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && (nt.spm.delta_nt.Type == NoteEditorType.Note || nt.spm.delta_nt.Type == NoteEditorType.Slider))
        {
            nt.spm.delta_nt.Number = 3;
            nt.euh.WriteToConsole("Note color is green");
        }

      
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            nt.euh.WriteToConsole("Level have been saved");
            nt.tfc.ChangeTrackFile();
        }
        if (!Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.S))
        {
            nt.euh.WriteToConsole("Start from Start!");
            nt.pm.PlayFromStart();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nt.pm.IsPlaying)
            {
                nt.euh.WriteToConsole("Pause");
                nt.euh.Pause();
            }
            else 
            {
                nt.euh.WriteToConsole("Play");
                nt.euh.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            nt.euh.PressRecordingMode();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            nt.euh.Moving();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (nt.euh.HelpPanel.active)
            {
                nt.euh.HelpPanel.SetActive(false);
            }
        }
    }   

}
