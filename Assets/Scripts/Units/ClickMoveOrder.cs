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
                //SetDestinationOfAllSelectedUnits(position);
                MoveUnitsInConcentricCircles2D(position);
            }
        }
    }

    void MoveUnitsInConcentricCircles2D(Vector2 center)
{
    int unitCount = selectedList.selectedList.Count;
    if (unitCount == 0) return;

    if (unitCount == 1)
    {
        // Если только один юнит, просто перемещаем его в указанную позицию
        selectedList.selectedList[0].GetComponent<Unit>().SetDestination(center);
        return;
    }

    float startRadius = 10f;      // Радиус самого внутреннего круга
    float radiusStep = 10f;       // Шаг между кругами
    float unitSpacing = 5f;      // Расстояние между юнитами по дуге

    int unitsPlaced = 0;
    int circleIndex = 0;

    while (unitsPlaced < unitCount)
    {
        circleIndex++;
        float currentRadius = startRadius + (circleIndex - 1) * radiusStep;
        float circumference = 2 * Mathf.PI * currentRadius;
        int maxUnitsOnCircle = Mathf.FloorToInt(circumference / unitSpacing);

        int unitsOnThisCircle = Mathf.Min(maxUnitsOnCircle, unitCount - unitsPlaced);

        for (int i = 0; i < unitsOnThisCircle; i++)
        {
            float angle = i * (2 * Mathf.PI / unitsOnThisCircle);
            float x = center.x + currentRadius * Mathf.Cos(angle);
            float y = center.y + currentRadius * Mathf.Sin(angle);

            Vector2 targetPos = new Vector2(x, y);
            selectedList.selectedList[unitsPlaced].GetComponent<Unit>().SetDestination(targetPos); // Вызов твоей логики движения
            unitsPlaced++;
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
            unit.GetComponent<Unit>().SetTarget(target);
        }
    }
}
