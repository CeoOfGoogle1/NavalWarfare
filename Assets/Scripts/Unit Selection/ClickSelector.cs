using UnityEngine;

public class ClickSelector : MonoBehaviour
{
    public SelectedList selectedList;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button clicked");

            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);

                GameObject clickedObject = hit.collider.gameObject;
                if (clickedObject.CompareTag("Unit"))
                {
                    Debug.Log("Clicked on a Unit: " + clickedObject.name);

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
            }
        }
    }
}
