using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tower", menuName = "TowerSetting")]
public class TowerSO : ScriptableObject
{
    [Header("타워 정보")]
    public TowerType type;
    public int tier;
    public string towerName;
    public GameObject prefabs;
    public int manaCost;

    [Header("타워 공격")]
    public float attackSpeed;
    public float towerDamage;
    public float attackRange;

    public bool isSplash;
    public float splashRange;

    [Header("업그레이드")]
    public List<TowerSO> nextTierTower;
   
}
