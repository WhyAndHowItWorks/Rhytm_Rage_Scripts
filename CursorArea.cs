using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorArea : MonoBehaviour
{
    public bool IsActive;
    public bool CursorInZone;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsActive)
        {
            CursorInZone = true;
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (IsActive)
        {
            CursorInZone = false;
        }
    }
}
