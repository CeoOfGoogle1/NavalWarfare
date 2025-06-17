using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class ClickMoveOrder : MonoBehaviour
{

    public SelectedList selectedList;

    void Update()
    {
        OnClick();
    }

    private void OnClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null)
            {
                Transform target = hit.collider.transform;
                if (target.CompareTag("Unit"))
                {
                    if (target.GetComponent<NetworkBehaviour>().IsOwner) return;

                    SetTargetForAllSelectedUnits(target);
                }
            }
            else
            {
                // If no target is clicked, set destination for all selected units
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SetDestinationOfAllSelectedUnits(position);
            }
        }
    }

    void SetDestinationOfAllSelectedUnits(Vector2 position)
    {
        foreach (GameObject unit in selectedList.selectedList)
        {
            unit.GetComponent<Unit>().SetDestination(position);
        }
    }
    
    void SetTargetForAllSelectedUnits(Transform target)
    {
        foreach (GameObject unit in selectedList.selectedList)
        {
            Debug.Log($"Setting target for unit: {unit.name} to target: {target.name}");
            unit.GetComponent<Unit>().SetTarget(target);
        }
    }
}
