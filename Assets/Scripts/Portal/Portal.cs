using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotateSpeed;
    private Vector2 newPosition;

    void Start()
    {
        // Initialize newPosition with a random location
        ChangePosition();
    }

    void Update()
    {
        if (Player.Instance == null)
        {
            Debug.LogError("Player instance is null. Portal cannot check weapon status.");
        }

        // Access Player and check if weaponHolder is present in WeaponPickup
        WeaponPickup weaponPickup = Player.Instance.GetComponentInChildren<WeaponPickup>();

        // Only show the portal if the player has weaponHolder
        if (GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Weapon>() != null)
        {
            Debug.Log("Weapon is found on player.");
            // Show the portal
            GetComponent<SpriteRenderer>().enabled = true;
            GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            // Hide the portal if the player doesn't have the weapon
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider2D>().enabled = false;
        }

        // Move the asteroid towards the new position
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
        transform.position = Vector2.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

        // Check if the asteroid is close enough to the target position
        if (Vector2.Distance(transform.position, newPosition) < 0.5f)
        {
            ChangePosition();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the asteroid collides with the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Asteroid collided with the player. Loading Main scene.");
            GameManager.Instance.LevelManager.LoadScene("Main");
        }
    }

    void ChangePosition()
    {
        // Set newPosition to a random position within a specific range (example)
        newPosition = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
    }
}