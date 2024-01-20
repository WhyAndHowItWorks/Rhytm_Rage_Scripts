using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunProgressHandler : MonoBehaviour
{
    public int CustomLevel
    {
        get { return customLevel; }
        set
        {
            customLevel = value;
            CurrentLevel = gameObject.GetComponent<LevelManager>().CustomLevel[customLevel];
        }
    }
    int customLevel;
    public int StoryLevel
    {
        get { return storylevel; }
        set { storylevel = value;
            if (storylevel > 3)
            {
                CurrentLevel = gameObject.GetComponent<LevelManager>().StoryLevel[3];
            }
            else { CurrentLevel = gameObject.GetComponent<LevelManager>().StoryLevel[storylevel]; }
            
        }
    }
    public int storylevel;

    public Level CurrentLevel;

    public bool IsCustomLevel;
    public int[] WeaponStartLevels = new int[4];

    public void Start()
    {
        LoadProgress();
    }
    public void SaveProgress()
    {
        if (!IsCustomLevel)
        {
            gameObject.GetComponent<OptionsHandler>().so.StoryLevel = StoryLevel;
            gameObject.GetComponent<OptionsHandler>().so.WeaponStartLevels = WeaponStartLevels;
            gameObject.GetComponent<OptionsHandler>().WriteOptionsToFile();
        }
       
    }
    public void LoadProgress()
    {
        if (!IsCustomLevel)
        {
            StoryLevel = gameObject.GetComponent<OptionsHandler>().so.StoryLevel;
            WeaponStartLevels = gameObject.GetComponent<OptionsHandler>().so.WeaponStartLevels;
        }
        
        

        
    }

    

}
