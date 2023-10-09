using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "new UnitData", order = 1)]
public class UnitStats : ScriptableObject
{
    public int hp, xp;
    public float damage, attackRate, attackRange, speed;
    public bool melee;
    public GameObject model, projectilePrefab;
}
