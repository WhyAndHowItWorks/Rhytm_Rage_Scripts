using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public float StandardSize;
    public float step;
    [SerializeField]
    float zoomSize;
    bool GoDown;
    public float ZoomSize
    {
        get { return zoomSize; }
        set 
        {
            zoomSize = value;      
            GoDown = true;
        }
    }

    public void Zoom(float scale) // Добавляет эффект зума
    {
        ZoomSize -= scale;
    }
    private void FixedUpdate()
    {
        if (GoDown)
        {
            gameObject.GetComponent<Camera>().orthographicSize -= step;          
            if (gameObject.GetComponent<Camera>().orthographicSize < zoomSize)
            {
                zoomSize = StandardSize;
                GoDown = false;
            }
        }
        else { if (gameObject.GetComponent<Camera>().orthographicSize < StandardSize) { gameObject.GetComponent<Camera>().orthographicSize += step; } }
    }

}
