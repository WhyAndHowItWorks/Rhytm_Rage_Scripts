using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseScreen : MonoBehaviour
{
    public SceneTravel st;

    public void Awake()
    {
        st = GameObject.Find("Хранитель настроек").GetComponent<SceneTravel>();
    }

    public void Retry()
    {
        st.LoadScene(1, true); 
    }
    public void GoToMainMenu()
    {
        st.LoadScene(0, false);
    }
}
