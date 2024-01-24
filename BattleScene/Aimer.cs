using UnityEngine;

public class Aimer : MonoBehaviour
{
    [Header("Admins")]
    public WeaponSystem ws;
    // Move the Pointer
    public GameObject Pointer;
    public GameObject[] Dots = new GameObject[2];
    public float TimeToMove;
    float currentSpeed;
    bool IsMoving;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangePlatform();
        }
        if (IsMoving)
        {
            float newPosition = Mathf.SmoothDamp(Pointer.transform.position.y, Dots[ws.ChosedPlatform].transform.position.y,ref currentSpeed, TimeToMove);
            Pointer.transform.position = new Vector3(Pointer.transform.position.x, newPosition,Pointer.transform.position.z);
            if (Pointer.transform.position.y == Dots[ws.ChosedPlatform].transform.position.y)
            {
                IsMoving = false;
            }
        }
      
    }
    public void ChangePlatform()
    {
        if (ws.ChosedPlatform == 0)
        {
            ws.ChosedPlatform = 1;
        }
        else
        {
            ws.ChosedPlatform = 0; 
        }
        if (ws.CurrentWeapon != null)
        { 
            ws.CurrentWeapon.ChangePlatform(ws.ChosedPlatform);
        }
        IsMoving = true;
    }

}
