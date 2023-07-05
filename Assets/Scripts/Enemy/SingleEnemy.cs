using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEnemy : MonoBehaviour
{
    public int Enemy_ID;
    public float MaxHealth = 4;
    public float CurHealth;
    public bool isDead = false;
    public bool outOfBound = false;
    public bool isGenerating = true;
    public List<Rigidbody2D> enemyBullet;
    public float ShotRate = 10;
    public int NumOfShot = 1;
    public float AtkSpeed = 0.25f;
    public float ShotCheckTimer = 1;
    public float MinShotCheckTimer = 0.1f;
    float Timer = 0;
    public static Action<int, bool, Vector3> callWhenDead;
    public static Action<int> SpeedMultiplier;
    public Animator anim;
    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "PlayerBullet")
        {            
            CurHealth -= theCollision.transform.GetComponent<PlayerBullet>().dmg;
        }
        if(theCollision.gameObject.tag == "Player")
        {            
            CurHealth -= theCollision.transform.GetComponent<HealthManager>().MaxHealth;
        }
        if(theCollision.gameObject.tag == "BoundaryLeft" && !isGenerating)
        {
            SpeedMultiplier?.Invoke(1);
        }
        if(theCollision.gameObject.tag == "BoundaryRight" && !isGenerating)
        {
            SpeedMultiplier?.Invoke(-1);
        }
    }

    void OnTriggerExit2D(Collider2D theCollision) 
    {
        if(theCollision.gameObject.tag == "LowerBound" && !isGenerating)
        {
            outOfBound = true;
            CurHealth = 0;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Timer = ShotCheckTimer;
        //SetInitialStats();
        //StartCoroutine(Shooting());
    }

    public void SetInitialStats(float statMultipler = 1)
    {
        MaxHealth = MaxHealth * statMultipler;
        CurHealth = MaxHealth;
        ShotRate = ShotRate*statMultipler > 100? 100 : ShotRate*statMultipler;
        ShotCheckTimer = ShotCheckTimer/statMultipler < MinShotCheckTimer? MinShotCheckTimer : ShotCheckTimer/statMultipler;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurHealth <= 0 && !isDead)
        {
            isDead = true;
            if(!outOfBound)
            {
                anim.SetBool("expl", true);
                //GetComponent<SpriteRenderer>().enabled = false;
            }                
            GetComponent<BoxCollider2D>().enabled = false;
            callWhenDead?.Invoke(Enemy_ID, !outOfBound, transform.position);
            StartCoroutine(DestroyThisObj());
        }
    }

    void FixedUpdate() 
    {
        if(Timer <= 0)
        {
            StartCoroutine(Shooting());
            Timer = ShotCheckTimer;
        }
        else
        {
            Timer -= 0.02f;
        }
    }

    IEnumerator DestroyThisObj()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }

    IEnumerator Shooting()
    {
        if(!isDead && UnityEngine.Random.Range(0f, 100f) <= ShotRate)
        {
            int count = 0;
            while(count < NumOfShot)
            {    
                Timer = ShotCheckTimer;
                count++;
                int i = UnityEngine.Random.Range(0, enemyBullet.Count);
                Rigidbody2D bulletInstance;
                bulletInstance = Instantiate(enemyBullet[i], transform.position, transform.rotation) as Rigidbody2D;
                //bulletInstance.GetComponent<EnemyBullet>().DistanceTravel = 20;
                bulletInstance.GetComponent<EnemyBullet>().SetShootDirect(bulletInstance.transform.up);
                //bulletInstance.AddForce(bulletInstance.transform.up * bulletInstance.GetComponent<EnemyBullet>().Speed);
                yield return new WaitForSeconds(AtkSpeed);
            }
        }            
    }
}
