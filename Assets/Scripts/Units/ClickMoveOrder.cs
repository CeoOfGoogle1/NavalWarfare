using UnityEngine;
using UnityEngine.AI;

public class ClickMoveOrder : MonoBehaviour
{

    public SelectedList selectedList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetDestinationOfAllSelectedUnits(position);
        }
    }

    void SetDestinationOfAllSelectedUnits(Vector2 position)
    {
        foreach (GameObject unit in selectedList.selectedList)
        {
            unit.GetComponent<Unit>().destination = position;
        }
    }
}
