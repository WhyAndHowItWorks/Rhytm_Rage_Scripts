using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_boxes : MonoBehaviour
{
    public float BoxSpeed;

    public float DistanceBetwwenBoxes;
    public Transform FinalDot;
    public GameObject Box;
    public bool UseList;
    public List<GameObject> RandomShit = new List<GameObject>();

    public List<GameObject> deltaBoxes = new List<GameObject>();

    int Counter 
    {
        get
        {
            counter++;
            if (counter >= RandomShit.Count)
            {
                counter = 0;
            }
            return counter;
        }
        set { counter = value; }
    }
    int counter;

    public void Start()
    {
        for (float i = FinalDot.position.x; i > transform.position.x; i -= DistanceBetwwenBoxes)
        {
            if (i > transform.position.x)
            {
                if (!UseList)
                {
                    deltaBoxes.Add(Instantiate(Box, new Vector3(i, transform.position.y, transform.position.z), transform.rotation));
                }
                else 
                {
                    deltaBoxes.Add(Instantiate(RandomShit[Counter], new Vector3(i, transform.position.y, transform.position.z), transform.rotation));
                }
                
            }
          
        }
    }

    public void FixedUpdate()
    {
        foreach (GameObject g in deltaBoxes)
        {
            g.transform.Translate(BoxSpeed, 0, 0);
        }
        if (deltaBoxes[0].transform.position.x >= FinalDot.position.x )
        {
                GameObject g = deltaBoxes[0];
                deltaBoxes.Remove(deltaBoxes[0]);
                Destroy(g);
                       

        }
        if (deltaBoxes[^1].transform.position.x - transform.position.x >= DistanceBetwwenBoxes)
        {
            if (!UseList)
            {
                deltaBoxes.Add(Instantiate(Box, transform.position, transform.rotation));
            }
            else
            {
                deltaBoxes.Add(Instantiate(RandomShit[Counter], transform.position, transform.rotation));
            }
        }
    }

}
