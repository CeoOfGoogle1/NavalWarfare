using System.Collections.Generic;
using UnityEngine;

public class SelectedList : MonoBehaviour
{
    // Create a list to store selected Units
    public List<GameObject> selectedList = new List<GameObject>();
    public List<GameObject> allUnitsList = new List<GameObject>();
    void Update()
    {
        allUnitsList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Unit"));
        // Iterate through all units in the scene
        foreach (GameObject unit in allUnitsList)
        {
            // Check if the unit is selected
            if (unit.GetComponent<Unit>().isSelected == true)
            {
                if (!selectedList.Contains(unit))
                {
                    // If the unit is selected and not already in the selectedList, add it
                    selectedList.Add(unit);
                }
            }
            else
            {
                if (selectedList.Contains(unit))
                {
                    // If the unit is not selected and already in the selectedList, remove it
                    selectedList.Remove(unit);
                }
            }
        }
    }
}
