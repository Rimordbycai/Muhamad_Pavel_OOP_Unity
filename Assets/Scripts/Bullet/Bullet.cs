using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float bulletSpeed = 20;
    public int damage = 10;
    private Rigidbody2D rb;
    public IObjectPool<Bullet> ObjectPool;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * bulletSpeed;
    }

    public void SetPool(IObjectPool<Bullet> pool)
    {
        ObjectPool = pool;
    }

    public void FixedUpdate()
    {
        // Set velocity of bullet to move upwards
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * bulletSpeed;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        var hitbox = collision.GetComponent<HitboxComponent>();
        if (hitbox != null)
        {
            // Apply damage to the enemy
            hitbox.Damage(damage);
            ObjectPool.Release(this);
        }   
    }

    private void OnBecameInvisible()
    {
        // Add condition to check if the bullet should be released
        if (gameObject.activeSelf && ObjectPool != null)
        {
            ObjectPool.Release(this);
        }
    }
}
