using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ButtonScript : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject hq;
    public Vector2 hqPos;
    public float xPos, yPos;
    public int unitIndex, ironCost, coalCost;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        hq = gameManager.playerUnits[0];
        hqPos = hq.transform.position;

        ironCost = gameManager.spawnCatalog[unitIndex].GetComponent<Unit>().stats.ironCost;
        coalCost = gameManager.spawnCatalog[unitIndex].GetComponent<Unit>().stats.coalCost;
    }

    public void OnButtonPress()
    {
        //Checking to see if the player has enough resources to spawn a unit
        if (gameManager.currentCoal >= coalCost && gameManager.currentIron >= ironCost)
        {
            SpawnUnit(unitIndex);
            gameManager.currentCoal = gameManager.currentCoal -= coalCost;
            gameManager.currentIron = gameManager.currentIron -= ironCost;
            Debug.Log("The player used " +coalCost+ " coal and " +ironCost+ " iron for " +gameManager.spawnCatalog[unitIndex].name);
            Debug.Log("They now have " +gameManager.currentCoal+ " coal and " +gameManager.currentIron+ " iron");
        }

        else
        {
            Debug.Log("Insufficient amount of resources");
        }
        
    }

    void SpawnUnit(int index)
    {
        Vector2 randomPos = hqPos + new Vector2(Random.Range(xPos, -xPos), Random.Range(yPos, -yPos));
        Instantiate(gameManager.spawnCatalog[index], randomPos, new Quaternion());
    }
}
