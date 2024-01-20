using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteForGame : MonoBehaviour
{   
    public float Speed;
    public Router rt;
    public NoteInfo noteInfo;

    public List<Mod> Modificators = new List<Mod>();

    
    private void Update()
    {
        transform.Translate(0, -Speed * Time.deltaTime, 0);
        for (int i = 0; i < Modificators.Count; i++)
        {
            Modificators[i].UpdateAction();
        }
    }
    
    public void OnDestroy()
    {
        rt.nam.DeltaTappedNote = noteInfo.UniqId;
        rt.nam.NotesOnScreen.Remove(gameObject.GetComponent<NoteForGame>());
        rt.nam.NotesHistory.Add(noteInfo);                  
       
       
    }
    public void DoEndActions(bool Pressed)
    {
        for (int i = 0; i < Modificators.Count; i++)
        {
            Modificators[i].EndAction(Pressed);
        }
    }
    public void AttachMod(Mod mod, object[] values)
    {
        Modificators.Add(mod);
        mod.InsertValues(values);
        mod.StartAction();
    }

    

}
