using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseEnemyButton : MonoBehaviour
{
    public int EnemyNumber;
    public NoteTrack nt;
    public void Sas()
    {
        nt.euh.ChooseEnemyId(EnemyNumber);
    }
}
