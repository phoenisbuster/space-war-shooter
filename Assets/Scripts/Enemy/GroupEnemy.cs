using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GroupEnemy : MonoBehaviour
{
    public float hozSpeed = 0.75f;
    public float curHozSpeed;
    public float verSpeed = -0.25f;
    public float transSpeed = -5;
    public float respawnSpeed;
    public bool canMove = false;

    public GameObject CanVasBound;
    public Vector2 Boudary;
    public float lowerbound = -2.5f;
    public float upperbound = 22f;
    public float leftbound = -23f;
    public float rightbound = 23f;

    public int noMove = 400;
    public int curNoMove = 0;
    public Vector3 respawnPosition;
    public float TimeToSpawn = 0.25f;
    public float EnemyCol = 5;
    public float EnemyRow = 5;
    public int CurEnemies;
    public bool allEnemiesDestroyed = false;

    public List<GameObject> EnemyPrefabs;
    // Start is called before the first frame update
    public void SetBoudary()
    {
        Boudary = CanVasBound.GetComponent<RectTransform>().rect.size;
        leftbound = -(Boudary.x * CanVasBound.GetComponent<RectTransform>().localScale.x)/2 + 1;
        rightbound = (Boudary.x * CanVasBound.GetComponent<RectTransform>().localScale.x)/2 - 1;
        lowerbound = 10 - (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2 + 1;
        upperbound = 10 + (Boudary.y * CanVasBound.GetComponent<RectTransform>().localScale.y)/2 - 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        CanVasBound = GameObject.FindGameObjectWithTag("Boundary");
        SetBoudary();
        respawnPosition = transform.position;
        curHozSpeed = hozSpeed;
        respawnSpeed = hozSpeed;
        ResetPos();
    }

    public void SetEnemyPrefabs(List<GameObject> enemyPrefabs)
    {
        EnemyPrefabs = new List<GameObject>();
        EnemyPrefabs = enemyPrefabs;
    }

    public void SpawnEnemies(List<int> spawnID, List<Vector3> SpawnPos, List<Vector3> MiddlePoint, float speed, float statMultipler = 1)
    {
        ResetPos(statMultipler);
        StartCoroutine(GenerateEnemy(spawnID, SpawnPos, MiddlePoint, speed, statMultipler));       
    }
    public void ResetPos(float statMultipler = 1)
    {
        allEnemiesDestroyed = false;
        transform.position = respawnPosition;
        respawnSpeed = respawnSpeed*statMultipler;
        verSpeed = verSpeed*statMultipler;
        transSpeed = transSpeed*statMultipler;
        curHozSpeed = respawnSpeed;
    }
    IEnumerator GenerateEnemy(List<int> spawnID, List<Vector3> SpawnPos, List<Vector3> MiddlePoint, float speed, float statMultipler = 1)
    {
        for(int i = 0; i < spawnID.Count; i++)
        {
            //canMove = false;
            int j = UnityEngine.Random.Range(0, SpawnPos.Count);
            int t = UnityEngine.Random.Range(0, EnemyPrefabs.Count);
            List<Vector3> path = new List<Vector3>();
            path.Add(SpawnPos[j]);
            if(MiddlePoint.Count != 0)
            {
                path.AddRange(MiddlePoint);
            }
            path.Add(transform.GetChild(spawnID[i]).position);
            GameObject enemy = Instantiate(EnemyPrefabs[t], SpawnPos[j], Quaternion.identity);
            enemy.GetComponent<SingleEnemy>().SetInitialStats(statMultipler);
            enemy.GetComponent<SingleEnemy>().Enemy_ID = spawnID[i];
            enemy.transform.SetParent(transform.GetChild(spawnID[i]));
            enemy.transform.DOPath(path.ToArray(), speed * path.Count, PathType.CatmullRom, PathMode.TopDown2D).SetEase(Ease.Linear).OnComplete(()=>
            {
                enemy.GetComponent<SingleEnemy>().isGenerating = false;
            }).SetLink(enemy);
            CurEnemies++;
            yield return new WaitForSeconds(TimeToSpawn);
        }
        canMove = true;
    }

    public void OnEnable() 
    {
        SingleEnemy.callWhenDead += DecreaseCurEnemy;
        SingleEnemy.SpeedMultiplier += SetHozSpeed;
    }
    public void OnDisable() 
    {
        SingleEnemy.callWhenDead -= DecreaseCurEnemy;
        SingleEnemy.SpeedMultiplier -= SetHozSpeed;
    }

    public void SetHozSpeed(int SpeedMultiplier)
    {
        curHozSpeed = hozSpeed * SpeedMultiplier;
        if(canMove)
        {
            transform.Translate(new Vector3(0, transSpeed, 0), Space.World);
        }
    }

    public void DecreaseCurEnemy(int ID, bool isDestroyedbyPlayer, Vector3 notUsed)
    {
        CurEnemies--;
        if(isDestroyedbyPlayer)
        {
            //transform.GetChild(ID).GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
        }
            
    }

    // Update is called once per frame
    void Update() 
    {
        if(CurEnemies <= 0 && !allEnemiesDestroyed)
        {
            allEnemiesDestroyed = true;;
        }
        if(canMove)
        {
            transform.Translate(new Vector3(curHozSpeed * Time.deltaTime, verSpeed * Time.deltaTime, 0), Space.World);
        }           
    }
    void FixedUpdate()
    {
        // if(canMove)
        // { 
        //     if(curNoMove <= 0)
        //     {
        //         transform.Translate(new Vector3(0, transSpeed * Time.deltaTime, 0), Space.World);
        //         //hozSpeed = -hozSpeed;
        //         curNoMove = noMove;
        //     }
        //     if(curNoMove > 0)
        //     {
        //         transform.Translate(new Vector3(curHozSpeed * Time.deltaTime, 0, 0), Space.World);
        //         curNoMove--;
        //     }
        //     transform.Translate(new Vector3(0, verSpeed * Time.deltaTime, 0), Space.World);
        // }
    }
}
