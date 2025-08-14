using UnityEngine; // Using directive for Unity's core functions and classes
using UnityEngine.Audio; // Using directive for handling audio features in Unity
using UnityEngine.UI; // Using directive for handling UI elements

public class characterStats : MonoBehaviour // Class definition (characterStats is a class that inherits from MonoBehaviour)
{
    // Variables (Serialized fields that can be set in the Unity Inspector)
    AudioSource ad; // Variable (Object reference to the AudioSource component)
    [SerializeField] AudioClip deathSound; // Variable (Audio clip for the death sound)

    [SerializeField] ParticleSystem ParticleDie; // Variable (Particle effect for when the character dies)

    [SerializeField] float maxHealth = 100f; // Variable (Maximum health of the character)
    public float power = 10f; // Variable (Power value, used for dealing damage)
    int killScore = 200; // Variable (Score awarded for killing this character if it's an enemy)

    // Property (Current health of the character, private set means it can only be modified within this class)
    public float currentHealth { get; private set; }

    // Start method (Called once before the first execution of Update after the MonoBehaviour is created)
    void Start()
    {
        currentHealth = maxHealth; // Set the current health to the maximum at the start
        ad = GetComponent<AudioSource>(); // Get the AudioSource component attached to the character
    }

    // Method (Changes the health of the character by a specified value)
    public void changeHealth(float value)
    {
        currentHealth += value; // Increase or decrease the current health by the value
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Ensure current health stays within 0 to maxHealth
        Debug.Log("current Health: " + currentHealth + "/" + maxHealth); // Log the current health to the console

        if (transform.CompareTag("Enemy")) // Check if the character is tagged as "Enemy"
        {
            // Update the enemy's health bar UI (if it exists)
            transform.Find("Canvas").GetChild(1).GetComponent<Image>().fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0) // Check if the character's health has dropped to 0 or below
            Die(); // Call the Die method to handle character death
    }

    // Method (Handles the death of the character)
    void Die()
    {
        playDeathSound(); // Play the death sound if it exists
        if (transform.CompareTag("Player")) // Check if the character is the player
        {
            // Restart the level when the player dies
            LevelManager.Instance.RestartLevel(); // Call the LevelManager to restart the level
        }
        else if (transform.CompareTag("Enemy")) // Check if the character is an enemy
        {
            LevelManager.Instance.score += killScore; // Increase the score by the killScore value
            LevelManager.Instance.EnemyKilled(); // Notify the LevelManager that an enemy has been killed

            // Play the death particle effect if it exists
            if (ParticleDie != null)
            {
                ParticleDie.Play(); // Play the particle effect
            }

            // Destroy the enemy object
            Destroy(gameObject); // Remove the enemy object from the game
        }
    }

    // Method (Plays the death sound effect)
    private void playDeathSound()
    {
        // Check if the death sound and AudioSource exist
        if (deathSound != null && ad != null)
        {
            ad.PlayOneShot(deathSound); // Play the death sound once
        }
    }
}
