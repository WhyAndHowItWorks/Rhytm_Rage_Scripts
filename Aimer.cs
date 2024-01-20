using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Aimer : MonoBehaviour
{
    public WeaponSystem ws;
    public GameObject Pointer;
    public GameObject[] Dots = new GameObject[2];
    public bool IsMoving;
    public float Time;
    float f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlatform();
        }
        if (IsMoving)
        {
            float newPosition = Mathf.SmoothDamp(Pointer.transform.position.y, Dots[ws.ChosedPlatform].transform.position.y,ref f, Time);
            Pointer.transform.position = new Vector3(Pointer.transform.position.x, newPosition,Pointer.transform.position.z);
            if (Pointer.transform.position.y == Dots[ws.ChosedPlatform].transform.position.y)
            {
                IsMoving = false;
            }
        }
      
    }
    public void ChangePlatform()
    {
        // ѕомен€ть переменную в системе оружи€
        if (ws.ChosedPlatform == 0)
        {
            ws.ChosedPlatform = 1;
        }
        else { ws.ChosedPlatform = 0; }
        if (ws.CurrentWeapon != null)
        { ws.CurrentWeapon.ChangePlatform(ws.ChosedPlatform); }
        
        // ѕереместить указатель на другую сторону
        IsMoving = true;
        


    }

}
