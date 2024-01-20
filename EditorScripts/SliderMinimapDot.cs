using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMinimapDot : MonoBehaviour
{
    public float Size
    {
        get
        {
            return size;
        }
        set
        {           
            size = value;
            RectTransform rt = Middle.GetComponent<RectTransform>();
            Middle.GetComponent<RectTransform>().sizeDelta = new Vector2(1, size);
            rt.offsetMax =new Vector2(rt.offsetMax.x, size*2.6f);
            rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
           
        }
    }

    public float size;
    public float SizeChanger;

    public float sizeY;
    public float TOP;

    public GameObject Middle;


    public void Update()
    {
        
        //Size = SizeChanger;
    }
    public void ChangeSize(float Size)
    {
        size = Size;
        RectTransform rt = Middle.GetComponent<RectTransform>();
        Middle.GetComponent<RectTransform>().sizeDelta = new Vector2(1, size);
        rt.offsetMax = new Vector2(rt.offsetMax.x, size * 2.6f);
        rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
    }

}
