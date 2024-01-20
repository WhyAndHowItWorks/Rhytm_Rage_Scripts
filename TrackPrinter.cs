using Ookii.Dialogs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPrinter : MonoBehaviour
{
    public Sprite[] Numbers = new Sprite[11];
    public Sprite TrackLine;
    public GameObject TG;
    public float CamSize;
    public List<GameObject> TrackDetails = new List<GameObject>();
    NoteTrack nt;


    public float deltaStep;

    public float StepInLength
    {
        get {
            stepInLength = 1f / 16f * nt.noteSpeed / nt.BPM * nt.standardBPM;
            return stepInLength; }
        set { stepInLength = value; }
    }
    public float stepInLength;
    
    
    public float spy;

    public GameObject[] dividelevels = new GameObject[5];

    public Color[] TrackLineColors = new Color[5];
    public int DivideLevel
    {
        get { return divideLevel; }
        set {
            divideLevel = value;
            for (int i = 0; i < 5; i++)
            {
                if (i <= divideLevel)
                {
                    dividelevels[i].SetActive(true);
                }
                else { dividelevels[i].SetActive(false) ; }
            }
        }
    }
    public int divideLevel;
    public int DIV;
    private void Start()
    {
      
        
        nt = gameObject.GetComponent<NoteTrack>();
        nt.ParameterChangedEvent += NoteSpeedChanged;
        spy = nt.Lines[4].transform.position.y;
        StepInLength = 1f/16f * nt.noteSpeed/nt.BPM*nt.standardBPM;
        
        DrawTrackArt();
        
        DivideLevel = DivideLevel;
        
        
    }
    private void Update()
    {
        
        UpdateTrackArt();
        
    }

    void DrawTrackArt()
    {
        for (float i = spy; i < spy + CamSize * 2; i += StepInLength)
        {
            
            GameObject g = Instantiate(TG, new Vector3(0, i, -1), Quaternion.Euler(0, 0, 0));
            TrackDetails.Add(g);
            g.GetComponent<TrackLine>().TactSeconds = deltaStep;
            
            SeparateLine(g);
            
            deltaStep += 0.0625f;
            

        }
    }
    void UpdateTrackArt()
    {
        if (TrackDetails[TrackDetails.Count - 1].transform.position.y - transform.position.y < CamSize)
        {
            GameObject g = Instantiate(TG, new Vector3(0, nt.NoteTactTimeToPosition(deltaStep), -1), Quaternion.Euler(0, 0, 0));
            g.GetComponent<TrackLine>().TactSeconds = deltaStep;
            SeparateLine(g);
            TrackDetails.Add(g);

            deltaStep += 0.0625f;

        }
    }
   
    public void SeparateLine(GameObject Line)
    {
        float time = Line.GetComponent<TrackLine>().TactSeconds;
        if (time % 1 == 0)
        {
            Line.transform.SetParent(dividelevels[0].transform);
            Line.GetComponent<SpriteRenderer>().color = TrackLineColors[0];
        }
        else if (time % 0.5f == 0)
        {
           
            Line.transform.SetParent(dividelevels[1].transform);
            Line.GetComponent<SpriteRenderer>().color = TrackLineColors[1];
            
        }
        else if (time % 0.25f == 0)
        {
            Line.transform.SetParent(dividelevels[2].transform);
            Line.GetComponent<SpriteRenderer>().color = TrackLineColors[2];
           
        }
        else if (time % 0.125f == 0)
        {
            Line.transform.SetParent(dividelevels[3].transform);
            Line.GetComponent<SpriteRenderer>().color = TrackLineColors[3];
           
        }
        else if (time % 0.0625f == 0)
        {
            Line.transform.SetParent(dividelevels[4].transform);
            Line.GetComponent<SpriteRenderer>().color = TrackLineColors[4];
           
        }
    }
    public void NoteSpeedChanged(TrackParameter par, float oldval, float newval) // Если изменилась скорость нот
    {
        if (par == TrackParameter.NoteSpeed || par == TrackParameter.BPM)
        {
            StepInLength = 1f / 16f * nt.noteSpeed / nt.BPM * nt.standardBPM;
            foreach (GameObject g in TrackDetails)
            {
                g.transform.position = new Vector3(0, nt.NoteTactTimeToPosition(g.GetComponent<TrackLine>().TactSeconds), -1);
            }
        }

    }
    
}
