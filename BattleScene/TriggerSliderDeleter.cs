using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSliderDeleter : MonoBehaviour
{
    public NoteTapper nt;
    public int WhatLine;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SliderDeleter")
        {

            collision.gameObject.GetComponent<SliderUp>().DestroySlider();
        }
    }
}
