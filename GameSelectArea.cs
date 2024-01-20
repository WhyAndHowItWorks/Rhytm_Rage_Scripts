using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectArea : MonoBehaviour
{
    public List<GameObject> NoteFound = new List<GameObject>();
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "NoteGame")
        {
            NoteFound.Add(collision.gameObject);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NoteGame")
        {
            NoteFound.Remove(collision.gameObject);
        }
    }
}
