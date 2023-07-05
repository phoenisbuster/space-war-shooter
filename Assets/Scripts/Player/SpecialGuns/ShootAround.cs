using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAround : MonoBehaviour
{
    public Rigidbody2D playerBullet;
    public float UpgradeLv = 1;
    public float FireRate = 0.5f;
    public float BulletSpeed = 10f;
    public float BulletDist = 50f;
    public float MaxAttributeUpgrade = 1;
    public float AttackSpeed = 1;
    public float AtkSpeedPerLv = 0.05f;
    public bool canShoot = false;
    public AudioClip ShootingSound;
    public void ShootCommand()
    {
        Debug.Log("This gun is shooting");
        StartCoroutine(Shooting());
    }
    public IEnumerator Shooting()
    {
        //Debug.Log("Shoot every " + FireRate / AttackSpeed);
        while(playerBullet != null)
        {
            
            if(canShoot && !transform.parent.parent.GetComponent<HealthManager>().isDead)
            {
                for(int i = 0; i < 8; i++)
                {
                    var Angle = Quaternion.Euler(0,0,i*45);
                    Rigidbody2D bulletInstance;
                    bulletInstance = Instantiate(playerBullet, transform.position, Angle) as Rigidbody2D;
                    bulletInstance.GetComponent<PlayerBullet>().DistanceTravel = BulletDist + BulletDist * 0.15f * MaxAttributeUpgrade;
                    bulletInstance.GetComponent<PlayerBullet>().Speed = BulletSpeed + BulletSpeed * 0.25f * MaxAttributeUpgrade;
                    bulletInstance.GetComponent<PlayerBullet>().SetShootDirection(bulletInstance.transform.up);
                }
                yield return new WaitForSeconds(FireRate / AttackSpeed);
            }
            else
            {
                //Debug.Log("Can not Shoot");
                yield return new WaitForSeconds(FireRate / AttackSpeed);
            }
        }
        //yield return new WaitForSeconds(FireRate / AttackSpeed);
    }
    
}
