using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Unit[] playerUnits;
    public EnemyUnit[] enemyUnits;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        playerUnits = FindObjectsOfType<Unit>();
        enemyUnits = FindObjectsOfType<EnemyUnit>();
    }
}
