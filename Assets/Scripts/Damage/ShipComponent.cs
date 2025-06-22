using System.Collections.Generic;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    public float defaultHealth;
    public float disableRange;
    private float health;
    [HideInInspector] public bool onFire;
    [HideInInspector] public bool isFlooded;
    private DamageController damageController;
    private Unit unit;
    private List<TurretController> turrets;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        damageController = GetComponentInParent<DamageController>();
        unit = GetComponentInParent<Unit>();

        turrets = new List<TurretController>();
        TurretController[] turretControllers = transform.parent.GetComponentsInChildren<TurretController>(true);
        turrets.AddRange(turretControllers);

        health = defaultHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            DisableComponent();
        }

        ApplyDebuffOrBuff(false, false);
        Debug.Log($"Component {name} took {amount} damage. Current health: {health}");
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

        if (Random.value <= 0.2f && health > 0 && health < defaultHealth)
        {
            health += 1; // repair
            Debug.Log($"Repairing component {name}. Current health: {health}");
            ApplyDebuffOrBuff(true, false);
        }
    }

    private void DisableComponent()
    {
        Debug.Log($"Disabling component {name}");
        ApplyDebuffOrBuff(false, true);
    }

    public void ApplyDebuffOrBuff(bool isBuff, bool isDisable)
    {
        string componentName = name;
        switch (componentName)
        {
            case "Engine":
                if (isDisable) { unit.maxSpeed = 0; break; }
                if (Random.value < 0.2f)
                {
                    if (isBuff)
                    {
                        unit.maxSpeed *= 1.2f;
                    }
                    else
                    {
                        unit.maxSpeed *= 0.8f;
                    }
                }
                break;
            case "FrontMagazine":
                if (isDisable) { DisableOrDebuffNearestTurrets(transform.position, true, false); break; }
                if (Random.value < 0.2f)
                {
                    if (isBuff)
                    {
                        DisableOrDebuffNearestTurrets(transform.position, false, true);
                    }
                    else
                    {
                        if (Random.value > 0.95f)
                        {
                            DisableOrDebuffNearestTurrets(transform.position, true, false);
                        }
                        else
                        {
                            DisableOrDebuffNearestTurrets(transform.position, false, false);
                        }
                    }
                }
                break;
            case "BackMagazine":
                if (isDisable) { DisableOrDebuffNearestTurrets(transform.position, true, false); break; }
                if (Random.value < 0.2f)
                {
                    if (isBuff)
                    {
                        DisableOrDebuffNearestTurrets(transform.position, false, true);
                    }
                    else
                    {
                        if (Random.value > 0.95f)
                        {
                            DisableOrDebuffNearestTurrets(transform.position, true, false);
                        }
                        else
                        {
                            DisableOrDebuffNearestTurrets(transform.position, false, false);
                        }
                    }
                }
                break;
            case "Rudder":
                if (isDisable) { unit.turnSpeed = 0; break; }
                if (Random.value < 0.2f)
                {
                    if (isBuff)
                    {
                        unit.turnSpeed *= 1.2f;
                    }
                    else
                    {
                        unit.turnSpeed *= 0.8f;
                    }
                }
                break;
        }
    }

    public TurretController[] DisableOrDebuffNearestTurrets(Vector2 position, bool killOrDebuff, bool fix)
    {
        List<TurretController> affectedTurrets = new List<TurretController>();

        foreach (TurretController turret in turrets)
        {
            float distance = Vector2.Distance(turret.transform.position, position);
            if (distance <= disableRange)
            {
                if (fix)
                {
                    turret.enabled = true;
                    turret.rotationSpeed *= 1.5f;
                    continue;
                }
                else if (killOrDebuff)
                {
                    turret.enabled = false;
                }
                else
                {
                    turret.rotationSpeed *= 0.5f;
                }
                affectedTurrets.Add(turret);
            }
        }
        
        Debug.Log($"Affected turrets: {affectedTurrets.Count} within range {disableRange} from position {position}");
        return affectedTurrets.ToArray();
    }
}
