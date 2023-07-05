using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float dmg = 1;
    public float DistanceTravel = 0;
    public Vector3 startPos;
    public float Speed = -5f;
    public float Accelerate = 2.5f;
    public bool isDestructible = false;
    public float ExistTime = 10f;
    public bool isNomalBehav = true;
    Vector3 ShootDirection;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        StartCoroutine(CoutDownToDestroy());
    }
    IEnumerator CoutDownToDestroy()
    {
        yield return new WaitForSeconds(ExistTime);
        Destroy(gameObject);
    }
    public void SetShootDirect(Vector3 direction)
    {
        ShootDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, startPos) > DistanceTravel && DistanceTravel > 0)
        {
            Destroy(gameObject);
        }
        if(isNomalBehav)
        {
            transform.Translate(ShootDirection * Speed * Time.deltaTime, Space.World);
        }        
    }
    void FixedUpdate() 
    {
        Speed += Accelerate * 0.02f;
    }

    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "Player")
        {            
            Destroy(gameObject);
        }
        if(theCollision.gameObject.tag == "PlayerBullet" && isDestructible)
        {            
            Destroy(gameObject);
        }
    }
}
