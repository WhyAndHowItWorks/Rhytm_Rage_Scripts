using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevOptions : MonoBehaviour
{
    public OptionsHandler oh;
    public OptionsHandler Oh
    {
        get { if (oh == null)
            {
                oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
                
            }
        return oh; 
        }
        set { oh = value; }
    }

    public List<OptionChanger> changers = new List<OptionChanger>();

    public GameObject MainPanel;
    public GameObject GGPanel;
    public GameObject EnemyPanel;
    public GameObject MainMenu;

    public void Start()
    {
        ClosePanel();
    }
    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftBracket) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.Q))
        {
            if (MainMenu.active && !MainPanel.active)
            {
                OpenPanel();
            }
        }
    }

    public void Save()
    {
        Oh.WriteOptionsToFile();
    }
    public void OpenPanel()
    {
        Oh.ReadOptionsFromFile();
        MainPanel.SetActive(true);
        MainMenu.SetActive(false);
        foreach (OptionChanger c in changers)
        {
            c.TakeOption();
        }
    }
    public void ClosePanel()
    {
        oh.WriteOptionsToFile();
        MainPanel.SetActive(false);
        GGPanel.SetActive(false);
        EnemyPanel.SetActive(false);
        MainMenu.SetActive(true);
        
    }

    public void OpenGGPanel()
    {
        EnemyPanel.SetActive(false);
        GGPanel.SetActive(true);
    }
    public void OpenEnemyPanel()
    {
        EnemyPanel.SetActive(true);
        GGPanel.SetActive(false);
    }
    
}
