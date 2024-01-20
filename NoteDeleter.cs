using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteTapper;

public class NoteDeleter : MonoBehaviour
{
    public NoteTapper nt;
    public int WhatLine;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "NoteGame" && collision.gameObject.GetComponent<NoteForGame>().noteInfo.type != NoteType.Slider)
            {
                nt.NoteLines[WhatLine].Remove(collision.gameObject);
                nt.DoNoteEvent(false, collision.gameObject.GetComponent<NoteForGame>());
                Destroy(collision.gameObject);

            }
        

    }
}
