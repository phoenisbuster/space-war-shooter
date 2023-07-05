using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss_HealthManager : MonoBehaviour
{
    bool isInvincible = false;
    [Header("Stats")]
    public float MaxHealth = 1000;
    public float CurHealth = 0;
    public float OriginDef = 10;
    public float CurDef = 0;
    public List<float> changePhraseAt;
    int PhraseIndex = 0;
    public GameObject HealthBar; 
    public bool isDead = false;
    public static Action<Vector3> callWhenDead;
    public GameObject explosionEffect;
    public static Action newPhrase;
    public static Action<float> setDifficulty;

    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "PlayerBullet")
        {            
            ReceiveDmg(theCollision.transform.GetComponent<PlayerBullet>().dmg * DmgMultiplier());
        }
        if(theCollision.gameObject.tag == "Player")
        {            
            ReceiveDmg(theCollision.transform.GetComponent<HealthManager>().MaxHealth);
        }
    }
    void ReceiveDmg(float dmg)
    {
        if(isInvincible)
        {
            dmg = 0;
        }
        CurHealth -= dmg;
        HealthBar.GetComponent<HealthBar>().SetHealth(CurHealth >= 0? CurHealth : 0);
        if(PhraseIndex < changePhraseAt.Count && CurHealth <= MaxHealth*changePhraseAt[PhraseIndex]/100)
        {
            PhraseIndex++;
            newPhrase?.Invoke();
        }
    }
    public float DmgMultiplier()
    {
        return 1 - (CurDef/(CurDef+100));
    }
    
    private void Awake() 
    {
        //CurHealth = MaxHealth;
        CurDef = OriginDef;
    }
    public void SetHealthBar(GameObject bar, float statMultipler = 1)
    {
        HealthBar = bar;
        HealthBar.SetActive(true);
        MaxHealth = MaxHealth*statMultipler;
        CurHealth = MaxHealth;
        HealthBar.GetComponent<HealthBar>().SetMaxHealth(MaxHealth);
        HealthBar.GetComponent<HealthBar>().SetHealth(CurHealth);
        setDifficulty?.Invoke(statMultipler);
    }

    void Start()
    {                

    }

    void Update()
    {
        if(CurHealth <= 0 && !isDead)
        {
            isDead = true;
            //anim.SetBool("expl", true);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            HealthBar.SetActive(false);
            callWhenDead?.Invoke(transform.position);
            StartCoroutine(DestroyThisObj());
        }
        else
        {
            CurDef = OriginDef + OriginDef * (MaxHealth - CurHealth)/(MaxHealth*0.1f);
        }
    }

    IEnumerator DestroyThisObj()
    {
        if(explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
