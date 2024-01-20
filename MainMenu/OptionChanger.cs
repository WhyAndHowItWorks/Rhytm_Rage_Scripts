using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionChanger : MonoBehaviour
{
    public int Massiveindex;
    public int IndexInMassive;

    public DevOptions DO;
    

    public void Awake()
    {
        DO = GameObject.Find("Настройки скрипт").GetComponent<DevOptions>();
        DO.changers.Add(this);
    }
    
    

    public void TakeOption()
    {
        gameObject.GetComponent<InputField>().text = ReturnValue().ToString();
        
    }
    public void ЫЫЫЫЫЫЫ()
    {
        int res;
        bool f = int.TryParse(gameObject.GetComponent<InputField>().text, out res);
        if (f)
        {
            ReturnValue() = res;
            DO.oh.WriteOptionsToFile();
        }
        else { gameObject.GetComponent<InputField>().text = "Неправильный ввод"; }
        
    }
    public ref int ReturnValue()
    {
        switch (Massiveindex)
        {
            case (0):
                
               return ref DO.oh.so.EnemyDamage[IndexInMassive];
            case (1):
                return ref DO.oh.so.EnemyHp[IndexInMassive];
            case (2):
                switch (IndexInMassive)
                {
                    case (0):
                        return ref DO.oh.so.BarrierHp;
                    case (1):
                        return ref DO.oh.so.LaserCutterPlusHP;
                    case (2):
                        return ref DO.oh.so.PolisherPlusDamage;
                    case (3):
                        return ref DO.oh.so.RepairerPlusHP;
                }
                break;
            case (3):
                switch (IndexInMassive)
                {
                    case (0):
                        return ref DO.oh.so.BossHP;
                    case (1):
                        return ref DO.oh.so.PistolDamage;
                    case (2):
                        return ref DO.oh.so.ShotgunDamage;
                    case (3):
                        return ref DO.oh.so.LaserDamage;
                    case (4):
                        return ref DO.oh.so.BladeDamage;
                    case (5):
                        return ref DO.oh.so.BossPlusShield;
                    case (6):
                        return ref DO.oh.so.BossPlusDamage;
                }
                break;
            case (4):
                return ref DO.oh.so.WeaponDamage[IndexInMassive];
            case (5):
                switch (IndexInMassive)
                {
                    case (0):
                        return ref DO.oh.so.HeroHP;
                    case (1):
                        return ref DO.oh.so.ShotgunMaxLoad;
                    case (2):
                        return ref DO.oh.so.ShotgunTimeToLoad;
                    case (3):
                        return ref DO.oh.so.LaserShieldPerNote;
                    case (4):
                        return ref DO.oh.so.RocketLauncerAddDamageToNote;
                }
                break;
        }
        return ref Massiveindex;
    }
}
