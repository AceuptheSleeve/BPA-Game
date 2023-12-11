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
        currentCoal = 100;
        currentIron = 100;
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

        /*Fixing 'Object Disposed Exception'?
        if (enemyUnits.Count == 0)
        {
            Instantiate();

            if (enemyUnits.Count != 0)
            {
                Destroy(gameObject);
            }
        }
        */
    }
}
