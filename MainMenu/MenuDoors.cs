using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDoors : MonoBehaviour
{
    public bool SoundTrigger;
    bool oldSoundTrigger;

    public AudioSource au;
    public AudioClip sound;
   
    public Animator an;
    public OptionsHandler oh;

    public bool IsActive;
    
    public bool FirstTime;

    public void Start()
    {

        Invoke("LateStart", 0.5f);
             
    }
    public void LateStart()
    {
        oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        au.volume = oh.Volume;
        oh.OnVarChange += VolumeChanged;
        Invoke("OpenDoors", 0.5f);
    }

    public void OnDestroy()
    {
        oh.OnVarChange -= VolumeChanged;
    }
    public void VolumeChanged(OptionVarName var, float value)
    {
        Debug.Log("FFFF");
        if (var == OptionVarName.Volume)
        {
            au.volume = value;
        }

    }
    public void OpenDoors()
    {
        an.Play("DoorOpen");
       
    }
    public void CloseDoors()
    {
        an.Play("DoorClose");
        
    }
    public void DoorsAnim()
    {
        
        if (!an.GetCurrentAnimatorStateInfo(0).IsName("DoorsAnim"))
        {
            an.Play("DoorsAnim");
        }
    }
    public void Update()
    {
        if (SoundTrigger != oldSoundTrigger)
        {
            
            if (SoundTrigger)
            {
                Debug.Log("Звук");
                au.PlayOneShot(sound);
            }
            oldSoundTrigger = SoundTrigger;
            
        }
        
    }
}
