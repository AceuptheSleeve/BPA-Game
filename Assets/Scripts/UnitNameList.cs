using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UnitNameList : MonoBehaviour
{
    public UnitSelections unitSelections;
    public Text unitsSelectedText;

    void Start()
    {
        
    }

    public void UpdateList()
    {
        foreach (GameObject unit in UnitSelections.Instance.unitsSelected)
        {
            Unit unitData = unit.GetComponent<Unit>();

            if (unit == !unitData.stats.building)
            {
                string unitsInList = "";

                foreach (GameObject _unit in UnitSelections.Instance.unitsSelected)
                {
                    if (unit == !unitData.stats.building)
                    {
                        unitsInList += _unit.name + "\nHP: " + unitData.currentHP + "/ 25\n";
                    }
                }

                unitsSelectedText.text = unitsInList;
            }

            if (UnitSelections.Instance.unitsSelected.Count == 0)
            {
                unitsSelectedText.text = "";
            }
        }
    }
}
