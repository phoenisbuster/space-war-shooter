using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChangeShip : MonoBehaviour
{
    public float existTime = 10f;
    public int ShipIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
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
            theCollision.transform.GetComponent<SpaceShipManager>().SetSpaceShip(ShipIndex);
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
