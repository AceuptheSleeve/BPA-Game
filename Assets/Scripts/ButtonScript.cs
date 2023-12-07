using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using Unity.Mathematics;

public class ButtonScript : MonoBehaviour
{
    public GameManager gameManager;
    public int unitIndex;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    public void OnButtonPress()
    {
        SpawnUnit(unitIndex);
    }

    void SpawnUnit(int index)
    {
        Instantiate(gameManager.spawnCatalog[index], new Vector2(), new Quaternion());
    }
}
