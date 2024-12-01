using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : Enemy
{
    private Transform playerTransform; // Posisi Player
    private float speed = 2.0f;        // Kecepatan gerakan enemy

    private void Start()
    {
        // Temukan Player di dalam scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Pastikan Player ditemukan sebelum menetapkan transform-nya
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found in the scene. EnemyTargeting will not move.");
        }
    }

    private void Update()
    {
        // Jika Player ditemukan, bergerak ke arahnya
        if (playerTransform != null)
        {
            // Hitung arah gerakan menuju Player
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Jika enemy bersentuhan dengan Player, maka enemy akan hilang
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject); // Menghancurkan enemy saat menyentuh Player
        }
    }
}
