using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncylopediaPanel : MonoBehaviour
{
    public GameObject[] Infos = new GameObject[7];
    public GameObject CurrentText;
    public Scrollbar scr;

    public void ShowInfo(int index)
    {
        if (CurrentText != null)
        {
            CurrentText.SetActive(false);
        }
        scr.value = 1;
        
        CurrentText = Infos[index];
        CurrentText.SetActive(true);
    }
}
