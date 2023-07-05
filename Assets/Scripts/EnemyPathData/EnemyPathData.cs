using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyPath")]
public class EnemyPathData : ScriptableObject
{
    /*
        Left: 0-9
        Right: 10-19
        Low: 20-25
        Up: 26-31
        Mid: 32-81
    */
    public List<int> firstPoint;
    public List<int> middlePointIndex;
    public float time = 0.5f;
    public GameObject testGObj;
}
