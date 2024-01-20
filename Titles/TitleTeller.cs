using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TitleTeller : MonoBehaviour
{
    public OptionsHandler oh;
    public SceneTravel st;

    public Text Info;

    public string[] Phrases = new string[10];
    public Transform[] Places = new Transform[10];
    public Transform TargetedPlace;

    public Transform TargetedPlaceMenu;

    public GameObject Cam;

    public float CamMaxSpeed;
    public Vector3 CurrentSpeed;
    public float TimeToSwap;
    public int CurrentFrame
    {
        get { return currentFrame; }
        set {
            currentFrame = value;
            if (currentFrame > MaxCadr)
            {
                currentFrame = 0;
            }
            if (currentFrame < 0)
            {
                currentFrame = MaxCadr;
            }
            GoToFrame(currentFrame);
        }
    }
    public int currentFrame;

    public int MaxCadr;

    public bool BackToMainMenu;
    
    
    void Start()
    {
        oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        st = GameObject.Find("Хранитель настроек").GetComponent<SceneTravel>();
        CurrentFrame = 0;
    }
    void GoToFrame(int Frame)
    {
        Info.text = Phrases[Frame];
        TargetedPlace = Places[Frame];
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Vector3.Distance(Cam.transform.position, TargetedPlace.transform.position) > 0.1f && !BackToMainMenu)
        {
            Cam.transform.position = Vector3.SmoothDamp(Cam.transform.position, TargetedPlace.transform.position, ref CurrentSpeed, TimeToSwap, CamMaxSpeed);

        }
        if (BackToMainMenu)
        {
            Cam.transform.position = Vector3.SmoothDamp(Cam.transform.position, TargetedPlaceMenu.transform.position, ref CurrentSpeed, TimeToSwap, CamMaxSpeed);

        }
        if (Input.GetMouseButtonDown(0)) // Нажатие левой
        {
            CurrentFrame++;
        }
        if (Input.GetMouseButtonDown(1))// Нажатие правой
        {
            CurrentFrame--;
        }
    }
    public void EndButton()
    {
        if (oh.so.Level_Unlocked == -1)
        {
            oh.so.Level_Unlocked = 0;
            oh.WriteOptionsToFile();
        }
        st.LoadScene(0, false);
       
        Vector3 delta = TargetedPlace.transform.position;
        TargetedPlaceMenu = TargetedPlace;
        CurrentSpeed = new Vector3(0, 0, 0);
        TargetedPlaceMenu.transform.position = new Vector3(delta.x-60, delta.y + 100, delta.z);
        Info.text = "TO MAIN MENU!!!";
        BackToMainMenu = true;
    }
    public void EndTitleEndButton()
    {
        st.LoadScene(0, false);

        Vector3 delta = TargetedPlace.transform.position;
        TargetedPlaceMenu = TargetedPlace;
        CurrentSpeed = new Vector3(0, 0, 0);
        TargetedPlaceMenu.transform.position = new Vector3(delta.x - 60, delta.y + 100, delta.z);
        Info.text = "TO MAIN MENU!!!";
        BackToMainMenu = true;

    }


    
}
