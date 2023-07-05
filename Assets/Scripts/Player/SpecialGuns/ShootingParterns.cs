using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingParterns : MonoBehaviour
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

    public virtual void ShootCommand(){}
}
