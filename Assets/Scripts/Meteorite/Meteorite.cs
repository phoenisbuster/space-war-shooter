using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Meteorite : MonoBehaviour
{
    public float existTime = 15f;
    public int maxRotateSpeed = -360;
    public int minRotateSpeed = 360;
    public int rotateSpeed;
    public float Maxhealth = 1;
    public float Curhealth = 1;
    public bool isDestroyed = false;
    public bool outOfBound = false;
    public static Action<Vector3, bool> callWhenDead;
    public AudioSource DestroyAudio;
    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = UnityEngine.Random.Range(minRotateSpeed, maxRotateSpeed);
        // transform.DORotate(new Vector3(0, 0, rotateSpeed), 1f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear).SetLink(gameObject);
        StartCoroutine(CoutDownToDestroy());
    }

    IEnumerator CoutDownToDestroy()
    {
        yield return new WaitForSeconds(existTime);
        outOfBound = true;
        Curhealth = 0;
    }

    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "PlayerBullet")
        {            
            Curhealth -= theCollision.transform.GetComponent<PlayerBullet>().dmg;
        }
        if(theCollision.gameObject.tag == "Player")
        {            
            Curhealth -= theCollision.transform.GetComponent<HealthManager>().CurHealth;
        }
    }
    
    public void SetMaxHealth(float scale, float statMultipler = 1)
    {
        if(scale < 0.25f)
        {
            Maxhealth = 10;
        }
        else if(scale >= 0.25f && scale < 0.5f)
        {
            Maxhealth = 20;
        }
        else if(scale >= 0.5f && scale < 0.75f)
        {
            Maxhealth = 30;
        }
        else if(scale >= 0.75f && scale < 1)
        {
            Maxhealth = 40;
        }
        else
        {
            Maxhealth = 50;
        }
        Maxhealth = Maxhealth * statMultipler;
        Curhealth = Maxhealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Curhealth <= 0 && !isDestroyed)
        {
            isDestroyed = true;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            if(!outOfBound)
            {
                //DestroyAudio.Play();
                transform.parent.GetComponent<AudioSource>().PlayOneShot(transform.parent.GetComponent<AudioSource>().clip);
                GetComponent<ParticleSystem>().Play();
            }
            callWhenDead?.Invoke(transform.position, !outOfBound);
            StartCoroutine(DestroyThisObj());
        }
        transform.Rotate(new Vector3(0, 0, rotateSpeed == 0? maxRotateSpeed : rotateSpeed), Space.Self);
    }

    IEnumerator DestroyThisObj()
    {
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
    }
}
