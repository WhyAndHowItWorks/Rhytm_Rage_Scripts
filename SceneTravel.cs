using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTravel : MonoBehaviour
{
    public GameObject Canv;
    public Animator an;
    public int WhatLoad;
    public AsyncOperation ap;

    public int FromScene;
    public int ToScene;

    public bool IsLoading;

    public void LoadScene(int WhatScene, bool NeedLoadingScreen)
    {
        if (!IsLoading)
        {


            IsLoading = true;
            if (NeedLoadingScreen)
            {
                WhatLoad = WhatScene;
                an.Play("Затемнение");
                Invoke("LoadLoadingScreen", 2f);


            }
            else
            {
                WhatLoad = WhatScene;
                an.Play("Затемнение");
                Invoke("Load", 2f);

            }
        }
       
    }
    public void LoadLoadingScreen()
    {
        SceneManager.LoadScene(5);
        Invoke("LoadTargetScene", 0.5f);
    }
    public void LoadTargetScene()
    {
        ap = SceneManager.LoadSceneAsync(WhatLoad);
        ap.allowSceneActivation = false;
    }
    public void LoadSceneLOL()
    {
        Invoke("LoadScenelol", 0.8f);
    }
    public void LoadScenelol()
    {
        ap.allowSceneActivation = true;
    }

    public void Start()
    {
        SceneManager.sceneLoaded += SceneLoadedE;
        DontDestroyOnLoad(Canv);        
    }
    
    public void Load()
    {
        if (WhatLoad != 999)
        {
            SceneManager.LoadScene(WhatLoad);
            
        }
        else 
        {
            Application.Quit();
        }
       
    }
    public void SceneLoadedE(Scene scene, LoadSceneMode mode)
    {
        IsLoading = false;
        an.Play("Осветление");
       
    }
    
}
