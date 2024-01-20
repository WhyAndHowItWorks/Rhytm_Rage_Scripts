using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StoryTeller : MonoBehaviour
{
    public Router rt;

    public GameObject[] Environment = new GameObject[4];

    public Transform PlaseForUsualLevels;
    public Transform PlaseForBossLevel;
    

    public bool BattleWasStarted;
    public bool BattleWasEnded;

    public PauseMenu Pm;

    
    public bool Quiting;

    [Header("Все для паузы")]
    public GameObject PausePanel;
    public bool IsPaused
    {
        get { return isPaused; }
        set
        {
            isPaused = value;
            Pm.IsPaused = value;
            if (isPaused)
            {
                Time.timeScale = 0f;
                if (BattleWasStarted)
                {
                    rt.ntgs.Music.Pause();
                }

            }
            else
            {
                DelayedUnPause();
            }
        }
    }
    public bool isPaused;

    public delegate void st();
    public delegate void st2 (bool IsWin);

    public event st BattleStart;
    public event st2 BattleEnd;

    public string[] Sas = new string[4];
    public int number;

    // Событие с паузой
    public bool IsUnpausing;
    public float TimeFromUnpause;

    public bool IsStarting;
    public void DelayedUnPause()
    {
        ChangeText("1!");
        IsUnpausing = true;
        TimeFromUnpause = 0;
    }

    public void CancelUnpause()
    {
        IsUnpausing = false;
        Time.timeScale = 1f;
        if (BattleWasStarted)
        {
            rt.ntgs.Music.UnPause();
        }
        ChangeText("To Main Menu...");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !BattleWasStarted && !Input.GetKeyDown(KeyCode.Escape) && !IsPaused && !Quiting && !IsStarting)
        {
            StartBattleDelayed();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !rt.sct.IsLoading)
        {
            Pause();
        }

        if (IsUnpausing)
        {
            TimeFromUnpause += Time.unscaledDeltaTime;
            if (TimeFromUnpause > 3f)
            {
                IsUnpausing = false;
                Time.timeScale = 1f;
                if (BattleWasStarted)
                {
                    rt.ntgs.Music.UnPause();
                }
                ChangeText("Let's move on!");
            }
            else if(TimeFromUnpause > 2f)
            {
                ChangeText("3!");  
            }
            else if (TimeFromUnpause > 1f)
            {
                ChangeText("2!");
            }
        }
    }
    public void Awake()
    {
        BuildScene();
        // Загружает окружение
       
        if (!rt.rp.IsCustomLevel)
        {
            Environment[rt.rp.StoryLevel].SetActive(true);
            rt.pa.gameObject.SetActive(false);
            if (rt.rp.StoryLevel != 3)
            {
               
                rt.pa.gameObject.transform.position = PlaseForUsualLevels.position;
            }
            else 
            {
                
                rt.pa.gameObject.transform.position = PlaseForBossLevel.position;
            }
            rt.pa.gameObject.SetActive(true);
        }
        else 
        {
            Environment[2].SetActive(true);
        }
    }
    public void Pause()
    {
        IsPaused = !IsPaused;
        PausePanel.SetActive(IsPaused);
    }
    public void StartBattle()
    {
        BattleWasStarted = true;
        BattleStart();
        IsStarting = false;
    }
    void StartBattleDelayed()
    {
        IsStarting = true;
        ChangeText("1!");
        Invoke("ChangeText", "2!", 1f);
        Invoke("ChangeText", "3!", 2f);
        Invoke("StartBattle", 3f);
    }
    public void ChangeText(string text)
    {
        rt.ui.Phase.text = text;
    }
    public void EndBattle(bool IsWin)
    {
        BattleEnd(IsWin);
        if (IsWin)
        {
            rt.pa.PlayerWon();
            Invoke("QuitSceneWin", 2.1f);
            
            BattleWasEnded = true;
        }
        else 
        {
            Invoke("QuitSceneLose", 2f);
        }
    }
    
    public void QuitSceneLose()
    {
        rt.sct.LoadScene(4, false);
    }
    public void QuitSceneWin()
    {
        if (rt.rp.IsCustomLevel)
        {
            rt.sct.LoadScene(0, false);
        }
        else 
        {
            rt.rp.StoryLevel++;
            rt.oh.so.Level_Unlocked++;
            if (rt.rp.StoryLevel <= 3)
            {
                rt.sct.LoadScene(1, true);
            }
            else
            {
                rt.sct.LoadScene(0, false);
            }
            if (rt.rp.StoryLevel > 3)
            {
                
                rt.rp.StoryLevel = 3;
                rt.oh.so.EndTitles_Unlocked = true;
            }
            rt.rp.SaveProgress();
            
        }
       
        
    }

    public void BuildScene()
    {
        rt.oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        rt.lm = GameObject.Find("Хранитель настроек").GetComponent<LevelManager>();
        rt.rp = GameObject.Find("Хранитель настроек").GetComponent<RunProgressHandler>();
        rt.sct = GameObject.Find("Хранитель настроек").GetComponent<SceneTravel>();
        // Подгружает в роутер все компоненты
        rt.rp.LoadProgress();
        rt.tl.lm = rt.lm;
        rt.ntgs.OnStart();
        rt.ws.OnStart();
        rt.pa.OnStart();

    }
    public void Invoke(string method, object options, float delay)
    {
        StartCoroutine(_invoke(method, delay, options));
    }
    private IEnumerator _invoke(string method, float delay,  object par)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        Type instance = GetType();

        MethodInfo[] mth = instance.GetMethods();
        List<MethodInfo> mthd = new List<MethodInfo>();

        for (int i = 0; i < mth.Length; i++)
        {
            if (mth[i].Name == method)
            {
                mthd.Add(mth[i]);
            }
        }

       
            mthd[0].Invoke(this, new object[] { par} );
        
        



        yield return null;
    }

}
