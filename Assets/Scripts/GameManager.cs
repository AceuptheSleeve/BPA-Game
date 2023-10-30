using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] playerUnits;
    public GameObject[] playerWorkers;
    public GameObject[] enemyUnits;
    private Vector2 mousePos;
    public List<GameObject> spawnUnits = new List<GameObject>();
    public List<string> names = new List<string>();
    public float currentCoal, currentIron, coalCap, ironCap, electricPool;

    // Start is called before the first frame update
    void Awake()
    {
        //Establishes the name list
        string[] lines = File.ReadAllLines(Path.Combine(Application.dataPath, "names.txt"));
        foreach (string line in lines)
        {
            string[] subs = line.Split("  ");
            names.Add(subs[0]);
        }

        //Resources
        coalCap = 100;
        ironCap = 100;
        electricPool = 100;
        currentCoal = 0;
        currentIron = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var input = Input.inputString;

        //Keeping tabs on all objects at once
        playerUnits = GameObject.FindGameObjectsWithTag("Unit");
        playerWorkers = GameObject.FindGameObjectsWithTag("Worker");
        enemyUnits = GameObject.FindGameObjectsWithTag("Enemy");

        //Spawn tool
        switch (input)
        {
            case "1":
                Unit unit1 = spawnUnits[0].GetComponent<Unit>();
                unit1.SpawnUnit(mousePos);
                break;

            case "2":
                EnemyUnit enemyUnit1 = spawnUnits[1].GetComponent<EnemyUnit>();
                enemyUnit1.SpawnUnit(mousePos);
                break;

            case "3":
                Unit worker = spawnUnits[2].GetComponent<Unit>();
                worker.SpawnUnit(mousePos);
                break;

            case "4":
                EnemyUnit coal = spawnUnits[3].GetComponent<EnemyUnit>();
                coal.SpawnUnit(mousePos);
                break;

            case "5":
                EnemyUnit iron = spawnUnits[4].GetComponent<EnemyUnit>();
                iron.SpawnUnit(mousePos);
                break;
        }

        //Setting resource caps
        if (currentCoal >= coalCap)
        {
            currentCoal = coalCap;
        }

        if (currentIron >= ironCap)
        {
            currentIron = ironCap;
        }
    }
}
