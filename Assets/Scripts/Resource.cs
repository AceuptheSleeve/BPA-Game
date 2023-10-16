using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class Resource : MonoBehaviour
{
    public TilemapCollider2D tilemapCollider;
    public BoxCollider2D hitBox;
    public GameManager gameManager;
    //true for metal it's iron, false means it's coal
    public bool metal;

    // Start is called before the first frame update
    void Start()
    {
        //Assigning values
        tilemapCollider = GetComponent<TilemapCollider2D>();
        hitBox = GetComponent<BoxCollider2D>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void LateUpdate()
    {

    }

    public void addToPool(float damage)
    {
        if (metal == true)
        {
            gameManager.ironCount = gameManager.ironCount += damage;
        }

        else
        {
            gameManager.coalCount = gameManager.coalCount += damage;
        }
    }
}
