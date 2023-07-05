using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float dmg = 1;
    public float DistanceTravel = 0;
    public Vector3 startPos;
    public float Speed = 5f;
    public float Accelerate = 2.5f;
    public float ExistTime = 10f;
    float initialAngle = 0;
    public bool isNormalBehav = true;
    Vector3 ShootDirection;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        initialAngle = transform.rotation.eulerAngles.z;
        StartCoroutine(CoutDownToDestroy());
    }
    IEnumerator CoutDownToDestroy()
    {
        yield return new WaitForSeconds(ExistTime);
        Destroy(gameObject);
    }
    public void SetShootDirection(Vector3 direct)
    {
        ShootDirection = direct;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, startPos) > DistanceTravel && DistanceTravel > 0)
        {
            Destroy(gameObject);
        }
        if(isNormalBehav)
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
        if(theCollision.gameObject.tag == "Enemy" || theCollision.gameObject.tag == "Meteorite" || theCollision.gameObject.tag == "Boss")
        {            
            Destroy(gameObject);
        }
        if(theCollision.gameObject.tag == "EnemyBullet" && theCollision.gameObject.GetComponent<EnemyBullet>().isDestructible)
        {            
            Destroy(gameObject);
        }
    }
}
