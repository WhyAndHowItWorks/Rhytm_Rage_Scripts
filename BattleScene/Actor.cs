using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static NoteTapper;
/// <summary>
/// ������� ����� ���� ��������, ������� ����� ����������� �� ������� ������
/// </summary>
public class Actor : MonoBehaviour
{
    public string description; // �������� ����� ������
    /// <summary>
    /// ������� ������ �� �������� � �����
    /// </summary>
    /// <param name="Pressed"></param>
    /// <param name="nfg"></param>
    public virtual void NoteAction(bool Pressed, NoteForGame nfg)
    {
        
    }
}


