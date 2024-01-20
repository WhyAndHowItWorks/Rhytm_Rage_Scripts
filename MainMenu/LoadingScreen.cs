using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Text Progress;
    public Text ProgressLabel;
    public SceneTravel st;
    public OptionsHandler oh;
    public RunProgressHandler rp;
    public GameObject GG;
    public Transform[] Dots = new Transform[5];
    public Transform Target;

    public Transform[] DotsForCustom = new Transform[2];

    public GameObject CompanyLoadScreen;
    public GameObject CustomLevelLoadScreen;

    Vector3 From;
    Vector3 To;
    public float SmoothTime;
    public float MaxSpeed;
    Vector2 speed;

    public bool StartMove;


    public float LoadProgress 
    {
        get { return loadProgress; }
        set {
            loadProgress = value;

            Target.transform.position = Vector3.Lerp(From, To, loadProgress);
            Progress.text = Mathf.Round(loadProgress * 100) + "%";
            if (loadProgress == 1)
            {
                ProgressLabel.text = "PRESS ANY KEY TO CONTINUE";
                
            }

        }
    }
    public float loadProgress;
    public void Update()
    {
        if (Vector2.Distance(GG.transform.position, Target.transform.position) > 0.1f)
        {
            GG.transform.position = Vector2.SmoothDamp(GG.transform.position, Target.transform.position, ref speed, SmoothTime, MaxSpeed);
            GG.transform.position = new Vector3(GG.transform.position.x, GG.transform.position.y, -2);
        }
        if (StartMove && Vector2.Distance(GG.transform.position, To) < 0.1f)
        {
            GG.GetComponent<Animator>().SetTrigger("End");
        }
        
        if (st.ap != null)
        {
            if (LoadProgress < st.ap.progress)
            {
                LoadProgress = st.ap.progress;
            }
            
            if (st.ap.progress >= 0.9f)
            {
                LoadProgress = 1;
                
               
                if (Input.anyKeyDown)
                {
                    st.an.Play("Затемнение");
                    st.LoadSceneLOL();
                }
            }
                    
        }
    }
    public void Start()
    {
        st = GameObject.Find("Хранитель настроек").GetComponent<SceneTravel>();
        oh = GameObject.Find("Хранитель настроек").GetComponent<OptionsHandler>();
        rp = GameObject.Find("Хранитель настроек").GetComponent<RunProgressHandler>();
        if (rp.IsCustomLevel)
        {
            CustomLevelLoadScreen.SetActive(true);
            From = DotsForCustom[0].transform.position;
            GG.transform.position = From;
            Target.transform.position = From;
            To = DotsForCustom[1].transform.position;
            StartMove = true;
            MaxSpeed = 4;
        }
        else 
        {
            CompanyLoadScreen.SetActive(true);
            From = Dots[rp.StoryLevel].transform.position;
            GG.transform.position = From;
            Target.transform.position = From;
            To = Dots[rp.StoryLevel + 1].transform.position;
            StartMove = true;
        }
       
    }
}
