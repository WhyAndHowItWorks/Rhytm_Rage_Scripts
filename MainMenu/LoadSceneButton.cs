using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public int WhatScene;
    public bool NeedLoadingScreen;
    public SceneTravel st;

    public void Sas()
    {
        st.LoadScene(WhatScene,NeedLoadingScreen);
    }
    public void Awake()
    {
        st = GameObject.Find("Хранитель настроек").GetComponent<SceneTravel>();
    }
}
