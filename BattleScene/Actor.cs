using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for all actors on the battlefield
/// </summary>
public class Actor : MonoBehaviour
{
    public string description; // Описание этого актера
    /// <summary>
    /// Reaction for NoteEvent
    /// </summary>
    /// <param name="Pressed"></param>
    /// <param name="nfg"></param>
    public virtual void NoteAction(bool Pressed, NoteForGame nfg)
    {
        
    }
}


