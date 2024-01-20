using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SliderNote : MonoBehaviour
{
    
    public float Size
    {
        get 
        {
            return size;
        }
        set
        {
            col.center = new Vector3(0, size,0);
            col.size = new Vector2(1, size*2);
            size = value;
            Up.transform.position = gameObject.transform.position + new Vector3(0, size*2, -1);
            Middle.transform.position = gameObject.transform.position + new Vector3(0, size , 0);
            Middle.transform.localScale = new Vector3(1, 1.46f*size, 1);
        }
    }
   
    float size;
    public float SizeChanger;
    
    public GameObject Up;
    public GameObject Down;
    public GameObject Middle;

    public BoxCollider col;

    void Start()
    {
        col = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Size = SizeChanger;
    }
}
