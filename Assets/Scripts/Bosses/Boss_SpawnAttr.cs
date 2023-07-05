using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss_SpawnAttr : MonoBehaviour
{
    [Header("Targets")]
    public GameObject MainTarget;
    public List<GameObject> SecondaryTargets;

    [Header("Move Points")]
    public List<Vector3> MovePoints;
    public float MovingTime = 1f;
    public bool finishSpawn = false;
    
    public void SetPath(Vector3 pos)
    {
        transform.DOMove(pos, MovingTime).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(()=>
        {
            finishSpawn = true;
        });
    }

    // Start is called before the first frame update
    void Start()
    {                
        if(MainTarget == null)
        {
            MainTarget = GameObject.FindGameObjectWithTag("Player");
        }
        //StartCoroutine(BossBehaviors());
    }

    void Update()
    {

    }
}
