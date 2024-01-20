using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking.Types;

public class NoteActionManager : MonoBehaviour
{
    [Header("Администраторы")]
    public Router rt;
    [Header("Все для установки действий нотам")]
    public int DeltaTappedNote;
    public float TimeToEvokeOnScreen;
    public List<NoteInfo> FutureNotes = new List<NoteInfo>(); // Еще не появившиеся ноты
    public List<NoteForGame> NotesOnScreen = new List<NoteForGame>(); // Все ноты на поле
    public List<NoteInfo> NotesHistory = new List<NoteInfo>(); // Уничтоженные ноты
    public List<FutureMods> NoteFutureMods = new List<FutureMods>();
    public struct FutureMods 
    {
        public int IdinTrack;
        public Mod mod;
        public object[] values;
        public FutureMods(int IdinTrack, Mod mod, object[] values)
        {
            this.IdinTrack = IdinTrack;
            this.mod = mod;
            this.values = values;
        }
    }

   
    public void CheckNoteMods(NoteForGame nfg)
    {
        
        for (int i = 0; i < NoteFutureMods.Count; i++)
        {
            if (nfg.noteInfo.IdInTrack == NoteFutureMods[i].IdinTrack)
            {
                nfg.AttachMod(NoteFutureMods[i].mod, NoteFutureMods[i].values);
                NoteFutureMods.RemoveAt(i);
                i--;
            }
        }
        
      
    }

    public NoteForGame FindNoteOnScreen(int NoteId)
    {
        for (int i = 0; i < NotesOnScreen.Count; i++)
        {
            if (NotesOnScreen[i].noteInfo.IdInTrack == NoteId)
            {
                return NotesOnScreen[i];
            }
        }
        return null;
    }
    public NoteInfo FindNoteOnFutureNote(int NoteId)
    {
        for (int i = 0; i < FutureNotes.Count; i++)
        {
            if (FutureNotes[i].IdInTrack == NoteId)
            {
                return FutureNotes[i];
            }
        }
        return null;
    }

    public NoteInfo FoundNoteById(int NoteId)
    {
        NoteForGame nfg = FindNoteOnScreen(NoteId);
        if (nfg != null)
        {
            return nfg.noteInfo;
        }
        else 
        {
            return FindNoteOnFutureNote(NoteId);
        }
    }
    
    void AttachModToNote(NoteForGame nfg,Mod mod, object[] values)
    {
        nfg.AttachMod(mod, values);
    }
   
    void AttachModToFutureNote(int IdInTrack, Mod mod, object[] values)
    {
        NoteFutureMods.Add(new FutureMods(IdInTrack, mod, values));
    }
    public void AttachModToNote(int IdInTrack, Mod mod, object[] values)
    {
        NoteForGame nfg = FindNoteOnScreen(IdInTrack);
        if (nfg != null)
        {
            AttachModToNote(nfg, mod, values);
        }
        else 
        {
            AttachModToFutureNote(IdInTrack, mod, values);
        }
    }
  
    public float TimeFromNowToNote(NoteInfo Note)
    {
        return ( Note.TimeToTap - rt.ntgs.Music.time) / (1 + rt.ntgs.RunNumber * 0.5f); 
    }

    public float TimeFromNowToNote(int Note)
    {
       NoteInfo noteinfo =  FoundNoteById(Note);
        return (noteinfo.TimeToTap - rt.ntgs.Music.time) / (1 + rt.ntgs.RunNumber * 0.5f);
    }
    /// <summary>
    /// Если возращает 99999, то неполучилось найти ноту
    /// </summary>
    /// <param name="StartNote"></param>
    /// <param name="MinTime"></param>
    /// <returns></returns>
    public int FoundNoteWithTime(NoteInfo StartNote, float MinTime)
    {
        // Ищет подходящую ноту на экране, если такой нету, то ищет в будущих нотах. Возращает Id ноты
        float MaxTime = 0f;
        NoteInfo foundnote = null ;
        float delta;
        for (int i = 0; i < NotesOnScreen.Count; i++)
        {
            if (NotesOnScreen[i].noteInfo.type != NoteType.SliderDot && NotesOnScreen[i].noteInfo.type != NoteType.Trigger)
            {
                delta = TimeBetweenNotes(StartNote, NotesOnScreen[i].noteInfo);
                if (MaxTime < delta)
                {
                    MaxTime = delta;
                    foundnote = NotesOnScreen[i].noteInfo;
                }
                if (delta > MinTime)
                {           
                    return NotesOnScreen[i].noteInfo.IdInTrack;
                }
            }
        }
        for (int i = 0; i < FutureNotes.Count; i++)
        {
            if (FutureNotes[i].type != NoteType.Trigger)
            {
                delta = TimeBetweenNotes(StartNote, FutureNotes[i]);
                if (MaxTime < delta)
                {
                    MaxTime = delta;
                    foundnote = FutureNotes[i];
                }

                if (delta > MinTime)
                {
                    return FutureNotes[i].IdInTrack;
                }
            }        
        }
        if (foundnote == null)
        {
            return 99999;
        }
        else 
        {
            return foundnote.IdInTrack;
        }
        
        
    }
   


    public float TimeBetweenNotes(NoteInfo _1_ni, NoteInfo _2_ni)
    {
        return (_2_ni.TimeToTap - _1_ni.TimeToTap)/(1+rt.ntgs.RunNumber*0.5f);
    }
    public float TimeBetweenNotes(NoteInfo _1_ni, int _2_ni)
    {
        NoteInfo _2 = FoundNoteById(_2_ni);
        return (_2.TimeToTap - _1_ni.TimeToTap) / (1 + rt.ntgs.RunNumber * 0.5f);
    }

}
