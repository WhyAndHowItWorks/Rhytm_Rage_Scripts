using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float maxHP;
    public float MaxHP 
    {
        get { return maxHP; }
        set 
        { 
            maxHP = value;
            UpdateHealth();
        }
    }
    public float health;
    public float Health 
    {
        get { return health; }
        set { health = value; UpdateHealth(); }
    }
    public Text Health_text;

   
    public float shield_health;
    public float Shield_health 
    {
        get { return shield_health; }
        set
        {
            shield_health = value;
            ShieldBar_text.text = shield_health.ToString();
            if (shield_health <= 0)
            {
                ShieldBar.SetActive(false);
            }
            else { ShieldBar.SetActive(true); }
        }
    }
    public GameObject ShieldBar;
    public Text ShieldBar_text;
    public Router rt;

    // Отображение колво противников в стаке
    public Text StackCountText;
    public int StackCount 
    {
        get { return stackCount; }
        set 
        {
            stackCount = value;
            StackCountText.gameObject.SetActive(stackCount > 1);
            StackCountText.text = "x" + stackCount;
        }
    }
    public int stackCount;
    public int EnemyPosition
    {
        get { return enemyPosition; }
        set 
        {
            enemyPosition = value;
            transform.position = rt.es.EnemyPositions[value].transform.position + new Vector3(0, -1, 0);
        }
    }
    int enemyPosition;
    public void UpdateHealth()
    {      
        healthBar.fillAmount = health / MaxHP;
        Health_text.text = health.ToString();
    }
    
   
}
