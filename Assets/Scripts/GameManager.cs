using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public List<GameObject> playerUnits, playerWorkers, enemyUnits = new List<GameObject>();
    public GameObject[] spawnCatalog;

    public List<string> names = new List<string>();
    public float currentCoal, currentIron, coalCap, ironCap, electricPool;
    public PlayerController playerController;
    public Tilemap[] mapLayers;
    public AudioClip[] soundBank;
    private int currentWave;
    public List<Vector2> spawnPoints = new List<Vector2>();
    public bool gameOn = false;

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

        //Enforcing resource caps
        if (currentCoal >= coalCap)
        {
            currentCoal = coalCap;
        }

        if (currentIron >= ironCap)
        {
            currentIron = ironCap;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            currentWave++;
            Debug.Log("60 seconds until wave " + currentWave+ " starts...");
            SpawnWave(currentWave);
        }
    }

    void SpawnWave(int wave)
    {
        int enemiesToSpawn = 5 * wave;
        int chosenPoint;


        for (int i = 0; i < enemiesToSpawn; i++)
        {
            chosenPoint = Random.Range(0, spawnPoints.Count);
            Instantiate(spawnCatalog[1], new Vector2(spawnPoints[chosenPoint].x + Random.Range(-5, 40), spawnPoints[chosenPoint].y + Random.Range(-5, 40)), new Quaternion());
        }

        Debug.Log(enemiesToSpawn+ "enemies have been spawned in");
    }
}
