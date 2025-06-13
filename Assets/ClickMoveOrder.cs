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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 1.0f, NavMesh.AllAreas))
                {
                    MoveAllSelectedUnits(navHit.position);
                }
            }
        }
    }

    void MoveAllSelectedUnits(Vector3 destination)
    {
        foreach (GameObject Unit in selectedList.selectedList)
        {
            NavMeshAgent agent = Unit.GetComponent<NavMeshAgent>();
            agent.SetDestination(destination);
        }
    }
}
