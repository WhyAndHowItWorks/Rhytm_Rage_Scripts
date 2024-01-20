using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public GameObject From;
    public GameObject To;
    
    public void Sas()
    {
        Sas1();
        
    }
    public void Sas1()
    {
        From.SetActive(false);
        To.SetActive(true);
    }
}
