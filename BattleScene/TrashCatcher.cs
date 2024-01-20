using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCatcher : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Trash")
        {
            Destroy(collision.gameObject.GetComponent<Rigidbody2D>());
            Destroy(collision.gameObject.GetComponent<Collider2D>());
        }
    }
}
