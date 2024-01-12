using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameManager gameManager;
    public Text baseHPText, energyText, ironText, coalText;
    public float hitPoints;

    void Update() // Sets all the stats in the UI
    {
        baseHPText.text = "Base HP: " + GameObject.Find("HQ").GetComponent<Unit>().currentHP + "/500";
        energyText.text = "Energy: " + GameObject.Find("Game Manager").GetComponent<GameManager>().electricPool + "/100";
        ironText.text = "Iron: " + GameObject.Find("Game Manager").GetComponent<GameManager>().currentIron + "/" + GameObject.Find("Game Manager").GetComponent<GameManager>().ironCap;
        coalText.text = "Coal: " + GameObject.Find("Game Manager").GetComponent<GameManager>().currentCoal + "/" + GameObject.Find("Game Manager").GetComponent<GameManager>().coalCap;
    }
}
