using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ProjectileMover : MonoBehaviour
{
    public Vector2 StepPerSecond;
    public GameObject Destroy_Effect;
  
    
    public void Update()
    {
        
        transform.position += new Vector3(StepPerSecond.x,StepPerSecond.y) * Time.deltaTime;
    }
    private void OnDestroy()
    {
        if (Destroy_Effect != null)
        {
            GameObject g = Instantiate(Destroy_Effect, transform.position, transform.rotation);
            g.GetComponent<ParticleSystem>().Play();
            Destroy(g, 1f);
        }
    }
    public void SetDirectionAndTime(Vector3 From, Vector3 Where, float Time, GameObject FromWho)
    {
        // the direction we want the X axis to face (from this object, towards the target)
        Vector3 xDirection = (Where - FromWho.transform.position).normalized;

        // Y axis is 90 degrees away from the X axis
        Vector3 yDirection = Quaternion.Euler(0, 0, 90) * xDirection;

        // Z should stay facing forward for 2D objects
        Vector3 zDirection = Vector3.forward;

        // apply the rotation to this object
        transform.rotation = Quaternion.LookRotation(zDirection, yDirection);

        StepPerSecond = (Where - From) / Time;
    }
    public void SetDirectionAndTime(Vector3 From, Vector3 Where, float Time)
    {        
        StepPerSecond = (Where - From) / Time;
        Destroy(gameObject, Time);
    }

}
