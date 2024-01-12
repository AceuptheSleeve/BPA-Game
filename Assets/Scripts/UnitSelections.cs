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

    public void ClickSelect(GameObject unitToAdd)
    {
        DeselectAll();
        unitsSelected.Add(unitToAdd);

        playerController.SelectUnit(unitToAdd.GetComponent<Unit>(), true);
        unitNameList.UpdateList();
    }

    public void ShiftClickSelect(GameObject unitToAdd)
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

    public void DragSelect(GameObject unitToAdd)
    {
        if (!unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            playerController.SelectUnit(unitToAdd.GetComponent<Unit>(), true);
            unitNameList.UpdateList();
        }
    }

    public void DeselectAll()
    {
        unitsSelected.Clear();

        foreach (GameObject unit in unitsSelected)
        {
            playerController.SelectUnit(unit.GetComponent<Unit>(), false);
        }

        unitNameList.UpdateList();
    }
}
