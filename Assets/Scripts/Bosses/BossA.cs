using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossA : MonoBehaviour
{
    [Header("Bullets")]
    public List<GameObject> WeaponsList;
    public List<Rigidbody2D> BossBulletPrefabs;
    public float ForceMultiplier = 1f;
    public float ShotRate = 25;
    public float MoveRate = 25;

    [Header("Skills")]
    public float TimeBetweenBehaviors = 1f;
    public float MinTimeBetweenBehaviors = 0.25f;
    public float MovingTime = 1f;
    public float MinMovingTime = 0.25f;
    public bool startPhrase = false;
    public int PhraseNo = 1;
    bool isFollow = false;
    private void Awake() 
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            WeaponsList.Add(transform.GetChild(i).gameObject);
        }
    }
    void OnEnable()
    {
        Boss_HealthManager.newPhrase += NewPhrase;
        Boss_HealthManager.setDifficulty += SetDifficulty;
    }
    void OnDisable()
    {
        Boss_HealthManager.newPhrase -= NewPhrase;
        Boss_HealthManager.setDifficulty -= SetDifficulty;
    }
    void NewPhrase()
    {
        PhraseNo++;
        if(PhraseNo > 1)
        {
            isFollow = true;
        }
    }
    void SetDifficulty(float statMultipler)
    {
        TimeBetweenBehaviors = TimeBetweenBehaviors/statMultipler < MinTimeBetweenBehaviors? MinTimeBetweenBehaviors : TimeBetweenBehaviors/statMultipler;
        MovingTime = MovingTime/statMultipler < MinMovingTime? MinMovingTime : MovingTime/statMultipler;
        ShotRate = ShotRate*statMultipler > 100? 100 : ShotRate*statMultipler;
        MoveRate = MoveRate*statMultipler > 100? 100 : MoveRate*statMultipler;
    }
    void Update()
    {
        if(GetComponent<Boss_SpawnAttr>().MainTarget != null && GetComponent<Boss_SpawnAttr>().finishSpawn && !startPhrase)
        {
            StartCoroutine(BossBehaviors());
            StartCoroutine(BossMoving());
            startPhrase = true;
        }
    }
    IEnumerator BossBehaviors()
    {
        while(!GetComponent<Boss_HealthManager>().isDead)
        {  
            if(UnityEngine.Random.Range(0f, 100f) <= ShotRate && (GetComponent<Boss_SpawnAttr>().MainTarget != null || GetComponent<Boss_SpawnAttr>().SecondaryTargets.Count > 0))
            {
                if(UnityEngine.Random.Range(0f, 100f) <= 100/PhraseNo)
                {    
                    for(int i = 0; i < WeaponsList.Count; i++)
                    {
                        Vector3 track = (GetComponent<Boss_SpawnAttr>().MainTarget.transform.position - transform.position).normalized;

                        Quaternion Angle = isFollow? Quaternion.FromToRotation(transform.up, track) : Quaternion.Euler(0,0,180);

                        int j = UnityEngine.Random.Range(0, BossBulletPrefabs.Count);
                        Rigidbody2D bulletInstance;
                        bulletInstance = Instantiate(BossBulletPrefabs[j], WeaponsList[i].transform.position, Quaternion.Euler(0, 0, Angle.eulerAngles.z - 180)) as Rigidbody2D;
                        //bulletInstance.GetComponent<EnemyBullet>().DistanceTravel = 20;
                        bulletInstance.GetComponent<EnemyBullet>().SetShootDirect(bulletInstance.transform.up);
                        //bulletInstance.AddForce(bulletInstance.transform.up * bulletInstance.GetComponent<EnemyBullet>().Speed * ForceMultiplier);
                    }
                }
                else
                {
                    float j = UnityEngine.Random.Range(0.0f, 25.0f);
                    for(int i = 0; i < 16; i++)
                    {
                        var Angle = Quaternion.Euler(0, 0, j+i*22.5f);
                        Rigidbody2D bulletInstance;
                        bulletInstance = Instantiate(BossBulletPrefabs[0], transform.position, Angle) as Rigidbody2D;
                        //bulletInstance.GetComponent<EnemyBullet>().DistanceTravel = 20;
                        bulletInstance.GetComponent<EnemyBullet>().SetShootDirect(bulletInstance.transform.up);
                        //bulletInstance.AddForce(bulletInstance.transform.up * bulletInstance.GetComponent<EnemyBullet>().Speed * ForceMultiplier);
                    }
                }
            }
            yield return new WaitForSeconds(TimeBetweenBehaviors);
        }
    }

    IEnumerator BossMoving()
    {
        while(!GetComponent<Boss_HealthManager>().isDead)
        {  
            if(UnityEngine.Random.Range(0f, 100f) <= MoveRate && (GetComponent<Boss_SpawnAttr>().MainTarget != null || GetComponent<Boss_SpawnAttr>().SecondaryTargets.Count > 0))
            {
                int i = 0;
                i = UnityEngine.Random.Range(0, (int)(GetComponent<Boss_SpawnAttr>().MovePoints.Count/4) * PhraseNo);
                transform.DOMove(GetComponent<Boss_SpawnAttr>().MovePoints[i], MovingTime).SetEase(Ease.Linear).SetLink(gameObject);
            }
            yield return new WaitForSeconds(MovingTime);
        }
    }
}
