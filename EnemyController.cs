using System.Collections; // Using directive for handling collections like lists, arrays, etc.
using UnityEngine; // Using UnityEngine library, necessary for accessing Unity's core functions and classes.

public class EnemyController : MonoBehaviour // Class definition (EnemyController is a class that inherits from MonoBehaviour)
{
    // Variables (Serialized fields that can be set in the Unity Inspector)
    [SerializeField] AudioClip roarSound;  // Variable (Audio clip for the enemy's roar)
    [SerializeField] AudioClip deathSound; // Variable (Audio clip for the enemy's death)

    AudioSource audioSource; // Variable (Object reference to the AudioSource component)

    public float roarRadius = 10f;  // Variable (Radius within which the roar sound should play)
    characterStats stats; // Variable (Reference to the characterStats script/component)
    Animator anim; // Variable (Reference to the Animator component for controlling animations)
    public Transform Player; // Variable (Reference to the player's transform to calculate distance)
    public float attackRadius = 15f; // Variable (Radius within which the enemy can attack)
    bool canAttack = true; // Variable (Flag to check if the enemy can attack)
    float attackCooldown = 3f; // Variable (Cooldown time between attacks)

    // Start method (Called once before the first execution of Update after the MonoBehaviour is created)
    void Start()
    {
        anim = GetComponent<Animator>(); // Method (Get the Animator component attached to the enemy object)
        stats = GetComponent<characterStats>(); // Method (Get the characterStats component attached to the enemy object)
        audioSource = GetComponent<AudioSource>(); // Method (Get the AudioSource component attached to the enemy object)
    }

    // Update method (Called once per frame to update the enemy's state)
    void Update()
    {
        float distance = Vector3.Distance(transform.position, Player.position); // Variable (Calculate the distance between the enemy and the player)

        if (distance < roarRadius) // Check if the player is within the roar radius
        {
            if (roarSound != null && !audioSource.isPlaying)  // Check if roar sound is assigned and not already playing
            {
                audioSource.PlayOneShot(roarSound); // Method (Play roar sound)
            }
        }

        if (distance < attackRadius) // Check if the player is within the attack radius
        {
            if (canAttack) // Check if the enemy can attack
            {
                StartCoroutine(cooldown()); // Method (Start a coroutine to handle attack cooldown)
                anim.SetTrigger("attack"); // Method (Trigger the attack animation)
            }
        }
    }

    IEnumerator cooldown() // Coroutine method (Handles the cooldown period between attacks)
    {
        canAttack = false; // Set canAttack to false to prevent attacking during cooldown
        yield return new WaitForSeconds(attackCooldown); // Coroutine step (Wait for the cooldown duration)
        canAttack = true; // Set canAttack to true, allowing the enemy to attack again
    }

    private void OnTriggerEnter(Collider other) // Method (Triggered when another collider enters this object's collider)
    {
        if (other.CompareTag("Player")) // Check if the collided object is tagged as "Player"
        {
            Debug.Log("player contacted"); // Log a message indicating that the player was contacted
            stats.changeHealth(-other.GetComponent<characterStats>().power); // Method (Reduce the enemy's health based on the player's power)
        }
    }

    public void DamagePlayer() // Method (Damages the player, called by animation events or other triggers)
    {
        LevelManager.Instance.player.GetComponent<characterStats>().changeHealth(-stats.power); // Method (Reduce the player's health based on the enemy's power)
    }
}
