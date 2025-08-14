using UnityEngine; // Using directive for Unity's core functions and classes
using UnityEngine.SceneManagement; // Using directive for managing and loading scenes

public class LevelManager : MonoBehaviour // Class definition (LevelManager is a class that inherits from MonoBehaviour)
{
    AudioSource audioSource; // Variable (Reference to the AudioSource component for playing sounds)

    [SerializeField] AudioClip winSound; // Variable (Audio clip for winning a level)
    [SerializeField] AudioClip lastWinSound; // Variable (Audio clip for winning the final level)
    [SerializeField] AudioClip startAgin; // Variable (Audio clip for restarting the level)

    public static LevelManager Instance; // Variable (Static instance of LevelManager for easy access from other scripts)
    public Transform player; // Variable (Reference to the player's transform component)

    public int score; // Variable (Tracks the player's score)
    public int levelItems; // Variable (Tracks the number of items collected in the level)

    public int enemiesKilled = 0; // Variable (Tracks the number of enemies killed in the current level)
    public int enemiesToKill = 1; // Variable (The number of enemies required to kill before progressing to the next level)

    // Awake method (Called once before Start, used for initializing variables)
    private void Awake()
    {
        Instance = this; // Assign this instance of LevelManager to the static variable Instance
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to the LevelManager
    }

    // Method (Called when an enemy is killed, checks if all required enemies are killed)
    public void EnemyKilled()
    {
        enemiesKilled++; // Increase the count of killed enemies by one
        if (enemiesKilled >= enemiesToKill) // Check if the required number of enemies have been killed
        {
            LoadNextLevel(); // Call the method to load the next level
        }
    }

    // Method (Loads the next level in the build settings, or loops back to the first level)
    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex; // Get the current scene's index in the build settings
        int nextIndex = currentIndex + 1; // Calculate the next scene's index

        if (nextIndex == SceneManager.sceneCountInBuildSettings) // Check if the next index is beyond the last level
        {
            audioSource.PlayOneShot(lastWinSound); // Play the special sound for winning the final level
            nextIndex = 0; // Loop back to the first level
        }
        else
        {
            audioSource.PlayOneShot(winSound); // Play the regular win sound for advancing to the next level
        }

        SceneManager.LoadScene(nextIndex); // Load the next scene
    }

    // Method (Restarts the current level)
    public void RestartLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex; // Get the current scene's index
        audioSource.PlayOneShot(startAgin); // Play the restart sound
        SceneManager.LoadScene(currentIndex); // Reload the current scene
    }
}
