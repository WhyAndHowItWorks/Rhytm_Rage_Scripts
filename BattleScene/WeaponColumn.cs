using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponColumn : Column
{
    public GameObject _0xpEffect;
    public override float TempValue
    {
        get { return base.TempValue; }
        set
        {
            base.TempValue = value;
            if (TempValue == 0)
            {
                _0xpEffect.GetComponent<Animator>().Play("Blink");
            }
        }
    }
}
