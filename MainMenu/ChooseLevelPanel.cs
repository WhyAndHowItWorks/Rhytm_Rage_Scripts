using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelPanel : MonoBehaviour
{
    public RunProgressHandler rp;
    public SceneTravel st;
    
    public int Level
    {
        get { return rp.StoryLevel; }
        set {
            if ( mo.oh.so.Level_Unlocked > -1)
            { 
                rp.StoryLevel = value;
            ContinueBText.text = "CONTINUE " + (rp.StoryLevel+1).ToString() + " LEVEL"; 
            }
            
        }
    }
    [Header("UI")]
    public Text ContinueBText;
    public Button ContinueB;
    public Text StartBText;
    public Button StartB;
    public Text EndTitleBText;
    public Button EndTitleB;
    public Dropdown ChooseCustomLevel;
    public GameObject MainLevelPanel;
    public GameObject MainMenuPanel;
    public MainOptions mo;
    public Text PlayCustomButtonText;

    public Color LockedColor = new Color(255, 0, 0, 255);
    public Color UnlockedColor = new Color(0, 255, 238, 255);

    public void Start()
    {
        Invoke("LateStart", 0.1f);
    }
    public void OpenPlayLevelPanel()
    {
        if (mo.Doors.IsActive)
        {
            Invoke("OpenPanel", 0.5f);
            mo.Doors.DoorsAnim();
        }
        else 
        {
            OpenPanel();
        }
        
    }
    public void OpenPanel()
    {
        MainMenuPanel.SetActive(false);
        MainLevelPanel.SetActive(true);
        Level = rp.StoryLevel;
        if (mo.oh.so.Level_Unlocked < 0)
        {
            StartBText.color = LockedColor;
            StartB.enabled = false;
            ContinueBText.color = LockedColor;
            ContinueB.enabled = false;
        }
        if (!mo.oh.so.EndTitles_Unlocked)
        {
            EndTitleBText.color = LockedColor;
            EndTitleB.enabled = false;
        }
    }
    public void ClosePanel()
    {
        MainMenuPanel.SetActive(true);
        MainLevelPanel.SetActive(false);
    }
    public void LateStart()
    {
        rp = GameObject.Find("Хранитель настроек").GetComponent<RunProgressHandler>();
        st = GameObject.Find("Хранитель настроек").GetComponent<SceneTravel>();
        LevelManager lm = GameObject.Find("Хранитель настроек").GetComponent<LevelManager>();
        List<string> delta = new List<string>();
        for (int i = 0; i < lm.CustomLevel.Count; i++)
        {
            delta.Add(lm.CustomLevel[i].LevelName);
        }
        ChooseCustomLevel.AddOptions(delta);
    }
    public void CloseLevelPanel()
    {
        if (mo.Doors.IsActive)
        {
            Invoke("ClosePanel", 0.5f);
            mo.Doors.DoorsAnim();
        }
        else
        {
            ClosePanel();
        }


    }

    public void GoFromBegin()
    {

        Level = 0;
        rp.IsCustomLevel = false;
        rp.WeaponStartLevels = new int[] { 0, 0, 0, 0 };
        rp.SaveProgress();
        Play();
    }
    public void Continue()
    {
        rp.IsCustomLevel = false;
        rp.StoryLevel = Level;
        Play();
    }
    
    public void Play()
    {
        st.LoadScene(1, true);
    }

    public void BeginTitles()
    {
        st.LoadScene(6, false);
    }
    public void EndTitles()
    {
        st.LoadScene(7, false);
    }

    public void ChooseLevelDropdownValueChanged()
    {
        PlayCustomButtonText.color = UnlockedColor;
        PlayCustomButtonText.text = "PLAY IT!";
    }
    public void PlayCustomLevel()
    {
        if (ChooseCustomLevel.value > -1)
        {
            rp.CustomLevel = ChooseCustomLevel.value;
            rp.IsCustomLevel = true;
            Play();
        }
        
    }

}
