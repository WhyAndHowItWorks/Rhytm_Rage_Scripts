using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Vector2 StepPerSecond;
 


    public void Update()
    {

        transform.position += new Vector3(StepPerSecond.x, StepPerSecond.y) * Time.deltaTime;
    }
   
  
    public void SetDirectionAndTime(Vector3 From, Vector3 Where, float Time)
    {
        StepPerSecond = (Where - From) / Time;
        Destroy(this, Time);
    }
}
