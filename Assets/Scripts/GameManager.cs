using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public List<GameObject> playerUnits, playerWorkers, enemyUnits, coalList, ironList = new List<GameObject>();
    public UnitStats[] stats;
    public GameObject[] spawnCatalog;
    public List<string> names = new List<string>();
    public float currentCoal, currentIron, coalCap, ironCap, electricPool, nextWaveTime = 0;
    public PlayerController playerController;
    public Tilemap[] mapLayers;
    public AudioClip[] soundBank;
    private int currentWave;
    public List<Vector2> spawnPoints = new List<Vector2>();

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

        //Spawn tool
        switch (Input.inputString)
        {
            //Player infantry spawn
            case "1":
                Instantiate(spawnCatalog[0], playerController.mousePos, new Quaternion());
                break;
            //Enemy infantry spawning
            case "2":
                Instantiate(spawnCatalog[1], playerController.mousePos, new Quaternion());
                break;
            //Worker spawn
            case "3":
                Instantiate(spawnCatalog[2], playerController.mousePos, new Quaternion());
                break;
            //Coal resource spawn
            case "4":
                Instantiate(spawnCatalog[3], playerController.mousePos, new Quaternion());
                break;
            //Iron resource spawn
            case "5":
                Instantiate(spawnCatalog[4], playerController.mousePos, new Quaternion());
                break;
        }

        switch (currentCoal)
        {
            case 10:
                Debug.Log("The coal total is now 10!");
                break;

            case 20:
                Debug.Log("The coal total is now 20!");
                break;

            case 30:
                Debug.Log("The coal total is now 30!");
                break;

            case 40:
                Debug.Log("The coal total is now 40!");
                break;

            case 50:
                Debug.Log("The coal total is now 50!");
                break;

            case 60:
                Debug.Log("The coal total is now 60!");
                break;

            case 70:
                Debug.Log("The coal total is now 70!");
                break;

            case 80:
                Debug.Log("The coal total is now 80!");
                break;

            case 90:
                Debug.Log("The coal total is now 90!");
                break;
        }

        switch (currentIron)
        {
            case 10:
                Debug.Log("The iron total is now 10!");
                break;

            case 20:
                Debug.Log("The iron total is now 20!");
                break;

            case 30:
                Debug.Log("The iron total is now 30!");
                break;

            case 40:
                Debug.Log("The iron total is now 40!");
                break;

            case 50:
                Debug.Log("The iron total is now 50!");
                break;

            case 60:
                Debug.Log("The iron total is now 60!");
                break;

            case 70:
                Debug.Log("The iron total is now 70!");
                break;

            case 80:
                Debug.Log("The iron total is now 80!");
                break;

            case 90:
                Debug.Log("The iron total is now 90!");
                break;
        }

        //Enforcing resource caps
        if (currentCoal >= coalCap)
        {
            currentCoal = coalCap;
        }

        if (currentIron >= ironCap)
        {
            currentIron = ironCap;
        }

        if (Time.time >= nextWaveTime)
        {
            SpawnWave(currentWave);
            nextWaveTime = Time.time + 45;
            currentWave++;
        }
    }
    void SpawnWave(int wave)
    {
        Debug.Log("Wave " + currentWave + " has started!");
        int enemiesToSpawn = 5 * wave;
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            int chosenPoint = Random.Range(0, spawnPoints.Count);
            Instantiate(spawnCatalog[1], new Vector2(spawnPoints[chosenPoint].x + Random.Range(-5, 40), spawnPoints[chosenPoint].y + Random.Range(-5, 40)), new Quaternion());
        }
        Debug.Log(enemiesToSpawn + " enemies have been spawned in!");
    }
}


