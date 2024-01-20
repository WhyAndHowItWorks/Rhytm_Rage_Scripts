using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectsManager : MonoBehaviour
{
    public Router rt;

    public List<GameObject> Effects = new List<GameObject>();
    public List<float> EffectTimeToLive = new List<float>(); 

    public GameObject[] deltaEffects = new GameObject[8];
    public EffectType[] Types = new EffectType[8];

    public void CastEffect(EffectType et, int Place)
    {
        Types[Place] = et;
        deltaEffects[Place] = Instantiate(Effects[(int)et], rt.es.EnemyPositions[Place].transform.position, rt.es.EnemyPositions[Place].transform.rotation);
        deltaEffects[Place].transform.Translate(0, 2, 0);
        Destroy(deltaEffects[Place], EffectTimeToLive[(int)et]);
    }
    public void CastEffectWithPeriod(EffectType et, int Place)
    {
        if (deltaEffects[Place] == null || Types[Place] != et)
        {
            Types[Place] = et;           
            deltaEffects[Place] = Instantiate(Effects[(int)et], rt.es.EnemyPositions[Place].transform.position, rt.es.EnemyPositions[Place].transform.rotation);
            deltaEffects[Place].transform.Translate(0, 2, 0);
            Destroy(deltaEffects[Place], EffectTimeToLive[(int)et]);
        }
    }
}
