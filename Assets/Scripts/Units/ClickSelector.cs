using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;
using System;

public class ClickSelector : NetworkBehaviour
{
    public SelectedList selectedList;
    Vector2 startPosition = Vector2.zero;

    void Update()
    {
        ThrowRay();

        DrawSelectionBox();
    }

    private void DrawSelectionBox()
    {

        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }


        if (Input.GetMouseButtonUp(0))
        {
            if (startPosition == Vector2.zero) return;

            if (!InputManager.Instance.IsShiftPressed())
            {
                // If shift is not pressed, clear the selection list
                foreach (GameObject unit in selectedList.selectedList)
                {
                    unit.GetComponent<Unit>().IsSelected = false;
                }
                selectedList.selectedList.Clear();
            }

            Collider2D[] colliders = Physics2D.OverlapAreaAll(startPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Unit"))
                {
                    if (!collider.GetComponent<NetworkBehaviour>().IsOwner) return;

                    Unit unitComponent = collider.GetComponent<Unit>();

                    collider.GetComponent<Unit>().IsSelected = true;
                    if (!selectedList.selectedList.Contains(collider.gameObject))
                    {
                        selectedList.selectedList.Add(collider.gameObject);
                    }
                }
            }
        }
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
