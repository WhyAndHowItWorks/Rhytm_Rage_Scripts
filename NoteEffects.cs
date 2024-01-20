using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEffects : MonoBehaviour
{
   public Router rt;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        NoteForGame nfg = collision.gameObject.GetComponent<NoteForGame>();
        DoEffect(nfg.noteInfo.Color,nfg.noteInfo.Line);
        Destroy(collision.gameObject);
    }

    public void DoEffect(int TypeEffect, int EffectSpecializer)
    {
        switch (TypeEffect)
        {
            case 0:
                rt.nt.SwitchFhase();
                break;
            case 1:
                if (rt.ntgs.runNumber == 0)
                {
                    rt.es.SpawnEmemy(EffectSpecializer);
                }
                

                break;
        }
    }
}
