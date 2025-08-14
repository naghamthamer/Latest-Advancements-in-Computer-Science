using System.Collections; // Using directive for handling collections like lists, arrays, etc.
using TMPro; // Using directive for TextMeshPro, which is used for rendering text in the UI.
using Unity.VisualScripting; // Using directive for Unity's Visual Scripting, used for creating visual scripts.
using UnityEngine; // Using UnityEngine library, necessary for accessing Unity's core functions and classes.
using UnityEngine.EventSystems; // Using directive for event systems, which manage input events like clicks, touches, etc.

public class CharacterMovement : MonoBehaviour // Class definition (CharacterMovement is a class that inherits from MonoBehaviour)
{
    // Variables (Serialized fields that can be set in the Unity Inspector)
    [SerializeField] AudioClip footstepSound;  // Variable (Audio clip for footsteps)
    [SerializeField] AudioClip jumpSound;      // Variable (Audio clip for jumping)
    [SerializeField] AudioClip attackSound;    // Variable (Audio clip for attacking)
    [SerializeField] AudioClip healthCollectSound;  // Variable (Audio clip for collecting health)
    [SerializeField] AudioClip itemCollectSound;    // Variable (Audio clip for collecting other items)

    AudioSource audioSource; // Variable (Object reference to the AudioSource component)

    [SerializeField] ParticleSystem ParticleSuccessCollect; // Variable (Particle system for visual effects on successful collection)

    public TextMeshProUGUI ItemText; // Variable (Reference to the UI text for item count display)
    public TextMeshProUGUI HeartText; // Variable (Reference to the UI text for heart/health display)

    CharacterController Controller; // Variable (Object reference to the CharacterController component)

    characterStats stats; // Variable (Reference to the characterStats script/component)

    private int Item = 0; // Variable (Count of items collected by the player)
    private int Heart = 0; // Variable (Count of hearts/health collected by the player)

    public float speed = 5f; // Variable (Movement speed of the character)

    public float jumpValue = 10f; // Variable (Force applied when the character jumps)

    public float gravity = 9.81f; // Variable (Gravity force applied to the character)

    private float verticalVelocity = 0f; // Variable (Current vertical speed of the character, for jumping/falling)

    Transform cam; // Variable (Reference to the camera's transform to align movement with the camera)

    Animator anim; // Variable (Reference to the Animator component for controlling animations)

    void Start() // Method (Called once at the start of the game)
    {
        audioSource = GetComponent<AudioSource>(); // Method (Get the AudioSource component attached to the player object)

        Controller = GetComponent<CharacterController>(); // Method (Get the CharacterController component attached to the player object)

        cam = Camera.main.transform; // Method (Get the main camera's transform)

        anim = GetComponent<Animator>(); // Method (Get the Animator component attached to the player object)

        stats = GetComponent<characterStats>(); // Method (Get the characterStats component attached to the player object)
    }

    void Update() // Method (Called every frame to update the character's state)
    {
        float horizontal = Input.GetAxis("Horizontal"); // Variable (Gets horizontal input from player)
        float vertical = Input.GetAxis("Vertical"); // Variable (Gets vertical input from player)
        bool isSprint = Input.GetKey(KeyCode.LeftShift); // Variable (Checks if the sprint key is pressed)
        float sprint = isSprint ? 2.5f : 1f; // Variable (Sets sprint multiplier based on whether sprint key is pressed)

        if (horizontal != 0 || vertical != 0)  // Check if the player is moving
        {
            if (!audioSource.isPlaying)  // Check if the footstep sound is not already playing
            {
                audioSource.PlayOneShot(footstepSound); // Method (Play footstep sound)
            }
        }

        if (Input.GetMouseButtonDown(0)) // Check if the left mouse button is clicked (for attack)
        {
            anim.SetTrigger("attack"); // Method (Trigger the attack animation)
            audioSource.PlayOneShot(attackSound); // Method (Play attack sound)
        }

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized; // Variable (Direction of movement based on input)

        if (moveDirection.magnitude > 0.1f) // Check if there is significant movement input
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Variable (Calculate angle for rotation)

            transform.rotation = Quaternion.Euler(0, angle, 0); // Method (Rotate the player based on movement direction)

            moveDirection = cam.TransformDirection(moveDirection); // Method (Align movement with the camera direction)
        }

        if (Controller.isGrounded) // Check if the player is on the ground
        {
            verticalVelocity = -gravity * Time.deltaTime; // Variable (Apply slight downward force to keep the player grounded)

            if (Input.GetKeyDown(KeyCode.Space)) // Check if the jump key is pressed
            {
                verticalVelocity = jumpValue; // Variable (Set vertical velocity for jumping)
                audioSource.PlayOneShot(jumpSound); // Method (Play jump sound)
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime; // Variable (Apply gravity when in the air)
        }

        moveDirection.y = verticalVelocity; // Variable (Add vertical movement to the direction)

        anim.SetFloat("speed", Mathf.Clamp(moveDirection.magnitude, 0, 0.5f) + (isSprint ? 0.5f : 0)); // Method (Update animation speed based on movement and sprint)

        Controller.Move(moveDirection * Time.deltaTime * speed * sprint); // Method (Move the character based on direction and speed)
    }

    public void DoAttack() // Method (Activates the attack by enabling the collider)
    {
        transform.Find("collider").GetComponent<BoxCollider>().enabled = true; // Method (Enable the collider for attack)
        StartCoroutine(HideCollider()); // Method (Start a coroutine to disable the collider after a delay)
    }

    IEnumerator HideCollider() // Coroutine Method (Waits and then disables the collider)
    {
        yield return new WaitForSeconds(0.5f); // Coroutine Step (Wait for 0.5 seconds)
        transform.Find("collider").GetComponent<BoxCollider>().enabled = false; // Method (Disable the collider after the delay)
    }

    private void OnTriggerEnter(Collider other) // Method (Triggered when another collider enters this object's collider)
    {
        if (other.CompareTag("Health")) // Check if the collided object is tagged as "Health"
        {
            Heart++; // Variable (Increase heart/health count)
            HeartText.text = "Heart num: " + Heart.ToString(); // Method (Update the heart/health count display)
            stats.changeHealth(20); // Method (Increase player's health by 20)
            audioSource.PlayOneShot(healthCollectSound); // Method (Play health collection sound)
            Destroy(other.gameObject); // Method (Destroy the collected health object)
            ParticleSuccessCollect.Play(); // Method (Play the particle effect for successful collection)
        }
        else if (other.CompareTag("Item")) // Check if the collided object is tagged as "Item"
        {
            Item++; // Variable (Increase item count)
            ItemText.text = "Item score: " + Item.ToString(); // Method (Update the item count display)
            LevelManager.Instance.levelItems++; // Variable (Increase level item count)
            Debug.Log("Item score: " + LevelManager.Instance.levelItems); // Method (Log the item score to the console)
            audioSource.PlayOneShot(itemCollectSound); // Method (Play item collection sound)
            Destroy(other.gameObject); // Method (Destroy the collected item object)
            ParticleSuccessCollect.Play(); // Method (Play the particle effect for successful collection)
        }
    }
}
