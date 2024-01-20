using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArea : MonoBehaviour
{
    public List<GameObject> SelectedObjects = new List<GameObject> ();

    public void OnTriggerEnter(Collider collision)
    {
        
        NotesForEditor nt;
        if (collision.gameObject.TryGetComponent<NotesForEditor>(out nt))
        {
            SelectedObjects.Add(collision.gameObject);
            nt.IsChosenByArea = true;
        }
    }
    public void OnTriggerExit(Collider collision)
    {
        NotesForEditor nt;
        if (collision.gameObject.TryGetComponent<NotesForEditor>(out nt))
        {
            SelectedObjects.Remove(collision.gameObject);
            nt.IsChosenByArea = false;
        }
    }
}
