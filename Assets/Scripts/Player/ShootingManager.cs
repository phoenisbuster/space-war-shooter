using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    public BulletTypesData playerBullet;
    public List<GameObject> GunList;
    public int UpgradeLv = 0;

    [Header("Level 2")]
    public bool isIncreaseNoBullet_2 = false;
    public bool isIncreaseAttrBullet_2 = false;
    public bool isIncreaseAtkSpeed_2 = false;
    public List<int> GunID_2;

    [Header("Level 3")]
    public bool isIncreaseNoBullet_3 = false;
    public bool isIncreaseAttrBullet_3 = false;
    public bool isIncreaseAtkSpeed_3 = false;
    public List<int> GunID_3;

    [Header("Level 4")]
    public bool isIncreaseNoBullet_4 = false;
    public bool isIncreaseAttrBullet_4 = false;
    public bool isIncreaseAtkSpeed_4 = false;
    public List<int> GunID_4;

    [Header("Level 5")]
    public bool isIncreaseNoBullet_5 = false;
    public bool isIncreaseAttrBullet_5 = false;
    public bool isIncreaseAtkSpeed_5 = false;
    public List<int> GunID_5;

    [Header("Level 6")]
    public bool isIncreaseNoBullet_6 = false;
    public bool isIncreaseAttrBullet_6 = false;
    public bool isIncreaseAtkSpeed_6 = false;
    public List<int> GunID_6;

    [Header("Level 7")]
    public bool isIncreaseNoBullet_7 = false;
    public bool isIncreaseAttrBullet_7 = false;
    public bool isIncreaseAtkSpeed_7 = false;
    public List<int> GunID_7;

    [Header("Level 8")]
    public bool isIncreaseNoBullet_8 = false;
    public bool isIncreaseAttrBullet_8 = false;
    public bool isIncreaseAtkSpeed_8 = false;
    public List<int> GunID_8;

    [Header("Level 9")]
    public bool isIncreaseNoBullet_9 = false;
    public bool isIncreaseAttrBullet_9 = false;
    public bool isIncreaseAtkSpeed_9 = false;
    public List<int> GunID_9;

    [Header("Level 10")]
    public bool isIncreaseNoBullet_10 = false;
    public bool isIncreaseAttrBullet_10 = false;
    public bool isIncreaseAtkSpeed_10 = false;
    public List<int> GunID_10;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if(!GunList.Contains(transform.GetChild(i).gameObject))    
                GunList.Add(transform.GetChild(i).gameObject);
        }
        StartCoroutine(PlayShootingShound());
    }

    public IEnumerator PlayShootingShound()
    {
        //Debug.Log("Setting Shoot " + canShoot);
        while(playerBullet != null && !transform.parent.GetComponent<HealthManager>().isDead)
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
            yield return new WaitForSeconds(0.1f);
        }
    }
    public void SetLevel(int level)
    {
        UpgradeLv = level;
        checkLevelUpgrade();
        //Debug.Log("Set Level: " + UpgradeLv);
    }
    public void checkLevelUpgrade()
    {
        switch(UpgradeLv)
        {
            case 2:
                Setlevel2();
                break;
            case 3:
                Setlevel3();
                break;
            case 4:
                Setlevel4();
                break;
            case 5:
                Setlevel5();
                break;
            case 6:
                Setlevel6();
                break;
            case 7:
                Setlevel7();
                break;
            case 8:
                Setlevel8();
                break;
            case 9:
                Setlevel9();
                break;
            case 10:
                Setlevel10();
                break;
            default:
                Setlevel1();
                break;
        }
    }

    void Setlevel1()
    {
        //Debug.Log("Set Level 1 With No Gun: " + GunList.Count);
        for(int i = 0; i < GunList.Count; i++)
        {
            if(i == 0)
            {
                GunList[i].GetComponent<GunManager>().SetCanShoot(true);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetCanShoot(false);
            }
            GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade = 1;
            GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
            GunList[i].GetComponent<GunManager>().StartCoroutine(GunList[i].GetComponent<GunManager>().Shooting());
            //Debug.Log("Set Gun ID: " + i);
        }
    }
    void Setlevel2()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_2)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(1);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_2)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_2)
            {
                if(GunID_2.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }        
    }

    void Setlevel3()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_3)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(2);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_3)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_3)
            {
                if(GunID_3.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel4()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_4)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(3);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_4)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_4)
            {
                if(GunID_4.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel5()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_5)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(4);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_5)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_5)
            {
                if(GunID_5.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel6()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_6)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(5);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_6)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_6)
            {
                if(GunID_6.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel7()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_7)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(6);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_7)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_7)
            {
                if(GunID_7.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel8()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_8)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(7);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_8)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_8)
            {
                if(GunID_8.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel9()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_9)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(8);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_9)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_9)
            {
                if(GunID_9.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    void Setlevel10()
    {
        for(int i = 0; i < GunList.Count; i++)
        {
            if(isIncreaseAtkSpeed_10)
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(20);
            }
            else
            {
                GunList[i].GetComponent<GunManager>().SetAttckSpeed(0);
            }
            if(isIncreaseAttrBullet_10)
            {
                GunList[i].GetComponent<GunManager>().MaxAttributeUpgrade++;
            }
            if(isIncreaseNoBullet_10)
            {
                if(GunID_10.Contains(i))
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(true);
                }
                else
                {
                    GunList[i].GetComponent<GunManager>().SetCanShoot(false);
                }
            }
            GunList[i].GetComponent<GunManager>().SetBullet(playerBullet.bullets[playerBullet.bulletOrder[i]]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
