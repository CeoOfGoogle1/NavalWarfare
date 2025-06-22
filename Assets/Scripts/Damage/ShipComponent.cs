using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    public bool isMain;
    public bool onFire;
    public bool isFlooded;
    public float health;
    private DamageController damageController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageController = GetComponentInParent<DamageController>();
        health *= damageController.shipResistance;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DisableComponent();
        }
    }

    public void Tick()
    {
        ShipComponent nearestComponent = damageController.GetNearestComponent(transform.position);

        if (onFire)
        {
            TakeDamage(1);
            if (Random.value < 0.2f) nearestComponent.onFire = true;
            if (Random.value < 0.5f) onFire = false;
            Debug.Log($"Component {name} is on fire. Current health: {health}");
        }

        if (isFlooded)
        {
            TakeDamage(1);
            if (Random.value < 0.2f) nearestComponent.isFlooded = true;
            if (Random.value < 0.5f) isFlooded = false;
            Debug.Log($"Component {name} is flooded. Current health: {health}");
        }

        if (Random.value <= 0.2f && health > 0)
        {
            health += 1; // repair
            Debug.Log($"Repairing component {name}. Current health: {health}");
        }
    }

    private void DisableComponent()
    {
        Debug.Log($"Disabling component {name}");
    }
}
