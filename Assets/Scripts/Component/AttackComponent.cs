using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AttackComponent : MonoBehaviour
{
    public Bullet bullet;  // Bullet used for attack
    public int damage;     // Damage dealt by the object

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collided with has HitboxComponent
        HitboxComponent hitbox = other.GetComponent<HitboxComponent>();

        // If the collided object has a HitboxComponent, deal damage
        if (hitbox != null)
        {
            hitbox.Damage(bullet); // Apply bullet damage to hitbox
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitbox = GetComponent<HitboxComponent>();
        if (collision.gameObject.tag == gameObject.tag)
        {
            return;
        }
        if (collision.CompareTag("Bullet"))
        {
            // int damage = collision.GetComponent<Bullet>().damage; // Get damage from Bullet

            if (hitbox != null)
            {
                hitbox.Damage(collision.GetComponent<Bullet>()); // Apply damage using HitboxComponent with Bullet parameter
            }
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player")
        {
            hitbox = GetComponent<HitboxComponent>();
            if (hitbox != null)
            {
                hitbox.Damage(damage);
            }
        }

    }
}

