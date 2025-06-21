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

    public void ProjectileImpact(Projectile projectile, Vector2 hitPosition)
    {
        float roll = Random.Range(-10f, 10f);
        float effectiveArmor = shipArmor + roll;

        bool penetrated = projectile.penetration > effectiveArmor;
        float fireChance = projectile.damage * projectile.penetration * (penetrated ? 10.0f : 1.0f);
        float floodChance = projectile.damage * projectile.penetration * (penetrated ? 10.0f : 1.0f);;

        ShipComponent nearestComponent = GetNearestComponent(transform.position);
        if (Random.value < fireChance)
        {
            nearestComponent.onFire = true;
        }
        if (Random.value < floodChance)
        {
            nearestComponent.isFlooded = true;
        }

        if (penetrated)
        {
            nearestComponent.TakeDamage(1);
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
    }
    private void AbandonShip()
    {

    }
}
