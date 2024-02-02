using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
    public Router rt;
    public bool IsPaused
    {
        get { return isPaused; }
        set { 
            isPaused = value;
            if (IsPaused)
            {
                PausePanel.SetActive(true);
                for (int i = 0; i < 4; i++)
                {
                    ButtonControls[i].text = rt.oh.so.Controls[i].ToString();
                }
                VolumeText.text = (rt.oh.Volume*100).ToString();
                VolumeSlider.SetValueWithoutNotify(rt.oh.Volume * 100);
                HitSoundVolumeText.text = (rt.oh.HitSoundVolume * 100).ToString();
                HitSoundVolumeSlider.SetValueWithoutNotify(rt.oh.HitSoundVolume * 100);
                PauseMusic.Play();
                PauseMusic.volume = rt.oh.Volume;
               
            }
            else 
            {
                rt.st.PausePanel.SetActive(false);
                PausePanel.SetActive(false);
                SettingsPanel.SetActive(false);
                PauseMusic.time = 0;
                PauseMusic.Stop();
                rt.oh.WriteOptionsToFile();
            }
        }
    }
    public bool isPaused;
    public GameObject PausePanel;
    public GameObject SettingsPanel;

    [Header("Переменные настроек")]
    public Slider VolumeSlider;
    public Text VolumeText;
    public AudioSource PauseMusic;

    public Slider HitSoundVolumeSlider;
    public Text HitSoundVolumeText;
    

    [Header("Смена управления")]
    public bool WaitingLetter;
    int LetterIndex;
    public Text MessageText;
    public Text[] ButtonControls = new Text[4];
    public void Restart()
    {
        rt.st.Quiting = true;
        rt.sct.LoadScene(1, false);
        rt.st.IsPaused = false;
        rt.st.CancelUnpause();
        rt.st.ChangeText("Restarting...");
        
    }
    public void Quit()
    {
        
        rt.st.Quiting = true;
        rt.sct.LoadScene(0, false);
        rt.st.IsPaused = false;
        rt.st.CancelUnpause();
    }
    public void Continue()
    {
        rt.st.IsPaused = false;
       
    }

    public void Start()
    {
        Invoke("LateStart", 0.1f);
    }
    public void LateStart()
    {
        rt.oh.OnVarChange += ChangePauseMenuMusicVolume;
    }
    public void OnDestroy()
    {
        rt.oh.OnVarChange -= ChangePauseMenuMusicVolume;
    }

    public void Update()
    {
        if (WaitingLetter)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in keyCodes)
                {

                    if (Input.GetKey(keyCode))
                    {
                        if (keyCode != KeyCode.Escape)
                        {
                            rt.oh.so.Controls[LetterIndex] = keyCode;
                            ButtonControls[LetterIndex].text = keyCode.ToString();
                            rt.oh.WriteOptionsToFile();
                        }
                        WaitingLetter = false;
                        MessageText.text = "";

                    }
                }
            }
        }
    }

    public void ChangeContol1()
    {
        WaitingLetter = true;
        LetterIndex = 0;
        
    }
    public void ChangeContol2()
    {
        WaitingLetter = true;
        LetterIndex = 1;
        MessageText.text = "Нажмите нужную клавишу или esc для отмены";
    }
    public void ChangeContol3()
    {
        WaitingLetter = true;
        LetterIndex = 2;
        MessageText.text = "Нажмите нужную клавишу или esc для отмены";
    }
    public void ChangeContol4()
    {
        WaitingLetter = true;
        LetterIndex = 3;
        MessageText.text = "Нажмите нужную клавишу или esc для отмены";
    }

    public void SliderValueChange()
    {
        rt.oh.Volume = VolumeSlider.value / 100f;
        VolumeText.text = VolumeSlider.value.ToString();
    }

    public void HitSoundSliderValueChange()
    {
        rt.oh.HitSoundVolume = HitSoundVolumeSlider.value / 100f;
        HitSoundVolumeText.text = HitSoundVolumeSlider.value.ToString();
    }

    public void ChangePauseMenuMusicVolume(OptionVarName var, float value)
    {
        if (var == OptionVarName.Volume)
        {
            PauseMusic.volume = value;
        }

    }
}
