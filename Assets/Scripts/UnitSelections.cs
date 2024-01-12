using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public PlayerController playerController;
    public UnitNameList unitNameList;

    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    private static UnitSelections _instance;
    public static UnitSelections Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd) // Clicking on a unit sets it to be the only selected unit
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);

        playerController.SelectUnit(unitToAdd.GetComponent<Unit>(), true);
        unitNameList.UpdateList();
    }

    public void ShiftClickSelect(GameObject unitToAdd) // Shift clicking adds/removes the unit
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);

            playerController.SelectUnit(unitToAdd.GetComponent<Unit>(), true);
            unitNameList.UpdateList();
        }
        else
        {
            unitsSelected.Remove(unitToAdd);

            playerController.SelectUnit(unitToAdd.GetComponent<Unit>(), false);
            unitNameList.UpdateList();
        }
    }

    public void DragSelect(GameObject unitToAdd) // Drag a rectangle and all that's in it is selected
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            playerController.SelectUnit(unitToAdd.GetComponent<Unit>(), true);
            unitNameList.UpdateList();
        }
    }

    public void DeselectAll() // Removes all units from the list
    {
        unitsSelected.Clear();

        foreach (GameObject unit in unitsSelected)
        {
            playerController.SelectUnit(unit.GetComponent<Unit>(), false);
        }

        unitNameList.UpdateList();
    }
}
