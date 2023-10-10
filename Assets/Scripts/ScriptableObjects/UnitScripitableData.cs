using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "new UnitData", order = 1)]
public class UnitStats : ScriptableObject
{
    public int totalHP;
    public float damage, attackRate, attackRange, speed, spawnTime, projectileSpeed;
    public GameObject projectilePrefab;
}
