using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int CurLevel = 1;
    public int OldLevel = 1;
    public float Lv10Time = 5f;
    public float CurLv10Time = 0;
    public bool isLv10 = false;
    public TMP_Text Lv_UI;
    public Image Lv10_UI;
    // Start is called before the first frame update
    void Start()
    {
        Lv10_UI.enabled = false;
    }

    public void SetLevel(int lvUp = 1, GameObject Ship = null)
    {
        if(OldLevel != CurLevel && lvUp != 0 && CurLevel != 10)
            OldLevel = CurLevel;
        if(lvUp == 1)
            CurLevel = CurLevel==10? 10 : CurLevel+1;
        else if(lvUp > 1 && lvUp <= 10)
            CurLevel = lvUp > CurLevel? lvUp : CurLevel;
        else if(lvUp > 10)
            CurLevel = 1;
        else if(lvUp == 0)
            CurLevel = OldLevel;
        else
            CurLevel = CurLevel==1? 1 : CurLevel-1;
        if(Ship == null)
            transform.GetChild(0).GetComponent<ShootingManager>().SetLevel(CurLevel);
        else
            Ship.GetComponent<ShootingManager>().SetLevel(GetComponent<PlayerStats>().CurLevel);
        if(CurLevel == 10)
        {
            isLv10 = true;
            Lv10_UI.enabled = true;
            CurLv10Time = Lv10Time;
        }
        Lv_UI.text = "Lv: " + CurLevel;
    }

    public void SetlvDebug()
    {
        SetLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            SetLevel(1);
        }
        if(CurLv10Time > 0 && isLv10)
        {
            CurLv10Time -= Time.deltaTime;
            if(Lv10_UI.enabled)
                Lv10_UI.GetComponent<Image>().fillAmount = CurLv10Time/Lv10Time;
        }
        else if(CurLv10Time <= 0 && isLv10)
        {
            isLv10 = false;
            Lv10_UI.enabled = false;
            SetLevel(0);
        }
        else if(!isLv10)
        {
            Lv10_UI.enabled = false;
        }
    }
}
