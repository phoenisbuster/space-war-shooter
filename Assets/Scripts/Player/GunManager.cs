using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public ShootAround ShootingScript;
    public Rigidbody2D playerBullet;
    public float UpgradeLv = 1;
    public float FireRate = 0.5f;
    public float BulletSpeed = 10f;
    public float BulletDist = 50f;
    public float MaxAttributeUpgrade = 1;
    public float AttackSpeed = 1;
    public float AtkSpeedPerLv = 0.05f;
    float curAtkSpeed = 1;
    public bool canShoot = false;
    public AudioClip ShootingSound;

    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<ShootAround>() == null)
            StartCoroutine(Shooting());
        else
        {
            ShootingScript = GetComponent<ShootAround>();
            ShootingScript.playerBullet = playerBullet;
            ShootingScript.UpgradeLv = UpgradeLv;
            ShootingScript.FireRate = FireRate;
            ShootingScript.BulletSpeed = BulletSpeed;
            ShootingScript.BulletDist = BulletDist;
            ShootingScript.MaxAttributeUpgrade = MaxAttributeUpgrade;
            ShootingScript.AttackSpeed = AttackSpeed;
            ShootingScript.AtkSpeedPerLv = AtkSpeedPerLv;
            ShootingScript.canShoot = canShoot;
            ShootingScript.ShootingSound = ShootingSound;
            ShootingScript.ShootCommand();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBullet(Rigidbody2D Bullet)
    {
        playerBullet = Bullet;
        if(ShootingScript != null)
        {
            ShootingScript.playerBullet = Bullet;
        }
    }
    public void SetAttckSpeed(int LvUpgrade)
    {
        curAtkSpeed = AttackSpeed*(1 + AtkSpeedPerLv*LvUpgrade);
        if(ShootingScript != null)
        {
            ShootingScript.AttackSpeed = curAtkSpeed;
        }
    }
    public void SetCanShoot(bool value)
    {
        canShoot = value;
        if(ShootingScript != null)
        {
            ShootingScript.canShoot = value;
        }
    }
    public IEnumerator Shooting()
    {
        //Debug.Log("Setting Shoot " + canShoot);
        while(playerBullet != null)
        {
            //Debug.Log("Keep Setting Shoot");
            if(canShoot && !transform.parent.parent.GetComponent<HealthManager>().isDead)
            {
                //Debug.Log("Can Shoot");
                Rigidbody2D bulletInstance;
                bulletInstance = Instantiate(playerBullet, transform.position, transform.rotation) as Rigidbody2D;
                bulletInstance.GetComponent<PlayerBullet>().DistanceTravel = BulletDist + BulletDist * 0.15f * MaxAttributeUpgrade;
                bulletInstance.GetComponent<PlayerBullet>().Speed = BulletSpeed + BulletSpeed * 0.25f * MaxAttributeUpgrade;
                bulletInstance.GetComponent<PlayerBullet>().SetShootDirection(bulletInstance.transform.up);
                yield return new WaitForSeconds(FireRate / curAtkSpeed);
            }
            else
            {
                //Debug.Log("Can not Shoot");
                yield return new WaitForSeconds(FireRate / curAtkSpeed);
            }
        }
    }
}
