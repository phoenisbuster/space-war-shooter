using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BulletTypesData")]
public class BulletTypesData : ScriptableObject
{
    public List<Rigidbody2D> bullets;
    public List<int> bulletOrder;
}
