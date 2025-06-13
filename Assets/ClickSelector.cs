using UnityEngine;

public class ClickSelector : MonoBehaviour
{
    public SelectedList selectedList;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
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
            }
        }
    }
}
