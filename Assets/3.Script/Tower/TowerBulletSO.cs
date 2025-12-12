using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bullet", menuName = "BulletSO")]
public class TowerBulletSO : ScriptableObject
{
    public float speed = 10f;
    public float lifeTime = 5f;
    public GameObject bulletSkin;
    public GameObject hitSkin;
}
