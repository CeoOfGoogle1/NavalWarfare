using UnityEngine;
using Unity.Netcode;

public class ClickSelector : NetworkBehaviour
{
    public SelectedList selectedList;

    void Update()
    {
        ThrowRay();
    }

    private void ThrowRay()
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
                    if (InputManager.Instance.IsShiftPressed() && !unitComponent.IsSelected)
                    {
                        // If shift is pressed and unit is not selected, add to selection
                        unitComponent.IsSelected = true;
                        selectedList.selectedList.Add(clickedObject);
                    }
                    else if (InputManager.Instance.IsShiftPressed() && unitComponent.IsSelected)
                    {
                        // If shift is pressed, toggle selection
                        unitComponent.IsSelected = false;
                    }
                    else
                    {
                        foreach (GameObject unit in selectedList.selectedList)
                        {
                            unit.GetComponent<Unit>().IsSelected = false;
                        }
                        unitComponent.IsSelected = true;
                    }
                }
                else
                {
                    // If clicked object is not a unit, deselect all units
                    foreach (GameObject unit in selectedList.selectedList)
                    {
                        unit.GetComponent<Unit>().IsSelected = false;
                    }
                }
            }
            else
            {
                // If no unit is clicked, deselect all units
                foreach (GameObject unit in selectedList.selectedList)
                {
                    unit.GetComponent<Unit>().IsSelected = false;
                }
            }
        }
    }

}
