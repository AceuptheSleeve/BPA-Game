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
    public float xPos;
    public float yPos;
    public int unitIndex;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        hq = GameObject.Find("HQ");
        hqPos = hq.transform.position;
    }

    public void OnButtonPress()
    {
        SpawnUnit(unitIndex);
    }

    void SpawnUnit(int index)
    {
        Vector2 randomPos = hqPos + new Vector2(Random.Range(xPos, -xPos), Random.Range(yPos, -yPos));
        Instantiate(gameManager.spawnCatalog[index], randomPos, new Quaternion());
    }
}
