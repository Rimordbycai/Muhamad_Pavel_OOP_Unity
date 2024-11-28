using UnityEngine;
using UnityEngine.UIElements;

public class GameStats : MonoBehaviour
{
    public VisualElement rootVisualElement;

    private Label healthLabel;
    private Label enemiesLeftLabel;
    private Label waveLabel;
    private Label pointsLabel;

    private HealthComponent playerHealthComponent;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        rootVisualElement = uiDocument.rootVisualElement;

        healthLabel = rootVisualElement.Q<Label>("health");
        enemiesLeftLabel = rootVisualElement.Q<Label>("enemiesleft");
        waveLabel = rootVisualElement.Q<Label>("wave");
        pointsLabel = rootVisualElement.Q<Label>("point");

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerHealthComponent = playerObject.GetComponent<HealthComponent>();
        }
    }

    public void UpdateHealth(float health)
    {
        healthLabel.text = $"Health: {health}";
    }

    public void UpdateEnemiesLeft(int enemiesLeft)
    {
        enemiesLeftLabel.text = $"Enemies Left: {enemiesLeft}";
    }

    public void UpdateWave(int wave)
    {
        waveLabel.text = $"Wave: {wave}";
    }

    public void UpdatePoints(int points)
    {
        pointsLabel.text = $"Points: {points}";
    }

    private void Update()
    {
        if (playerHealthComponent != null)
        {
            UpdateHealth(playerHealthComponent.Health);
        }
    }
}
