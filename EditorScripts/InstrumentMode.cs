using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InstrumentMode : MonoBehaviour
{
    [Header("Базовая логика инструмента")]
    public NoteTrack nt;

    public bool isActive;
    public bool IsActive 
    {
        get { return isActive; }
        set {
            isActive = value;
            if (isActive)
            {
                StartWork();
            }
            else { EndWork(); }
        }
    }
    public string InstrumentName;
    

    public abstract void StartWork();
    public abstract void EndWork();
    

}
