using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageController : MonoBehaviour
{
    public float shipArmor;
    public float shipResistance;
    public float tickInterval => shipResistance / 60f;
    public float tickTimer;
    public int fireCount;
    public int floodCount;
    public List<ShipComponent> components;

    // Update is called once per frame
    void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickInterval)
        {
            tickTimer = 0f;
            TickEffects();
        }
    }

    public void ProjectileImpact(Vector2 hitPosition, float damage, float penetration)
    {
        Debug.Log($"Projectile impact at {hitPosition} with damage {damage} and penetration {penetration}");
        float roll = Random.Range(-10f, 10f);
        float effectiveArmor = shipArmor + roll;

        bool penetrated = penetration > effectiveArmor;
        float fireChance = damage * penetration * (penetrated ? 10.0f : 1.0f);
        float floodChance = damage * penetration * (penetrated ? 10.0f : 1.0f);

        ShipComponent nearestComponent = GetNearestComponent(hitPosition);
        if (Random.value < fireChance)
        {
            nearestComponent.onFire = true;
            Debug.Log($"Setting component {nearestComponent.name} on fire with damage {damage} and penetration {penetration}. Effective armor: {effectiveArmor}");
        }
        if (Random.value < floodChance)
        {
            nearestComponent.isFlooded = true;
            Debug.Log($"Flooding component {nearestComponent.name} with damage {damage} and penetration {penetration}. Effective armor: {effectiveArmor}");
        }

        if (penetrated)
        {
            nearestComponent.TakeDamage(1);
            Debug.Log($"Hit component {nearestComponent.name} with damage {damage} and penetration {penetration}. Effective armor: {effectiveArmor}. Penetrated: {penetrated}");
        }
    }

    void TickEffects()
    {
        fireCount = 0;
        floodCount = 0;

        foreach (ShipComponent component in components)
        {
            component.Tick();

            if (component.isMain)
            {
                if (component.onFire) fireCount++;
                if (component.isFlooded) floodCount++;
            }
        }

        if (fireCount >= 3) AbandonShip();
        if (floodCount >= 3) Sink();
    }

    public ShipComponent GetNearestComponent(Vector2 position)
    {
        return components.OrderBy(c => Vector2.Distance(c.transform.position, position)).FirstOrDefault();
    }

    private void Sink()
    {
        AbandonShip();
        Debug.Log("Ship is sinking!");
        // Implement logic for sinking the ship, e.g., triggering an animation
    }
    private void AbandonShip()
    {
        Debug.Log("Abandon Ship!");
        // Implement logic for abandoning the ship, e.g., triggering an animation, disabling controls, etc.
        // This could also involve notifying other players or game systems about the ship's status.
    }
}
