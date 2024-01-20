using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteComand : CancelComand
{
    int AmountOfComand;
    public void SetInformation(int AmountOfCommand)
    {
        this.AmountOfComand = AmountOfCommand;
    }
    public override void Undo()
    {
        for (int i = 0; i < AmountOfComand; i++)
        {
            c.History[^2].Undo();
            c.History.RemoveAt(c.History.Count - 2);
        }
    }
}
