using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public List<GameObject> ObjectsForThisWave;

    public List<EnemiesShape> enemiesShapes;
    public List<EnemyPathData> enemyPathDatas;
}
