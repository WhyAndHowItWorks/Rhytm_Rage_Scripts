using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static NoteTapper;
/// <summary>
/// Базовый класс всех объектов, которые могут реагировать на нажатие клавиш
/// </summary>
public class Actor : MonoBehaviour
{
    public string description; // Описание этого актера
    /// <summary>
    /// Реакция актера на действие с нотой
    /// </summary>
    /// <param name="Pressed"></param>
    /// <param name="nfg"></param>
    public virtual void NoteAction(bool Pressed, NoteForGame nfg)
    {
        
    }
}


