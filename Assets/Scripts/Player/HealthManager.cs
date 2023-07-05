using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float MaxHealth = 4;
    public float CurHealth;
    public float Def = 10;
    public float Armor = 10;
    public float MagicResist = 10;
    public bool isInvincible = false;
    public bool isDead = false;
    public GameObject HealthBar;
    public static Action<bool> callWhenDead;
    public Animator anim;
    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "EnemyBullet")
        {            
            ReceiveDmg(theCollision.transform.GetComponent<EnemyBullet>().dmg);
        }
        if(theCollision.gameObject.tag == "Enemy")
        {            
            //ReceiveDmg(theCollision.transform.GetComponent<SingleEnemy>().MaxHealth);
            ReceiveDmg(MaxHealth*1000);
        }
        if(theCollision.gameObject.tag == "Meteorite")
        {            
            ReceiveDmg(theCollision.transform.GetComponent<Meteorite>().Maxhealth);
        }
        if(theCollision.gameObject.tag == "Boss")
        {            
            ReceiveDmg(theCollision.transform.GetComponent<Boss_HealthManager>().MaxHealth);
        }
        
    }
    void ReceiveDmg(float dmg)
    {
        if(isInvincible)
        {
            dmg = 0;
        }
        else
        {
            dmg = dmg - dmg*(Def/(Def + 100));
        }
        CurHealth -= dmg;
        HealthBar.GetComponent<HealthBar>().SetHealth(CurHealth >= 0? CurHealth : 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        CurHealth = MaxHealth;
        HealthBar.GetComponent<HealthBar>().SetMaxHealth(MaxHealth);
        HealthBar.GetComponent<HealthBar>().SetHealth(CurHealth);
    }

    public void Healing(float heal)
    {
        CurHealth = CurHealth+heal > MaxHealth? MaxHealth : CurHealth + heal;
        HealthBar.GetComponent<HealthBar>().SetHealth(CurHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(CurHealth <= 0 && !isDead)
        {
            isDead = true;
            transform.GetChild(0).GetComponent<Animator>().SetBool("expl", true);
            GetComponent<CapsuleCollider2D>().enabled = false;
            callWhenDead?.Invoke(false);
            StartCoroutine(DestroyThisObj());
        }
    }
    IEnumerator DestroyThisObj()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
