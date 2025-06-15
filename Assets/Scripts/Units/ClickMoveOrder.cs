using UnityEngine;
using UnityEngine.AI;

public class ClickMoveOrder : MonoBehaviour
{

    public SelectedList selectedList;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            Debug.Log("Right mouse button clicked, setting destination for selected units.");
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetDestinationOfAllSelectedUnits(position);
        }
    }

    void SetDestinationOfAllSelectedUnits(Vector2 position)
    {
        foreach (GameObject unit in selectedList.selectedList)
        {
            Debug.Log($"Setting destination for unit: {unit.name} to position: {position}");
            unit.GetComponent<Unit>().destination = position;
        }
    }
}
