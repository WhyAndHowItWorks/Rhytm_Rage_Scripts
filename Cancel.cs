using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel : MonoBehaviour
{
    public NoteTrack nt;
    public EditorNoteFactory enf;

    public List<CancelComand> History = new List<CancelComand>();

    public GameObject CreateComand;
    public GameObject DeleteComand;
    public GameObject MoveComand;
    public GameObject UniteComand;

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Z))
        {
            if (History.Count > 0)
            {
                GameObject g = History[^1].gameObject;
                History[^1].Undo();
                History.RemoveAt(History.Count-1);
                Destroy(g);
            }
            
        }
    }

    public void ClearHistory()
    {
        foreach (CancelComand c in History)
        {
            Destroy(c.gameObject);
        }
        History.Clear();
    }
    public void AddDeleteComand(NotesForEditor n)
    {
        GameObject g = Instantiate(DeleteComand, transform);
        DeleteComand d = g.GetComponent<DeleteComand>();
        d.c = this;
        d.SetInformaion(n.Type, n.Number, n.Line, n.Position.y, n.Length, n.IsChosen,n.NoteUniqId);
        History.Add(d);
    }
    public void AddCreateComand(int NewNoteId)
    {
        GameObject g = Instantiate(CreateComand, transform);
        CreateComand c = g.GetComponent<CreateComand>();
        c.c = this;
        c.SetInformation(NewNoteId);
        History.Add(c);
    }
    public void AddMovingComand(int MovedNoteId, Vector3 OldPos)
    {
        GameObject g = Instantiate(MoveComand, transform);
        MovingComand m = g.GetComponent<MovingComand>();
        m.c = this;
        m.SetInformation(OldPos,MovedNoteId);
        History.Add(m);
    }
    public void AddUniteComand(int AmountOfComands)
    {
        GameObject g = Instantiate(UniteComand, transform);
        UniteComand u = g.GetComponent<UniteComand>();
        u.c = this;
        u.SetInformation(AmountOfComands);
        History.Add(u);
    }

}
