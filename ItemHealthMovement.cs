using UnityEngine;

public class ItemHealthMovement : MonoBehaviour
{
    public float amp = 0.5f; // Amplitude - the vertical range of the movement
    public float freq = 1f;  // Frequency - how fast the object moves up and down
    private Vector3 initPos; // Stores the initial position of the object

    void Start()
    {
        initPos = transform.position; // Save the initial position when the game starts
    }

    void Update()
    {
        // Update the position to move the object up and down using the Sin function
        transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * amp + initPos.y, initPos.z);
    }
}
