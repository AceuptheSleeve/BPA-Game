using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Unit[] playerUnits;
    public EnemyUnit[] enemyUnits;
    private Vector2 mousePos;
    public List<GameObject> spawnUnits = new List<GameObject>();
    public List<string> names = new List<string>();
    public float coalCount, ironCount, electricCount, coalCap, ironCap, electricCap;

    // Start is called before the first frame update
    void Awake()
    {
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\24CarterK\Downloads\BPA Project\Assets\Names.txt");
        foreach (string line in lines)
        {
            string[] subs = line.Split("  ");
            names.Add(subs[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var input = Input.inputString;

        playerUnits = FindObjectsOfType<Unit>();
        enemyUnits = FindObjectsOfType<EnemyUnit>();

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
        }
    }
}
