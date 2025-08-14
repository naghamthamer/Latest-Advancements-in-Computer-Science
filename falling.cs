using UnityEngine;
using UnityEngine.SceneManagement; // Using directive for scene management functions, such as restarting the scene

public class falling : MonoBehaviour // Class definition (falling is a class that inherits from MonoBehaviour)
{
    [SerializeField] float falling_time = 3; // Variable (Time before the item starts falling)
    [SerializeField] AudioClip fallingSound; // Variable (The sound to play when the item falls)
    private AudioSource audioSource; // Variable (Reference to the AudioSource component for playing sounds)
    private bool hasFallen = false; // Variable (Flag to check if the item has already fallen)

    // Start method (Called once before the first frame update)
    void Start()
    {
        // Disable gravity and make the item invisible initially
        GetComponent<Rigidbody>().useGravity = false; // Object (Rigidbody component, disables gravity)
        GetComponent<MeshRenderer>().enabled = false; // Object (MeshRenderer component, hides the item)

        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>(); // Object (AudioSource component)
    }

    // Update method (Called once per frame)
    void Update()
    {
        // Check if the time has passed to start the fall
        if (Time.time >= falling_time && !hasFallen) // Condition (Checks if enough time has passed and the item hasn't fallen yet)
        {
            GetComponent<Rigidbody>().useGravity = true; // Object (Enables gravity to make the item fall)
            GetComponent<MeshRenderer>().enabled = true; // Object (Makes the item visible)
            hasFallen = true; // Variable (Sets the flag to true, indicating the item has fallen)

            // Play the falling sound if it's assigned and the audio source is available
            if (fallingSound != null && audioSource != null) // Condition (Checks if the sound and audio source are valid)
            {
                audioSource.PlayOneShot(fallingSound); // Method (Plays the falling sound)
            }
        }
    }

    // OnCollisionEnter method (Called when the item collides with another object)
    void OnCollisionEnter(Collision collision) // Method (Handles the collision event)
    {
        // Check if the item has hit the ground
        if (collision.gameObject.CompareTag("Ground")) // Condition (Checks if the item collides with an object tagged as "Ground")
        {
            GetComponent<Rigidbody>().isKinematic = true; // Object (Makes the Rigidbody kinematic, stopping further movement)
        }

        // Check if the item has hit the player
        if (collision.gameObject.CompareTag("Player")) // Condition (Checks if the item collides with an object tagged as "Player")
        {
            LevelManager.Instance.RestartLevel(); // Method (Calls the LevelManager to restart the current level)
        }
    }
}
