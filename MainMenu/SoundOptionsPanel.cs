using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundOptionsPanel : MonoBehaviour
{
    public Slider MainSoundChanger;
    public Text MainSoundChanger_text;

    public Slider HitSoundVolumeChanger;
    public Text HitSoundChanger_text;

    public OptionsHandler oh;
    public OptionsHandler OH 
    {
        get
        {
            if (oh != null)
            {
                return oh;
            }
            else 
            {
                oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
                return oh;
            }
        }
        set { oh = value; }
    }

    public void Start()
    {
        Invoke("LateStart", 0.1f);
    }
    public void LateStart()
    {
        MainSoundChanger.SetValueWithoutNotify(OH.Volume * 100);
        MainSoundChanger_text.text = (OH.Volume * 100).ToString();
        HitSoundVolumeChanger.SetValueWithoutNotify(OH.HitSoundVolume * 100);
        HitSoundChanger_text.text = (OH.HitSoundVolume * 100).ToString();
    }
    public void MainSoundChanged()
    {
        OH.Volume = MainSoundChanger.value / 100f;
        MainSoundChanger_text.text = MainSoundChanger.value.ToString();
    }
    public void HitSoundChanged()
    {
        OH.HitSoundVolume = HitSoundVolumeChanger.value / 100f;
        HitSoundChanger_text.text = HitSoundVolumeChanger.value.ToString();
    }

}
