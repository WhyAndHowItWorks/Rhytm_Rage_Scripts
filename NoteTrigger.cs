using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    public int WhatLine;
    public NoteTapper nt;
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.tag == "NoteGame")
        {
            nt.NoteLines[WhatLine].Add(collision.gameObject);
        }
    
    }             
}