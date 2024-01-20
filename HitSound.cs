using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    public Router rt;
    public AudioSource au;
    public AudioClip Hit;

    public void Start()
    {
        rt.nt.NoteActedEvent += NoteAction;
        rt.oh.OnVarChange += ValueChange;
        au.volume = rt.oh.HitSoundVolume;
    }

    public void ValueChange(OptionVarName v, float Value)
    {
        if (v == OptionVarName.HitSoundVolume)
        {
            au.volume = Value;
        }
    }
    public void OnDestroy()
    {
        rt.oh.OnVarChange -= ValueChange;
    }

    public void NoteAction(bool IsPressed, NoteForGame nfg)
    {
        if (nfg.noteInfo.type == NoteType.Note && IsPressed)
        {
            au.PlayOneShot(Hit);
        }
    }
    public void PlayHitSound()
    {
        au.PlayOneShot(Hit);
    }
}
