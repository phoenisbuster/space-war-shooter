using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Level")]
[System.Serializable]
public class LevelSetting : ScriptableObject
{
    [Header("Back Ground")]
    public Sprite BG_Image;
    
    [Header("Difficulty of this Level")]
    public bool isStrongerEachWave = false;
    public float statMultipler = 1.0f;

    [Header("Properties for Meteorites of this Level")]
    public float MeteoriteGenSpeed = 0.1f;
    public float maxInterval = 10f;
    public float minInterval = 20f;
    public float maxSize = 1f;
    public float minSize = 0.25f;
    public List<Wave> WaveList;

    [Header("Properties for Item Drop Rate")]
    public float PowerUpRate = 15;
    public float ChangeSpaceShipRate = 15;
    public float HealthRate = 10;
    public float MoneyRate = 50;
}
