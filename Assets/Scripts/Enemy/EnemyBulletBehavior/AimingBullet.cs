using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingBullet : MonoBehaviour
{
    public GameObject Target;
    public float Speed = 5f;
    public float TurnSpeed = 100f;
    public float TimeToAim = 1f;
    bool isHomming = false;
    public bool HommingOnSpawn = false;

    void OnTriggerEnter2D(Collider2D theCollision)
    {        
        if(theCollision.gameObject.tag == "Player" && !isHomming)
        {            
            Target = theCollision.gameObject;
            //transform.parent.GetComponent<EnemyBullet>().isNomalBehav = false;
            StartCoroutine(CountDownHomming());
        }
    }
    IEnumerator CountDownHomming()
    {
        isHomming = true;
        yield return new WaitForSeconds(TimeToAim);
        isHomming = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        if(HommingOnSpawn == true)
        {
            Target = GameObject.FindGameObjectWithTag("Player");
            if(Target != null)
            {
                StartCoroutine(CountDownHomming());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isHomming)
        {
            Vector2 Direction = ((Vector2)(Target.transform.position - transform.parent.position));
            Direction.Normalize();

            float rotateAmmount = Vector3.Cross(Direction, transform.parent.transform.up).z;

            transform.parent.Rotate(new Vector3(0,0,rotateAmmount*TurnSpeed*Time.deltaTime), Space.Self);
            transform.parent.GetComponent<EnemyBullet>().SetShootDirect(transform.parent.up);
        }
    }
}
