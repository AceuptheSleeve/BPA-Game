using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "new UnitData", order = 1)]
public class UnitStats : ScriptableObject
{
    public string unitName;
    public int totalHP, electricUsage, coalCost, ironCost;
    public float damage, attackRate, attackRange, speed, spawnTime;
    public bool worker, building, iron, coal;
}
