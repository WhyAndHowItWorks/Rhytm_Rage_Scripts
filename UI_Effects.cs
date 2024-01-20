using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Effects : MonoBehaviour
{
    public Router rt;
    public GameObject Text_Damage;
    public GameObject HealthBar;
    public float DamageTextLife;
    public float Text_speed;

    public Color Damage_C;
    public Color Heal_C;


    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float Damage,GameObject Creature)
    {
        GameObject g = Instantiate(Text_Damage, Creature.transform.position + new Vector3(0, 0, -10), Creature.transform.rotation );
        g.SetActive(true);
        g.transform.SetParent(gameObject.transform);

        if (Damage > 0)
        {
            g.GetComponent<Text>().color = Damage_C;
            g.GetComponent<Text>().text = Damage.ToString();
        }
        else
        {
            g.GetComponent<Text>().color = Heal_C;
            g.GetComponent<Text>().text = (Damage*-1).ToString();
        }
        

        Mover m = g.GetComponent<Mover>();
       m.speed = Text_speed;
       
        m.Where = Quaternion.Euler(Random.Range(0,360), Random.Range(0,360), 0) * new Vector3(1, 1, 0);
        Destroy(g, DamageTextLife);
    }

    public HealthBar CreateGGHealthBar(PlayerAbilities pa)
    {
        GameObject g = Instantiate(HealthBar, pa.HealthBarPoint.position, pa.HealthBarPoint.rotation);
        g.transform.SetParent(gameObject.transform);
        g.SetActive(true);
        HealthBar bar = g.GetComponent<HealthBar>();
        pa.Healthbar = bar;
        bar.rt = rt;
        g.GetComponent<HealthBar>().MaxHP = pa.MaxHealth;
        g.GetComponent<HealthBar>().Health = pa.MaxHealth;
        return g.GetComponent<HealthBar>();
    }
    public HealthBar CreateEnemyHealthBar(Enemy enemy)
    {
        GameObject g = Instantiate(HealthBar, rt.es.EnemiesOnPositions[enemy.MyNumber].transform.position, rt.es.EnemiesOnPositions[enemy.MyNumber].transform.rotation);
        g.transform.SetParent(gameObject.transform);
        g.SetActive(true);
        HealthBar bar = g.GetComponent<HealthBar>();
        enemy.HealthBar = bar;
        bar.rt = rt;
        bar.MaxHP = enemy.MaxHealth;
        bar.Health = enemy.MaxHealth;
        bar.EnemyPosition = enemy.MyNumber;
        return g.GetComponent<HealthBar>();
    }
}
