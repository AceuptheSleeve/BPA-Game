using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class UnitNameList : MonoBehaviour
{
    public UnitSelections unitSelections;
    public Text unitsSelectedText;

    // A method that updates the text of the unit selected text
    public void UpdateList()
    {
        foreach (GameObject unit in UnitSelections.Instance.unitsSelected) // Runs for every unit in the unitsSelected list
        {
            Unit unitData = unit.GetComponent<Unit>();

            if (unit == !unitData.stats.building)
            {
                string unitsInList = "";

                foreach (GameObject _unit in UnitSelections.Instance.unitsSelected) // Runs for every unit in the unitsSelected list
                {
                    if (unit == !unitData.stats.building)
                    {
                        unitData = _unit.GetComponent<Unit>();

                        unitsInList += _unit.name + "\nHP: " + unitData.currentHP + "/"+unitData.stats.totalHP+"\n";
                    }
                }

                unitsSelectedText.text = unitsInList;
            }
            // Clears text when there aren't any units
            if (UnitSelections.Instance.unitsSelected.Count == 0)
            {
                unitsSelectedText.text = "";
            }
        }
    }
}
