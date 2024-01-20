using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainOptions : MonoBehaviour
{
    public Dropdown ChooseTrack;
    public Camera Cam;
    private readonly Array keyCodes = Enum.GetValues(typeof(KeyCode));
    public Text[] ButtonControls = new Text[4];

    public OptionsHandler oh;

    public GameObject DevOptionsPanel;
    public GameObject MainMenuPanel;
    public GameObject UserOptionsPanel;


    public List<OptionChanger> Changers = new List<OptionChanger>();

    public GameObject Cursor;

    public Text MessageText;

    public bool WaitingLetter;
    public int LetterIndex;

    [Header("Музыка в меню")]
    public AudioSource MenuMusic;

    public MenuDoors Doors;

    public GameObject AutorsPanel;
    public GameObject EncylopediaPanel;

    public void BackToMainMenuFromDevPanel()
    {
        oh.WriteOptionsToFile();
        DevOptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
    public void BackToMainMenuFromUserPanel()
    {
        oh.WriteOptionsToFile();
        if (Doors.IsActive)
        {
            Invoke("BackToMainMenuFromUserPanelAction", 0.5f);
            Doors.DoorsAnim();
        }
        else 
        {
            BackToMainMenuFromUserPanelAction();
        }
        
        
    }
    public void BackToMainMenuFromUserPanelAction()
    {
        UserOptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
    

    public void ChangeContol1()
    {
        WaitingLetter = true;
        LetterIndex = 0;
        MessageText.text = "Press Key Or ESC to cancel";
    }
    public void ChangeContol2()
    {
        WaitingLetter = true;
        LetterIndex = 1;
        MessageText.text = "Press Key Or ESC to cancel";
    }
    public void ChangeContol3()
    {
        WaitingLetter = true;
        LetterIndex = 2;
        MessageText.text = "Press Key Or ESC to cancel";
    }
    public void ChangeContol4()
    {
        WaitingLetter = true;
        LetterIndex = 3;
        MessageText.text = "Press Key Or ESC to cancel";
    }
    public void OpenDevOptionsPanel()
    {
        
        DevOptionsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);  
    }

    public void OpenUserOptions()
    {
        if (Doors.IsActive)
        {
            Invoke("OpenUserOptionsPanel", 0.5f);
            Doors.DoorsAnim();
        }
        else
        {
            OpenUserOptionsPanel();
        }
    }
    public void OpenUserOptionsPanel()
    {
        UserOptionsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }

    public void Start()
    {

        MenuMusic.volume = oh.Volume;

        Invoke("SAS", 0.1f);
    }
    public void Update()
    {
        FindCursorCoordinates();
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
                            oh.so.Controls[LetterIndex] = keyCode;
                            ButtonControls[LetterIndex].text = keyCode.ToString();
                            oh.WriteOptionsToFile();
                        }
                        WaitingLetter = false;
                        MessageText.text = "";

                    }
                }
            }
        }
        
            oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        
    }

    public void OnDestroy()
    {
        oh.OnVarChange -= ChangeMainMenuMusicVolume;
    }

    public void SAS()
    {
        oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        oh.OnVarChange += ChangeMainMenuMusicVolume;
        
        for (int i = 0; i < 4; i++)
        {
            ButtonControls[i].text = oh.so.Controls[i].ToString();
        }
        Doors.gameObject.SetActive(oh.so.DoorsAnim);
        Doors.IsActive = oh.so.DoorsAnim;
    }

    public void ChoosedTrackChanged()
    {
        oh.gameObject.GetComponent<RunProgressHandler>().StoryLevel = ChooseTrack.value;
        if (ChooseTrack.value > 2)
        {
            oh.gameObject.GetComponent<RunProgressHandler>().IsCustomLevel = true;
        }
        else 
        {
            oh.gameObject.GetComponent<RunProgressHandler>().IsCustomLevel = false;
        }
        
    }

    public void OpenAuthorsA()
    {
        AutorsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }
    public void OpenAuthors()
    {
        if (Doors.IsActive)
        {
            Invoke("OpenAuthorsA", 0.5f);
            Doors.DoorsAnim();
        }
        else
        {
            OpenAuthorsA();
        }
    }
    public void BackOpenAuthorsA()
    {
        AutorsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
    public void BackOpenAuthors()
    {
        if (Doors.IsActive)
        {
            Invoke("BackOpenAuthorsA", 0.5f);
            Doors.DoorsAnim();
        }
        else
        {
            BackOpenAuthorsA();
        }
    }
    public void OpenEncyclA()
    {
        EncylopediaPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
    }
    public void OpenEncycls()
    {
        if (Doors.IsActive)
        {
            Invoke("OpenEncyclA", 0.5f);
            Doors.DoorsAnim();
        }
        else
        {
            OpenEncyclA();
        }
    }
    public void BackOpenEncyclA()
    {
        EncylopediaPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }
    public void BackOpenEncycl()
    {
        if (Doors.IsActive)
        {
            Invoke("BackOpenEncyclA", 0.5f);
            Doors.DoorsAnim();
        }
        else
        {
            BackOpenEncyclA();
        }
    }

    public void FindCursorCoordinates()// Находит координаты курсора
    {
        Vector3 mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
        
        Cursor.transform.position = new Vector3(mousePos.x,mousePos.y, 0); // Перемещает на эти координаты объект курсора
    }

    public void ChangeMainMenuMusicVolume(OptionVarName var, float value)
    {
        if (var == OptionVarName.Volume)
        {
            MenuMusic.volume = value;
        }
        
    }
}
