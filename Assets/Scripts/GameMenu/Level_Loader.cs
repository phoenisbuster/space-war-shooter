using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Level_Loader : MonoBehaviour
{
    public GameObject UI_Level_Button;

    public List<LevelSetting> Lv_List;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelSetting[] result1 = Array.ConvertAll(Resources.LoadAll("Levels", typeof(LevelSetting)), asset => (LevelSetting)asset);
        Lv_List = result1.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
