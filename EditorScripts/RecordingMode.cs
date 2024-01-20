using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingMode : InstrumentMode
{
    SpawningMode spm;
    // Start is called before the first frame update
    void Start()
    {
        spm = (SpawningMode)nt.Instruments[(int)ToolMode.PlaseNote];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsActive)
        {
            nt.CameraYpos = (nt.Music.time - nt.oh.so.Delay) * nt.NoteSpeed + nt.DeltaCamVal;
            if (Input.GetKeyDown(nt.oh.so.Controls[3]))
            {
                spm.SpawnNote(NoteEditorType.Note, 0, 3, nt.TapLine.transform.position.y, 0);
            }
            if (Input.GetKeyDown(nt.oh.so.Controls[2]))
            {
                spm.SpawnNote(NoteEditorType.Note, 0, 2, nt.TapLine.transform.position.y, 0);
            }
            if (Input.GetKeyDown(nt.oh.so.Controls[1]))
            {
                spm.SpawnNote(NoteEditorType.Note, 0, 1, nt.TapLine.transform.position.y, 0);
            }
            if (Input.GetKeyDown(nt.oh.so.Controls[0]))
            {
                spm.SpawnNote(NoteEditorType.Note, 0, 0, nt.TapLine.transform.position.y, 0);
            }
        }
        
    }
    public override void EndWork()
    {
        nt.Music.Stop();
    }
    public override void StartWork()
    {
        nt.Music.time = nt.NotePositionToTime(nt.CameraYpos) + nt.oh.so.Delay;
        nt.Music.Play();
    }
}
