using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HitboxComponent : MonoBehaviour
{
    [SerializeField] private HealthComponent healthComponent;

    private void Start()
    {
        // Fetch the HealthComponent attached to the same GameObject or parent
        healthComponent = GetComponent<HealthComponent>(); 
        if (healthComponent == null)
        {
            Debug.LogError("HealthComponent not found on " + gameObject.name);
        }
    }

    // Method to damage using Bullet
    public void Damage(Bullet bullet)
    {
        if (healthComponent != null)
        {
            healthComponent.Subtract(bullet.damage); // Decrease health based on Bullet damage
        }
    }

    // Method to damage using an integer value
    public void Damage(int damage)
    {
        healthComponent.Subtract(damage); // Decrease health based on the integer damage
    }
}
