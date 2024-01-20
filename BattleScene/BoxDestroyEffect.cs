using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDestroyEffect : MonoBehaviour
{
    public List<Sprite> Shard = new List<Sprite>();
    public GameObject ShardG;

    public void OnDestroy()
    {
        for (int i = 0; i < Shard.Count; i++)
        {
            GameObject g = Instantiate(ShardG, transform.position, transform.rotation);
            g.GetComponent<SpriteRenderer>().sprite = Shard[i];
            Vector3 From = transform.position;
            Vector3 To = transform.position + Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0) * new Vector3(1, 1, 0);
            g.GetComponent<ProjectileMover>().SetDirectionAndTime(From, To, 0.2f);
        }
    }
}
