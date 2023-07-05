using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemHealth : MonoBehaviour
{
    public float existTime = 10f;
    public float healAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(new Vector3(0, 360, 0), 1f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetLink(gameObject);
        StartCoroutine(CoutDownToDestroy());
    }

    IEnumerator CoutDownToDestroy()
    {
        yield return new WaitForSeconds(existTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "Player")
        {            
            theCollision.transform.GetComponent<HealthManager>().Healing(healAmount);
            Destroy(gameObject);
        }
        if(theCollision.gameObject.tag == "EnemyBullet" && theCollision.gameObject.GetComponent<EnemyBullet>().isDestructible)
        {            
            Destroy(gameObject);
        }
    }
        

    // Update is called once per frame
    void Update()
    {
        
    }
}
