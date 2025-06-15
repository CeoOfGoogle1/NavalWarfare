using UnityEngine;
using Unity.Netcode;

public class ClickSelector : NetworkBehaviour
{
    public SelectedList selectedList;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.CompareTag("Unit"))
                {
                    if (!clickedObject.GetComponent<NetworkBehaviour>().IsOwner) return;

                    Unit unitComponent = clickedObject.GetComponent<Unit>();
                    if (InputManager.Instance.IsShiftPressed() && !unitComponent.isSelected)
                    {
                        // If shift is pressed and unit is not selected, add to selection
                        unitComponent.isSelected = true;
                        selectedList.selectedList.Add(clickedObject);
                    }
                    else if (InputManager.Instance.IsShiftPressed() && unitComponent.isSelected)
                    {
                        // If shift is pressed, toggle selection
                        unitComponent.isSelected = false;
                    }
                    else
                    {
                        foreach (GameObject unit in selectedList.selectedList)
                        {
                            unit.GetComponent<Unit>().isSelected = false;
                        }
                        unitComponent.isSelected = true;
                    }
                }
                else
                {
                    // If clicked object is not a unit, deselect all units
                    foreach (GameObject unit in selectedList.selectedList)
                    {
                        unit.GetComponent<Unit>().isSelected = false;
                    }
                }
            }
            else
            {
                // If no unit is clicked, deselect all units
                foreach (GameObject unit in selectedList.selectedList)
                {
                    unit.GetComponent<Unit>().isSelected = false;
                }
            }
        }
    }
}
