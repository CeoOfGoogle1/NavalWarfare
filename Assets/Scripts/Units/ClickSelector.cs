using UnityEngine;

public class ClickSelector : MonoBehaviour
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
                    Unit unitComponent = clickedObject.GetComponent<Unit>();
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        unitComponent.isSelected = false;
                    }
                    else if (Input.GetKey(KeyCode.LeftControl))
                    {
                        unitComponent.isSelected = true;
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
