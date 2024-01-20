using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mover : MonoBehaviour
{
    public Vector3 Where;
    public float speed;
   
    void Update()
    {
        transform.Translate(Where * speed * Time.deltaTime);
       
    }
}
