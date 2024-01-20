using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DelayFinder : MonoBehaviour
{
    public OptionsHandler OH
    {
        get {
            if (oh == null)
            {
                oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
            }
            return oh; }
        set { oh = value; }
    }
    public OptionsHandler oh;
    public MainOptions mo;
    public AudioSource Temp;
    public Text info;
    public Text FoundDelay_Text;
    public Text ButtonText;
    public CursorArea ClickZone;
    public float FoundDelay
    {
        get { return foundDelay; }
        set 
        {
            foundDelay = value;
            FoundDelay_Text.text = "Delay: " + Mathf.Round(foundDelay * 1000).ToString() + " ms";
        }
    }
    public float foundDelay;
    public int ClickTime;
    public float[] DelayMassive = new float[5];
    public bool IsActive 
    {
        get 
        {
            return isActive;
        }
        set {
            if (isActive != value)
            {
                isActive = value;
                if (isActive)
                {
                    ClickZone.IsActive = true;
                    FoundDelay = 0;
                    ClickTime = 0;
                    Temp.time = 0;
                    Temp.volume = oh.Volume;
                    Temp.Play();
                    mo.MenuMusic.Pause();
                }
                else 
                {
                    ClickZone.IsActive = false;
                    Temp.Stop();
                    mo.MenuMusic.Play();
                }
            }
            
            
        }
    }
    public bool isActive;

    public GameObject BackPanel;

    public bool IsActivePanel 
    {
        get { return isActivePanel; }
        set { isActivePanel = value;
            if (isActivePanel)
            {
                // Получение из хранителя настроек задержки
                gameObject.SetActive(true);
                BackPanel.SetActive(false);
            }
            else 
            {
                gameObject.SetActive(false);
                BackPanel.SetActive(true);
            }
        }
    }
    public bool isActivePanel;
    public void Update()
    {
        if (isActive)
        {
            if (Input.GetMouseButtonDown(0) && ClickZone.CursorInZone)
            {
                float deltaDelay = Temp.time % 1f;
                if (deltaDelay > 0.5f)
                {
                    deltaDelay = 1 - deltaDelay;
                }
                DelayMassive[ClickTime] = deltaDelay;
                float sum = 0 ;
                for (int i = 0; i < 5; i++)
                {
                    sum += DelayMassive[i];
                }
                FoundDelay = sum / 5;
                ClickTime++;
                if (ClickTime > 4)
                {
                    ClickTime = 0;
                }
            }
        }
    }

    public void StartButton()
    {
        IsActive = !IsActive;
        if (IsActive) // Включение режима поиска задержки
        {
            info.text = "CLICK IT!!!";
            ButtonText.text = "DONE";
        }
        else // Выключение режима поиска задержки
        {
            info.text = "Press the button to find delay!";
            ButtonText.text = "Delay Search";
        }
    }
    public void SaveDelayButtonAction()
    {
        // Перенос задержки в Хранителя и в файл
        OH.so.Delay = FoundDelay;
        OH.WriteOptionsToFile();
        ExitFromDelayPanel();
    }
    public void GoToDelayPanel()
    {
        
           
        
            GoToDelayPanelAction();
        
        
    }
    public void GoToDelayPanelAction()
    {
        IsActivePanel = true;
        FoundDelay = OH.so.Delay;
        IsActive = false;
    }
    public void ExitFromDelayPanel()
    {
       
            ExitFromDelayPanelAction();
        
    }
    public void ExitFromDelayPanelAction()
    {
        IsActivePanel = false;
        FoundDelay = OH.so.Delay;
        IsActive = false;
    }

    

}
