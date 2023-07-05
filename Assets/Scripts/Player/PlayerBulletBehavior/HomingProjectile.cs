using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public List<GameObject> TargetList;
    public Transform Shooter;
    public Transform Target;
    public float Speed = 5f;
    public float TurnSpeed = 100f;
    public float TimeToAim = 1f;
    bool isHomming = false;
    public bool HommingOnSpawn = false;
    public bool HommingEnemies = true;
    public bool HommingMeteo = true;
    public bool HommingBoss = true;

    // void OnTriggerEnter2D(Collider2D theCollision)
    // {        
    //     if(theCollision.gameObject.tag == "Player" && !isHomming)
    //     {            
    //         Target = theCollision.gameObject;
    //         //transform.parent.GetComponent<EnemyBullet>().isNomalBehav = false;
    //         StartCoroutine(CountDownHomming());
    //     }
    // }
    IEnumerator CountDownHomming()
    {
        isHomming = true;
        yield return new WaitForSeconds(TimeToAim);
        isHomming = false;
    }
    // Start is called before the first frame update
    void Awake()
    {
        Shooter = GameObject.FindGameObjectWithTag("Player").transform;
        if(HommingOnSpawn == true)
        {
            GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            GameObject[] Meteorite = GameObject.FindGameObjectsWithTag("Meteorite");
            GameObject Boss = GameObject.FindGameObjectWithTag("Boss");
            if(Enemies.Length > 0)
            {
                TargetList = Enemies.ToList();
            }
            if(Meteorite.Length > 0)
            {
                TargetList = Meteorite.ToList();
            }
            if(Boss != null)
            {
                TargetList.Add(Boss);
            }    
        }
    }
    void Start()
    {
        if(TargetList.Count > 0)
        {
            StartCoroutine(CountDownHomming());
            Target = MostCloseEnemy();
        }
    }

    public Transform MostCloseEnemy()
    {
        int index = 0;
        if(TargetList.Count == 1)
        {
            index = 0;
        }
        else
        {
            index = 0;
            float distance = 999999;
            if(TargetList[0] != null && Shooter != null)
            {
                distance = (TargetList[0].transform.position - Shooter.position).magnitude;
            } 
            for(int i = 1; i < TargetList.Count; i++)
            {
                if(TargetList[i] != null && Shooter != null && distance > (TargetList[i].transform.position - Shooter.position).magnitude)
                {
                    distance = (TargetList[i].transform.position - Shooter.position).magnitude;
                    index = i;
                }
            }
        }
        return TargetList[index]!=null? TargetList[index].transform : null;
    }

    // Update is called once per frame
    void Update()
    {
        if(isHomming && Target != null)
        {
            Vector2 Direction = ((Vector2)(Target.position - transform.position));
            Direction.Normalize();

            float rotateAmmount = Vector3.Cross(Direction, transform.up).z;

            transform.Rotate(new Vector3(0,0,-rotateAmmount * TurnSpeed * Time.deltaTime), Space.Self);
            transform.GetComponent<PlayerBullet>().SetShootDirection(transform.up);
        }
    }
}
